namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alteracao_PrescricaoItemResposta_DataInicial_para_Nullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AssPrescricaoItemResposta", "DataInicial", c => c.DateTime());
        }

        public override void Down()
        {
            AlterColumn("dbo.AssPrescricaoItemResposta", "DataInicial", c => c.DateTime(nullable: false));
        }
    }
}
