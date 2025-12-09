using System.Linq;
using System.Threading;

#if NET || NETCOREAPP
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Services;
#else
using Umbraco.Web;
using Umbraco.Web.Composing;
#endif

namespace Umbraco.DataAnnotations
{
    public sealed class UmbracoDictionary
    {
        public static string GetDictionaryValue(string dictionaryKey)
        {
#if NET10_0_OR_GREATER
            var dictionaryItemService = HttpContextHelper.Current.RequestServices.GetService<IDictionaryItemService>();
            var dictItem = dictionaryItemService?.GetAsync(dictionaryKey).GetAwaiter().GetResult();
#elif NET5_0_OR_GREATER
            var localizationService = HttpContextHelper.Current.RequestServices.GetService<ILocalizationService>();
            var dictItem = localizationService.GetDictionaryItemByKey(dictionaryKey);
#else
            var dictItem = Current.Services.LocalizationService.GetDictionaryItemByKey(dictionaryKey);
#endif

            string key = "";
            if (dictItem != null)
            {
#if NET8_0_OR_GREATER
                key = dictItem.Translations.
                    FirstOrDefault(x => x.LanguageIsoCode == Thread.CurrentThread.CurrentCulture.Name)?.Value;
#elif NET5_0_OR_GREATER
                key = dictItem.Translations.
                    FirstOrDefault(x => x.Language.CultureInfo.Name == Thread.CurrentThread.CurrentCulture.Name)?.Value;
#else
                key = dictItem.Translations.
                    FirstOrDefault(x => x.Language.CultureInfo.Name == Thread.CurrentThread.CurrentCulture.Name)?.Value;
#endif
            }

            if (!string.IsNullOrEmpty(key))
                return key;
            return string.Format(dictionaryKey);
        }
    }
}