using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
    public static class LocalizationManager
    {
        private static ResourceManager rm = new ResourceManager(
            "WpfClient.Resources.Strings",
            System.Reflection.Assembly.Load("WpfClient")); 

        public static CultureInfo CurrentCulture { get; private set; } = CultureInfo.CurrentCulture;

        public static void SetLanguage(string culture)
        {
            CurrentCulture = new CultureInfo(culture);
        }

        public static string GetString(string key)
        {
            try
            {
                return rm.GetString(key, CurrentCulture) ?? key;
            }
            catch
            {
                return key; 
            }
        }
    }
}
