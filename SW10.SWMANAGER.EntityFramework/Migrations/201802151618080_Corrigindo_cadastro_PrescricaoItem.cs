namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Corrigindo_cadastro_PrescricaoItem : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AssPrescricaoItem", name: "EstUnidadeInfusaoId", newName: "EstUnidadeRequisicaoId");
            RenameIndex(table: "dbo.AssPrescricaoItem", name: "IX_EstUnidadeInfusaoId", newName: "IX_EstUnidadeRequisicaoId");
        }

        public override void Down()
        {
            RenameIndex(table: "dbo.AssPrescricaoItem", name: "IX_EstUnidadeRequisicaoId", newName: "IX_EstUnidadeInfusaoId");
            RenameColumn(table: "dbo.AssPrescricaoItem", name: "EstUnidadeRequisicaoId", newName: "EstUnidadeInfusaoId");
        }
    }
}
