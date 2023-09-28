namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Inclusao_campos_Divisao_relacao_FormTipoResposta_PrescricaoMedica : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssFormTipoResposta", "SisMedicoId", "dbo.SisMedico");
            DropIndex("dbo.AssFormTipoResposta", new[] { "SisMedicoId" });
            AddColumn("dbo.AssDivisao", "IsQuantidade", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "IsUnidadeMedida", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "IsVelocidadeInfusao", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "IsDuracao", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "IsFormaAplicacao", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "IsFrequencia", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "IsUniddeOrganizacional", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "IsMedico", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "IsDataInicio", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "IsDiasAplicacao", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "IsObservacao", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "IsCopiarPrescricao", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "IsTipoMedicacao", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "IsExameImagem", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "IsExameLaboratorial", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "IsSetorExame", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "IsProdutoEstoque", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "IsControlaVolume", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "IsSangueDerivado", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "IsSeNecessario", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "IsUrgente", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "IsAgora", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "IsAcm", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssFormTipoResposta", "AssPrescricaoMedicaId", c => c.Long());
            AlterColumn("dbo.AssFormTipoResposta", "Quantidade", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.AssFormTipoResposta", "SisMedicoId", c => c.Long());
            AlterColumn("dbo.AssFormTipoResposta", "TotalDias", c => c.Int());
            CreateIndex("dbo.AssFormTipoResposta", "SisMedicoId");
            CreateIndex("dbo.AssFormTipoResposta", "AssPrescricaoMedicaId");
            AddForeignKey("dbo.AssFormTipoResposta", "AssPrescricaoMedicaId", "dbo.AssPrescricaoMedica", "Id");
            AddForeignKey("dbo.AssFormTipoResposta", "SisMedicoId", "dbo.SisMedico", "Id");
            DropColumn("dbo.AssFormTipoResposta", "DiaAtual");
        }

        public override void Down()
        {
            AddColumn("dbo.AssFormTipoResposta", "DiaAtual", c => c.Int(nullable: false));
            DropForeignKey("dbo.AssFormTipoResposta", "SisMedicoId", "dbo.SisMedico");
            DropForeignKey("dbo.AssFormTipoResposta", "AssPrescricaoMedicaId", "dbo.AssPrescricaoMedica");
            DropIndex("dbo.AssFormTipoResposta", new[] { "AssPrescricaoMedicaId" });
            DropIndex("dbo.AssFormTipoResposta", new[] { "SisMedicoId" });
            AlterColumn("dbo.AssFormTipoResposta", "TotalDias", c => c.Int(nullable: false));
            AlterColumn("dbo.AssFormTipoResposta", "SisMedicoId", c => c.Long(nullable: false));
            AlterColumn("dbo.AssFormTipoResposta", "Quantidade", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.AssFormTipoResposta", "AssPrescricaoMedicaId");
            DropColumn("dbo.AssDivisao", "IsAcm");
            DropColumn("dbo.AssDivisao", "IsAgora");
            DropColumn("dbo.AssDivisao", "IsUrgente");
            DropColumn("dbo.AssDivisao", "IsSeNecessario");
            DropColumn("dbo.AssDivisao", "IsSangueDerivado");
            DropColumn("dbo.AssDivisao", "IsControlaVolume");
            DropColumn("dbo.AssDivisao", "IsProdutoEstoque");
            DropColumn("dbo.AssDivisao", "IsSetorExame");
            DropColumn("dbo.AssDivisao", "IsExameLaboratorial");
            DropColumn("dbo.AssDivisao", "IsExameImagem");
            DropColumn("dbo.AssDivisao", "IsTipoMedicacao");
            DropColumn("dbo.AssDivisao", "IsCopiarPrescricao");
            DropColumn("dbo.AssDivisao", "IsObservacao");
            DropColumn("dbo.AssDivisao", "IsDiasAplicacao");
            DropColumn("dbo.AssDivisao", "IsDataInicio");
            DropColumn("dbo.AssDivisao", "IsMedico");
            DropColumn("dbo.AssDivisao", "IsUniddeOrganizacional");
            DropColumn("dbo.AssDivisao", "IsFrequencia");
            DropColumn("dbo.AssDivisao", "IsFormaAplicacao");
            DropColumn("dbo.AssDivisao", "IsDuracao");
            DropColumn("dbo.AssDivisao", "IsVelocidadeInfusao");
            DropColumn("dbo.AssDivisao", "IsUnidadeMedida");
            DropColumn("dbo.AssDivisao", "IsQuantidade");
            CreateIndex("dbo.AssFormTipoResposta", "SisMedicoId");
            AddForeignKey("dbo.AssFormTipoResposta", "SisMedicoId", "dbo.SisMedico", "Id", cascadeDelete: false);
        }
    }
}
