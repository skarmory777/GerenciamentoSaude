namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alteracoes_Pos_merge_faturamento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatGrupo", "IsExame", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatGrupo", "IsEquipeMedicaVazia", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatGrupo", "IsTratamento", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatGrupo", "IsNaoAgrupaXml", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatGrupo", "IsDescontoCbhpm", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatGrupo", "IsTurno", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatGrupo", "IsMedicamento", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatGrupo", "IsOrteseProtese", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatSubGrupo", "Codigo", c => c.String(maxLength: 10));
        }

        public override void Down()
        {

            DropColumn("dbo.FatSubGrupo", "Codigo");
            DropColumn("dbo.FatGrupo", "IsOrteseProtese");
            DropColumn("dbo.FatGrupo", "IsMedicamento");
            DropColumn("dbo.FatGrupo", "IsTurno");
            DropColumn("dbo.FatGrupo", "IsDescontoCbhpm");
            DropColumn("dbo.FatGrupo", "IsNaoAgrupaXml");
            DropColumn("dbo.FatGrupo", "IsTratamento");
            DropColumn("dbo.FatGrupo", "IsEquipeMedicaVazia");
            DropColumn("dbo.FatGrupo", "IsExame");
        }
    }
}
