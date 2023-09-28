namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Atualizando_CompraCotacao_ColunasRetornoBionexo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CmpRequisicao", "CmpCotacaoStatusId", "dbo.CmpCotacaoStatus");
            DropIndex("dbo.CmpRequisicao", new[] { "CmpCotacaoStatusId" });
            AddColumn("dbo.CmpCotacao", "DataRetornoBionexo", c => c.DateTime());
            AddColumn("dbo.CmpCotacao", "CmpCompraCotacaoStatusId", c => c.Long(nullable: false));
            AddColumn("dbo.CmpCotacao", "DataStatusCancelado", c => c.DateTime());
            AddColumn("dbo.CmpCotacao", "UserIdStatusCancelado", c => c.Long());
            AddColumn("dbo.CmpCotacao", "DataStatusFinalizado", c => c.DateTime());
            AddColumn("dbo.CmpCotacao", "UserIdStatusFinalizado", c => c.Long());
            CreateIndex("dbo.CmpCotacao", "CmpCompraCotacaoStatusId");
            AddForeignKey("dbo.CmpCotacao", "CmpCompraCotacaoStatusId", "dbo.CmpCotacaoStatus", "Id", cascadeDelete: true);
            DropColumn("dbo.CmpRequisicao", "CmpCotacaoStatusId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CmpRequisicao", "CmpCotacaoStatusId", c => c.Long(nullable: false));
            DropForeignKey("dbo.CmpCotacao", "CmpCompraCotacaoStatusId", "dbo.CmpCotacaoStatus");
            DropIndex("dbo.CmpCotacao", new[] { "CmpCompraCotacaoStatusId" });
            DropColumn("dbo.CmpCotacao", "UserIdStatusFinalizado");
            DropColumn("dbo.CmpCotacao", "DataStatusFinalizado");
            DropColumn("dbo.CmpCotacao", "UserIdStatusCancelado");
            DropColumn("dbo.CmpCotacao", "DataStatusCancelado");
            DropColumn("dbo.CmpCotacao", "CmpCompraCotacaoStatusId");
            DropColumn("dbo.CmpCotacao", "DataRetornoBionexo");
            CreateIndex("dbo.CmpRequisicao", "CmpCotacaoStatusId");
            AddForeignKey("dbo.CmpRequisicao", "CmpCotacaoStatusId", "dbo.CmpCotacaoStatus", "Id", cascadeDelete: true);
        }
    }
}
