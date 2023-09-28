namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Inclusao_Relacionamentos_FormTipoResposta : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AssFormTipoResposta", name: "VelocidadeInfusaoId", newName: "AssVelocidadeInfusaoId");
            RenameColumn(table: "dbo.AssFormTipoResposta", name: "FrequenciaId", newName: "AssFrequenciaId");
            RenameColumn(table: "dbo.AssFormTipoResposta", name: "UnidadeOrganizacionalId", newName: "SisUnidadeOrganizacionalId");
            RenameColumn(table: "dbo.AssFormTipoResposta", name: "MedicoId", newName: "SisMedicoId");
            CreateTable(
                "dbo.AssFormaAplicacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
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
                    { "DynamicFilter_FormaAplicacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AssVelocidadeInfusao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
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
                    { "DynamicFilter_VelocidadeInfusao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.AssFormTipoResposta", "AssFormaAplicacaoId", c => c.Long(nullable: false));
            CreateIndex("dbo.AssFormTipoResposta", "AssVelocidadeInfusaoId");
            CreateIndex("dbo.AssFormTipoResposta", "AssFormaAplicacaoId");
            CreateIndex("dbo.AssFormTipoResposta", "AssFrequenciaId");
            CreateIndex("dbo.AssFormTipoResposta", "SisUnidadeOrganizacionalId");
            CreateIndex("dbo.AssFormTipoResposta", "SisMedicoId");
            AddForeignKey("dbo.AssFormTipoResposta", "AssFormaAplicacaoId", "dbo.AssFormaAplicacao", "Id", cascadeDelete: false);
            AddForeignKey("dbo.AssFormTipoResposta", "AssFrequenciaId", "dbo.AssFrequencia", "Id", cascadeDelete: false);
            AddForeignKey("dbo.AssFormTipoResposta", "SisMedicoId", "dbo.Medico", "Id", cascadeDelete: false);
            AddForeignKey("dbo.AssFormTipoResposta", "SisUnidadeOrganizacionalId", "dbo.UnidadeOrganizacional", "Id", cascadeDelete: false);
            AddForeignKey("dbo.AssFormTipoResposta", "AssVelocidadeInfusaoId", "dbo.AssVelocidadeInfusao", "Id", cascadeDelete: false);
            DropColumn("dbo.AssFormTipoResposta", "AplicacaoId");
        }

        public override void Down()
        {
            AddColumn("dbo.AssFormTipoResposta", "AplicacaoId", c => c.Long(nullable: false));
            DropForeignKey("dbo.AssFormTipoResposta", "AssVelocidadeInfusaoId", "dbo.AssVelocidadeInfusao");
            DropForeignKey("dbo.AssFormTipoResposta", "SisUnidadeOrganizacionalId", "dbo.UnidadeOrganizacional");
            DropForeignKey("dbo.AssFormTipoResposta", "SisMedicoId", "dbo.Medico");
            DropForeignKey("dbo.AssFormTipoResposta", "AssFrequenciaId", "dbo.AssFrequencia");
            DropForeignKey("dbo.AssFormTipoResposta", "AssFormaAplicacaoId", "dbo.AssFormaAplicacao");
            DropIndex("dbo.AssFormTipoResposta", new[] { "SisMedicoId" });
            DropIndex("dbo.AssFormTipoResposta", new[] { "SisUnidadeOrganizacionalId" });
            DropIndex("dbo.AssFormTipoResposta", new[] { "AssFrequenciaId" });
            DropIndex("dbo.AssFormTipoResposta", new[] { "AssFormaAplicacaoId" });
            DropIndex("dbo.AssFormTipoResposta", new[] { "AssVelocidadeInfusaoId" });
            DropColumn("dbo.AssFormTipoResposta", "AssFormaAplicacaoId");
            DropTable("dbo.AssVelocidadeInfusao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_VelocidadeInfusao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssFormaAplicacao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FormaAplicacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            RenameColumn(table: "dbo.AssFormTipoResposta", name: "SisMedicoId", newName: "MedicoId");
            RenameColumn(table: "dbo.AssFormTipoResposta", name: "SisUnidadeOrganizacionalId", newName: "UnidadeOrganizacionalId");
            RenameColumn(table: "dbo.AssFormTipoResposta", name: "AssFrequenciaId", newName: "FrequenciaId");
            RenameColumn(table: "dbo.AssFormTipoResposta", name: "AssVelocidadeInfusaoId", newName: "VelocidadeInfusaoId");
        }
    }
}
