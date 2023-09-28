namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Fat_ContaKit_Item_Turno : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FaturamentoContaItemFaturamentoContas", "FaturamentoContaItem_Id", "dbo.FatContaItem");
            DropForeignKey("dbo.FaturamentoContaItemFaturamentoContas", "FaturamentoConta_Id", "dbo.FatConta");
            DropIndex("dbo.FaturamentoContaItemFaturamentoContas", new[] { "FaturamentoContaItem_Id" });
            DropIndex("dbo.FaturamentoContaItemFaturamentoContas", new[] { "FaturamentoConta_Id" });
            RenameColumn(table: "dbo.UnidadeOrganizacional", name: "CentroCusto_Id", newName: "CentroCustoId");
            RenameIndex(table: "dbo.UnidadeOrganizacional", name: "IX_CentroCusto_Id", newName: "IX_CentroCustoId");
            CreateTable(
                "dbo.FatContaKit",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 100),
                    FatContaId = c.Long(),
                    Data = c.DateTime(),
                    Qtde = c.Single(nullable: false),
                    CentroCustoId = c.Long(),
                    TurnoId = c.Long(),
                    TipoLeitoId = c.Long(),
                    HoraIncio = c.DateTime(),
                    HoraFim = c.DateTime(),
                    MedicoId = c.Long(),
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
                    { "DynamicFilter_FaturamentoContaKit_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CentroCusto", t => t.CentroCustoId)
                .ForeignKey("dbo.FatConta", t => t.FatContaId)
                .ForeignKey("dbo.Medico", t => t.MedicoId)
                .ForeignKey("dbo.TipoLeito", t => t.TipoLeitoId)
                .ForeignKey("dbo.Turno", t => t.TurnoId)
                .Index(t => t.FatContaId)
                .Index(t => t.CentroCustoId)
                .Index(t => t.TurnoId)
                .Index(t => t.TipoLeitoId)
                .Index(t => t.MedicoId);

            CreateTable(
                "dbo.Turno",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
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
                    { "DynamicFilter_Turno_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.UnidadeOrganizacional", "IsLocalUtilizacao", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatConta", "Matricula", c => c.String(maxLength: 20));
            AddColumn("dbo.FatConta", "CodDependente", c => c.String(maxLength: 20));
            AddColumn("dbo.FatConta", "NumeroGuia", c => c.String(maxLength: 20));
            AddColumn("dbo.FatConta", "Titular", c => c.String(maxLength: 100));
            AddColumn("dbo.FatConta", "SisMedicoId", c => c.Long());
            AddColumn("dbo.FatConta", "PlanoId", c => c.Long());
            AddColumn("dbo.FatConta", "FatGuiaId", c => c.Long());
            AddColumn("dbo.FatConta", "DataPagamento", c => c.DateTime());
            AddColumn("dbo.FatConta", "ValidadeCarteira", c => c.DateTime());
            AddColumn("dbo.FatConta", "DataAutorizacao", c => c.DateTime());
            AddColumn("dbo.FatConta", "DiaSerie1", c => c.DateTime());
            AddColumn("dbo.FatConta", "DiaSerie2", c => c.DateTime());
            AddColumn("dbo.FatConta", "DiaSerie3", c => c.DateTime());
            AddColumn("dbo.FatConta", "DiaSerie4", c => c.DateTime());
            AddColumn("dbo.FatConta", "DiaSerie5", c => c.DateTime());
            AddColumn("dbo.FatConta", "DiaSerie6", c => c.DateTime());
            AddColumn("dbo.FatConta", "DiaSerie7", c => c.DateTime());
            AddColumn("dbo.FatConta", "DiaSerie8", c => c.DateTime());
            AddColumn("dbo.FatConta", "DiaSerie9", c => c.DateTime());
            AddColumn("dbo.FatConta", "DiaSerie10", c => c.DateTime());
            AddColumn("dbo.FatConta", "DataEntrFolhaSala", c => c.DateTime());
            AddColumn("dbo.FatConta", "DataEntrDescCir", c => c.DateTime());
            AddColumn("dbo.FatConta", "DataEntrBolAnest", c => c.DateTime());
            AddColumn("dbo.FatConta", "DataEntrCDFilme", c => c.DateTime());
            AddColumn("dbo.FatConta", "DataValidadeSenha", c => c.DateTime());
            AddColumn("dbo.FatConta", "GuiaOperadora", c => c.String(maxLength: 30));
            AddColumn("dbo.FatConta", "GuiaPrincipal", c => c.String(maxLength: 20));
            AddColumn("dbo.FatConta", "TipoAtendimento", c => c.Int(nullable: false));
            AddColumn("dbo.FatConta", "IsAutorizador", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatConta", "TipoLeitoId", c => c.Long());
            AddColumn("dbo.FatConta", "Observacao", c => c.String());
            AddColumn("dbo.FatConta", "SenhaAutorizacao", c => c.String(maxLength: 20));
            AddColumn("dbo.FatConta", "IdentAcompanhante", c => c.String(maxLength: 20));
            AddColumn("dbo.FatConta", "StatusEntrega", c => c.Int(nullable: false));
            AddColumn("dbo.FatContaItem", "FatItemId", c => c.Long());
            AddColumn("dbo.FatContaItem", "FatContaId", c => c.Long());
            AddColumn("dbo.FatContaItem", "Data", c => c.DateTime());
            AddColumn("dbo.FatContaItem", "Qtde", c => c.Single(nullable: false));
            AddColumn("dbo.FatContaItem", "CentroCustoId", c => c.Long());
            AddColumn("dbo.FatContaItem", "TurnoId", c => c.Long());
            AddColumn("dbo.FatContaItem", "TipoLeitoId", c => c.Long());
            AddColumn("dbo.FatContaItem", "ValorTemp", c => c.Single(nullable: false));
            AddColumn("dbo.FatContaItem", "MedicoId", c => c.Long());
            AddColumn("dbo.FatContaItem", "IsMedCrendenciado", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatContaItem", "IsGlosaMedico", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatContaItem", "MedicoEspecialidadeId", c => c.Long());
            AddColumn("dbo.FatContaItem", "FaturamentoContaKitId", c => c.Long());
            AddColumn("dbo.FatContaItem", "IsCirurgia", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatContaItem", "ValorAprovado", c => c.Single(nullable: false));
            AddColumn("dbo.FatContaItem", "ValorTaxas", c => c.Single(nullable: false));
            AddColumn("dbo.FatContaItem", "IsValorItemManual", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatContaItem", "ValorItem", c => c.Single(nullable: false));
            AddColumn("dbo.FatContaItem", "HMCH", c => c.String());
            AddColumn("dbo.FatContaItem", "ValorFilme", c => c.Single(nullable: false));
            AddColumn("dbo.FatContaItem", "ValorFilmeAprovado", c => c.Single(nullable: false));
            AddColumn("dbo.FatContaItem", "ValorCOCH", c => c.Single(nullable: false));
            AddColumn("dbo.FatContaItem", "ValorCOCHAprovado", c => c.Single(nullable: false));
            AddColumn("dbo.FatContaItem", "Percentual", c => c.Single(nullable: false));
            AddColumn("dbo.FatContaItem", "IsInstrCredenciado", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatContaItem", "ValorTotalRecuperado", c => c.Single(nullable: false));
            AddColumn("dbo.FatContaItem", "ValorTotalRecebido", c => c.Single(nullable: false));
            AddColumn("dbo.FatContaItem", "MetragemFilme", c => c.Single(nullable: false));
            AddColumn("dbo.FatContaItem", "MetragemFilmeAprovada", c => c.Single(nullable: false));
            AddColumn("dbo.FatContaItem", "COCH", c => c.Single(nullable: false));
            AddColumn("dbo.FatContaItem", "COCHAprovado", c => c.Single(nullable: false));
            AddColumn("dbo.FatContaItem", "StatusEntrega", c => c.String());
            AddColumn("dbo.FatContaItem", "IsRecuperaMedico", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatContaItem", "IsAux1Credenciado", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatContaItem", "IsRecebeAuxiliar1", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatContaItem", "IsGlosaAuxiliar1", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatContaItem", "IsRecuperaAuxiliar1", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatContaItem", "IsAux2Credenciado", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatContaItem", "IsRecebeAuxiliar2", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatContaItem", "IsGlosaAuxiliar2", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatContaItem", "IsRecuperaAuxiliar2", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatContaItem", "IsAux3Credenciado", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatContaItem", "IsRecebeAuxiliar3", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatContaItem", "IsGlosaAuxiliar3", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatContaItem", "IsRecuperaAuxiliar3", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatContaItem", "IsRecebeInstrumentador", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatContaItem", "IsGlosaInstrumentador", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatContaItem", "IsRecuperaInstrumentador", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatContaItem", "Observacao", c => c.String());
            AddColumn("dbo.FatContaItem", "QtdeRecuperada", c => c.Int(nullable: false));
            AddColumn("dbo.FatContaItem", "QtdeAprovada", c => c.Int(nullable: false));
            AddColumn("dbo.FatContaItem", "QtdeRecebida", c => c.Int(nullable: false));
            AddColumn("dbo.FatContaItem", "ValorMoedaAprovado", c => c.Single(nullable: false));
            AddColumn("dbo.FatContaItem", "SisMoedaId", c => c.Long());
            AddColumn("dbo.FatContaItem", "DataAutorizacao", c => c.DateTime());
            AddColumn("dbo.FatContaItem", "SenhaAutorizacao", c => c.String());
            AddColumn("dbo.FatContaItem", "NomeAutorizacao", c => c.String());
            AddColumn("dbo.FatContaItem", "ObsAutorizacao", c => c.String());
            AddColumn("dbo.FatContaItem", "HoraIncio", c => c.DateTime());
            AddColumn("dbo.FatContaItem", "HoraFim", c => c.DateTime());
            AddColumn("dbo.FatContaItem", "ViaAcesso", c => c.String());
            AddColumn("dbo.FatContaItem", "Tecnica", c => c.String());
            AddColumn("dbo.FatContaItem", "ClinicaId", c => c.String());
            AddColumn("dbo.FatContaItem", "FornecedorId", c => c.Long());
            AddColumn("dbo.FatContaItem", "NumeroNF", c => c.String());
            AddColumn("dbo.FatContaItem", "IsImportaEstoque", c => c.Boolean(nullable: false));
            CreateIndex("dbo.FatConta", "SisMedicoId");
            CreateIndex("dbo.FatConta", "PlanoId");
            CreateIndex("dbo.FatConta", "FatGuiaId");
            CreateIndex("dbo.FatConta", "TipoLeitoId");
            CreateIndex("dbo.FatContaItem", "FatItemId");
            CreateIndex("dbo.FatContaItem", "FatContaId");
            CreateIndex("dbo.FatContaItem", "CentroCustoId");
            CreateIndex("dbo.FatContaItem", "TurnoId");
            CreateIndex("dbo.FatContaItem", "TipoLeitoId");
            CreateIndex("dbo.FatContaItem", "MedicoId");
            CreateIndex("dbo.FatContaItem", "MedicoEspecialidadeId");
            CreateIndex("dbo.FatContaItem", "FaturamentoContaKitId");
            CreateIndex("dbo.FatContaItem", "SisMoedaId");
            CreateIndex("dbo.FatContaItem", "FornecedorId");
            AddForeignKey("dbo.FatConta", "FatGuiaId", "dbo.Guia", "Id");
            AddForeignKey("dbo.FatConta", "SisMedicoId", "dbo.Medico", "Id");
            AddForeignKey("dbo.FatConta", "PlanoId", "dbo.Plano", "Id");
            AddForeignKey("dbo.FatConta", "TipoLeitoId", "dbo.TipoLeito", "Id");
            AddForeignKey("dbo.FatContaItem", "CentroCustoId", "dbo.CentroCusto", "Id");
            AddForeignKey("dbo.FatContaItem", "FatContaId", "dbo.FatConta", "Id");
            AddForeignKey("dbo.FatContaItem", "FaturamentoContaKitId", "dbo.FatContaKit", "Id");
            AddForeignKey("dbo.FatContaItem", "FatItemId", "dbo.FatItem", "Id");
            AddForeignKey("dbo.FatContaItem", "FornecedorId", "dbo.Fornecedor", "Id");
            AddForeignKey("dbo.FatContaItem", "MedicoId", "dbo.Medico", "Id");
            AddForeignKey("dbo.FatContaItem", "MedicoEspecialidadeId", "dbo.MedicoEspecialidade", "Id");
            AddForeignKey("dbo.FatContaItem", "SisMoedaId", "dbo.SisMoeda", "Id");
            AddForeignKey("dbo.FatContaItem", "TipoLeitoId", "dbo.TipoLeito", "Id");
            AddForeignKey("dbo.FatContaItem", "TurnoId", "dbo.Turno", "Id");
            DropColumn("dbo.FatConta", "Descricao");
            DropTable("dbo.FaturamentoContaItemFaturamentoContas");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.FaturamentoContaItemFaturamentoContas",
                c => new
                {
                    FaturamentoContaItem_Id = c.Long(nullable: false),
                    FaturamentoConta_Id = c.Long(nullable: false),
                })
                .PrimaryKey(t => new { t.FaturamentoContaItem_Id, t.FaturamentoConta_Id });

            AddColumn("dbo.FatConta", "Descricao", c => c.String(maxLength: 100));
            DropForeignKey("dbo.FatContaItem", "TurnoId", "dbo.Turno");
            DropForeignKey("dbo.FatContaItem", "TipoLeitoId", "dbo.TipoLeito");
            DropForeignKey("dbo.FatContaItem", "SisMoedaId", "dbo.SisMoeda");
            DropForeignKey("dbo.FatContaItem", "MedicoEspecialidadeId", "dbo.MedicoEspecialidade");
            DropForeignKey("dbo.FatContaItem", "MedicoId", "dbo.Medico");
            DropForeignKey("dbo.FatContaItem", "FornecedorId", "dbo.Fornecedor");
            DropForeignKey("dbo.FatContaItem", "FatItemId", "dbo.FatItem");
            DropForeignKey("dbo.FatContaItem", "FaturamentoContaKitId", "dbo.FatContaKit");
            DropForeignKey("dbo.FatContaKit", "TurnoId", "dbo.Turno");
            DropForeignKey("dbo.FatContaKit", "TipoLeitoId", "dbo.TipoLeito");
            DropForeignKey("dbo.FatContaKit", "MedicoId", "dbo.Medico");
            DropForeignKey("dbo.FatContaKit", "FatContaId", "dbo.FatConta");
            DropForeignKey("dbo.FatContaKit", "CentroCustoId", "dbo.CentroCusto");
            DropForeignKey("dbo.FatContaItem", "FatContaId", "dbo.FatConta");
            DropForeignKey("dbo.FatContaItem", "CentroCustoId", "dbo.CentroCusto");
            DropForeignKey("dbo.FatConta", "TipoLeitoId", "dbo.TipoLeito");
            DropForeignKey("dbo.FatConta", "PlanoId", "dbo.Plano");
            DropForeignKey("dbo.FatConta", "SisMedicoId", "dbo.Medico");
            DropForeignKey("dbo.FatConta", "FatGuiaId", "dbo.Guia");
            DropIndex("dbo.FatContaKit", new[] { "MedicoId" });
            DropIndex("dbo.FatContaKit", new[] { "TipoLeitoId" });
            DropIndex("dbo.FatContaKit", new[] { "TurnoId" });
            DropIndex("dbo.FatContaKit", new[] { "CentroCustoId" });
            DropIndex("dbo.FatContaKit", new[] { "FatContaId" });
            DropIndex("dbo.FatContaItem", new[] { "FornecedorId" });
            DropIndex("dbo.FatContaItem", new[] { "SisMoedaId" });
            DropIndex("dbo.FatContaItem", new[] { "FaturamentoContaKitId" });
            DropIndex("dbo.FatContaItem", new[] { "MedicoEspecialidadeId" });
            DropIndex("dbo.FatContaItem", new[] { "MedicoId" });
            DropIndex("dbo.FatContaItem", new[] { "TipoLeitoId" });
            DropIndex("dbo.FatContaItem", new[] { "TurnoId" });
            DropIndex("dbo.FatContaItem", new[] { "CentroCustoId" });
            DropIndex("dbo.FatContaItem", new[] { "FatContaId" });
            DropIndex("dbo.FatContaItem", new[] { "FatItemId" });
            DropIndex("dbo.FatConta", new[] { "TipoLeitoId" });
            DropIndex("dbo.FatConta", new[] { "FatGuiaId" });
            DropIndex("dbo.FatConta", new[] { "PlanoId" });
            DropIndex("dbo.FatConta", new[] { "SisMedicoId" });
            DropColumn("dbo.FatContaItem", "IsImportaEstoque");
            DropColumn("dbo.FatContaItem", "NumeroNF");
            DropColumn("dbo.FatContaItem", "FornecedorId");
            DropColumn("dbo.FatContaItem", "ClinicaId");
            DropColumn("dbo.FatContaItem", "Tecnica");
            DropColumn("dbo.FatContaItem", "ViaAcesso");
            DropColumn("dbo.FatContaItem", "HoraFim");
            DropColumn("dbo.FatContaItem", "HoraIncio");
            DropColumn("dbo.FatContaItem", "ObsAutorizacao");
            DropColumn("dbo.FatContaItem", "NomeAutorizacao");
            DropColumn("dbo.FatContaItem", "SenhaAutorizacao");
            DropColumn("dbo.FatContaItem", "DataAutorizacao");
            DropColumn("dbo.FatContaItem", "SisMoedaId");
            DropColumn("dbo.FatContaItem", "ValorMoedaAprovado");
            DropColumn("dbo.FatContaItem", "QtdeRecebida");
            DropColumn("dbo.FatContaItem", "QtdeAprovada");
            DropColumn("dbo.FatContaItem", "QtdeRecuperada");
            DropColumn("dbo.FatContaItem", "Observacao");
            DropColumn("dbo.FatContaItem", "IsRecuperaInstrumentador");
            DropColumn("dbo.FatContaItem", "IsGlosaInstrumentador");
            DropColumn("dbo.FatContaItem", "IsRecebeInstrumentador");
            DropColumn("dbo.FatContaItem", "IsRecuperaAuxiliar3");
            DropColumn("dbo.FatContaItem", "IsGlosaAuxiliar3");
            DropColumn("dbo.FatContaItem", "IsRecebeAuxiliar3");
            DropColumn("dbo.FatContaItem", "IsAux3Credenciado");
            DropColumn("dbo.FatContaItem", "IsRecuperaAuxiliar2");
            DropColumn("dbo.FatContaItem", "IsGlosaAuxiliar2");
            DropColumn("dbo.FatContaItem", "IsRecebeAuxiliar2");
            DropColumn("dbo.FatContaItem", "IsAux2Credenciado");
            DropColumn("dbo.FatContaItem", "IsRecuperaAuxiliar1");
            DropColumn("dbo.FatContaItem", "IsGlosaAuxiliar1");
            DropColumn("dbo.FatContaItem", "IsRecebeAuxiliar1");
            DropColumn("dbo.FatContaItem", "IsAux1Credenciado");
            DropColumn("dbo.FatContaItem", "IsRecuperaMedico");
            DropColumn("dbo.FatContaItem", "StatusEntrega");
            DropColumn("dbo.FatContaItem", "COCHAprovado");
            DropColumn("dbo.FatContaItem", "COCH");
            DropColumn("dbo.FatContaItem", "MetragemFilmeAprovada");
            DropColumn("dbo.FatContaItem", "MetragemFilme");
            DropColumn("dbo.FatContaItem", "ValorTotalRecebido");
            DropColumn("dbo.FatContaItem", "ValorTotalRecuperado");
            DropColumn("dbo.FatContaItem", "IsInstrCredenciado");
            DropColumn("dbo.FatContaItem", "Percentual");
            DropColumn("dbo.FatContaItem", "ValorCOCHAprovado");
            DropColumn("dbo.FatContaItem", "ValorCOCH");
            DropColumn("dbo.FatContaItem", "ValorFilmeAprovado");
            DropColumn("dbo.FatContaItem", "ValorFilme");
            DropColumn("dbo.FatContaItem", "HMCH");
            DropColumn("dbo.FatContaItem", "ValorItem");
            DropColumn("dbo.FatContaItem", "IsValorItemManual");
            DropColumn("dbo.FatContaItem", "ValorTaxas");
            DropColumn("dbo.FatContaItem", "ValorAprovado");
            DropColumn("dbo.FatContaItem", "IsCirurgia");
            DropColumn("dbo.FatContaItem", "FaturamentoContaKitId");
            DropColumn("dbo.FatContaItem", "MedicoEspecialidadeId");
            DropColumn("dbo.FatContaItem", "IsGlosaMedico");
            DropColumn("dbo.FatContaItem", "IsMedCrendenciado");
            DropColumn("dbo.FatContaItem", "MedicoId");
            DropColumn("dbo.FatContaItem", "ValorTemp");
            DropColumn("dbo.FatContaItem", "TipoLeitoId");
            DropColumn("dbo.FatContaItem", "TurnoId");
            DropColumn("dbo.FatContaItem", "CentroCustoId");
            DropColumn("dbo.FatContaItem", "Qtde");
            DropColumn("dbo.FatContaItem", "Data");
            DropColumn("dbo.FatContaItem", "FatContaId");
            DropColumn("dbo.FatContaItem", "FatItemId");
            DropColumn("dbo.FatConta", "StatusEntrega");
            DropColumn("dbo.FatConta", "IdentAcompanhante");
            DropColumn("dbo.FatConta", "SenhaAutorizacao");
            DropColumn("dbo.FatConta", "Observacao");
            DropColumn("dbo.FatConta", "TipoLeitoId");
            DropColumn("dbo.FatConta", "IsAutorizador");
            DropColumn("dbo.FatConta", "TipoAtendimento");
            DropColumn("dbo.FatConta", "GuiaPrincipal");
            DropColumn("dbo.FatConta", "GuiaOperadora");
            DropColumn("dbo.FatConta", "DataValidadeSenha");
            DropColumn("dbo.FatConta", "DataEntrCDFilme");
            DropColumn("dbo.FatConta", "DataEntrBolAnest");
            DropColumn("dbo.FatConta", "DataEntrDescCir");
            DropColumn("dbo.FatConta", "DataEntrFolhaSala");
            DropColumn("dbo.FatConta", "DiaSerie10");
            DropColumn("dbo.FatConta", "DiaSerie9");
            DropColumn("dbo.FatConta", "DiaSerie8");
            DropColumn("dbo.FatConta", "DiaSerie7");
            DropColumn("dbo.FatConta", "DiaSerie6");
            DropColumn("dbo.FatConta", "DiaSerie5");
            DropColumn("dbo.FatConta", "DiaSerie4");
            DropColumn("dbo.FatConta", "DiaSerie3");
            DropColumn("dbo.FatConta", "DiaSerie2");
            DropColumn("dbo.FatConta", "DiaSerie1");
            DropColumn("dbo.FatConta", "DataAutorizacao");
            DropColumn("dbo.FatConta", "ValidadeCarteira");
            DropColumn("dbo.FatConta", "DataPagamento");
            DropColumn("dbo.FatConta", "FatGuiaId");
            DropColumn("dbo.FatConta", "PlanoId");
            DropColumn("dbo.FatConta", "SisMedicoId");
            DropColumn("dbo.FatConta", "Titular");
            DropColumn("dbo.FatConta", "NumeroGuia");
            DropColumn("dbo.FatConta", "CodDependente");
            DropColumn("dbo.FatConta", "Matricula");
            DropColumn("dbo.UnidadeOrganizacional", "IsLocalUtilizacao");
            DropTable("dbo.Turno",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Turno_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FatContaKit",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoContaKit_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            RenameIndex(table: "dbo.UnidadeOrganizacional", name: "IX_CentroCustoId", newName: "IX_CentroCusto_Id");
            RenameColumn(table: "dbo.UnidadeOrganizacional", name: "CentroCustoId", newName: "CentroCusto_Id");
            CreateIndex("dbo.FaturamentoContaItemFaturamentoContas", "FaturamentoConta_Id");
            CreateIndex("dbo.FaturamentoContaItemFaturamentoContas", "FaturamentoContaItem_Id");
            AddForeignKey("dbo.FaturamentoContaItemFaturamentoContas", "FaturamentoConta_Id", "dbo.FatConta", "Id", cascadeDelete: false);
            AddForeignKey("dbo.FaturamentoContaItemFaturamentoContas", "FaturamentoContaItem_Id", "dbo.FatContaItem", "Id", cascadeDelete: false);
        }
    }
}
