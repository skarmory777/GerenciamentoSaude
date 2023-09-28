namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Faturamento_Alteracoes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatKit", "FaturamentoItemId", c => c.Long());
            AddColumn("dbo.FatConta", "FatContaMedicaId", c => c.Long());
            AddColumn("dbo.FatConta", "IsAtivo", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatConta", "Versao", c => c.Long(nullable: false));
            CreateIndex("dbo.FatKit", "FaturamentoItemId");
            CreateIndex("dbo.FatConta", "FatContaMedicaId");
            AddForeignKey("dbo.FatKit", "FaturamentoItemId", "dbo.FatItem", "Id");
            AddForeignKey("dbo.FatConta", "FatContaMedicaId", "dbo.FatConta", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FatConta", "FatContaMedicaId", "dbo.FatConta");
            DropForeignKey("dbo.FatKit", "FaturamentoItemId", "dbo.FatItem");
            DropIndex("dbo.FatConta", new[] { "FatContaMedicaId" });
            DropIndex("dbo.FatKit", new[] { "FaturamentoItemId" });
            DropColumn("dbo.FatConta", "Versao");
            DropColumn("dbo.FatConta", "IsAtivo");
            DropColumn("dbo.FatConta", "FatContaMedicaId");
            DropColumn("dbo.FatKit", "FaturamentoItemId");
        }
    }
}
