using System;
using System.Globalization;

namespace NHibernate.JetDriver.Tests
{
    public class ContextCultureSwitch : IDisposable
    {
        private CultureInfo _oldCulture;
        private CultureInfo _oldUiCulture;

        public ContextCultureSwitch(CultureInfo culture)
        {
            SnapshotCulture();
            ISetThreadCulture(culture, culture);
        }

        private void SnapshotCulture()
        {
            _oldCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            _oldUiCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
        }

        private static void ISetThreadCulture(CultureInfo culture, CultureInfo uiCulture)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = uiCulture;
        }

        public CultureInfo Current
        {
            get { return System.Threading.Thread.CurrentThread.CurrentCulture; }
        }

        public void Dispose()
        {
            ISetThreadCulture(_oldCulture, _oldUiCulture);
        }
    }
}