namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InclusaoCamposAtendimento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteAtendimento", "CNS", c => c.String());
            AddColumn("dbo.AteAtendimento", "CaraterAtendimentoId", c => c.Long());
            AddColumn("dbo.AteAtendimento", "IndicacaoAcidenteId", c => c.Long());
            CreateIndex("dbo.AteAtendimento", "CaraterAtendimentoId");
            CreateIndex("dbo.AteAtendimento", "IndicacaoAcidenteId");
            AddForeignKey("dbo.AteAtendimento", "CaraterAtendimentoId", "dbo.SisTabelaDominio", "Id");
            AddForeignKey("dbo.AteAtendimento", "IndicacaoAcidenteId", "dbo.SisTabelaDominio", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AteAtendimento", "IndicacaoAcidenteId", "dbo.SisTabelaDominio");
            DropForeignKey("dbo.AteAtendimento", "CaraterAtendimentoId", "dbo.SisTabelaDominio");
            DropIndex("dbo.AteAtendimento", new[] { "IndicacaoAcidenteId" });
            DropIndex("dbo.AteAtendimento", new[] { "CaraterAtendimentoId" });
            DropColumn("dbo.AteAtendimento", "IndicacaoAcidenteId");
            DropColumn("dbo.AteAtendimento", "CaraterAtendimentoId");
            DropColumn("dbo.AteAtendimento", "CNS");
        }
    }
}
