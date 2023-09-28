namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Fat_Entrega_Conta_Usuario_e_Data_Conferencia_UsuarioLote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatConta", "DataConferencia", c => c.DateTime());
            AddColumn("dbo.FatConta", "FatUsuarioConferenciaId", c => c.Long());
            AddColumn("dbo.FatEntregaLote", "FatUsuarioLoteId", c => c.Long());
            CreateIndex("dbo.FatConta", "FatUsuarioConferenciaId");
            CreateIndex("dbo.FatEntregaLote", "FatUsuarioLoteId");
            AddForeignKey("dbo.FatConta", "FatUsuarioConferenciaId", "dbo.AbpUsers", "Id");
            AddForeignKey("dbo.FatEntregaLote", "FatUsuarioLoteId", "dbo.AbpUsers", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.FatEntregaLote", "FatUsuarioLoteId", "dbo.AbpUsers");
            DropForeignKey("dbo.FatConta", "FatUsuarioConferenciaId", "dbo.AbpUsers");
            DropIndex("dbo.FatEntregaLote", new[] { "FatUsuarioLoteId" });
            DropIndex("dbo.FatConta", new[] { "FatUsuarioConferenciaId" });
            DropColumn("dbo.FatEntregaLote", "FatUsuarioLoteId");
            DropColumn("dbo.FatConta", "FatUsuarioConferenciaId");
            DropColumn("dbo.FatConta", "DataConferencia");
        }
    }
}
