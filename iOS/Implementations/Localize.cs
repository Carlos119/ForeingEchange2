using Xamarin.Forms;

[assembly: Dependency(typeof(ForeingEchange2.iOS.Implementations.Localize))]

namespace ForeingEchange2.iOS.Implementations
{
    using System;
    using System.Globalization;
    using ForeingEchange2.Interfaces;
    using System.Threading;
    using Helpers;
    using Foundation;

    public class Localize : ILocalize
    {
        public Localize()
        {
        }

        public CultureInfo GetCurrentCultureInfo()
        {
            var netLanguage = "en";
            if (NSLocale.PreferredLanguages.Length >0){

                var pref = NSLocale.PreferredLanguages[0];
                netLanguage = iOSToDotnetLenguages(pref);
            }

            System.Globalization.CultureInfo ci = null;
            try
            {
                ci = new System.Globalization.CultureInfo(netLanguage);
            }
            catch (CultureNotFoundException ex)
            {
                try
                {
                    var fallback = ToDotnetFallbackLanguage(new PlatformCulture(netLanguage));
                    ci = new System.Globalization.CultureInfo(fallback);
                }
                catch (CultureNotFoundException ex2)
                {
                    ci = new System.Globalization.CultureInfo("en");
                }
            }
            return ci;
        }

        public void SetLocale(CultureInfo ci)
        {
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }

        string iOSToDotnetLenguages(string iOSLanguage){
            var netLanguage = iOSLanguage;

            switch (iOSLanguage)
            {
                case "ms-MY":
                case "ms-SG":
                    netLanguage = "ms";
                    break;
                case "gsw-CH":
                    netLanguage = "de-CH";
                    break;
            }

            return netLanguage;
        }

        string ToDotnetFallbackLanguage(PlatformCulture platCulture){

            var netLanguage = platCulture.LocaleCode;

            switch (platCulture.LocaleCode)
            {
                case "pt":
                    netLanguage = "pt-PT";
                    break;
                case "gws":
                    netLanguage = "de-DE";
                    break;
            }
            return netLanguage;
        }
    }
}
