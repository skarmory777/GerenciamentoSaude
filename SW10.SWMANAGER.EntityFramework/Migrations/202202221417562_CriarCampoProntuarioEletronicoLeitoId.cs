namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CriarCampoProntuarioEletronicoLeitoId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssProntuario", "AteLeitoId", c => c.Long());
            CreateIndex("dbo.AssProntuario", "AteLeitoId");
            AddForeignKey("dbo.AssProntuario", "AteLeitoId", "dbo.AteLeito", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssProntuario", "AteLeitoId", "dbo.AteLeito");
            DropIndex("dbo.AssProntuario", new[] { "AteLeitoId" });
            DropColumn("dbo.AssProntuario", "AteLeitoId");
        }
    }
}
