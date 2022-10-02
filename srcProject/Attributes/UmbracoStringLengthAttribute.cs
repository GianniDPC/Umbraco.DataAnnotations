using System;
using System.ComponentModel.DataAnnotations;
using Umbraco.DataAnnotations.Interfaces;

namespace Umbraco.DataAnnotations.Attributes
{
    /// <summary>
    ///     Validation attribute to assert a string property, field or parameter does not exceed a maximum length
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
    AllowMultiple = false)]
    public sealed class UmbracoStringLengthAttribute : StringLengthAttribute, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; } = "MinMaxLengthError";

        public UmbracoStringLengthAttribute(int maximumLength)
            : base(maximumLength)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            ErrorMessage = UmbracoDictionary.GetDictionaryValue(DictionaryKey);
            return base.FormatErrorMessage(name);
        }
    }
}
