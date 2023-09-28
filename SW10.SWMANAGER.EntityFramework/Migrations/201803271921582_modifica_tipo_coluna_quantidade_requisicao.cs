namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class modifica_tipo_coluna_quantidade_requisicao : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CmpRequisicaoItem", "Quantidade", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }

        public override void Down()
        {
            AlterColumn("dbo.CmpRequisicaoItem", "Quantidade", c => c.Long(nullable: false));
        }
    }
}
