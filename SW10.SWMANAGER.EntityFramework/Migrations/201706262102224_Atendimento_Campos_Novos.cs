namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Atendimento_Campos_Novos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Atendimento", "CpfResponsavel", c => c.String());
            AddColumn("dbo.Atendimento", "NumeroGuia", c => c.String());
            AddColumn("dbo.Atendimento", "QtdSessoes", c => c.Int(nullable: false));
            AddColumn("dbo.Atendimento", "DataRetorno", c => c.DateTime(nullable: false));
            AddColumn("dbo.Atendimento", "DataRevisao", c => c.DateTime(nullable: false));
            AddColumn("dbo.Atendimento", "NacionalidadeResponsavelId", c => c.Long());
            CreateIndex("dbo.Atendimento", "NacionalidadeResponsavelId");
            AddForeignKey("dbo.Atendimento", "NacionalidadeResponsavelId", "dbo.Nacionalidade", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Atendimento", "NacionalidadeResponsavelId", "dbo.Nacionalidade");
            DropIndex("dbo.Atendimento", new[] { "NacionalidadeResponsavelId" });
            DropColumn("dbo.Atendimento", "NacionalidadeResponsavelId");
            DropColumn("dbo.Atendimento", "DataRevisao");
            DropColumn("dbo.Atendimento", "DataRetorno");
            DropColumn("dbo.Atendimento", "QtdSessoes");
            DropColumn("dbo.Atendimento", "NumeroGuia");
            DropColumn("dbo.Atendimento", "CpfResponsavel");
        }
    }
}
