namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class PAblo_Ajuste_Visitante : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteVisitante", "LeitoId", c => c.Long());
            CreateIndex("dbo.AteVisitante", "LeitoId");
            AddForeignKey("dbo.AteVisitante", "LeitoId", "dbo.Leito", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AteVisitante", "LeitoId", "dbo.Leito");
            DropIndex("dbo.AteVisitante", new[] { "LeitoId" });
            DropColumn("dbo.AteVisitante", "LeitoId");
        }
    }
}
