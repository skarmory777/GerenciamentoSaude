namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class add_emissao_comprarequisicao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CmpRequisicao", "DataEmissao", c => c.DateTime(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.CmpRequisicao", "DataEmissao");
        }
    }
}
