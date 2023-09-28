namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Marcus_Alteracao_AdmissaoMedica_Relacionamento_FormResposta : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AdmissaoMedica", "FormConfigId", "dbo.FormConfig");
            DropIndex("dbo.AdmissaoMedica", new[] { "FormConfigId" });
            AddColumn("dbo.AdmissaoMedica", "FormRespostaId", c => c.Long());
            CreateIndex("dbo.AdmissaoMedica", "FormRespostaId");
            AddForeignKey("dbo.AdmissaoMedica", "FormRespostaId", "dbo.FormResposta", "Id");
            DropColumn("dbo.AdmissaoMedica", "FormConfigId");
        }

        public override void Down()
        {
            AddColumn("dbo.AdmissaoMedica", "FormConfigId", c => c.Long());
            DropForeignKey("dbo.AdmissaoMedica", "FormRespostaId", "dbo.FormResposta");
            DropIndex("dbo.AdmissaoMedica", new[] { "FormRespostaId" });
            DropColumn("dbo.AdmissaoMedica", "FormRespostaId");
            CreateIndex("dbo.AdmissaoMedica", "FormConfigId");
            AddForeignKey("dbo.AdmissaoMedica", "FormConfigId", "dbo.FormConfig", "Id");
        }
    }
}
