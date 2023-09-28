namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class cad_produto_add_etiquetaid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Est_Produto", "EtiquetaId", c => c.Long());
        }

        public override void Down()
        {
            DropColumn("dbo.Est_Produto", "EtiquetaId");
        }
    }
}
