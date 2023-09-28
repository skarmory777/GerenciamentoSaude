namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class NormalizarVersao : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.TipoDocumento", newName: "EstTipoMovimento");
            //DropForeignKey("dbo.EstoquePreMovimento", "TipoMovimentoId", "dbo.EstoqueTipoMovimento");
            //DropForeignKey("dbo.EstoqueMovimento", "TipoMovimentoId", "dbo.EstoqueTipoMovimento");
            //DropIndex("dbo.EstoqueMovimento", new[] { "TipoMovimentoId" });
            //DropIndex("dbo.EstoquePreMovimento", new[] { "TipoMovimentoId" });
            //RenameColumn(table: "dbo.EstoquePreMovimento", name: "TipoDocumentoId", newName: "EstTipoMovimentoId");
            //RenameColumn(table: "dbo.EstoqueMovimento", name: "TipoDocumentoId", newName: "EstTipoMovimentoId");
            //RenameIndex(table: "dbo.EstoqueMovimento", name: "IX_TipoDocumentoId", newName: "IX_EstTipoMovimentoId");
            //RenameIndex(table: "dbo.EstoquePreMovimento", name: "IX_TipoDocumentoId", newName: "IX_EstTipoMovimentoId");
            //    CreateTable(
            //        "dbo.EstTipoOperacao",
            //        c => new
            //            {
            //                Id = c.Long(nullable: false, identity: true),
            //                Descricao = c.String(),
            //                IsSistema = c.Boolean(nullable: false),
            //                IsDeleted = c.Boolean(nullable: false),
            //                DeleterUserId = c.Long(),
            //                DeletionTime = c.DateTime(),
            //                LastModificationTime = c.DateTime(),
            //                LastModifierUserId = c.Long(),
            //                CreationTime = c.DateTime(nullable: false),
            //                CreatorUserId = c.Long(),
            //            },
            //        annotations: new Dictionary<string, object>
            //        {
            //            { "DynamicFilter_TipoOperacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //        })
            //        .PrimaryKey(t => t.Id);

            //    CreateTable(
            //        "dbo.SisMoedaCotacaoItem",
            //        c => new
            //            {
            //                Id = c.Long(nullable: false, identity: true),
            //                SisMoedaCotacaoId = c.Long(),
            //                ItemId = c.Long(),
            //                IsSistema = c.Boolean(nullable: false),
            //                IsDeleted = c.Boolean(nullable: false),
            //                DeleterUserId = c.Long(),
            //                DeletionTime = c.DateTime(),
            //                LastModificationTime = c.DateTime(),
            //                LastModifierUserId = c.Long(),
            //                CreationTime = c.DateTime(nullable: false),
            //                CreatorUserId = c.Long(),
            //            },
            //        annotations: new Dictionary<string, object>
            //        {
            //            { "DynamicFilter_SisMoedaCotacaoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //        })
            //        .PrimaryKey(t => t.Id)
            //        .ForeignKey("dbo.FatItem", t => t.ItemId)
            //        .ForeignKey("dbo.SisMoedaCotacao", t => t.SisMoedaCotacaoId)
            //        .Index(t => t.SisMoedaCotacaoId)
            //        .Index(t => t.ItemId);

            //    CreateTable(
            //        "dbo.SisMoedaCotacao",
            //        c => new
            //            {
            //                Id = c.Long(nullable: false, identity: true),
            //                SisMoedaId = c.Long(),
            //                EmpresaId = c.Long(),
            //                ConvenioId = c.Long(),
            //                FaturamentoGrupoId = c.Long(),
            //                DataInicio = c.DateTime(nullable: false),
            //                DataFinal = c.DateTime(nullable: false),
            //                Valor = c.Single(nullable: false),
            //                IsTodosPlano = c.Boolean(nullable: false),
            //                IsTodosItem = c.Boolean(nullable: false),
            //                IsSistema = c.Boolean(nullable: false),
            //                IsDeleted = c.Boolean(nullable: false),
            //                DeleterUserId = c.Long(),
            //                DeletionTime = c.DateTime(),
            //                LastModificationTime = c.DateTime(),
            //                LastModifierUserId = c.Long(),
            //                CreationTime = c.DateTime(nullable: false),
            //                CreatorUserId = c.Long(),
            //            },
            //        annotations: new Dictionary<string, object>
            //        {
            //            { "DynamicFilter_SisMoedaCotacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //        })
            //        .PrimaryKey(t => t.Id)
            //        .ForeignKey("dbo.Convenio", t => t.ConvenioId)
            //        .ForeignKey("dbo.Empresa", t => t.EmpresaId)
            //        .ForeignKey("dbo.FatGrupo", t => t.FaturamentoGrupoId)
            //        .ForeignKey("dbo.SisMoeda", t => t.SisMoedaId)
            //        .Index(t => t.SisMoedaId)
            //        .Index(t => t.EmpresaId)
            //        .Index(t => t.ConvenioId)
            //        .Index(t => t.FaturamentoGrupoId);

            //    CreateTable(
            //        "dbo.SisMoeda",
            //        c => new
            //            {
            //                Id = c.Long(nullable: false, identity: true),
            //                Codigo = c.String(),
            //                Descricao = c.String(),
            //                Tipo = c.Int(nullable: false),
            //                IsSistema = c.Boolean(nullable: false),
            //                IsDeleted = c.Boolean(nullable: false),
            //                DeleterUserId = c.Long(),
            //                DeletionTime = c.DateTime(),
            //                LastModificationTime = c.DateTime(),
            //                LastModifierUserId = c.Long(),
            //                CreationTime = c.DateTime(nullable: false),
            //                CreatorUserId = c.Long(),
            //            },
            //        annotations: new Dictionary<string, object>
            //        {
            //            { "DynamicFilter_SisMoeda_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //        })
            //        .PrimaryKey(t => t.Id);

            //    CreateTable(
            //        "dbo.SisMoedaCotacaoPlano",
            //        c => new
            //            {
            //                Id = c.Long(nullable: false, identity: true),
            //                SisMoedaCotacaoId = c.Long(),
            //                PlanoId = c.Long(),
            //                IsSistema = c.Boolean(nullable: false),
            //                IsDeleted = c.Boolean(nullable: false),
            //                DeleterUserId = c.Long(),
            //                DeletionTime = c.DateTime(),
            //                LastModificationTime = c.DateTime(),
            //                LastModifierUserId = c.Long(),
            //                CreationTime = c.DateTime(nullable: false),
            //                CreatorUserId = c.Long(),
            //            },
            //        annotations: new Dictionary<string, object>
            //        {
            //            { "DynamicFilter_SisMoedaCotacaoPlano_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //        })
            //        .PrimaryKey(t => t.Id)
            //        .ForeignKey("dbo.Plano", t => t.PlanoId)
            //        .ForeignKey("dbo.SisMoedaCotacao", t => t.SisMoedaCotacaoId)
            //        .Index(t => t.SisMoedaCotacaoId)
            //        .Index(t => t.PlanoId);

            //    CreateTable(
            //        "dbo.TenantLogo",
            //        c => new
            //            {
            //                Id = c.Long(nullable: false, identity: true),
            //                Logotipo = c.Binary(),
            //                LogotipoMimeType = c.String(),
            //                TenantId = c.Long(),
            //                IsSistema = c.Boolean(nullable: false),
            //                IsDeleted = c.Boolean(nullable: false),
            //                DeleterUserId = c.Long(),
            //                DeletionTime = c.DateTime(),
            //                LastModificationTime = c.DateTime(),
            //                LastModifierUserId = c.Long(),
            //                CreationTime = c.DateTime(nullable: false),
            //                CreatorUserId = c.Long(),
            //            },
            //        annotations: new Dictionary<string, object>
            //        {
            //            { "DynamicFilter_TenantLogo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //        })
            //        .PrimaryKey(t => t.Id);

            //    AlterTableAnnotations(
            //        "dbo.EstTipoMovimento",
            //        c => new
            //            {
            //                Id = c.Long(nullable: false, identity: true),
            //                Descricao = c.String(),
            //                IsEntrada = c.Boolean(nullable: false),
            //                IsSistema = c.Boolean(nullable: false),
            //                IsDeleted = c.Boolean(nullable: false),
            //                DeleterUserId = c.Long(),
            //                DeletionTime = c.DateTime(),
            //                LastModificationTime = c.DateTime(),
            //                LastModifierUserId = c.Long(),
            //                CreationTime = c.DateTime(nullable: false),
            //                CreatorUserId = c.Long(),
            //            },
            //        annotations: new Dictionary<string, AnnotationValues>
            //        {
            //            { 
            //                "DynamicFilter_TipoDocumento_SoftDelete",
            //                new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
            //            },
            //            { 
            //                "DynamicFilter_TipoMovimento_SoftDelete",
            //                new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
            //            },
            //        });

            //    AddColumn("dbo.EstoqueMovimento", "EstTipoOperacaoId", c => c.Long());
            //    AddColumn("dbo.EstoquePreMovimento", "EstTipoOperacaoId", c => c.Long());
            //    AddColumn("dbo.FatItem", "BrasItemId", c => c.Long());
            //    AlterColumn("dbo.EstTipoMovimento", "Descricao", c => c.String());
            //    CreateIndex("dbo.EstoqueMovimento", "EstTipoOperacaoId");
            //    CreateIndex("dbo.EstoquePreMovimento", "EstTipoOperacaoId");
            //    CreateIndex("dbo.FatItem", "BrasItemId");
            //    AddForeignKey("dbo.EstoquePreMovimento", "EstTipoOperacaoId", "dbo.EstTipoOperacao", "Id");
            //    AddForeignKey("dbo.EstoqueMovimento", "EstTipoOperacaoId", "dbo.EstTipoOperacao", "Id");
            //    AddForeignKey("dbo.FatItem", "BrasItemId", "dbo.FatBrasItem", "Id");
            //    DropColumn("dbo.EstoqueMovimento", "TipoMovimentoId");
            //    DropColumn("dbo.EstoquePreMovimento", "TipoMovimentoId");
            //    DropTable("dbo.EstoqueTipoMovimento",
            //        removedAnnotations: new Dictionary<string, object>
            //        {
            //            { "DynamicFilter_EstoqueTipoMovimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //        });
        }

        public override void Down()
        {
            //CreateTable(
            //    "dbo.EstoqueTipoMovimento",
            //    c => new
            //        {
            //            Id = c.Long(nullable: false, identity: true),
            //            Descricao = c.String(),
            //            IsSistema = c.Boolean(nullable: false),
            //            IsDeleted = c.Boolean(nullable: false),
            //            DeleterUserId = c.Long(),
            //            DeletionTime = c.DateTime(),
            //            LastModificationTime = c.DateTime(),
            //            LastModifierUserId = c.Long(),
            //            CreationTime = c.DateTime(nullable: false),
            //            CreatorUserId = c.Long(),
            //        },
            //    annotations: new Dictionary<string, object>
            //    {
            //        { "DynamicFilter_EstoqueTipoMovimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //    })
            //    .PrimaryKey(t => t.Id);

            //AddColumn("dbo.EstoquePreMovimento", "TipoMovimentoId", c => c.Long(nullable: false));
            //AddColumn("dbo.EstoqueMovimento", "TipoMovimentoId", c => c.Long(nullable: false));
            //DropForeignKey("dbo.SisMoedaCotacaoPlano", "SisMoedaCotacaoId", "dbo.SisMoedaCotacao");
            //DropForeignKey("dbo.SisMoedaCotacaoPlano", "PlanoId", "dbo.Plano");
            //DropForeignKey("dbo.SisMoedaCotacaoItem", "SisMoedaCotacaoId", "dbo.SisMoedaCotacao");
            //DropForeignKey("dbo.SisMoedaCotacao", "SisMoedaId", "dbo.SisMoeda");
            //DropForeignKey("dbo.SisMoedaCotacao", "FaturamentoGrupoId", "dbo.FatGrupo");
            //DropForeignKey("dbo.SisMoedaCotacao", "EmpresaId", "dbo.Empresa");
            //DropForeignKey("dbo.SisMoedaCotacao", "ConvenioId", "dbo.Convenio");
            //DropForeignKey("dbo.SisMoedaCotacaoItem", "ItemId", "dbo.FatItem");
            //DropForeignKey("dbo.FatItem", "BrasItemId", "dbo.FatBrasItem");
            //DropForeignKey("dbo.EstoqueMovimento", "EstTipoOperacaoId", "dbo.EstTipoOperacao");
            //DropForeignKey("dbo.EstoquePreMovimento", "EstTipoOperacaoId", "dbo.EstTipoOperacao");
            //DropIndex("dbo.SisMoedaCotacaoPlano", new[] { "PlanoId" });
            //DropIndex("dbo.SisMoedaCotacaoPlano", new[] { "SisMoedaCotacaoId" });
            //DropIndex("dbo.SisMoedaCotacao", new[] { "FaturamentoGrupoId" });
            //DropIndex("dbo.SisMoedaCotacao", new[] { "ConvenioId" });
            //DropIndex("dbo.SisMoedaCotacao", new[] { "EmpresaId" });
            //DropIndex("dbo.SisMoedaCotacao", new[] { "SisMoedaId" });
            //DropIndex("dbo.SisMoedaCotacaoItem", new[] { "ItemId" });
            //DropIndex("dbo.SisMoedaCotacaoItem", new[] { "SisMoedaCotacaoId" });
            //DropIndex("dbo.FatItem", new[] { "BrasItemId" });
            //DropIndex("dbo.EstoquePreMovimento", new[] { "EstTipoOperacaoId" });
            //DropIndex("dbo.EstoqueMovimento", new[] { "EstTipoOperacaoId" });
            //AlterColumn("dbo.EstTipoMovimento", "Descricao", c => c.String(maxLength: 30));
            //DropColumn("dbo.FatItem", "BrasItemId");
            //DropColumn("dbo.EstoquePreMovimento", "EstTipoOperacaoId");
            //DropColumn("dbo.EstoqueMovimento", "EstTipoOperacaoId");
            //AlterTableAnnotations(
            //    "dbo.EstTipoMovimento",
            //    c => new
            //        {
            //            Id = c.Long(nullable: false, identity: true),
            //            Descricao = c.String(),
            //            IsEntrada = c.Boolean(nullable: false),
            //            IsSistema = c.Boolean(nullable: false),
            //            IsDeleted = c.Boolean(nullable: false),
            //            DeleterUserId = c.Long(),
            //            DeletionTime = c.DateTime(),
            //            LastModificationTime = c.DateTime(),
            //            LastModifierUserId = c.Long(),
            //            CreationTime = c.DateTime(nullable: false),
            //            CreatorUserId = c.Long(),
            //        },
            //    annotations: new Dictionary<string, AnnotationValues>
            //    {
            //        { 
            //            "DynamicFilter_TipoDocumento_SoftDelete",
            //            new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
            //        },
            //        { 
            //            "DynamicFilter_TipoMovimento_SoftDelete",
            //            new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
            //        },
            //    });

            //DropTable("dbo.TenantLogo",
            //    removedAnnotations: new Dictionary<string, object>
            //    {
            //        { "DynamicFilter_TenantLogo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //    });
            //DropTable("dbo.SisMoedaCotacaoPlano",
            //    removedAnnotations: new Dictionary<string, object>
            //    {
            //        { "DynamicFilter_SisMoedaCotacaoPlano_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //    });
            //DropTable("dbo.SisMoeda",
            //    removedAnnotations: new Dictionary<string, object>
            //    {
            //        { "DynamicFilter_SisMoeda_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //    });
            //DropTable("dbo.SisMoedaCotacao",
            //    removedAnnotations: new Dictionary<string, object>
            //    {
            //        { "DynamicFilter_SisMoedaCotacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //    });
            //DropTable("dbo.SisMoedaCotacaoItem",
            //    removedAnnotations: new Dictionary<string, object>
            //    {
            //        { "DynamicFilter_SisMoedaCotacaoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //    });
            //DropTable("dbo.EstTipoOperacao",
            //    removedAnnotations: new Dictionary<string, object>
            //    {
            //        { "DynamicFilter_TipoOperacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //    });
            //RenameIndex(table: "dbo.EstoquePreMovimento", name: "IX_EstTipoMovimentoId", newName: "IX_TipoDocumentoId");
            //RenameIndex(table: "dbo.EstoqueMovimento", name: "IX_EstTipoMovimentoId", newName: "IX_TipoDocumentoId");
            //RenameColumn(table: "dbo.EstoqueMovimento", name: "EstTipoMovimentoId", newName: "TipoDocumentoId");
            //RenameColumn(table: "dbo.EstoquePreMovimento", name: "EstTipoMovimentoId", newName: "TipoDocumentoId");
            //CreateIndex("dbo.EstoquePreMovimento", "TipoMovimentoId");
            //CreateIndex("dbo.EstoqueMovimento", "TipoMovimentoId");
            //AddForeignKey("dbo.EstoqueMovimento", "TipoMovimentoId", "dbo.EstoqueTipoMovimento", "Id", cascadeDelete: true);
            //AddForeignKey("dbo.EstoquePreMovimento", "TipoMovimentoId", "dbo.EstoqueTipoMovimento", "Id", cascadeDelete: true);
            //RenameTable(name: "dbo.EstTipoMovimento", newName: "TipoDocumento");
        }
    }
}
