namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoGrupoLaudo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LauGrupo", "ModalidadeId", c => c.Long());
            CreateIndex("dbo.LauGrupo", "ModalidadeId");
            AddForeignKey("dbo.LauGrupo", "ModalidadeId", "dbo.LauModalidade", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.LauGrupo", "ModalidadeId", "dbo.LauModalidade");
            DropIndex("dbo.LauGrupo", new[] { "ModalidadeId" });
            DropColumn("dbo.LauGrupo", "ModalidadeId");
        }
    }
}
