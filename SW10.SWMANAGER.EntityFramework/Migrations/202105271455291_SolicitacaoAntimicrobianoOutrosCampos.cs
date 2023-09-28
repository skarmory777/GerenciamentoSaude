namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SolicitacaoAntimicrobianoOutrosCampos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssSolicitacaoAntimicrobianos", "UnidadeId", c => c.Long());
            AddColumn("dbo.AssSolicitacaoAntimicrobianos", "AssFormaAplicacaoId", c => c.Long());
            AddColumn("dbo.AssSolicitacaoAntimicrobianos", "AssVelocidadeInfusaoId", c => c.Long());
            CreateIndex("dbo.AssSolicitacaoAntimicrobianos", "UnidadeId");
            CreateIndex("dbo.AssSolicitacaoAntimicrobianos", "AssFormaAplicacaoId");
            CreateIndex("dbo.AssSolicitacaoAntimicrobianos", "AssVelocidadeInfusaoId");
            AddForeignKey("dbo.AssSolicitacaoAntimicrobianos", "AssFormaAplicacaoId", "dbo.AssFormaAplicacao", "Id");
            AddForeignKey("dbo.AssSolicitacaoAntimicrobianos", "UnidadeId", "dbo.Est_Unidade", "Id");
            AddForeignKey("dbo.AssSolicitacaoAntimicrobianos", "AssVelocidadeInfusaoId", "dbo.AssVelocidadeInfusao", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssSolicitacaoAntimicrobianos", "AssVelocidadeInfusaoId", "dbo.AssVelocidadeInfusao");
            DropForeignKey("dbo.AssSolicitacaoAntimicrobianos", "UnidadeId", "dbo.Est_Unidade");
            DropForeignKey("dbo.AssSolicitacaoAntimicrobianos", "AssFormaAplicacaoId", "dbo.AssFormaAplicacao");
            DropIndex("dbo.AssSolicitacaoAntimicrobianos", new[] { "AssVelocidadeInfusaoId" });
            DropIndex("dbo.AssSolicitacaoAntimicrobianos", new[] { "AssFormaAplicacaoId" });
            DropIndex("dbo.AssSolicitacaoAntimicrobianos", new[] { "UnidadeId" });
            DropColumn("dbo.AssSolicitacaoAntimicrobianos", "AssVelocidadeInfusaoId");
            DropColumn("dbo.AssSolicitacaoAntimicrobianos", "AssFormaAplicacaoId");
            DropColumn("dbo.AssSolicitacaoAntimicrobianos", "UnidadeId");
        }
    }
}
