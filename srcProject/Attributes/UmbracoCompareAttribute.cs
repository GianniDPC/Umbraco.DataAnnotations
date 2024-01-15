#if NET || NETCOREAPP
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
#else
using System.Web.Mvc;
#endif
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
#if NET || NETCOREAPP

    public sealed class UmbracoCompareAttribute : CompareAttribute, IUmbracoValidationAttribute, IClientModelValidator
#else
    public sealed class UmbracoCompareAttribute : System.ComponentModel.DataAnnotations.CompareAttribute, IUmbracoValidationAttribute, IClientValidatable
#endif
    {
        public string DictionaryKey { get; set; } = "EqualToError";
        public new string ErrorMessageString { get; set; }
        public new string OtherPropertyDisplayName { get; set; }

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

#if NET || NETCOREAPP
        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            ErrorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());

            if (context.ModelMetadata.ContainerType != null)
            {
                if (OtherPropertyDisplayName == null)
                {
                    var otherPropertyMetadata = context.MetadataProvider.GetMetadataForProperty(context.ModelMetadata.ContainerType, OtherProperty);
                    OtherPropertyDisplayName = otherPropertyMetadata.GetDisplayName();
                }
            }

            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-equalto", ErrorMessage);
            context.Attributes.Add("data-val-equalto-other", $"*.{OtherProperty}");
        }
#else
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ErrorMessageString = UmbracoDictionary.GetDictionaryValue(DictionaryKey);

            if (metadata.ContainerType != null)
            {
                if (OtherPropertyDisplayName == null)
                {
                    var otherPropertyMetadata = ModelMetadataProviders.Current.GetMetadataForProperty(() => metadata.Model, metadata.ContainerType, OtherProperty);
                    OtherPropertyDisplayName = otherPropertyMetadata.GetDisplayName();
                }
            }

            yield return new ModelClientValidationEqualToRule(FormatErrorMessage(metadata.GetDisplayName()), OtherProperty);
        }
#endif
    }
}
