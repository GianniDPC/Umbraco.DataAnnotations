using System;
using System.ComponentModel.DataAnnotations;
using Umbraco.DataAnnotations.Interfaces;

namespace Umbraco.DataAnnotations.Attributes
{
    /// <summary>
    ///     Used for specifying a range constraint
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
    AllowMultiple = false)]
    public sealed class UmbracoRangeAttribute : RangeAttribute, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; } = "RangeError";

        public UmbracoRangeAttribute(int minimum, int maximum)
            : base(minimum, maximum)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            ErrorMessage = UmbracoDictionary.GetDictionaryValue(DictionaryKey);
            return base.FormatErrorMessage(name);
        }
    }
}
