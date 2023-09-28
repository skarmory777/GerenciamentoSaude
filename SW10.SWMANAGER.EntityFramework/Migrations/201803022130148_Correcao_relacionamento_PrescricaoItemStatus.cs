namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Correcao_relacionamento_PrescricaoItemStatus : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssPrescricaoItem", "AssPrescricaoItemStatusId", "dbo.AssPrescricaoItemStatus");
            DropIndex("dbo.AssPrescricaoItem", new[] { "AssPrescricaoItemStatusId" });
            AddColumn("dbo.AssFormTipoResposta", "AssPrescricaoItemStatusId", c => c.Long());
            CreateIndex("dbo.AssFormTipoResposta", "AssPrescricaoItemStatusId");
            AddForeignKey("dbo.AssFormTipoResposta", "AssPrescricaoItemStatusId", "dbo.AssPrescricaoItemStatus", "Id");
            DropColumn("dbo.AssPrescricaoItem", "AssPrescricaoItemStatusId");
        }

        public override void Down()
        {
            AddColumn("dbo.AssPrescricaoItem", "AssPrescricaoItemStatusId", c => c.Long());
            DropForeignKey("dbo.AssFormTipoResposta", "AssPrescricaoItemStatusId", "dbo.AssPrescricaoItemStatus");
            DropIndex("dbo.AssFormTipoResposta", new[] { "AssPrescricaoItemStatusId" });
            DropColumn("dbo.AssFormTipoResposta", "AssPrescricaoItemStatusId");
            CreateIndex("dbo.AssPrescricaoItem", "AssPrescricaoItemStatusId");
            AddForeignKey("dbo.AssPrescricaoItem", "AssPrescricaoItemStatusId", "dbo.AssPrescricaoItemStatus", "Id");
        }
    }
}
