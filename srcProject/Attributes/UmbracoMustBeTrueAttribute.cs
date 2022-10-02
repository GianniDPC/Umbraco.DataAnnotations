using System;
using System.ComponentModel.DataAnnotations;
using Umbraco.DataAnnotations.Interfaces;

namespace Umbraco.DataAnnotations.Attributes
{
    /// <summary>
    /// Specifies that the property must be true.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class UmbracoMustBeTrueAttribute : ValidationAttribute, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; } = "MustBeTrueError";

        public UmbracoMustBeTrueAttribute()
        {
        }

        public override bool IsValid(object value)
        {
            return value != null && value is bool && (bool)value;
        }

        public override string FormatErrorMessage(string name)
        {
            ErrorMessage = UmbracoDictionary.GetDictionaryValue(DictionaryKey);
            return base.FormatErrorMessage(name);
        }
    }
}
