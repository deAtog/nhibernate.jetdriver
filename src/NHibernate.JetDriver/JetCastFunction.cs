using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Dialect.Function;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;

namespace NHibernate.JetDriver
{
    /// <summary>
    /// ANSI-SQL style cast(foo as type) where the type is a NHibernate type
    /// </summary>
    [Serializable]
    public class JetCastFunction : CastFunction
    {
        static JetCastFunction()
        {
            JetConversionFunctions.Add("TINYINT", "CByte");
            JetConversionFunctions.Add("MONEY", "CCur");
            JetConversionFunctions.Add("DATETIME", "CDate");
            JetConversionFunctions.Add("DECIMAL", "CDec");
            JetConversionFunctions.Add("REAL", "CSng");
            JetConversionFunctions.Add("FLOAT", "CDbl");
            JetConversionFunctions.Add("SMALLINT", "CInt");
            JetConversionFunctions.Add("INTEGER", "CLng");
            JetConversionFunctions.Add("INT", "CLng");
            JetConversionFunctions.Add("LONG", "CLng");
        }

        private static readonly Dictionary<string, string> JetConversionFunctions = new Dictionary<string, string>();

        private SqlString GetJetConvertionFunction(Dialect.Dialect dialect, string sqlType, object arg)
        {
            sqlType = sqlType.ToUpper();

            if (JetConversionFunctions.ContainsKey(sqlType))
            {
                string functionname = JetConversionFunctions[sqlType];
                var sqlArg = arg.ToString();

                if (sqlArg.IndexOf("?") >= 0)
                {
                    return new SqlString(functionname, "(", arg, ")");
                }

                return new SqlString("iff", "(", "ISNULL", "(", arg, ")", ",", "NULL", ",", functionname, "(", arg, ")", ")");
            }
            else if (sqlType.Contains("TEXT") || sqlType.Contains("CHAR"))
            {
                return new SqlString("CStr", "(", arg, ")");
            }
            else if (sqlType == "BIT")
            {
                string true_value = dialect.ToBooleanValueString(true);
                string false_value = dialect.ToBooleanValueString(false);

                return new SqlString("iif", "(", arg, "<>", "0", ",", true_value, ",", false_value, ")");
            }

            return new SqlString("(", arg, ")");
        }

        protected override SqlString Render(IList args, string sqlType, ISessionFactoryImplementor factory)
        {
            return GetJetConvertionFunction(factory.Dialect, sqlType, args[0]);
        }
    }
}