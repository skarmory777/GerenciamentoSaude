namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class add_tabelas_AteTexto_AteTextoEmpresa_AteTextoGuia1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AteTextoEmpresa",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    TextoId = c.Long(),
                    EmpresaId = c.Long(),
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
                    { "DynamicFilter_TextoModeloEmpresa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisEmpresa", t => t.EmpresaId)
                .ForeignKey("dbo.AteTextoModelo", t => t.TextoId)
                .Index(t => t.TextoId)
                .Index(t => t.EmpresaId);

            CreateTable(
                "dbo.AteTextoModelo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Texto = c.String(),
                    IsAmbulatorioEmergencia = c.Boolean(nullable: false),
                    IsInternacao = c.Boolean(nullable: false),
                    IsMostraAtendimento = c.Boolean(nullable: false),
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
                    { "DynamicFilter_TextoModelo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AteTextoGuia",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    TextoId = c.Long(),
                    FatGuiaId = c.Long(),
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
                    { "DynamicFilter_TextoModeloGuia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatGuia", t => t.FatGuiaId)
                .ForeignKey("dbo.AteTextoModelo", t => t.TextoId)
                .Index(t => t.TextoId)
                .Index(t => t.FatGuiaId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AteTextoGuia", "TextoId", "dbo.AteTextoModelo");
            DropForeignKey("dbo.AteTextoGuia", "FatGuiaId", "dbo.FatGuia");
            DropForeignKey("dbo.AteTextoEmpresa", "TextoId", "dbo.AteTextoModelo");
            DropForeignKey("dbo.AteTextoEmpresa", "EmpresaId", "dbo.SisEmpresa");
            DropIndex("dbo.AteTextoGuia", new[] { "FatGuiaId" });
            DropIndex("dbo.AteTextoGuia", new[] { "TextoId" });
            DropIndex("dbo.AteTextoEmpresa", new[] { "EmpresaId" });
            DropIndex("dbo.AteTextoEmpresa", new[] { "TextoId" });
            DropTable("dbo.AteTextoGuia",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TextoModeloGuia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AteTextoModelo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TextoModelo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AteTextoEmpresa",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TextoModeloEmpresa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
