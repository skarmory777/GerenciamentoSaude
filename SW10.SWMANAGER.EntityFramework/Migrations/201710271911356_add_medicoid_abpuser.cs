namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class add_medicoid_abpuser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpUsers", "MedicoId", c => c.Long());
            CreateIndex("dbo.AbpUsers", "MedicoId");
            AddForeignKey("dbo.AbpUsers", "MedicoId", "dbo.SisMedico", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AbpUsers", "MedicoId", "dbo.SisMedico");
            DropIndex("dbo.AbpUsers", new[] { "MedicoId" });
            DropColumn("dbo.AbpUsers", "MedicoId");
        }
    }
}
