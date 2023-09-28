namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Marcus_Substituicao_UnidadeInternacao_por_UnidadeOrganizacional_Modulos_Assistenciais : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AdmissaoMedica", "UnidadeInternacaoId", "dbo.UnidadeInternacao");
            DropIndex("dbo.AdmissaoMedica", new[] { "UnidadeInternacaoId" });
            AddColumn("dbo.AdmissaoMedica", "UnidadeOrganizacionalId", c => c.Long(nullable: false));
            CreateIndex("dbo.AdmissaoMedica", "UnidadeOrganizacionalId");
            AddForeignKey("dbo.AdmissaoMedica", "UnidadeOrganizacionalId", "dbo.UnidadeOrganizacional", "Id", cascadeDelete: false);
            DropColumn("dbo.AdmissaoMedica", "UnidadeInternacaoId");
        }

        public override void Down()
        {
            AddColumn("dbo.AdmissaoMedica", "UnidadeInternacaoId", c => c.Long(nullable: false));
            DropForeignKey("dbo.AdmissaoMedica", "UnidadeOrganizacionalId", "dbo.UnidadeOrganizacional");
            DropIndex("dbo.AdmissaoMedica", new[] { "UnidadeOrganizacionalId" });
            DropColumn("dbo.AdmissaoMedica", "UnidadeOrganizacionalId");
            CreateIndex("dbo.AdmissaoMedica", "UnidadeInternacaoId");
            AddForeignKey("dbo.AdmissaoMedica", "UnidadeInternacaoId", "dbo.UnidadeInternacao", "Id", cascadeDelete: false);
        }
    }
}
