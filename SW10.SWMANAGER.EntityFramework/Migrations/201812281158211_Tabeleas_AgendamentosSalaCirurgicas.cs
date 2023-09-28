namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Tabeleas_AgendamentosSalaCirurgicas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AteAgendamentoCirurgico",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    SalaCirurgicaId = c.Long(),
                    IsEletiva = c.Boolean(nullable: false),
                    IsEmergencia = c.Boolean(nullable: false),
                    IsPossuiOPME = c.Boolean(nullable: false),
                    StatusAutorizacaoCirurgiaExame = c.Long(nullable: false),
                    StatusAutorizacaoOPME = c.Long(nullable: false),
                    IsNecessitaSangue = c.Boolean(nullable: false),
                    IsNecessitaVideo = c.Boolean(nullable: false),
                    IsNecessitaCTI = c.Boolean(nullable: false),
                    IsNecesssitaItencificador = c.Boolean(nullable: false),
                    OPMESolicitada = c.String(),
                    OPMEAutorizada = c.String(),
                    IsPossuiAlergia = c.Boolean(nullable: false),
                    Alergias = c.String(),
                    IsPossuiPrecaucoes = c.Boolean(nullable: false),
                    Precaucoes = c.String(),
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
                    { "DynamicFilter_AgendamentoCirurgico_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteSalaCirurgica", t => t.SalaCirurgicaId)
                .Index(t => t.SalaCirurgicaId);

            CreateTable(
                "dbo.AteSalaCirurgica",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
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
                    { "DynamicFilter_SalaCirurgica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AteAgendamentoItemFaturamento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    AgendamentoCirurgicoId = c.Long(nullable: false),
                    FaturamentoItemId = c.Long(nullable: false),
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
                    { "DynamicFilter_AgendamentoItemFaturamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteAgendamentoCirurgico", t => t.AgendamentoCirurgicoId, cascadeDelete: true)
                .ForeignKey("dbo.FatItem", t => t.FaturamentoItemId, cascadeDelete: true)
                .Index(t => t.AgendamentoCirurgicoId)
                .Index(t => t.FaturamentoItemId);

            CreateTable(
                "dbo.AteAgendamentoMaterial",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    AgendamentoCirurgicoId = c.Long(nullable: false),
                    FaturamentoItemId = c.Long(nullable: false),
                    Quantidade = c.Decimal(nullable: false, precision: 18, scale: 2),
                    DataRecebimento = c.DateTime(),
                    DataPrevista = c.DateTime(),
                    NumeroNotaFiscal = c.String(),
                    ValorNotaFiscal = c.Decimal(nullable: false, precision: 18, scale: 2),
                    FornecedorId = c.Long(nullable: false),
                    IsCobrarPeloHospital = c.Boolean(nullable: false),
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
                    { "DynamicFilter_AgendamentoMaterial_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteAgendamentoCirurgico", t => t.AgendamentoCirurgicoId, cascadeDelete: true)
                .ForeignKey("dbo.FatItem", t => t.FaturamentoItemId, cascadeDelete: true)
                .ForeignKey("dbo.Fornecedor", t => t.FornecedorId, cascadeDelete: true)
                .Index(t => t.AgendamentoCirurgicoId)
                .Index(t => t.FaturamentoItemId)
                .Index(t => t.FornecedorId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AteAgendamentoMaterial", "FornecedorId", "dbo.Fornecedor");
            DropForeignKey("dbo.AteAgendamentoMaterial", "FaturamentoItemId", "dbo.FatItem");
            DropForeignKey("dbo.AteAgendamentoMaterial", "AgendamentoCirurgicoId", "dbo.AteAgendamentoCirurgico");
            DropForeignKey("dbo.AteAgendamentoItemFaturamento", "FaturamentoItemId", "dbo.FatItem");
            DropForeignKey("dbo.AteAgendamentoItemFaturamento", "AgendamentoCirurgicoId", "dbo.AteAgendamentoCirurgico");
            DropForeignKey("dbo.AteAgendamentoCirurgico", "SalaCirurgicaId", "dbo.AteSalaCirurgica");
            DropIndex("dbo.AteAgendamentoMaterial", new[] { "FornecedorId" });
            DropIndex("dbo.AteAgendamentoMaterial", new[] { "FaturamentoItemId" });
            DropIndex("dbo.AteAgendamentoMaterial", new[] { "AgendamentoCirurgicoId" });
            DropIndex("dbo.AteAgendamentoItemFaturamento", new[] { "FaturamentoItemId" });
            DropIndex("dbo.AteAgendamentoItemFaturamento", new[] { "AgendamentoCirurgicoId" });
            DropIndex("dbo.AteAgendamentoCirurgico", new[] { "SalaCirurgicaId" });
            DropTable("dbo.AteAgendamentoMaterial",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AgendamentoMaterial_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AteAgendamentoItemFaturamento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AgendamentoItemFaturamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AteSalaCirurgica",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SalaCirurgica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AteAgendamentoCirurgico",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AgendamentoCirurgico_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
