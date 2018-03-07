using Xamarin.Forms;

[assembly: Dependency(typeof(ForeingEchange2.Droid.Implementations.Localize))]

namespace ForeingEchange2.Droid.Implementations
{
	using System;
    using System.Globalization;
    using ForeingEchange2.Interfaces;
    using System.Threading;
    using Helpers;

    public class Localize : ILocalize
    {
        public Localize()
        {
        }

        public CultureInfo GetCurrentCultureInfo()
        {
            var netLanguage = "en";
            var androidLocale = Java.Util.Locale.Default;
            netLanguage = AndroidToDotnetLanguage(androidLocale.ToString().Replace("_", "-"));

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
                catch (CultureNotFoundException ex2 )
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

        string AndroidToDotnetLanguage(string androidLanguage){
            var netLanguage = androidLanguage;

            switch (androidLanguage)
            {
                case "ms-BN":
                case "ms-MY":
                case "ms-SG":
                    netLanguage = "ms";
                    break;

                case "in-ID":
                    netLanguage = "id-ID";
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
                case "gws":
                    netLanguage = "de-CH";
                    break;
            }
            return netLanguage;
        }
    }
}
