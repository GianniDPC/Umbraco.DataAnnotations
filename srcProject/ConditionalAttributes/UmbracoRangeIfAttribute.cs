using Our.Umbraco.DataAnnotations.Conditionals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Umbraco.DataAnnotations.Interfaces;

namespace Umbraco.DataAnnotations.ConditionalAttributes
{
    /// <summary>
    ///     Used for conditionally specifying a range constraint
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
    AllowMultiple = false)]
    public sealed class UmbracoRangeIfAttribute : ConditionalValidationAttribute, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; }

        private readonly int minimum;
        private readonly int maximum;

        protected override string ValidationName
        {
            get { return "rangeif"; }
        }

        public UmbracoRangeIfAttribute(int minimum, int maximum, string dependentProperty, object targetValue)
            : base(new RangeAttribute(minimum, maximum), dependentProperty, targetValue)
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }

        protected override IDictionary<string, object> GetExtraValidationParameters()
        {
            // Set the rule Range and the rule param [minumum,maximum]
            return new Dictionary<string, object>()
            {
                {"rule", "range"},
                { "ruleparam", string.Format("[{0},{1}]", this.minimum, this.maximum) }
            };
        }

        public override string FormatErrorMessage(string name)
        {
            ErrorMessage = UmbracoDictionary.GetDictionaryValue(DictionaryKey);
            return base.FormatErrorMessage(name);
        }
    }
}
