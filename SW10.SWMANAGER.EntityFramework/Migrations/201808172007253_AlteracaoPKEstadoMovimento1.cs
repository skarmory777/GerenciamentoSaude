namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoPKEstadoMovimento1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EstoquePreMovimentoEstado", "Id", c => c.Long(nullable: false, identity: false));
        }

        public override void Down()
        {
            AlterColumn("dbo.EstoquePreMovimentoEstado", "Id", c => c.Long(nullable: false, identity: true));
        }
    }
}
