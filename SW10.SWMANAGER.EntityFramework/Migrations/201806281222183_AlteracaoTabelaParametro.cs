namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoTabelaParametro : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.SisParametro", new[] { "EmpresaId" });
            CreateIndex("dbo.SisParametro", new[] { "Codigo", "EmpresaId" }, unique: true, name: "UK_Parametro");
        }

        public override void Down()
        {
            DropIndex("dbo.SisParametro", "UK_Parametro");
            CreateIndex("dbo.SisParametro", "EmpresaId");
        }
    }
}
