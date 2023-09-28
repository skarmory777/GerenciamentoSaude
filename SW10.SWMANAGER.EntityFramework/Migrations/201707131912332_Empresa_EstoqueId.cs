namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Empresa_EstoqueId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Empresa", "EstoqueId", c => c.Long());
            CreateIndex("dbo.Empresa", "EstoqueId");
            AddForeignKey("dbo.Empresa", "EstoqueId", "dbo.Est_Estoque", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Empresa", "EstoqueId", "dbo.Est_Estoque");
            DropIndex("dbo.Empresa", new[] { "EstoqueId" });
            DropColumn("dbo.Empresa", "EstoqueId");
        }
    }
}
