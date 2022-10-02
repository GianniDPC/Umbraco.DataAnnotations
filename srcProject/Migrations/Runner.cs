using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;

namespace Umbraco.DataAnnotations.Migrations
{
    public class MigrationRunnerComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Components().Append<Migrations>();
        }
    }

    public class Migrations : IComponent
    {
        private ICoreScopeProvider _scopeProvider;
        private IMigrationPlanExecutor _migrationPlanExecutor;
        private IKeyValueService _keyValueService;
        private IRuntimeState _runtimeState;

        public Migrations(ICoreScopeProvider scopeProvider, IMigrationPlanExecutor migrationPlanExecutor, IKeyValueService keyValueService, IRuntimeState runtimeState)
        {
            _scopeProvider = scopeProvider;
            _migrationPlanExecutor = migrationPlanExecutor;
            _keyValueService = keyValueService;
            _runtimeState = runtimeState;
        }

        public void Initialize()
        {
            if (_runtimeState.Level < RuntimeLevel.Run)
            {
                return;
            }

            HandleMigrations();
        }

        private void HandleMigrations()
        {
            const string productName = Constants.PluginName;

            var migrationPlan = new MigrationPlan(productName);
            migrationPlan.From(string.Empty).To<CreateNotesTable>("add-dictionary-keys");

            var upgrader = new Upgrader(migrationPlan);
            upgrader.Execute(_migrationPlanExecutor, _scopeProvider, _keyValueService);
        }

        public void Terminate() { }
    }
}
