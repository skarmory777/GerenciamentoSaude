namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Emprestimo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstoquePreMovimento", "EmprestimoEmpresaId", c => c.Long());
            CreateIndex("dbo.EstoquePreMovimento", "EmprestimoEmpresaId");
            AddForeignKey("dbo.EstoquePreMovimento", "EmprestimoEmpresaId", "dbo.SisPessoa", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EstoquePreMovimento", "EmprestimoEmpresaId", "dbo.SisPessoa");
            DropIndex("dbo.EstoquePreMovimento", new[] { "EmprestimoEmpresaId" });
            DropColumn("dbo.EstoquePreMovimento", "EmprestimoEmpresaId");
        }
    }
}
