using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Umbraco.DataAnnotations;
using Umbraco.DataAnnotations.Interfaces;


namespace Our.Umbraco.DataAnnotations.Conditionals
{
    public sealed class UmbracoRegularExpressionIfAttribute : ConditionalValidationAttribute, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; }

        private readonly string pattern;
        protected override string ValidationName => "regularexpressionif";

        public UmbracoRegularExpressionIfAttribute(string pattern, string dependentProperty, object targetValue)
            : base(new RegularExpressionAttribute(pattern), dependentProperty, targetValue)
        {
            this.pattern = pattern;
        }

        protected override IDictionary<string, object> GetExtraValidationParameters()
        {
            return new Dictionary<string, object>
            {
                { "rule", "regex" },
                { "ruleparam", pattern }
            };
        }

        public override string FormatErrorMessage(string name)
        {
            base.ErrorMessage = UmbracoDictionary.GetDictionaryValue(DictionaryKey);
            return base.FormatErrorMessage(name);
        }
    }
}
