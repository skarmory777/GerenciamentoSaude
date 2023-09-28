namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoFilaSenha : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteFila", "EmpresaId", c => c.Long());
            CreateIndex("dbo.AteFila", "EmpresaId");
            AddForeignKey("dbo.AteFila", "EmpresaId", "dbo.SisEmpresa", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AteFila", "EmpresaId", "dbo.SisEmpresa");
            DropIndex("dbo.AteFila", new[] { "EmpresaId" });
            DropColumn("dbo.AteFila", "EmpresaId");
        }
    }
}
