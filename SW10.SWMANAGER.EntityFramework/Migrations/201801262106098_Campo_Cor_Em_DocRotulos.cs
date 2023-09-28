namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Campo_Cor_Em_DocRotulos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocRotulo", "Cor", c => c.String(maxLength: 7));
        }

        public override void Down()
        {
            DropColumn("dbo.DocRotulo", "Cor");
        }
    }
}
