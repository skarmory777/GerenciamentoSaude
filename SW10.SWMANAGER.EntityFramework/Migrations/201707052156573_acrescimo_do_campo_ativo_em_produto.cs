namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class acrescimo_do_campo_ativo_em_produto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Est_Produto", "IsAtivo", c => c.Long(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Est_Produto", "IsAtivo");
        }
    }
}
