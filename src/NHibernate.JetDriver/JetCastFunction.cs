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

        protected override SqlString Render(object expression, string sqlType, ISessionFactoryImplementor factory)
        {
            sqlType = sqlType.ToUpper();

            if (JetConversionFunctions.ContainsKey(sqlType))
            {
                string functionname = JetConversionFunctions[sqlType];
                var sqlArg = expression.ToString();

                if (sqlArg.IndexOf("?") >= 0)
                {
                    return new SqlString(functionname, "(", expression, ")");
                }

                return new SqlString("iif", "(", "ISNULL", "(", expression, ")", ",", "NULL", ",", functionname, "(", expression, ")", ")");
            }
            else if (sqlType.Contains("TEXT") || sqlType.Contains("CHAR"))
            {
                return new SqlString("CStr", "(", expression, ")");
            }
            else if (sqlType == "BIT")
            {
                Dialect.Dialect dialect = factory.Dialect;
                string true_value = dialect.ToBooleanValueString(true);
                string false_value = dialect.ToBooleanValueString(false);

                return new SqlString("iif", "(", expression, "<>", "0", ",", true_value, ",", false_value, ")");
            }

            return new SqlString("(", expression, ")");
        }
    }
}