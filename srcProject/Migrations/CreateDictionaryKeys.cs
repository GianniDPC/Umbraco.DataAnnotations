using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations;

namespace Umbraco.DataAnnotations.Migrations
{
    public class CreateNotesTable : MigrationBase
    {
        private ILocalizationService _localizationService;

        public CreateNotesTable(IMigrationContext context, ILocalizationService localizationService) : base(context)
        {
            _localizationService = localizationService;
        }

        private readonly Dictionary<string, string> defaultDictionaryItems = new Dictionary<string, string>() {
            { "RequiredError", "{0} is required." },
            { "EqualToError", "The '{0}' and {1} fields must match." },
            { "EmailError", "Doesn't look like a valid e-mail." },
            { "MaxLengthError", "Must not exceed {1} characters." },
            { "MinLengthError", "Must not be less than {1} characters." },
            { "MinMaxLengthError", "Must not be less than {1} and greather than {1} characters." },
            { "MustBeTrueError", "{0} is required." },
            { "RangeError", "Please enter a number between {1} and {2}." },
            { "PasswordError", "Must be at least {1} characters long and contain {2} symbol (!, @, #, etc)." },
            { "MinPasswordLengthError", "Must be at least {1} characters." },
            { "MinNonAlphanumericCharactersError", "Must contain at leat {2} symbol (!, @, #, etc)." },
            { "PasswordStrengthError", "Must be at least {1} characters long and contain {2} symbol (!, @, #, etc)." },
        };

        protected override void Migrate()
        {
            var languages = _localizationService.GetAllLanguages();
            var language = languages.FirstOrDefault(x => x.IsoCode == "en-GB" || x.IsoCode == "en-US")
                ?? languages.FirstOrDefault();

            if (language == null)
            {
                return;
            }

            var dataAnnotations = _localizationService.GetDictionaryItemByKey("DataAnnotations");
            if (dataAnnotations != null)
            {
                return;
            }

            dataAnnotations = _localizationService.CreateDictionaryItemWithIdentity("DataAnnotations", null);

            foreach (var item in defaultDictionaryItems)
            {
                if (_localizationService.DictionaryItemExists(item.Key))
                    continue;

                _localizationService.CreateDictionaryItemWithIdentity(item.Key, dataAnnotations.Key, item.Value);
            }
        }
    }
}
