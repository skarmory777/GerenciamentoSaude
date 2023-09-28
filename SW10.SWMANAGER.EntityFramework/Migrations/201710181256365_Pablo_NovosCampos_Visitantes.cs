namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Pablo_NovosCampos_Visitantes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteVisitante", "IsEmergencia", c => c.Boolean(nullable: false));
            AddColumn("dbo.AteVisitante", "IsInternado", c => c.Boolean(nullable: false));
            AddColumn("dbo.AteVisitante", "IsFornecedor", c => c.Boolean(nullable: false));
            AddColumn("dbo.AteVisitante", "IsSetor", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.AteVisitante", "IsSetor");
            DropColumn("dbo.AteVisitante", "IsFornecedor");
            DropColumn("dbo.AteVisitante", "IsInternado");
            DropColumn("dbo.AteVisitante", "IsEmergencia");
        }
    }
}
