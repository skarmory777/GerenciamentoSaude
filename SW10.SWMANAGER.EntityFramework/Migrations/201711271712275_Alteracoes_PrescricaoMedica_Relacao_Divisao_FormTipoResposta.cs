namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alteracoes_PrescricaoMedica_Relacao_Divisao_FormTipoResposta : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssFormTipoResposta", "AssPrescricaoItemId", "dbo.AssPrescricaoItem");
            DropIndex("dbo.AssFormTipoResposta", new[] { "AssPrescricaoItemId" });
            AddColumn("dbo.AssFormTipoResposta", "AssDivisaoId", c => c.Long());
            AddColumn("dbo.AssPrescricaoMedica", "SisUnidadeOrganizacionalId", c => c.Long());
            AddColumn("dbo.AssPrescricaoMedica", "AteAtendimentoId", c => c.Long());
            AddColumn("dbo.AssPrescricaoMedica", "SisPrestadorId", c => c.Long());
            AddColumn("dbo.AssPrescricaoMedica", "Observacao", c => c.String());
            AlterColumn("dbo.AssFormTipoResposta", "AssPrescricaoItemId", c => c.Long());
            CreateIndex("dbo.AssFormTipoResposta", "AssPrescricaoItemId");
            CreateIndex("dbo.AssFormTipoResposta", "AssDivisaoId");
            CreateIndex("dbo.AssPrescricaoMedica", "SisUnidadeOrganizacionalId");
            CreateIndex("dbo.AssPrescricaoMedica", "AteAtendimentoId");
            CreateIndex("dbo.AssPrescricaoMedica", "SisPrestadorId");
            AddForeignKey("dbo.AssFormTipoResposta", "AssDivisaoId", "dbo.AssDivisao", "Id");
            AddForeignKey("dbo.AssPrescricaoMedica", "AteAtendimentoId", "dbo.AteAtendimento", "Id");
            AddForeignKey("dbo.AssPrescricaoMedica", "SisPrestadorId", "dbo.SisPrestador", "Id");
            AddForeignKey("dbo.AssPrescricaoMedica", "SisUnidadeOrganizacionalId", "dbo.SisUnidadeOrganizacional", "Id");
            AddForeignKey("dbo.AssFormTipoResposta", "AssPrescricaoItemId", "dbo.AssPrescricaoItem", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AssFormTipoResposta", "AssPrescricaoItemId", "dbo.AssPrescricaoItem");
            DropForeignKey("dbo.AssPrescricaoMedica", "SisUnidadeOrganizacionalId", "dbo.SisUnidadeOrganizacional");
            DropForeignKey("dbo.AssPrescricaoMedica", "SisPrestadorId", "dbo.SisPrestador");
            DropForeignKey("dbo.AssPrescricaoMedica", "AteAtendimentoId", "dbo.AteAtendimento");
            DropForeignKey("dbo.AssFormTipoResposta", "AssDivisaoId", "dbo.AssDivisao");
            DropIndex("dbo.AssPrescricaoMedica", new[] { "SisPrestadorId" });
            DropIndex("dbo.AssPrescricaoMedica", new[] { "AteAtendimentoId" });
            DropIndex("dbo.AssPrescricaoMedica", new[] { "SisUnidadeOrganizacionalId" });
            DropIndex("dbo.AssFormTipoResposta", new[] { "AssDivisaoId" });
            DropIndex("dbo.AssFormTipoResposta", new[] { "AssPrescricaoItemId" });
            AlterColumn("dbo.AssFormTipoResposta", "AssPrescricaoItemId", c => c.Long(nullable: false));
            DropColumn("dbo.AssPrescricaoMedica", "Observacao");
            DropColumn("dbo.AssPrescricaoMedica", "SisPrestadorId");
            DropColumn("dbo.AssPrescricaoMedica", "AteAtendimentoId");
            DropColumn("dbo.AssPrescricaoMedica", "SisUnidadeOrganizacionalId");
            DropColumn("dbo.AssFormTipoResposta", "AssDivisaoId");
            CreateIndex("dbo.AssFormTipoResposta", "AssPrescricaoItemId");
            AddForeignKey("dbo.AssFormTipoResposta", "AssPrescricaoItemId", "dbo.AssPrescricaoItem", "Id", cascadeDelete: false);
        }
    }
}
