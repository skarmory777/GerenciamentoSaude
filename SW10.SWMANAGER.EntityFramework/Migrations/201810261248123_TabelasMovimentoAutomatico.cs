namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class TabelasMovimentoAutomatico : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisMovAutomatico",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    EmpresaId = c.Long(nullable: false),
                    UnidadeOrganizacionalId = c.Long(),
                    CentroCustoId = c.Long(),
                    TerceirizadoId = c.Long(),
                    TurnoId = c.Long(),
                    TipoAcomodacaoId = c.Long(),
                    Quantidade = c.Single(nullable: false),
                    IsAmbulatorio = c.Boolean(nullable: false),
                    IsInternacao = c.Boolean(nullable: false),
                    IsNovoAtendimento = c.Boolean(nullable: false),
                    IsDiaria = c.Boolean(nullable: false),
                    IsCobraPernoite = c.Boolean(nullable: false),
                    IsCobraRefeicao = c.Boolean(nullable: false),
                    IsCobraFralda = c.Boolean(nullable: false),
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
                    { "DynamicFilter_MovimentoAutomatico_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CentroCusto", t => t.CentroCustoId)
                .ForeignKey("dbo.SisEmpresa", t => t.EmpresaId, cascadeDelete: true)
                .ForeignKey("dbo.Terceirizado", t => t.TerceirizadoId)
                .ForeignKey("dbo.SisTipoAcomodacao", t => t.TipoAcomodacaoId)
                .ForeignKey("dbo.SisTurno", t => t.TurnoId)
                .ForeignKey("dbo.SisUnidadeOrganizacional", t => t.UnidadeOrganizacionalId)
                .Index(t => t.EmpresaId)
                .Index(t => t.UnidadeOrganizacionalId)
                .Index(t => t.CentroCustoId)
                .Index(t => t.TerceirizadoId)
                .Index(t => t.TurnoId)
                .Index(t => t.TipoAcomodacaoId);

            CreateTable(
                "dbo.SisMovutomaticoConvenioPlano",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    MovimentoAutomaticoId = c.Long(nullable: false),
                    ConvenioId = c.Long(),
                    PlanoId = c.Long(),
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
                    { "DynamicFilter_MovimentoAutomaticoConvenioPlano_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisConvenio", t => t.ConvenioId)
                .ForeignKey("dbo.SisMovAutomatico", t => t.MovimentoAutomaticoId, cascadeDelete: true)
                .ForeignKey("dbo.SisPlano", t => t.PlanoId)
                .Index(t => t.MovimentoAutomaticoId)
                .Index(t => t.ConvenioId)
                .Index(t => t.PlanoId);

            CreateTable(
                "dbo.SisMovAutomaticoEspecialidade",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    MovimentoAutomaticoId = c.Long(nullable: false),
                    EspecialidadeId = c.Long(nullable: false),
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
                    { "DynamicFilter_MovimentoAutomaticoEspecialidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisEspecialidade", t => t.EspecialidadeId, cascadeDelete: true)
                .ForeignKey("dbo.SisMovAutomatico", t => t.MovimentoAutomaticoId, cascadeDelete: true)
                .Index(t => t.MovimentoAutomaticoId)
                .Index(t => t.EspecialidadeId);

            CreateTable(
                "dbo.SisMovFaturamentoItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    MovimentoAutomaticoId = c.Long(nullable: false),
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
                    { "DynamicFilter_MovimentoAutomaticoFaturamentoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatItem", t => t.FaturamentoItemId, cascadeDelete: true)
                .ForeignKey("dbo.SisMovAutomatico", t => t.MovimentoAutomaticoId, cascadeDelete: true)
                .Index(t => t.MovimentoAutomaticoId)
                .Index(t => t.FaturamentoItemId);

            CreateTable(
                "dbo.SisMovAutomaticoTipoGuia",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    MovimentoAutomaticoId = c.Long(nullable: false),
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
                    { "DynamicFilter_MovimentoAutomaticoTipoGuia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisMovAutomatico", t => t.MovimentoAutomaticoId, cascadeDelete: true)
                .Index(t => t.MovimentoAutomaticoId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.SisMovAutomaticoTipoGuia", "MovimentoAutomaticoId", "dbo.SisMovAutomatico");
            DropForeignKey("dbo.SisMovFaturamentoItem", "MovimentoAutomaticoId", "dbo.SisMovAutomatico");
            DropForeignKey("dbo.SisMovFaturamentoItem", "FaturamentoItemId", "dbo.FatItem");
            DropForeignKey("dbo.SisMovAutomaticoEspecialidade", "MovimentoAutomaticoId", "dbo.SisMovAutomatico");
            DropForeignKey("dbo.SisMovAutomaticoEspecialidade", "EspecialidadeId", "dbo.SisEspecialidade");
            DropForeignKey("dbo.SisMovutomaticoConvenioPlano", "PlanoId", "dbo.SisPlano");
            DropForeignKey("dbo.SisMovutomaticoConvenioPlano", "MovimentoAutomaticoId", "dbo.SisMovAutomatico");
            DropForeignKey("dbo.SisMovutomaticoConvenioPlano", "ConvenioId", "dbo.SisConvenio");
            DropForeignKey("dbo.SisMovAutomatico", "UnidadeOrganizacionalId", "dbo.SisUnidadeOrganizacional");
            DropForeignKey("dbo.SisMovAutomatico", "TurnoId", "dbo.SisTurno");
            DropForeignKey("dbo.SisMovAutomatico", "TipoAcomodacaoId", "dbo.SisTipoAcomodacao");
            DropForeignKey("dbo.SisMovAutomatico", "TerceirizadoId", "dbo.Terceirizado");
            DropForeignKey("dbo.SisMovAutomatico", "EmpresaId", "dbo.SisEmpresa");
            DropForeignKey("dbo.SisMovAutomatico", "CentroCustoId", "dbo.CentroCusto");
            DropIndex("dbo.SisMovAutomaticoTipoGuia", new[] { "MovimentoAutomaticoId" });
            DropIndex("dbo.SisMovFaturamentoItem", new[] { "FaturamentoItemId" });
            DropIndex("dbo.SisMovFaturamentoItem", new[] { "MovimentoAutomaticoId" });
            DropIndex("dbo.SisMovAutomaticoEspecialidade", new[] { "EspecialidadeId" });
            DropIndex("dbo.SisMovAutomaticoEspecialidade", new[] { "MovimentoAutomaticoId" });
            DropIndex("dbo.SisMovutomaticoConvenioPlano", new[] { "PlanoId" });
            DropIndex("dbo.SisMovutomaticoConvenioPlano", new[] { "ConvenioId" });
            DropIndex("dbo.SisMovutomaticoConvenioPlano", new[] { "MovimentoAutomaticoId" });
            DropIndex("dbo.SisMovAutomatico", new[] { "TipoAcomodacaoId" });
            DropIndex("dbo.SisMovAutomatico", new[] { "TurnoId" });
            DropIndex("dbo.SisMovAutomatico", new[] { "TerceirizadoId" });
            DropIndex("dbo.SisMovAutomatico", new[] { "CentroCustoId" });
            DropIndex("dbo.SisMovAutomatico", new[] { "UnidadeOrganizacionalId" });
            DropIndex("dbo.SisMovAutomatico", new[] { "EmpresaId" });
            DropTable("dbo.SisMovAutomaticoTipoGuia",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MovimentoAutomaticoTipoGuia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SisMovFaturamentoItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MovimentoAutomaticoFaturamentoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SisMovAutomaticoEspecialidade",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MovimentoAutomaticoEspecialidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SisMovutomaticoConvenioPlano",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MovimentoAutomaticoConvenioPlano_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SisMovAutomatico",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MovimentoAutomatico_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
