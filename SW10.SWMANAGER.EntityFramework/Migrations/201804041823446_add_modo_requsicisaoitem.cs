namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class add_modo_requsicisaoitem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CmpRequisicaoItem", "ModoInclusao", c => c.String(maxLength: 1));
        }

        public override void Down()
        {
            DropColumn("dbo.CmpRequisicaoItem", "ModoInclusao");
        }
    }
}
