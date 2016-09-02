using System;
using System.Globalization;

namespace LaazyDate
{
    public class SystemCulture
    {
        static Func<CultureInfo> current = () => CultureInfo.CurrentCulture;
        public static CultureInfo Current
        {
            get { return current(); }
            set { current = () => value; }
        }

        public static void Reset()
        {
            current = () => CultureInfo.CurrentCulture;
        }
    }
}
