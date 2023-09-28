namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Marcus_Alteracoes_model_admissao_medica_evolucao_medica : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EvolucaoMedica", "FormConfigId", "dbo.FormConfig");
            DropIndex("dbo.EvolucaoMedica", new[] { "FormConfigId" });
            AddColumn("dbo.EvolucaoMedica", "FormRespostaId", c => c.Long(nullable: false));
            CreateIndex("dbo.EvolucaoMedica", "FormRespostaId");
            AddForeignKey("dbo.EvolucaoMedica", "FormRespostaId", "dbo.FormResposta", "Id", cascadeDelete: true);
            DropColumn("dbo.EvolucaoMedica", "FormConfigId");
        }

        public override void Down()
        {
            AddColumn("dbo.EvolucaoMedica", "FormConfigId", c => c.Long(nullable: false));
            DropForeignKey("dbo.EvolucaoMedica", "FormRespostaId", "dbo.FormResposta");
            DropIndex("dbo.EvolucaoMedica", new[] { "FormRespostaId" });
            DropColumn("dbo.EvolucaoMedica", "FormRespostaId");
            CreateIndex("dbo.EvolucaoMedica", "FormConfigId");
            AddForeignKey("dbo.EvolucaoMedica", "FormConfigId", "dbo.FormConfig", "Id", cascadeDelete: true);
        }
    }
}
