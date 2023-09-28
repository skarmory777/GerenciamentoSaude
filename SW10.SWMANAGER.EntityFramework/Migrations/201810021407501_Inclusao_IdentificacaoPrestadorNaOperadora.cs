namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Inclusao_IdentificacaoPrestadorNaOperadora : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisIdentificacaoPrestadorNaOperadora",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ConvenioId = c.Long(nullable: false),
                    EmpresaId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    ImportaId = c.Int(),
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
                    { "DynamicFilter_IdentificacaoPrestadorNaOperadora_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisConvenio", t => t.ConvenioId, cascadeDelete: true)
                .ForeignKey("dbo.SisEmpresa", t => t.EmpresaId, cascadeDelete: true)
                .Index(t => t.ConvenioId)
                .Index(t => t.EmpresaId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.SisIdentificacaoPrestadorNaOperadora", "EmpresaId", "dbo.SisEmpresa");
            DropForeignKey("dbo.SisIdentificacaoPrestadorNaOperadora", "ConvenioId", "dbo.SisConvenio");
            DropIndex("dbo.SisIdentificacaoPrestadorNaOperadora", new[] { "EmpresaId" });
            DropIndex("dbo.SisIdentificacaoPrestadorNaOperadora", new[] { "ConvenioId" });
            DropTable("dbo.SisIdentificacaoPrestadorNaOperadora",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_IdentificacaoPrestadorNaOperadora_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
