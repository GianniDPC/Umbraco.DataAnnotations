using Our.Umbraco.DataAnnotations.Conditionals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Umbraco.DataAnnotations.Interfaces;

namespace Umbraco.DataAnnotations.ConditionalAttributes
{
    /// <summary>
    ///     Conditional validation attribute to indicate that a property field or parameter is required.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
    AllowMultiple = false)]
    public sealed class UmbracoRequiredIfAttribute : ConditionalValidationAttribute, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; }

        protected override string ValidationName
        {
            get { return "requiredif"; }
        }

        public UmbracoRequiredIfAttribute(string propertyName, object desiredvalue) : base(new RequiredAttribute(), propertyName, desiredvalue)
        {
        }

        protected override IDictionary<string, object> GetExtraValidationParameters()
        {
            return new Dictionary<string, object>()
            {
                { "rule", "required" }
            };
        }

        public override string FormatErrorMessage(string name)
        {
            ErrorMessage = UmbracoDictionary.GetDictionaryValue(DictionaryKey);
            return base.FormatErrorMessage(name);
        }
    }
}
