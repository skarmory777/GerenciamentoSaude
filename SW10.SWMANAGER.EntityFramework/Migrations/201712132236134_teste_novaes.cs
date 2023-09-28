namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class teste_novaes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LabTeste",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Trabalho = c.String(maxLength: 60),
                    SisPessoaId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LaboratorioTeste_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisPessoa", t => t.SisPessoaId)
                .Index(t => t.SisPessoaId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.LabTeste", "SisPessoaId", "dbo.SisPessoa");
            DropIndex("dbo.LabTeste", new[] { "SisPessoaId" });
            DropTable("dbo.LabTeste",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LaboratorioTeste_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
