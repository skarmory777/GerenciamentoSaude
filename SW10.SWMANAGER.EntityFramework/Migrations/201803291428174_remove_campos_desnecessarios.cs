namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class remove_campos_desnecessarios : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CmpRequisicao", "IsTipoRequisicaoServico");
            DropColumn("dbo.CmpRequisicao", "IsTipoRequisicaoProduto");
        }

        public override void Down()
        {
            AddColumn("dbo.CmpRequisicao", "IsTipoRequisicaoProduto", c => c.Boolean(nullable: false));
            AddColumn("dbo.CmpRequisicao", "IsTipoRequisicaoServico", c => c.Boolean(nullable: false));
        }
    }
}
