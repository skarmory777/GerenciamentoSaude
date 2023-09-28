namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Tornando_Campos_DateTime_em_Nullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LabResultadoExame", "DataInclusao", c => c.DateTime());
            AlterColumn("dbo.LabResultadoExame", "DataAlteracao", c => c.DateTime());
            AlterColumn("dbo.LabResultadoExame", "DataExclusao", c => c.DateTime());
            AlterColumn("dbo.LabResultadoExame", "DataConferidoExame", c => c.DateTime());
            AlterColumn("dbo.LabResultadoExame", "DataDigitadoExame", c => c.DateTime());
            AlterColumn("dbo.LabResultadoExame", "DataPendenteExame", c => c.DateTime());
            AlterColumn("dbo.LabResultadoExame", "DataImpressoExame", c => c.DateTime());
            AlterColumn("dbo.LabResultadoExame", "DataImporta", c => c.DateTime());
            AlterColumn("dbo.LabResultadoExame", "DataUsuarioCienteExame", c => c.DateTime());
            AlterColumn("dbo.LabResultadoExame", "DataImpSolicita", c => c.DateTime());
            AlterColumn("dbo.LabResultadoExame", "DataAlteradoExame", c => c.DateTime());
            AlterColumn("dbo.LabResultadoExame", "DataEnvioEmail", c => c.DateTime());
        }

        public override void Down()
        {
            AlterColumn("dbo.LabResultadoExame", "DataEnvioEmail", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LabResultadoExame", "DataAlteradoExame", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LabResultadoExame", "DataImpSolicita", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LabResultadoExame", "DataUsuarioCienteExame", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LabResultadoExame", "DataImporta", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LabResultadoExame", "DataImpressoExame", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LabResultadoExame", "DataPendenteExame", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LabResultadoExame", "DataDigitadoExame", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LabResultadoExame", "DataConferidoExame", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LabResultadoExame", "DataExclusao", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LabResultadoExame", "DataAlteracao", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LabResultadoExame", "DataInclusao", c => c.DateTime(nullable: false));
        }
    }
}
