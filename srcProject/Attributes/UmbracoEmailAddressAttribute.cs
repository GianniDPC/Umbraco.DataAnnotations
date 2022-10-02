using System;
using System.ComponentModel.DataAnnotations;
using Umbraco.DataAnnotations.Interfaces;

namespace Umbraco.DataAnnotations.Attributes
{
    /// <summary>
    /// Specifies that a data field value must be a valid Email Address
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
    AllowMultiple = false)]
    public sealed class UmbracoEmailAddressAttribute : RegularExpressionAttribute, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; } = "EmailError";

        private static new string Pattern { get; set; } = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        public UmbracoEmailAddressAttribute()
            : base(Pattern)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            ErrorMessage = UmbracoDictionary.GetDictionaryValue(DictionaryKey);
            return base.FormatErrorMessage(name);
        }
    }
}
