namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Excluindo_Tabelas_Laboratorio_Fora_da_Documentacao : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Coleta", "AtendimentoId", "dbo.AteAtendimento");
            DropForeignKey("dbo.LabColetaExame", "ColetaId", "dbo.Coleta");
            DropForeignKey("dbo.LabColetaExameInformacao", "ColetaExameId", "dbo.LabColetaExame");
            DropForeignKey("dbo.LabColetaExame", "MaterialId", "dbo.LabMaterial");
            DropForeignKey("dbo.LabColetaExameInformacao", "FormatacaoId", "dbo.LabFormatacao");
            DropForeignKey("dbo.LabFormatacaoInformacao", "InformacaoId", "dbo.LabInformacao");
            DropForeignKey("dbo.LabFormatacaoInformacao", "FormatacaoId", "dbo.LabFormatacao");
            DropForeignKey("dbo.LabColetaExameInformacao", "InformacaoId", "dbo.LabInformacao");
            DropForeignKey("dbo.LabInformacao", "TabelaResultado_Id", "dbo.LabTabelaResultado");
            DropForeignKey("dbo.LabTabelaResultadoItem", "TabelaResultadoId", "dbo.LabTabelaResultado");
            DropForeignKey("dbo.LabColetaExameInformacao", "TabelaResultadoId", "dbo.LabTabelaResultado");
            DropForeignKey("dbo.LabEquipamento", "Informacao_Id", "dbo.LabInformacao");
            DropIndex("dbo.LabColetaExameInformacao", new[] { "TabelaResultadoId" });
            DropIndex("dbo.LabColetaExameInformacao", new[] { "InformacaoId" });
            DropIndex("dbo.LabColetaExameInformacao", new[] { "FormatacaoId" });
            DropIndex("dbo.LabColetaExameInformacao", new[] { "ColetaExameId" });
            DropIndex("dbo.LabColetaExame", new[] { "MaterialId" });
            DropIndex("dbo.LabColetaExame", new[] { "ColetaId" });
            DropIndex("dbo.Coleta", new[] { "AtendimentoId" });
            DropIndex("dbo.LabFormatacaoInformacao", new[] { "InformacaoId" });
            DropIndex("dbo.LabFormatacaoInformacao", new[] { "FormatacaoId" });
            DropIndex("dbo.LabInformacao", new[] { "TabelaResultado_Id" });
            DropIndex("dbo.LabTabelaResultadoItem", new[] { "TabelaResultadoId" });
            DropIndex("dbo.LabEquipamento", new[] { "Informacao_Id" });
            DropColumn("dbo.LabEquipamento", "Informacao_Id");
            DropTable("dbo.LabColetaExameInformacao");
            DropTable("dbo.LabColetaExame");
            DropTable("dbo.Coleta");
            DropTable("dbo.LabFormatacao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Formatacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.LabFormatacaoInformacao");
            DropTable("dbo.LabInformacao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Informacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.LabTabelaResultadoItem");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.LabTabelaResultadoItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(),
                    Descricao = c.String(),
                    TabelaResultadoId = c.Long(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.LabInformacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    CasaDecimal = c.Long(nullable: false),
                    Minimo = c.Double(nullable: false),
                    Maximo = c.Double(nullable: false),
                    MinimoExistente = c.Double(nullable: false),
                    MaximoExistente = c.Double(nullable: false),
                    Referencia = c.String(),
                    IsSoma100 = c.Boolean(nullable: false),
                    ObsAnormal = c.String(),
                    Islongerface = c.Boolean(nullable: false),
                    LongerfaceEnvio = c.String(),
                    LongerfaceRetorno = c.String(),
                    Dividelongerface = c.String(),
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
                    TabelaResultado_Id = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Informacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
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
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.LabFormatacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    NomeFormatacao = c.String(),
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
                    { "DynamicFilter_Formatacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

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
                .PrimaryKey(t => t.Id);

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
                .PrimaryKey(t => t.Id);

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
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.LabEquipamento", "Informacao_Id", c => c.Long());
            CreateIndex("dbo.LabEquipamento", "Informacao_Id");
            CreateIndex("dbo.LabTabelaResultadoItem", "TabelaResultadoId");
            CreateIndex("dbo.LabInformacao", "TabelaResultado_Id");
            CreateIndex("dbo.LabFormatacaoInformacao", "FormatacaoId");
            CreateIndex("dbo.LabFormatacaoInformacao", "InformacaoId");
            CreateIndex("dbo.Coleta", "AtendimentoId");
            CreateIndex("dbo.LabColetaExame", "ColetaId");
            CreateIndex("dbo.LabColetaExame", "MaterialId");
            CreateIndex("dbo.LabColetaExameInformacao", "ColetaExameId");
            CreateIndex("dbo.LabColetaExameInformacao", "FormatacaoId");
            CreateIndex("dbo.LabColetaExameInformacao", "InformacaoId");
            CreateIndex("dbo.LabColetaExameInformacao", "TabelaResultadoId");
            AddForeignKey("dbo.LabEquipamento", "Informacao_Id", "dbo.LabInformacao", "Id");
            AddForeignKey("dbo.LabColetaExameInformacao", "TabelaResultadoId", "dbo.LabTabelaResultado", "Id");
            AddForeignKey("dbo.LabTabelaResultadoItem", "TabelaResultadoId", "dbo.LabTabelaResultado", "Id");
            AddForeignKey("dbo.LabInformacao", "TabelaResultado_Id", "dbo.LabTabelaResultado", "Id");
            AddForeignKey("dbo.LabColetaExameInformacao", "InformacaoId", "dbo.LabInformacao", "Id");
            AddForeignKey("dbo.LabFormatacaoInformacao", "FormatacaoId", "dbo.LabFormatacao", "Id");
            AddForeignKey("dbo.LabFormatacaoInformacao", "InformacaoId", "dbo.LabInformacao", "Id");
            AddForeignKey("dbo.LabColetaExameInformacao", "FormatacaoId", "dbo.LabFormatacao", "Id");
            AddForeignKey("dbo.LabColetaExame", "MaterialId", "dbo.LabMaterial", "Id");
            AddForeignKey("dbo.LabColetaExameInformacao", "ColetaExameId", "dbo.LabColetaExame", "Id");
            AddForeignKey("dbo.LabColetaExame", "ColetaId", "dbo.Coleta", "Id");
            AddForeignKey("dbo.Coleta", "AtendimentoId", "dbo.AteAtendimento", "Id", cascadeDelete: true);
        }
    }
}
