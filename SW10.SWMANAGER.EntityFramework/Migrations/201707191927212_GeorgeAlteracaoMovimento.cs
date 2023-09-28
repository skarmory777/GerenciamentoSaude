namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GeorgeAlteracaoMovimento : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EstoqueMovimento", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.EstoquePreMovimento", "EmpresaId", "dbo.Empresa");
            DropIndex("dbo.EstoqueMovimento", new[] { "EmpresaId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "EmpresaId" });
            AlterColumn("dbo.EstoqueMovimento", "EmpresaId", c => c.Long());
            AlterColumn("dbo.EstoquePreMovimento", "EmpresaId", c => c.Long());
            CreateIndex("dbo.EstoqueMovimento", "EmpresaId");
            CreateIndex("dbo.EstoquePreMovimento", "EmpresaId");
            AddForeignKey("dbo.EstoqueMovimento", "EmpresaId", "dbo.Empresa", "Id");
            AddForeignKey("dbo.EstoquePreMovimento", "EmpresaId", "dbo.Empresa", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.EstoquePreMovimento", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.EstoqueMovimento", "EmpresaId", "dbo.Empresa");
            DropIndex("dbo.EstoquePreMovimento", new[] { "EmpresaId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "EmpresaId" });
            AlterColumn("dbo.EstoquePreMovimento", "EmpresaId", c => c.Long(nullable: false));
            AlterColumn("dbo.EstoqueMovimento", "EmpresaId", c => c.Long(nullable: false));
            CreateIndex("dbo.EstoquePreMovimento", "EmpresaId");
            CreateIndex("dbo.EstoqueMovimento", "EmpresaId");
            AddForeignKey("dbo.EstoquePreMovimento", "EmpresaId", "dbo.Empresa", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EstoqueMovimento", "EmpresaId", "dbo.Empresa", "Id", cascadeDelete: true);
        }
    }
}
