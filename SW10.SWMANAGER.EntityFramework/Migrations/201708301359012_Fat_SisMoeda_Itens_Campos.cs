namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Fat_SisMoeda_Itens_Campos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatItem", "DivideBrasindice", c => c.Single(nullable: false));
            AddColumn("dbo.FatItemTabela", "Codigo", c => c.String());
            AddColumn("dbo.FatItemTabela", "Descricao", c => c.String());
            AddColumn("dbo.FatItemTabela", "SisMoedaId", c => c.Long(nullable: false));
            AddColumn("dbo.FatItemTabela", "COCH", c => c.Single(nullable: false));
            AddColumn("dbo.FatItemTabela", "HMCH", c => c.Single(nullable: false));
            AddColumn("dbo.FatItemTabela", "Auxiliar", c => c.Int(nullable: false));
            AddColumn("dbo.FatItemTabela", "Porte", c => c.Int(nullable: false));
            AddColumn("dbo.FatItemTabela", "Filme", c => c.Single(nullable: false));
            AddColumn("dbo.FatItemTabela", "Preco", c => c.Single(nullable: false));
            AddColumn("dbo.FatItemTabela", "IsInclusaoManual", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisMoedaCotacao", "IsTodosConvenio", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisMoeda", "IsCobraCoch", c => c.Boolean(nullable: false));
            CreateIndex("dbo.FatItemTabela", "SisMoedaId");
            AddForeignKey("dbo.FatItemTabela", "SisMoedaId", "dbo.SisMoeda", "Id", cascadeDelete: false);
            DropColumn("dbo.FatItemTabela", "ValorHonorario");
            DropColumn("dbo.FatItemTabela", "ValorOperacional");
        }

        public override void Down()
        {
            AddColumn("dbo.FatItemTabela", "ValorOperacional", c => c.Single(nullable: false));
            AddColumn("dbo.FatItemTabela", "ValorHonorario", c => c.Single(nullable: false));
            DropForeignKey("dbo.FatItemTabela", "SisMoedaId", "dbo.SisMoeda");
            DropIndex("dbo.FatItemTabela", new[] { "SisMoedaId" });
            DropColumn("dbo.SisMoeda", "IsCobraCoch");
            DropColumn("dbo.SisMoedaCotacao", "IsTodosConvenio");
            DropColumn("dbo.FatItemTabela", "IsInclusaoManual");
            DropColumn("dbo.FatItemTabela", "Preco");
            DropColumn("dbo.FatItemTabela", "Filme");
            DropColumn("dbo.FatItemTabela", "Porte");
            DropColumn("dbo.FatItemTabela", "Auxiliar");
            DropColumn("dbo.FatItemTabela", "HMCH");
            DropColumn("dbo.FatItemTabela", "COCH");
            DropColumn("dbo.FatItemTabela", "SisMoedaId");
            DropColumn("dbo.FatItemTabela", "Descricao");
            DropColumn("dbo.FatItemTabela", "Codigo");
            DropColumn("dbo.FatItem", "DivideBrasindice");
        }
    }
}
