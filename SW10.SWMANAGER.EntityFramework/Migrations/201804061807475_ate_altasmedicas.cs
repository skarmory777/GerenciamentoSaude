namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ate_altasmedicas : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AteAltaMedica", "AteMotivoAltaId", "dbo.AssMotivoAlta");
            DropIndex("dbo.AteAltaMedica", new[] { "AteMotivoAltaId" });
            AddColumn("dbo.AteAtendimento", "DataPrevistaAlta", c => c.DateTime());
            AddColumn("dbo.AteAltaMedica", "AteLeitoId", c => c.Long());
            CreateIndex("dbo.AteAltaMedica", "AteLeitoId");
            AddForeignKey("dbo.AteAltaMedica", "AteLeitoId", "dbo.AteLeito", "Id");
            DropColumn("dbo.AteAltaMedica", "AteMotivoAltaId");
        }

        public override void Down()
        {
            AddColumn("dbo.AteAltaMedica", "AteMotivoAltaId", c => c.Long());
            DropForeignKey("dbo.AteAltaMedica", "AteLeitoId", "dbo.AteLeito");
            DropIndex("dbo.AteAltaMedica", new[] { "AteLeitoId" });
            DropColumn("dbo.AteAltaMedica", "AteLeitoId");
            DropColumn("dbo.AteAtendimento", "DataPrevistaAlta");
            CreateIndex("dbo.AteAltaMedica", "AteMotivoAltaId");
            AddForeignKey("dbo.AteAltaMedica", "AteMotivoAltaId", "dbo.AssMotivoAlta", "Id");
        }
    }
}
