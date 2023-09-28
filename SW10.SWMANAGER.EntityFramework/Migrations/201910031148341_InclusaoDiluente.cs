namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InclusaoDiluente : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssPrescricaoItem", "IsDiluente", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssPrescricaoItemResposta", "DiluenteId", c => c.Long());
            CreateIndex("dbo.AssPrescricaoItemResposta", "DiluenteId");
            AddForeignKey("dbo.AssPrescricaoItemResposta", "DiluenteId", "dbo.AssPrescricaoItem", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssPrescricaoItemResposta", "DiluenteId", "dbo.AssPrescricaoItem");
            DropIndex("dbo.AssPrescricaoItemResposta", new[] { "DiluenteId" });
            DropColumn("dbo.AssPrescricaoItemResposta", "DiluenteId");
            DropColumn("dbo.AssPrescricaoItem", "IsDiluente");
        }
    }
}
