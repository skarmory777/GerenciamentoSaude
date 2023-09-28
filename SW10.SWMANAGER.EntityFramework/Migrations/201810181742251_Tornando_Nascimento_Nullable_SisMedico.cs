namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Tornando_Nascimento_Nullable_SisMedico : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SisMedico", "Nascimento", c => c.DateTime());
        }

        public override void Down()
        {
            AlterColumn("dbo.SisMedico", "Nascimento", c => c.DateTime(nullable: false));
        }
    }
}
