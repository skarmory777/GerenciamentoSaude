namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdicionandoCamposParaInativacaoAoProntuario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssProntuario", "EstaInativo", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssProntuario", "InativacaoUserId", c => c.Long());
            AddColumn("dbo.AssProntuario", "InativacaoData", c => c.DateTime());
            AddColumn("dbo.AssProntuario", "InativacaoJustificativa", c => c.String());
            AddColumn("dbo.AssProntuario", "AtivacaoUserId", c => c.Long());
            AddColumn("dbo.AssProntuario", "AtivacaoData", c => c.DateTime());
            AddColumn("dbo.AssProntuario", "AtivacaoJustificativa", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssProntuario", "AtivacaoJustificativa");
            DropColumn("dbo.AssProntuario", "AtivacaoData");
            DropColumn("dbo.AssProntuario", "AtivacaoUserId");
            DropColumn("dbo.AssProntuario", "InativacaoJustificativa");
            DropColumn("dbo.AssProntuario", "InativacaoData");
            DropColumn("dbo.AssProntuario", "InativacaoUserId");
            DropColumn("dbo.AssProntuario", "EstaInativo");
        }
    }
}
