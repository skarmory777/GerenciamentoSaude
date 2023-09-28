namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class alterando_campo_isAtivo_em_produto_para_bool : DbMigration
    {
        public override void Up()
        {
            //removendo o valor default para conseguir alterar o tipo de dados
            AddColumn("dbo.Est_Produto", "IsAtivoTemp", c => c.Boolean(nullable: false));
            Sql("UPDATE EST_PRODUTO SET ISATIVOTEMP=ISATIVO");
            DropColumn("dbo.Est_Produto", "IsAtivo");
            RenameColumn("dbo.Est_Produto", "IsAtivoTemp", "IsAtivo");
        }

        public override void Down()
        {
            AddColumn("dbo.Est_Produto", "IsAtivoTemp", c => c.Long(nullable: false));
            Sql("UPDATE EST_PRODUTO SET ISATIVOTEMP=ISATIVO");
            DropColumn("dbo.Est_Produto", "IsAtivo");
            RenameColumn("dbo.Est_Produto", "IsAtivoTemp", "IsAtivo");
        }
    }
}
