namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InclusaoCampoEmUnidadeOrganizacional : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Est_Estoque", "SetorId", "dbo.UnidadeOrganizacional");
            DropIndex("dbo.Est_Estoque", new[] { "SetorId" });
            AddColumn("dbo.SisUnidadeOrganizacional", "EstEstoqueId", c => c.Long());
            CreateIndex("dbo.SisUnidadeOrganizacional", "EstEstoqueId");
            AddForeignKey("dbo.SisUnidadeOrganizacional", "EstEstoqueId", "dbo.Est_Estoque", "Id");
            DropColumn("dbo.Est_Estoque", "SetorId");
        }

        public override void Down()
        {
            AddColumn("dbo.Est_Estoque", "SetorId", c => c.Long());
            DropForeignKey("dbo.SisUnidadeOrganizacional", "EstEstoqueId", "dbo.Est_Estoque");
            DropIndex("dbo.SisUnidadeOrganizacional", new[] { "EstEstoqueId" });
            DropColumn("dbo.SisUnidadeOrganizacional", "EstEstoqueId");
            CreateIndex("dbo.Est_Estoque", "SetorId");
            AddForeignKey("dbo.Est_Estoque", "SetorId", "dbo.SisUnidadeOrganizacional", "Id");
        }
    }
}
