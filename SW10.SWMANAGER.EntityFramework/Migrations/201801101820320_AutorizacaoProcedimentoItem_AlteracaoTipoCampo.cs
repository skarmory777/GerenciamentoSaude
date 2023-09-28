namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AutorizacaoProcedimentoItem_AlteracaoTipoCampo : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AteAutorizacaoProcedimentoItem", "QuantidadeSolicitada", c => c.Int());
            AlterColumn("dbo.AteAutorizacaoProcedimentoItem", "QuantidadeAutorizada", c => c.Int());
        }

        public override void Down()
        {
            AlterColumn("dbo.AteAutorizacaoProcedimentoItem", "QuantidadeAutorizada", c => c.Int(nullable: false));
            AlterColumn("dbo.AteAutorizacaoProcedimentoItem", "QuantidadeSolicitada", c => c.Int(nullable: false));
        }
    }
}
