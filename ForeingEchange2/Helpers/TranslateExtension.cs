﻿namespace ForeingEchange2.Helpers
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Resources;
    using Interfaces;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        readonly CultureInfo ci;
        const string ResourceId = "ForeingEchange2.Resources.Resource";

        static readonly Lazy<ResourceManager> ResMgr =
            new Lazy<ResourceManager>(() => new ResourceManager(
            ResourceId,
                typeof(TranslateExtension).GetTypeInfo().Assembly));

        public TranslateExtension(){
            ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
        }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider){
            if(Text == null){
                return "";
            }

            var translation = ResMgr.Value.GetString(Text, ci);

            if(translation == null){
                throw new ArgumentException(
                    string.Format("key '{0}' was not found in resources " +
                                  "'{1}' for culture '{2} .'", Text, ResourceId,
                                  ci.Name), "Text");

            }
            return translation;
        }

    }
  
}