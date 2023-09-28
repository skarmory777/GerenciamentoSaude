namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alterando_stringlength_Conselho : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SisConselho", "Sigla", c => c.String(maxLength: 10));
        }

        public override void Down()
        {
            AlterColumn("dbo.SisConselho", "Sigla", c => c.String(maxLength: 5));
        }
    }
}
