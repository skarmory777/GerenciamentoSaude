namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class FatGrupo_AjusteCampos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatGrupo", "IsObrigaMedico", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatGrupo", "IsTaxaUrgencia", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatGrupo", "IsPediatria", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatGrupo", "IsProcedimentoSerie", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatGrupo", "IsRequisicaoExame", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatGrupo", "IsPermiteRevisao", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatGrupo", "IsPrecoManual", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatGrupo", "IsAutorizacao", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatGrupo", "IsInternacao", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatGrupo", "IsAmbulatorio", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatGrupo", "IsCirurgia", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatGrupo", "IsPorte", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatGrupo", "IsConsultor", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatGrupo", "IsPlantonista", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatGrupo", "IsExtraCaixa", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.FatGrupo", "IsExtraCaixa");
            DropColumn("dbo.FatGrupo", "IsPlantonista");
            DropColumn("dbo.FatGrupo", "IsConsultor");
            DropColumn("dbo.FatGrupo", "IsPorte");
            DropColumn("dbo.FatGrupo", "IsCirurgia");
            DropColumn("dbo.FatGrupo", "IsAmbulatorio");
            DropColumn("dbo.FatGrupo", "IsInternacao");
            DropColumn("dbo.FatGrupo", "IsAutorizacao");
            DropColumn("dbo.FatGrupo", "IsPrecoManual");
            DropColumn("dbo.FatGrupo", "IsPermiteRevisao");
            DropColumn("dbo.FatGrupo", "IsRequisicaoExame");
            DropColumn("dbo.FatGrupo", "IsProcedimentoSerie");
            DropColumn("dbo.FatGrupo", "IsPediatria");
            DropColumn("dbo.FatGrupo", "IsTaxaUrgencia");
            DropColumn("dbo.FatGrupo", "IsObrigaMedico");
        }
    }
}
