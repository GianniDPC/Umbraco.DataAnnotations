#if NET || NETCOREAPP
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;
#else
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations.Upgrade;
using Umbraco.Core.Migrations;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;
#endif

namespace Umbraco.DataAnnotations.Migrations
{

#if NET || NETCOREAPP
    public class MigrationRunnerComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Components().Append<Migrations>();
        }
    }
#else
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class MigrationRunnerComposer : IComposer
    {
        public void Compose(Composition composition)
        {
            composition.Components().Append<Migrations>();
        }
    }
#endif

    public class Migrations : IComponent
    {
#if NET6_0_OR_GREATER
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
#elif NET5_0
        private IScopeProvider _scopeProvider;
        private IMigrationPlanExecutor _migrationPlanExecutor;
        private IKeyValueService _keyValueService;
        private IRuntimeState _runtimeState;

        public Migrations(IScopeProvider scopeProvider, IMigrationPlanExecutor migrationPlanExecutor, IKeyValueService keyValueService, IRuntimeState runtimeState)
        {
            _scopeProvider = scopeProvider;
            _migrationPlanExecutor = migrationPlanExecutor;
            _keyValueService = keyValueService;
            _runtimeState = runtimeState;
        }
#else
        private IScopeProvider _scopeProvider;
        private IMigrationBuilder _migrationBuilder;
        private IKeyValueService _keyValueService;
        private ILogger _logger;
        public Migrations(IScopeProvider scopeProvider,
            IMigrationBuilder migrationBuilder, IKeyValueService keyValueService, ILogger logger)
        {
            _scopeProvider = scopeProvider;
            _migrationBuilder = migrationBuilder;
            _keyValueService = keyValueService;
            _logger = logger;
        }
#endif

        public void Initialize()
        {
#if NET || NETCOREAPP
            if (_runtimeState.Level < RuntimeLevel.Run)
            {
                return;
            }
#endif
            HandleMigrations();
        }

        private void HandleMigrations()
        {
            const string productName = Constants.PluginName;

            var migrationPlan = new MigrationPlan(productName);
            migrationPlan.From(string.Empty).To<CreateNotesTable>("add-dictionary-keys");

            var upgrader = new Upgrader(migrationPlan);

#if NET || NETCOREAPP
            upgrader.Execute(_migrationPlanExecutor, _scopeProvider, _keyValueService);
#else
            upgrader.Execute(_scopeProvider, _migrationBuilder, _keyValueService, _logger);
#endif
        }

        public void Terminate() { }
    }
}
