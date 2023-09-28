namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class relacao_prescricaoitem_formtiporesposta : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssFormTipoResposta", "AssFormaAplicacaoId", "dbo.AssFormaAplicacao");
            DropForeignKey("dbo.AssFormTipoResposta", "AssFrequenciaId", "dbo.AssFrequencia");
            DropForeignKey("dbo.AssFormTipoResposta", "SisUnidadeOrganizacionalId", "dbo.SisUnidadeOrganizacional");
            DropForeignKey("dbo.AssFormTipoResposta", "AssVelocidadeInfusaoId", "dbo.AssVelocidadeInfusao");
            DropIndex("dbo.AssFormTipoResposta", new[] { "AssVelocidadeInfusaoId" });
            DropIndex("dbo.AssFormTipoResposta", new[] { "AssFormaAplicacaoId" });
            DropIndex("dbo.AssFormTipoResposta", new[] { "AssFrequenciaId" });
            DropIndex("dbo.AssFormTipoResposta", new[] { "SisUnidadeOrganizacionalId" });
            AddColumn("dbo.AssFormTipoResposta", "AssPrescricaoItemId", c => c.Long(nullable: false));
            AlterColumn("dbo.AssFormTipoResposta", "AssVelocidadeInfusaoId", c => c.Long());
            AlterColumn("dbo.AssFormTipoResposta", "AssFormaAplicacaoId", c => c.Long());
            AlterColumn("dbo.AssFormTipoResposta", "AssFrequenciaId", c => c.Long());
            AlterColumn("dbo.AssFormTipoResposta", "SisUnidadeOrganizacionalId", c => c.Long());
            CreateIndex("dbo.AssFormTipoResposta", "AssVelocidadeInfusaoId");
            CreateIndex("dbo.AssFormTipoResposta", "AssFormaAplicacaoId");
            CreateIndex("dbo.AssFormTipoResposta", "AssFrequenciaId");
            CreateIndex("dbo.AssFormTipoResposta", "SisUnidadeOrganizacionalId");
            CreateIndex("dbo.AssFormTipoResposta", "AssPrescricaoItemId");
            AddForeignKey("dbo.AssFormTipoResposta", "AssPrescricaoItemId", "dbo.AssPrescricaoItem", "Id", cascadeDelete: false);
            AddForeignKey("dbo.AssFormTipoResposta", "AssFormaAplicacaoId", "dbo.AssFormaAplicacao", "Id");
            AddForeignKey("dbo.AssFormTipoResposta", "AssFrequenciaId", "dbo.AssFrequencia", "Id");
            AddForeignKey("dbo.AssFormTipoResposta", "SisUnidadeOrganizacionalId", "dbo.SisUnidadeOrganizacional", "Id");
            AddForeignKey("dbo.AssFormTipoResposta", "AssVelocidadeInfusaoId", "dbo.AssVelocidadeInfusao", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AssFormTipoResposta", "AssVelocidadeInfusaoId", "dbo.AssVelocidadeInfusao");
            DropForeignKey("dbo.AssFormTipoResposta", "SisUnidadeOrganizacionalId", "dbo.SisUnidadeOrganizacional");
            DropForeignKey("dbo.AssFormTipoResposta", "AssFrequenciaId", "dbo.AssFrequencia");
            DropForeignKey("dbo.AssFormTipoResposta", "AssFormaAplicacaoId", "dbo.AssFormaAplicacao");
            DropForeignKey("dbo.AssFormTipoResposta", "AssPrescricaoItemId", "dbo.AssPrescricaoItem");
            DropIndex("dbo.AssFormTipoResposta", new[] { "AssPrescricaoItemId" });
            DropIndex("dbo.AssFormTipoResposta", new[] { "SisUnidadeOrganizacionalId" });
            DropIndex("dbo.AssFormTipoResposta", new[] { "AssFrequenciaId" });
            DropIndex("dbo.AssFormTipoResposta", new[] { "AssFormaAplicacaoId" });
            DropIndex("dbo.AssFormTipoResposta", new[] { "AssVelocidadeInfusaoId" });
            AlterColumn("dbo.AssFormTipoResposta", "SisUnidadeOrganizacionalId", c => c.Long(nullable: false));
            AlterColumn("dbo.AssFormTipoResposta", "AssFrequenciaId", c => c.Long(nullable: false));
            AlterColumn("dbo.AssFormTipoResposta", "AssFormaAplicacaoId", c => c.Long(nullable: false));
            AlterColumn("dbo.AssFormTipoResposta", "AssVelocidadeInfusaoId", c => c.Long(nullable: false));
            DropColumn("dbo.AssFormTipoResposta", "AssPrescricaoItemId");
            CreateIndex("dbo.AssFormTipoResposta", "SisUnidadeOrganizacionalId");
            CreateIndex("dbo.AssFormTipoResposta", "AssFrequenciaId");
            CreateIndex("dbo.AssFormTipoResposta", "AssFormaAplicacaoId");
            CreateIndex("dbo.AssFormTipoResposta", "AssVelocidadeInfusaoId");
            AddForeignKey("dbo.AssFormTipoResposta", "AssVelocidadeInfusaoId", "dbo.AssVelocidadeInfusao", "Id", cascadeDelete: false);
            AddForeignKey("dbo.AssFormTipoResposta", "SisUnidadeOrganizacionalId", "dbo.SisUnidadeOrganizacional", "Id", cascadeDelete: false);
            AddForeignKey("dbo.AssFormTipoResposta", "AssFrequenciaId", "dbo.AssFrequencia", "Id", cascadeDelete: false);
            AddForeignKey("dbo.AssFormTipoResposta", "AssFormaAplicacaoId", "dbo.AssFormaAplicacao", "Id", cascadeDelete: false);
        }
    }
}
