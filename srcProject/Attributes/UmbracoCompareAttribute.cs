using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Umbraco.DataAnnotations.Interfaces;

namespace Umbraco.DataAnnotations.Attributes
{
    /// <summary>
    /// Specifies that two properties data field value must match.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class UmbracoCompareAttribute : CompareAttribute, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; } = "EqualToError";

        public UmbracoCompareAttribute(string otherProperty)
            : base(otherProperty)
        {

        }

        private static ICustomTypeDescriptor GetTypeDescriptor(Type type)
        {
            return new AssociatedMetadataTypeTypeDescriptionProvider(type).GetTypeDescriptor(type);
        }

        private static string GetDisplayNameForProperty(Type containerType, string propertyName)
        {
            ICustomTypeDescriptor typeDescriptor = GetTypeDescriptor(containerType);
            PropertyDescriptor property = typeDescriptor.GetProperties().Find(propertyName, true);

            if (property == null)
            {
                throw new ArgumentNullException(propertyName);
            }

            IEnumerable<Attribute> attributes = property.Attributes.Cast<Attribute>();
            DisplayAttribute display = attributes.OfType<DisplayAttribute>().FirstOrDefault();
            if (display != null)
            {
                return display.GetName();
            }

            DisplayNameAttribute displayName = attributes.OfType<DisplayNameAttribute>().FirstOrDefault();
            if (displayName != null)
            {
                return displayName.DisplayName;
            }
            return propertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Workaround because otherPropertyDisplayName was always equal to otherPropertyName instead
            var displayName = GetDisplayNameForProperty(validationContext.ObjectType, OtherProperty);
            var otherPropertyDisplayName = typeof(UmbracoCompareAttribute).GetProperty(nameof(OtherPropertyDisplayName));
            otherPropertyDisplayName.SetValue(this, displayName);

            return base.IsValid(value, validationContext);
        }

        public override string FormatErrorMessage(string name)
        {
            ErrorMessage = UmbracoDictionary.GetDictionaryValue(DictionaryKey);
            return base.FormatErrorMessage(name);
        }
    }
}
