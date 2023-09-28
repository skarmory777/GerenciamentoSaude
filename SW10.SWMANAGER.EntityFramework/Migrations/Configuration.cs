using Abp.Events.Bus;
using Abp.Events.Bus.Entities;
using Abp.MultiTenancy;
using Abp.Zero.EntityFramework;
using EntityFramework.DynamicFilters;
using SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao;
using SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao.Imagens;
using SW10.SWMANAGER.Migrations.Seed.ClasseAplicacao.TISS;
using SW10.SWMANAGER.Migrations.Seed.Host;
using SW10.SWMANAGER.Migrations.Seed.Tenants;
using System;
using System.Data.Entity.Migrations;

namespace SW10.SWMANAGER.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<EntityFramework.SWMANAGERDbContext>, IMultiTenantSeed
    {
        public AbpTenantBase Tenant { get; set; }

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "SWMANAGER";
        }

        protected override void Seed(EntityFramework.SWMANAGERDbContext context)
        {
            //Para habilitar digite no Package Manager Console antes de executar Update-database
            //PM> $Env:DEBUG_SEED = "true"
            if ("true".Equals(Environment.GetEnvironmentVariable("DEBUG_SEED")))
                if (System.Diagnostics.Debugger.IsAttached == false)
                    System.Diagnostics.Debugger.Launch();

            context.DisableAllFilters();

            context.EntityChangeEventHelper = NullEntityChangeEventHelper.Instance;
            context.EventBus = NullEventBus.Instance;

            if (Tenant == null)
            {
                //Host seed
                new InitialHostDbBuilder(context).Create();

                //Default tenant seed (in host database).
                new DefaultTenantBuilder(context).Create();
                new TenantRoleAndUserBuilder(context, 1).Create();
            }
            else
            {
                //You can add seed for tenant databases using Tenant property...
            }
            new ConteudoFixoBuilder(context).Create();
            new SeedSuprimentos(context).Create();
            new SeedAssistenciais(context).Create();
            new SeedImagens(context).Create();
            new SeedTISS(context).Create();

            //new SeedModeloTextoBuilder(context).Create();
            context.SaveChanges();
        }
    }
}
