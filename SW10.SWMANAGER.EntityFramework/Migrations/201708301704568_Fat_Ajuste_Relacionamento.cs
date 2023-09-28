namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Fat_Ajuste_Relacionamento : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FatItem", "GrupoId", "dbo.FatGrupo");
            DropForeignKey("dbo.FatItemTabela", "ItemId", "dbo.FatItem");
            DropForeignKey("dbo.FatItemTabela", "SisMoedaId", "dbo.SisMoeda");
            DropForeignKey("dbo.FatItemTabela", "TabelaId", "dbo.FatTabela");
            DropIndex("dbo.FatItem", new[] { "GrupoId" });
            DropIndex("dbo.FatItemTabela", new[] { "TabelaId" });
            DropIndex("dbo.FatItemTabela", new[] { "ItemId" });
            DropIndex("dbo.FatItemTabela", new[] { "SisMoedaId" });
            AlterColumn("dbo.FatItem", "GrupoId", c => c.Long());
            AlterColumn("dbo.FatItemTabela", "TabelaId", c => c.Long());
            AlterColumn("dbo.FatItemTabela", "ItemId", c => c.Long());
            AlterColumn("dbo.FatItemTabela", "SisMoedaId", c => c.Long());
            CreateIndex("dbo.FatItem", "GrupoId");
            CreateIndex("dbo.FatItemTabela", "TabelaId");
            CreateIndex("dbo.FatItemTabela", "ItemId");
            CreateIndex("dbo.FatItemTabela", "SisMoedaId");
            AddForeignKey("dbo.FatItem", "GrupoId", "dbo.FatGrupo", "Id");
            AddForeignKey("dbo.FatItemTabela", "ItemId", "dbo.FatItem", "Id");
            AddForeignKey("dbo.FatItemTabela", "SisMoedaId", "dbo.SisMoeda", "Id");
            AddForeignKey("dbo.FatItemTabela", "TabelaId", "dbo.FatTabela", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.FatItemTabela", "TabelaId", "dbo.FatTabela");
            DropForeignKey("dbo.FatItemTabela", "SisMoedaId", "dbo.SisMoeda");
            DropForeignKey("dbo.FatItemTabela", "ItemId", "dbo.FatItem");
            DropForeignKey("dbo.FatItem", "GrupoId", "dbo.FatGrupo");
            DropIndex("dbo.FatItemTabela", new[] { "SisMoedaId" });
            DropIndex("dbo.FatItemTabela", new[] { "ItemId" });
            DropIndex("dbo.FatItemTabela", new[] { "TabelaId" });
            DropIndex("dbo.FatItem", new[] { "GrupoId" });
            AlterColumn("dbo.FatItemTabela", "SisMoedaId", c => c.Long(nullable: false));
            AlterColumn("dbo.FatItemTabela", "ItemId", c => c.Long(nullable: false));
            AlterColumn("dbo.FatItemTabela", "TabelaId", c => c.Long(nullable: false));
            AlterColumn("dbo.FatItem", "GrupoId", c => c.Long(nullable: false));
            CreateIndex("dbo.FatItemTabela", "SisMoedaId");
            CreateIndex("dbo.FatItemTabela", "ItemId");
            CreateIndex("dbo.FatItemTabela", "TabelaId");
            CreateIndex("dbo.FatItem", "GrupoId");
            AddForeignKey("dbo.FatItemTabela", "TabelaId", "dbo.FatTabela", "Id", cascadeDelete: true);
            AddForeignKey("dbo.FatItemTabela", "SisMoedaId", "dbo.SisMoeda", "Id", cascadeDelete: true);
            AddForeignKey("dbo.FatItemTabela", "ItemId", "dbo.FatItem", "Id", cascadeDelete: true);
            AddForeignKey("dbo.FatItem", "GrupoId", "dbo.FatGrupo", "Id", cascadeDelete: true);
        }
    }
}
