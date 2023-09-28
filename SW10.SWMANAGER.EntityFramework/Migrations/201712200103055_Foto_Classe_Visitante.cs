namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Foto_Classe_Visitante : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteVisitante", "Foto", c => c.Binary());
            AddColumn("dbo.AteVisitante", "FotoMimeType", c => c.String());
            AlterColumn("dbo.AteVisitante", "Nome", c => c.String(maxLength: 100));
            AlterColumn("dbo.AteVisitante", "Documento", c => c.String(maxLength: 14));
        }

        public override void Down()
        {
            AlterColumn("dbo.AteVisitante", "Documento", c => c.String());
            AlterColumn("dbo.AteVisitante", "Nome", c => c.String());
            DropColumn("dbo.AteVisitante", "FotoMimeType");
            DropColumn("dbo.AteVisitante", "Foto");
        }
    }
}
