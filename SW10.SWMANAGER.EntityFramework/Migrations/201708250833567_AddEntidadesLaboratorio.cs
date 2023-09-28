namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddEntidadesLaboratorio : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LabColetaExameInformacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Numerico = c.Long(nullable: false),
                    Resultado = c.String(),
                    Referencia = c.String(),
                    UsuarioLaudoId = c.Long(nullable: false),
                    DataLaudo = c.DateTime(nullable: false),
                    Versaolongerface = c.String(),
                    Islongerface = c.Boolean(nullable: false),
                    LabUnidadeId = c.Long(),
                    TabelaResultadoId = c.Long(),
                    InformacaoId = c.Long(),
                    FormatacaoId = c.Long(),
                    ColetaExameId = c.Long(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LabColetaExame", t => t.ColetaExameId)
                .ForeignKey("dbo.LabFormatacao", t => t.FormatacaoId)
                .ForeignKey("dbo.LabInformacao", t => t.InformacaoId)
                .ForeignKey("dbo.LabTabelaResultado", t => t.TabelaResultadoId)
                .ForeignKey("dbo.LabUnidade", t => t.LabUnidadeId)
                .Index(t => t.LabUnidadeId)
                .Index(t => t.TabelaResultadoId)
                .Index(t => t.InformacaoId)
                .Index(t => t.FormatacaoId)
                .Index(t => t.ColetaExameId);

            CreateTable(
                "dbo.LabColetaExame",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    FatItemId = c.Long(nullable: false),
                    FatContaMovimentoId = c.Long(nullable: false),
                    Sequencia = c.Long(nullable: false),
                    UsuarioInclusaoId = c.Long(nullable: false),
                    DataInclusao = c.DateTime(),
                    UsuarioAlteracaoId = c.Long(nullable: false),
                    DataAlteracao = c.DateTime(),
                    UsuarioExclusaoId = c.Long(nullable: false),
                    DataExclusao = c.DateTime(),
                    FormataId = c.Long(nullable: false),
                    QtdExame = c.Long(nullable: false),
                    UsuarioDigitadoId = c.Long(nullable: false),
                    DataDigitadoId = c.Long(nullable: false),
                    UsuarioPendenteId = c.Long(nullable: false),
                    DataPendente = c.DateTime(),
                    MotivoPendente = c.String(),
                    UsuarioImpressoId = c.Long(nullable: false),
                    DataImpresso = c.DateTime(),
                    DataImporta = c.DateTime(),
                    IsSigiloso = c.Boolean(nullable: false),
                    ObsExame = c.String(),
                    UsuarioCienteId = c.Long(nullable: false),
                    DataCiente = c.DateTime(nullable: false),
                    UsuarioSolicitacaoId = c.Long(nullable: false),
                    DataSolicitacao = c.DateTime(nullable: false),
                    MaquinaImprimeSolicitacao = c.String(),
                    Resultado = c.String(),
                    MaterialId = c.Long(),
                    ColetaId = c.Long(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Coleta", t => t.ColetaId)
                .ForeignKey("dbo.LabMaterial", t => t.MaterialId)
                .Index(t => t.MaterialId)
                .Index(t => t.ColetaId);

            CreateTable(
                "dbo.Coleta",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(),
                    AtendimentoId = c.Long(nullable: false),
                    TecnicoId = c.Long(),
                    DataColeta = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Atendimento", t => t.AtendimentoId, cascadeDelete: true)
                .ForeignKey("dbo.LabTecnico", t => t.TecnicoId)
                .Index(t => t.AtendimentoId)
                .Index(t => t.TecnicoId);

            CreateTable(
                "dbo.LabTecnico",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Nome = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.LabFormatacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    NomeFormatacao = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.LabFormatacaoInformacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Ordem = c.Long(nullable: false),
                    Formula = c.String(),
                    InformacaoId = c.Long(),
                    FormatacaoId = c.Long(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LabInformacao", t => t.InformacaoId)
                .ForeignKey("dbo.LabFormatacao", t => t.FormatacaoId)
                .Index(t => t.InformacaoId)
                .Index(t => t.FormatacaoId);

            CreateTable(
                "dbo.LabInformacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(),
                    Descricao = c.String(),
                    CasaDecimal = c.Long(nullable: false),
                    Minimo = c.Double(nullable: false),
                    Maximo = c.Double(nullable: false),
                    MinimoExistente = c.Double(nullable: false),
                    MaximoExistente = c.Double(nullable: false),
                    Referencia = c.String(),
                    IsSoma100 = c.Boolean(nullable: false),
                    ObsAnormal = c.String(),
                    Islongerface = c.Boolean(nullable: false),
                    longerfaceEnvio = c.String(),
                    longerfaceRetorno = c.String(),
                    Dividelongerface = c.String(),
                    UnidadeId = c.Long(nullable: false),
                    TipoResultadoId = c.Long(),
                    EquipamentoId = c.Long(),
                    TabelaResultadoId = c.Long(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LabEquipamento", t => t.EquipamentoId)
                .ForeignKey("dbo.LabUnidade", t => t.UnidadeId, cascadeDelete: true)
                .ForeignKey("dbo.LabTabelaResultado", t => t.TabelaResultadoId)
                .ForeignKey("dbo.LabTipoResultado", t => t.TipoResultadoId)
                .Index(t => t.UnidadeId)
                .Index(t => t.TipoResultadoId)
                .Index(t => t.EquipamentoId)
                .Index(t => t.TabelaResultadoId);

            CreateTable(
                "dbo.LabEquipamento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    DiretorioOrdem = c.String(),
                    DiretorioResultado = c.String(),
                    Informacao_Id = c.Long(),
                    Informacao_Id1 = c.Long(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LabInformacao", t => t.Informacao_Id)
                .ForeignKey("dbo.LabInformacao", t => t.Informacao_Id1)
                .Index(t => t.Informacao_Id)
                .Index(t => t.Informacao_Id1);

            CreateTable(
                "dbo.LabUnidade",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.LabTabelaResultado",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.LabTabelaResultadoItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(),
                    Descricao = c.String(),
                    TabelaResultadoId = c.Long(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LabTabelaResultado", t => t.TabelaResultadoId)
                .Index(t => t.TabelaResultadoId);

            CreateTable(
                "dbo.LabTipoResultado",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.LabMetodo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.LabColetaExameInformacao", "LabUnidadeId", "dbo.LabUnidade");
            DropForeignKey("dbo.LabColetaExameInformacao", "TabelaResultadoId", "dbo.LabTabelaResultado");
            DropForeignKey("dbo.LabColetaExameInformacao", "InformacaoId", "dbo.LabInformacao");
            DropForeignKey("dbo.LabFormatacaoInformacao", "FormatacaoId", "dbo.LabFormatacao");
            DropForeignKey("dbo.LabInformacao", "TipoResultadoId", "dbo.LabTipoResultado");
            DropForeignKey("dbo.LabTabelaResultadoItem", "TabelaResultadoId", "dbo.LabTabelaResultado");
            DropForeignKey("dbo.LabInformacao", "TabelaResultadoId", "dbo.LabTabelaResultado");
            DropForeignKey("dbo.LabInformacao", "UnidadeId", "dbo.LabUnidade");
            DropForeignKey("dbo.LabFormatacaoInformacao", "InformacaoId", "dbo.LabInformacao");
            DropForeignKey("dbo.LabEquipamento", "Informacao_Id1", "dbo.LabInformacao");
            DropForeignKey("dbo.LabInformacao", "EquipamentoId", "dbo.LabEquipamento");
            DropForeignKey("dbo.LabEquipamento", "Informacao_Id", "dbo.LabInformacao");
            DropForeignKey("dbo.LabColetaExameInformacao", "FormatacaoId", "dbo.LabFormatacao");
            DropForeignKey("dbo.LabColetaExame", "MaterialId", "dbo.LabMaterial");
            DropForeignKey("dbo.LabColetaExameInformacao", "ColetaExameId", "dbo.LabColetaExame");
            DropForeignKey("dbo.LabColetaExame", "ColetaId", "dbo.Coleta");
            DropForeignKey("dbo.Coleta", "TecnicoId", "dbo.LabTecnico");
            DropForeignKey("dbo.Coleta", "AtendimentoId", "dbo.Atendimento");
            DropIndex("dbo.LabTabelaResultadoItem", new[] { "TabelaResultadoId" });
            DropIndex("dbo.LabEquipamento", new[] { "Informacao_Id1" });
            DropIndex("dbo.LabEquipamento", new[] { "Informacao_Id" });
            DropIndex("dbo.LabInformacao", new[] { "TabelaResultadoId" });
            DropIndex("dbo.LabInformacao", new[] { "EquipamentoId" });
            DropIndex("dbo.LabInformacao", new[] { "TipoResultadoId" });
            DropIndex("dbo.LabInformacao", new[] { "UnidadeId" });
            DropIndex("dbo.LabFormatacaoInformacao", new[] { "FormatacaoId" });
            DropIndex("dbo.LabFormatacaoInformacao", new[] { "InformacaoId" });
            DropIndex("dbo.Coleta", new[] { "TecnicoId" });
            DropIndex("dbo.Coleta", new[] { "AtendimentoId" });
            DropIndex("dbo.LabColetaExame", new[] { "ColetaId" });
            DropIndex("dbo.LabColetaExame", new[] { "MaterialId" });
            DropIndex("dbo.LabColetaExameInformacao", new[] { "ColetaExameId" });
            DropIndex("dbo.LabColetaExameInformacao", new[] { "FormatacaoId" });
            DropIndex("dbo.LabColetaExameInformacao", new[] { "InformacaoId" });
            DropIndex("dbo.LabColetaExameInformacao", new[] { "TabelaResultadoId" });
            DropIndex("dbo.LabColetaExameInformacao", new[] { "LabUnidadeId" });
            DropTable("dbo.LabMetodo");
            DropTable("dbo.LabTipoResultado");
            DropTable("dbo.LabTabelaResultadoItem");
            DropTable("dbo.LabTabelaResultado");
            DropTable("dbo.LabUnidade");
            DropTable("dbo.LabEquipamento");
            DropTable("dbo.LabInformacao");
            DropTable("dbo.LabFormatacaoInformacao");
            DropTable("dbo.LabFormatacao");
            DropTable("dbo.LabTecnico");
            DropTable("dbo.Coleta");
            DropTable("dbo.LabColetaExame");
            DropTable("dbo.LabColetaExameInformacao");
        }
    }
}
