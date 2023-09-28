namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Ajuste_Evento : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuaEvento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    RealizaSindicancia = c.Boolean(nullable: false),
                    QuaEventoGrupoId = c.Long(),
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
                    { "DynamicFilter_Evento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuaEventoGrupo", t => t.QuaEventoGrupoId)
                .Index(t => t.QuaEventoGrupoId);

            CreateTable(
                "dbo.QuaEventoGrupo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
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
                    { "DynamicFilter_EventoGrupo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.QuaEventoMov",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Data = c.DateTime(nullable: false),
                    Descricao = c.String(),
                    IsConferido = c.Boolean(nullable: false),
                    IsObito = c.Boolean(nullable: false),
                    IsNaoConformidade = c.Boolean(nullable: false),
                    IsSentinela = c.Boolean(nullable: false),
                    IsAdverso = c.Boolean(nullable: false),
                    DataInclusao = c.DateTime(nullable: false),
                    IsAlteraClinica = c.Boolean(nullable: false),
                    Obs = c.Binary(),
                    GrauSentinela = c.Int(),
                    DataConferido = c.DateTime(nullable: false),
                    Medicamento = c.String(),
                    Lote = c.String(),
                    Validade = c.DateTime(nullable: false),
                    Fabricante = c.String(),
                    DataNotificacao = c.DateTime(nullable: false),
                    NomeNotificacao = c.String(),
                    TipoEvento = c.String(),
                    PrazoResposta = c.DateTime(nullable: false),
                    CausaMaterial = c.Binary(),
                    CausaMetodo = c.Binary(),
                    CausaComunicacao = c.Binary(),
                    CausaEfeito = c.Binary(),
                    CausaMaodeObra = c.Binary(),
                    CausaMeioAmbiente = c.Binary(),
                    CausaMedida = c.Binary(),
                    PlanoOque = c.Binary(),
                    PlanoComo = c.Binary(),
                    PlanoQuem = c.Binary(),
                    PlanoQuando = c.Binary(),
                    PlanoPorQue = c.Binary(),
                    PlanoQuanto = c.Binary(),
                    PlanoStatus = c.Binary(),
                    PlanoPrazo = c.DateTime(nullable: false),
                    AcaoImediata = c.Binary(),
                    ClassificaEvento = c.String(),
                    StatusEvento = c.Int(),
                    ObsQualidade = c.Binary(),
                    DataUsuarioFinaliza = c.DateTime(nullable: false),
                    IDUsuarioEncaminha = c.Long(),
                    DataUsuarioEncaminha = c.DateTime(nullable: false),
                    DataUsuarioResponde = c.DateTime(nullable: false),
                    ObsGestor = c.Binary(),
                    CausaCultura = c.Binary(),
                    EmailNotificador = c.String(),
                    QuaEventoId = c.Long(),
                    QuaEventoGrupoId = c.Long(),
                    AteAtendimentoId = c.Long(nullable: false),
                    LabSetorId = c.Long(),
                    EstProdutoId = c.Long(),
                    SisPacienteId = c.Long(),
                    CentroCustoId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
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
                    { "DynamicFilter_EventoMov_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteAtendimento", t => t.AteAtendimentoId, cascadeDelete: true)
                .ForeignKey("dbo.CentroCusto", t => t.CentroCustoId)
                .ForeignKey("dbo.QuaEvento", t => t.QuaEventoId)
                .ForeignKey("dbo.QuaEventoGrupo", t => t.QuaEventoGrupoId)
                .ForeignKey("dbo.SisPaciente", t => t.SisPacienteId)
                .ForeignKey("dbo.Est_Produto", t => t.EstProdutoId)
                .ForeignKey("dbo.LabSetor", t => t.LabSetorId)
                .Index(t => t.QuaEventoId)
                .Index(t => t.QuaEventoGrupoId)
                .Index(t => t.AteAtendimentoId)
                .Index(t => t.LabSetorId)
                .Index(t => t.EstProdutoId)
                .Index(t => t.SisPacienteId)
                .Index(t => t.CentroCustoId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.QuaEventoMov", "LabSetorId", "dbo.LabSetor");
            DropForeignKey("dbo.QuaEventoMov", "EstProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.QuaEventoMov", "SisPacienteId", "dbo.SisPaciente");
            DropForeignKey("dbo.QuaEventoMov", "QuaEventoGrupoId", "dbo.QuaEventoGrupo");
            DropForeignKey("dbo.QuaEventoMov", "QuaEventoId", "dbo.QuaEvento");
            DropForeignKey("dbo.QuaEventoMov", "CentroCustoId", "dbo.CentroCusto");
            DropForeignKey("dbo.QuaEventoMov", "AteAtendimentoId", "dbo.AteAtendimento");
            DropForeignKey("dbo.QuaEvento", "QuaEventoGrupoId", "dbo.QuaEventoGrupo");
            DropIndex("dbo.QuaEventoMov", new[] { "CentroCustoId" });
            DropIndex("dbo.QuaEventoMov", new[] { "SisPacienteId" });
            DropIndex("dbo.QuaEventoMov", new[] { "EstProdutoId" });
            DropIndex("dbo.QuaEventoMov", new[] { "LabSetorId" });
            DropIndex("dbo.QuaEventoMov", new[] { "AteAtendimentoId" });
            DropIndex("dbo.QuaEventoMov", new[] { "QuaEventoGrupoId" });
            DropIndex("dbo.QuaEventoMov", new[] { "QuaEventoId" });
            DropIndex("dbo.QuaEvento", new[] { "QuaEventoGrupoId" });
            DropTable("dbo.QuaEventoMov",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EventoMov_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.QuaEventoGrupo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EventoGrupo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.QuaEvento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Evento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
