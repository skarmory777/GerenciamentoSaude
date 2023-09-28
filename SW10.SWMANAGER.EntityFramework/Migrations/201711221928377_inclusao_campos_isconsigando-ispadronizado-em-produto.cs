namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class inclusao_campos_isconsigandoispadronizadoemproduto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Est_Produto", "IsConsignado", c => c.Boolean(nullable: false));
            AddColumn("dbo.Est_Produto", "IsPadronizado", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Est_Produto", "IsPadronizado");
            DropColumn("dbo.Est_Produto", "IsConsignado");
        }
    }
}
