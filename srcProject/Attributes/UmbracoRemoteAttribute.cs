using Microsoft.AspNetCore.Mvc;
using System;
using Umbraco.DataAnnotations.Interfaces;

namespace Umbraco.DataAnnotations.Attributes
{
    /// <summary>
    /// A <see cref="RemoteAttributeBase"/> for controllers which configures Unobtrusive validation to send an Ajax request to the
    /// web site. The invoked action should return JSON indicating whether the value is valid.    
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class UmbracoRemoteAttribute : RemoteAttribute, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; }

        public UmbracoRemoteAttribute(string routeName)
            : base(routeName)
        {
        }

        public UmbracoRemoteAttribute(string action, string controller)
            : base(action, controller)
        {

        }

        public UmbracoRemoteAttribute(string action, string controller, string areaName)
            : base(action, controller, areaName)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            ErrorMessage = UmbracoDictionary.GetDictionaryValue(DictionaryKey);
            return base.FormatErrorMessage(name);
        }
    }
}