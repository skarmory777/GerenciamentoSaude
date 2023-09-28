namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Tarefa_StatusModIds : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisTarefa", "ModuloId", c => c.Long());
            AddColumn("dbo.SisTarefa", "StatusId", c => c.Long());
            DropColumn("dbo.SisTarefa", "Modulo");
            DropColumn("dbo.SisTarefa", "Status");
        }

        public override void Down()
        {
            AddColumn("dbo.SisTarefa", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.SisTarefa", "Modulo", c => c.Long());
            DropColumn("dbo.SisTarefa", "StatusId");
            DropColumn("dbo.SisTarefa", "ModuloId");
        }
    }
}
