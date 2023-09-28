namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlteracoesResultadoExameItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LabResultadoExame", "DataColetaBaixa", c => c.DateTime());
            AddColumn("dbo.LabResultadoExame", "UsuarioColetaBaixaId", c => c.Long());
            CreateIndex("dbo.LabResultadoExame", "UsuarioColetaBaixaId");
            AddForeignKey("dbo.LabResultadoExame", "UsuarioColetaBaixaId", "dbo.AbpUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LabResultadoExame", "UsuarioColetaBaixaId", "dbo.AbpUsers");
            DropIndex("dbo.LabResultadoExame", new[] { "UsuarioColetaBaixaId" });
            DropColumn("dbo.LabResultadoExame", "UsuarioColetaBaixaId");
            DropColumn("dbo.LabResultadoExame", "DataColetaBaixa");
        }
    }
}
