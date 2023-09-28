namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjustesZeusNovo : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FinDocumento", "DataEmissao", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.FatContaItem", "Data", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.FinLancamento", "DataLancamento", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.EstoqueMovimento", "Movimento", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.EstoqueMovimento", "Emissao", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.EstoquePreMovimento", "Movimento", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.EstoquePreMovimento", "Emissao", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EstoquePreMovimento", "Emissao", c => c.DateTime(nullable: false));
            AlterColumn("dbo.EstoquePreMovimento", "Movimento", c => c.DateTime(nullable: false));
            AlterColumn("dbo.EstoqueMovimento", "Emissao", c => c.DateTime(nullable: false));
            AlterColumn("dbo.EstoqueMovimento", "Movimento", c => c.DateTime(nullable: false));
            AlterColumn("dbo.FinLancamento", "DataLancamento", c => c.DateTime());
            AlterColumn("dbo.FatContaItem", "Data", c => c.DateTime());
            AlterColumn("dbo.FinDocumento", "DataEmissao", c => c.DateTime());
        }
    }
}
