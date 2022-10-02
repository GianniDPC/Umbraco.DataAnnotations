using System;
using System.ComponentModel.DataAnnotations;
using Umbraco.DataAnnotations.Interfaces;

namespace Umbraco.DataAnnotations.Attributes
{
    /// <summary>
    ///     Specifies the maximum length of collection/string data allowed in a property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
    AllowMultiple = false)]
    public sealed class UmbracoMaxLengthAttribute : MaxLengthAttribute, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; } = "MaxLengthError";

        public UmbracoMaxLengthAttribute(int length)
            : base(length)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            ErrorMessage = UmbracoDictionary.GetDictionaryValue(DictionaryKey);
            return base.FormatErrorMessage(name);
        }
    }
}
