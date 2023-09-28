namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class _201802271441294_Fat_EntregaLoteConta : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FatEntregaConta",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    FatContaMedicaId = c.Long(),
                    FatEntregaLoteId = c.Long(),
                    ValorConta = c.Single(nullable: false),
                    ValorTaxas = c.Single(nullable: false),
                    ValorFranquia = c.Single(nullable: false),
                    ValorProduzido = c.Single(nullable: false),
                    ValorProduzidoTaxas = c.Single(nullable: false),
                    ValorRecebido = c.Single(nullable: false),
                    ValorRecebidoTemp = c.Single(nullable: false),
                    IsGlosa = c.Boolean(nullable: false),
                    IsRecebe = c.Boolean(nullable: false),
                    IsRecebeTudo = c.Boolean(nullable: false),
                    IsErroGuia = c.Boolean(nullable: false),
                    DataEntrega = c.DateTime(),
                    DataFinalEntrega = c.DateTime(),
                    DataUsuarioEntrega = c.DateTime(),
                    DataUsuarioTemp = c.DateTime(),
                    UsuarioTempId = c.Long(),
                    UsuarioEntregaId = c.Long(),
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
                    { "DynamicFilter_FaturamentoEntregaConta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatConta", t => t.FatContaMedicaId)
                .ForeignKey("dbo.FatEntregaLote", t => t.FatEntregaLoteId)
                .Index(t => t.FatContaMedicaId)
                .Index(t => t.FatEntregaLoteId);

            CreateTable(
                "dbo.FatEntregaLote",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    SisEmpresaId = c.Long(),
                    SisConvenioId = c.Long(),
                    CodEntregaLote = c.String(),
                    NumeroProcesso = c.String(),
                    DataInicial = c.DateTime(),
                    DataFinal = c.DateTime(),
                    DataEntrega = c.DateTime(),
                    ValorFatura = c.Single(nullable: false),
                    ValorTaxas = c.Single(nullable: false),
                    IsAmbulatorio = c.Boolean(nullable: false),
                    IsInternacao = c.Boolean(nullable: false),
                    Desativado = c.Boolean(nullable: false),
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
                    { "DynamicFilter_FaturamentoEntregaLote_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisConvenio", t => t.SisConvenioId)
                .ForeignKey("dbo.SisEmpresa", t => t.SisEmpresaId)
                .Index(t => t.SisEmpresaId)
                .Index(t => t.SisConvenioId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.FatEntregaConta", "FatEntregaLoteId", "dbo.FatEntregaLote");
            DropForeignKey("dbo.FatEntregaLote", "SisEmpresaId", "dbo.SisEmpresa");
            DropForeignKey("dbo.FatEntregaLote", "SisConvenioId", "dbo.SisConvenio");
            DropForeignKey("dbo.FatEntregaConta", "FatContaMedicaId", "dbo.FatConta");
            DropIndex("dbo.FatEntregaLote", new[] { "SisConvenioId" });
            DropIndex("dbo.FatEntregaLote", new[] { "SisEmpresaId" });
            DropIndex("dbo.FatEntregaConta", new[] { "FatEntregaLoteId" });
            DropIndex("dbo.FatEntregaConta", new[] { "FatContaMedicaId" });
            DropTable("dbo.FatEntregaLote",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoEntregaLote_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FatEntregaConta",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoEntregaConta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
