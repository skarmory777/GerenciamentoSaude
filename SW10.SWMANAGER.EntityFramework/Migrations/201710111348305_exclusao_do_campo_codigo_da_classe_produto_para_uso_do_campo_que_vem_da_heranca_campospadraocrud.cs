namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class exclusao_do_campo_codigo_da_classe_produto_para_uso_do_campo_que_vem_da_heranca_campospadraocrud : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Est_Produto", "Codigo", c => c.String(maxLength: 10));
        }

        public override void Down()
        {
            AlterColumn("dbo.Est_Produto", "Codigo", c => c.Long(nullable: false));
        }
    }
}
