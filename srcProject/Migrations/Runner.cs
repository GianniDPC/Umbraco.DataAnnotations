#if NET || NETCOREAPP
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
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
    public class UmbracoDataAnnotationsComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            // Ensure IHttpContextAccessor is registered in the DI container.
            // HttpContextHelper will resolve it via the Migrations component Initialize().
            builder.Services.AddHttpContextAccessor();

            builder.Components().Append<Migrations>();
        }
    }
#else
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class UmbracoDataAnnotationsComposer : IComposer
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
        private IHttpContextAccessor _httpContextAccessor;

        public Migrations(ICoreScopeProvider scopeProvider, IMigrationPlanExecutor migrationPlanExecutor, IKeyValueService keyValueService, IRuntimeState runtimeState, IHttpContextAccessor httpContextAccessor)
        {
            _scopeProvider = scopeProvider;
            _migrationPlanExecutor = migrationPlanExecutor;
            _keyValueService = keyValueService;
            _runtimeState = runtimeState;
            _httpContextAccessor = httpContextAccessor;
        }
#elif NET5_0
        private IScopeProvider _scopeProvider;
        private IMigrationPlanExecutor _migrationPlanExecutor;
        private IKeyValueService _keyValueService;
        private IRuntimeState _runtimeState;
        private IHttpContextAccessor _httpContextAccessor;

        public Migrations(IScopeProvider scopeProvider, IMigrationPlanExecutor migrationPlanExecutor, IKeyValueService keyValueService, IRuntimeState runtimeState, IHttpContextAccessor httpContextAccessor)
        {
            _scopeProvider = scopeProvider;
            _migrationPlanExecutor = migrationPlanExecutor;
            _keyValueService = keyValueService;
            _runtimeState = runtimeState;
            _httpContextAccessor = httpContextAccessor;
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
            // Wire up the DI-managed IHttpContextAccessor into HttpContextHelper.
            // This replaces the broken static `new HttpContextAccessor()` pattern which
            // always returns null on threads previously used by background job schedulers.
            HttpContextHelper.SetHttpContextAccessor(_httpContextAccessor);

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
