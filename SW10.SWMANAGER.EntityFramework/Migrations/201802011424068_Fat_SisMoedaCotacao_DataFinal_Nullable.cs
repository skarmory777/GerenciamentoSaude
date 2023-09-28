namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Fat_SisMoedaCotacao_DataFinal_Nullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SisMoedaCotacao", "DataFinal", c => c.DateTime());
        }

        public override void Down()
        {
            AlterColumn("dbo.SisMoedaCotacao", "DataFinal", c => c.DateTime(nullable: false));
        }
    }
}
