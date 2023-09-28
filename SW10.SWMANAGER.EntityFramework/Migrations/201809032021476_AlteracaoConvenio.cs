namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoConvenio : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisConvenio", "VersaoTissId", c => c.Long());
            CreateIndex("dbo.SisConvenio", "VersaoTissId");
            AddForeignKey("dbo.SisConvenio", "VersaoTissId", "dbo.SisVersaoTiss", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.SisConvenio", "VersaoTissId", "dbo.SisVersaoTiss");
            DropIndex("dbo.SisConvenio", new[] { "VersaoTissId" });
            DropColumn("dbo.SisConvenio", "VersaoTissId");
        }
    }
}
