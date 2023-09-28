namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Inserindo_campos_LabResultado_Exame : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LabResultadoExame", "SisUsuarioIncluidoId", c => c.Long());
            AddColumn("dbo.LabResultadoExame", "DataEnvioEmail", c => c.DateTime(nullable: false));
            CreateIndex("dbo.LabResultadoExame", "SisUsuarioIncluidoId");
            AddForeignKey("dbo.LabResultadoExame", "SisUsuarioIncluidoId", "dbo.AbpUsers", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.LabResultadoExame", "SisUsuarioIncluidoId", "dbo.AbpUsers");
            DropIndex("dbo.LabResultadoExame", new[] { "SisUsuarioIncluidoId" });
            DropColumn("dbo.LabResultadoExame", "DataEnvioEmail");
            DropColumn("dbo.LabResultadoExame", "SisUsuarioIncluidoId");
        }
    }
}
