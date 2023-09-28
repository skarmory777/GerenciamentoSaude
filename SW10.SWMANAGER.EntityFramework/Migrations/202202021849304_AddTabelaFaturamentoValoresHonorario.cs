namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddTabelaFaturamentoValoresHonorario : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FaturamentoValoresHonorarios",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PercentualMedico = c.Single(nullable: false),
                        PercentualAuxiliar1 = c.Single(nullable: false),
                        PercentualAuxiliar2 = c.Single(nullable: false),
                        PercentualAuxiliar3 = c.Single(nullable: false),
                        PercentualInstrumentador = c.Single(nullable: false),
                        PercentualAnestesista = c.Single(nullable: false),
                        IsSistema = c.Boolean(nullable: false),
                        Codigo = c.String(maxLength: 10),
                        Descricao = c.String(),
                        ImportaId = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoValoresHonorario_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FaturamentoValoresHonorarios",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoValoresHonorario_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
