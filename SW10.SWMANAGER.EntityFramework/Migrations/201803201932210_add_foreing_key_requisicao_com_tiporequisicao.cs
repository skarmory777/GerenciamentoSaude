namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class add_foreing_key_requisicao_com_tiporequisicao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CmpRequisicao", "CmpTipoRequisicaoId", c => c.Long(nullable: false));
            CreateIndex("dbo.CmpRequisicao", "CmpTipoRequisicaoId");
            AddForeignKey("dbo.CmpRequisicao", "CmpTipoRequisicaoId", "dbo.CmpTipoRequisicao", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.CmpRequisicao", "CmpTipoRequisicaoId", "dbo.CmpTipoRequisicao");
            DropIndex("dbo.CmpRequisicao", new[] { "CmpTipoRequisicaoId" });
            DropColumn("dbo.CmpRequisicao", "CmpTipoRequisicaoId");
        }
    }
}
