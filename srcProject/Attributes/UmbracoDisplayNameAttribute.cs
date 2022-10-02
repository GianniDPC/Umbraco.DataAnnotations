using System;
using System.ComponentModel;
using Umbraco.DataAnnotations.Interfaces;

namespace Umbraco.DataAnnotations.Attributes
{
    /// <summary>
    /// Specifies the display name for a property or event.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class UmbracoDisplayNameAttribute : DisplayNameAttribute, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; }

        public UmbracoDisplayNameAttribute(string dictionaryKey)
            : base()
        {
            DictionaryKey = dictionaryKey;
        }

        public override string DisplayName
        {
            get
            {
                return UmbracoDictionary.GetDictionaryValue(DictionaryKey);
            }
        }
    }
}
