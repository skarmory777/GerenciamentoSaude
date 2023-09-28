namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoLancamentos : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FinLancamento", "DocumentoRelacionadoId", "dbo.FinDocumento");
            DropForeignKey("dbo.FinLancamento", "DocumentoId", "dbo.FinDocumento");
            DropIndex("dbo.FinLancamento", new[] { "DocumentoId" });
            DropIndex("dbo.FinLancamento", new[] { "DocumentoRelacionadoId" });
            AlterColumn("dbo.FinLancamento", "DocumentoId", c => c.Long());
            CreateIndex("dbo.FinLancamento", "DocumentoId");
            AddForeignKey("dbo.FinLancamento", "DocumentoId", "dbo.FinDocumento", "Id");
            DropColumn("dbo.FinLancamento", "DocumentoRelacionadoId");
        }

        public override void Down()
        {
            AddColumn("dbo.FinLancamento", "DocumentoRelacionadoId", c => c.Long());
            DropForeignKey("dbo.FinLancamento", "DocumentoId", "dbo.FinDocumento");
            DropIndex("dbo.FinLancamento", new[] { "DocumentoId" });
            AlterColumn("dbo.FinLancamento", "DocumentoId", c => c.Long(nullable: false));
            CreateIndex("dbo.FinLancamento", "DocumentoRelacionadoId");
            CreateIndex("dbo.FinLancamento", "DocumentoId");
            AddForeignKey("dbo.FinLancamento", "DocumentoId", "dbo.FinDocumento", "Id", cascadeDelete: true);
            AddForeignKey("dbo.FinLancamento", "DocumentoRelacionadoId", "dbo.FinDocumento", "Id");
        }
    }
}
