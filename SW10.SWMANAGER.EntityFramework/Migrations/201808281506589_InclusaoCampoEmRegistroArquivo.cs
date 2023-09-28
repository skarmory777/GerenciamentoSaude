namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InclusaoCampoEmRegistroArquivo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisRegistroArquivo", "AtendimentoId", c => c.Long());
            CreateIndex("dbo.SisRegistroArquivo", "AtendimentoId");
            AddForeignKey("dbo.SisRegistroArquivo", "AtendimentoId", "dbo.AteAtendimento", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.SisRegistroArquivo", "AtendimentoId", "dbo.AteAtendimento");
            DropIndex("dbo.SisRegistroArquivo", new[] { "AtendimentoId" });
            DropColumn("dbo.SisRegistroArquivo", "AtendimentoId");
        }
    }
}
