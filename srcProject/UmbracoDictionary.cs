using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading;
using Umbraco.Cms.Core.Services;

namespace Umbraco.DataAnnotations
{
    public sealed class UmbracoDictionary
    {
        public static string GetDictionaryValue(string dictionaryKey)
        {
            var localizationService = HttpContextHelper.Current.RequestServices.GetService<ILocalizationService>();
            var dictItem = localizationService.GetDictionaryItemByKey(dictionaryKey);

            string key = "";
            if (dictItem != null)
            {
                key = dictItem.Translations.
                    FirstOrDefault(x => x.Language.CultureInfo.Name == Thread.CurrentThread.CurrentCulture.Name)?.Value;
            }

            if (!string.IsNullOrEmpty(key))
                return key;
            return string.Format(dictionaryKey);
        }
    }
}