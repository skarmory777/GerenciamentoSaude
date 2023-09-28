namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Criando_Atualizando_Tabelas_Compras : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CmpCotacaoItem", "FornecedorId", "dbo.Fornecedor");
            DropForeignKey("dbo.CmpRequisicao", "SisMedicoId", "dbo.SisMedico");
            DropForeignKey("dbo.CmpRequisicao", "SisPacienteId", "dbo.SisPaciente");
            DropForeignKey("dbo.CmpRequisicao", "EstEstoqueId", "dbo.Est_Estoque");
            DropIndex("dbo.CmpCotacaoItem", new[] { "FornecedorId" });
            DropIndex("dbo.CmpRequisicao", new[] { "SisMedicoId" });
            DropIndex("dbo.CmpRequisicao", new[] { "SisPacienteId" });
            DropIndex("dbo.CmpRequisicao", new[] { "EstEstoqueId" });
            CreateTable(
                "dbo.CmpOrdemCompra",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PrazoPagamento = c.Int(nullable: false),
                        DataOrdemCompra = c.DateTime(nullable: false),
                        DataPrevistaEntrega = c.DateTime(nullable: false),
                        DataFinalEntrega = c.DateTime(nullable: false),
                        ValorFrete = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EnderecoEntrega = c.String(),
                        ValorDesconto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CmpEmpresaId = c.Long(nullable: false),
                        SisUnidadeOrganizacionalId = c.Long(),
                        CmpOrdemCompraStatusId = c.Long(nullable: false),
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
                    { "DynamicFilter_CmpOrdemCompra_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisEmpresa", t => t.CmpEmpresaId, cascadeDelete: true)
                .ForeignKey("dbo.CmpOrdemCompraStatus", t => t.CmpOrdemCompraStatusId, cascadeDelete: true)
                .ForeignKey("dbo.SisUnidadeOrganizacional", t => t.SisUnidadeOrganizacionalId)
                .Index(t => t.CmpEmpresaId)
                .Index(t => t.SisUnidadeOrganizacionalId)
                .Index(t => t.CmpOrdemCompraStatusId);
            
            CreateTable(
                "dbo.CmpOrdemCompraStatus",
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
                    { "DynamicFilter_OrdemCompraStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CmpCotacao",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CmpRequisicaoId = c.Long(nullable: false),
                        SisFornecedorId = c.Long(),
                        FinFormaPagamentoId = c.Long(),
                        PrazoEntregaEmDias = c.Int(),
                        DataEnvioBionexo = c.DateTime(),
                        UserIdEnvioBionexo = c.Long(),
                        IdBionexo = c.Long(),
                        MensagemErroRetornoBionexo = c.String(),
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
                    { "DynamicFilter_CompraCotacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FinFormaPagamento", t => t.FinFormaPagamentoId)
                .ForeignKey("dbo.SisFornecedor", t => t.SisFornecedorId)
                .ForeignKey("dbo.CmpRequisicao", t => t.CmpRequisicaoId, cascadeDelete: false)
                .Index(t => t.CmpRequisicaoId)
                .Index(t => t.SisFornecedorId)
                .Index(t => t.FinFormaPagamentoId);
            
            CreateTable(
                "dbo.CmpOrdemCompraItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CmpOrdemCompraId = c.Long(nullable: false),
                        CmpRequisicaoItemId = c.Long(),
                        ValorUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantidade = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValorTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnidadeId = c.Long(nullable: false),
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
                    { "DynamicFilter_OrdemCompraItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CmpOrdemCompra", t => t.CmpOrdemCompraId, cascadeDelete: true)
                .ForeignKey("dbo.CmpRequisicaoItem", t => t.CmpRequisicaoItemId)
                .ForeignKey("dbo.Est_Unidade", t => t.UnidadeId, cascadeDelete: true)
                .Index(t => t.CmpOrdemCompraId)
                .Index(t => t.CmpRequisicaoItemId)
                .Index(t => t.UnidadeId);
            
            AddColumn("dbo.CmpAprovacaoStatus", "IsStatusRequisicao", c => c.Boolean());
            AddColumn("dbo.CmpAprovacaoStatus", "IsStatusCotacao", c => c.Boolean());
            AddColumn("dbo.CmpCotacaoItem", "CmpCompraCotacaoId", c => c.Long(nullable: false));
            AddColumn("dbo.CmpCotacaoItem", "ValorUnitario", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.CmpCotacaoItem", "Quantidade", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.CmpCotacaoItem", "LaboratorioId", c => c.Long());
            AddColumn("dbo.CmpCotacaoItem", "OpcaoComprador", c => c.Boolean(nullable: false));
            AddColumn("dbo.CmpCotacaoItem", "PrazoEntregaEmDias", c => c.Int());
            AddColumn("dbo.CmpRequisicao", "FinFormaPagamentoId", c => c.Long());
            AddColumn("dbo.CmpRequisicao", "IsRequisicaoAprovada", c => c.Boolean(nullable: false));
            AddColumn("dbo.CmpRequisicao", "IsOrdemCompraFinalizada", c => c.Boolean(nullable: false));
            AddColumn("dbo.CmpRequisicao", "DataRequisicao", c => c.DateTime(nullable: false));
            AddColumn("dbo.CmpRequisicao", "DataLimiteEntrega", c => c.DateTime(nullable: false));
            AddColumn("dbo.CmpRequisicao", "DataInicioCotacao", c => c.DateTime());
            AddColumn("dbo.CmpRequisicao", "DataFinalCotacao", c => c.DateTime());
            AddColumn("dbo.CmpRequisicao", "DataHoraVencimento", c => c.DateTime());
            AddColumn("dbo.FatGrupoConvenio", "IsCobrancaDia", c => c.Boolean());
            AddColumn("dbo.FinFormaPagamento", "CodigoBionexo", c => c.Long());
            AlterColumn("dbo.CmpRequisicao", "EstEstoqueId", c => c.Long());
            CreateIndex("dbo.CmpRequisicao", "FinFormaPagamentoId");
            CreateIndex("dbo.CmpRequisicao", "EstEstoqueId");
            CreateIndex("dbo.CmpCotacaoItem", "CmpCompraCotacaoId");
            CreateIndex("dbo.CmpCotacaoItem", "LaboratorioId");
            AddForeignKey("dbo.CmpRequisicao", "FinFormaPagamentoId", "dbo.FinFormaPagamento", "Id");
            AddForeignKey("dbo.CmpCotacaoItem", "CmpCompraCotacaoId", "dbo.CmpCotacao", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CmpCotacaoItem", "LaboratorioId", "dbo.EstLaboratorio", "Id");
            AddForeignKey("dbo.CmpRequisicao", "EstEstoqueId", "dbo.Est_Estoque", "Id");
            DropColumn("dbo.CmpCotacaoItem", "FornecedorId");
            DropColumn("dbo.CmpCotacaoItem", "Preco");
            DropColumn("dbo.CmpRequisicao", "IsEncerrada");
            DropColumn("dbo.CmpRequisicao", "IsAprovada");
            DropColumn("dbo.CmpRequisicao", "DataEmissao");
            DropColumn("dbo.CmpRequisicao", "SisMedicoId");
            DropColumn("dbo.CmpRequisicao", "SisPacienteId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CmpRequisicao", "SisPacienteId", c => c.Long());
            AddColumn("dbo.CmpRequisicao", "SisMedicoId", c => c.Long());
            AddColumn("dbo.CmpRequisicao", "DataEmissao", c => c.DateTime(nullable: false));
            AddColumn("dbo.CmpRequisicao", "IsAprovada", c => c.Boolean(nullable: false));
            AddColumn("dbo.CmpRequisicao", "IsEncerrada", c => c.Boolean(nullable: false));
            AddColumn("dbo.CmpCotacaoItem", "Preco", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.CmpCotacaoItem", "FornecedorId", c => c.Long(nullable: false));
            DropForeignKey("dbo.CmpRequisicao", "EstEstoqueId", "dbo.Est_Estoque");
            DropForeignKey("dbo.CmpOrdemCompraItem", "UnidadeId", "dbo.Est_Unidade");
            DropForeignKey("dbo.CmpOrdemCompraItem", "CmpRequisicaoItemId", "dbo.CmpRequisicaoItem");
            DropForeignKey("dbo.CmpOrdemCompraItem", "CmpOrdemCompraId", "dbo.CmpOrdemCompra");
            DropForeignKey("dbo.CmpCotacaoItem", "LaboratorioId", "dbo.EstLaboratorio");
            DropForeignKey("dbo.CmpCotacaoItem", "CmpCompraCotacaoId", "dbo.CmpCotacao");
            DropForeignKey("dbo.CmpCotacao", "CmpRequisicaoId", "dbo.CmpRequisicao");
            DropForeignKey("dbo.CmpRequisicao", "FinFormaPagamentoId", "dbo.FinFormaPagamento");
            DropForeignKey("dbo.CmpCotacao", "SisFornecedorId", "dbo.SisFornecedor");
            DropForeignKey("dbo.CmpCotacao", "FinFormaPagamentoId", "dbo.FinFormaPagamento");
            DropForeignKey("dbo.CmpOrdemCompra", "SisUnidadeOrganizacionalId", "dbo.SisUnidadeOrganizacional");
            DropForeignKey("dbo.CmpOrdemCompra", "CmpOrdemCompraStatusId", "dbo.CmpOrdemCompraStatus");
            DropForeignKey("dbo.CmpOrdemCompra", "CmpEmpresaId", "dbo.SisEmpresa");
            DropIndex("dbo.CmpOrdemCompraItem", new[] { "UnidadeId" });
            DropIndex("dbo.CmpOrdemCompraItem", new[] { "CmpRequisicaoItemId" });
            DropIndex("dbo.CmpOrdemCompraItem", new[] { "CmpOrdemCompraId" });
            DropIndex("dbo.CmpCotacaoItem", new[] { "LaboratorioId" });
            DropIndex("dbo.CmpCotacaoItem", new[] { "CmpCompraCotacaoId" });
            DropIndex("dbo.CmpRequisicao", new[] { "EstEstoqueId" });
            DropIndex("dbo.CmpRequisicao", new[] { "FinFormaPagamentoId" });
            DropIndex("dbo.CmpCotacao", new[] { "FinFormaPagamentoId" });
            DropIndex("dbo.CmpCotacao", new[] { "SisFornecedorId" });
            DropIndex("dbo.CmpCotacao", new[] { "CmpRequisicaoId" });
            DropIndex("dbo.CmpOrdemCompra", new[] { "CmpOrdemCompraStatusId" });
            DropIndex("dbo.CmpOrdemCompra", new[] { "SisUnidadeOrganizacionalId" });
            DropIndex("dbo.CmpOrdemCompra", new[] { "CmpEmpresaId" });
            AlterColumn("dbo.CmpRequisicao", "EstEstoqueId", c => c.Long(nullable: false));
            DropColumn("dbo.FinFormaPagamento", "CodigoBionexo");
            DropColumn("dbo.FatGrupoConvenio", "IsCobrancaDia");
            DropColumn("dbo.CmpRequisicao", "DataHoraVencimento");
            DropColumn("dbo.CmpRequisicao", "DataFinalCotacao");
            DropColumn("dbo.CmpRequisicao", "DataInicioCotacao");
            DropColumn("dbo.CmpRequisicao", "DataLimiteEntrega");
            DropColumn("dbo.CmpRequisicao", "DataRequisicao");
            DropColumn("dbo.CmpRequisicao", "IsOrdemCompraFinalizada");
            DropColumn("dbo.CmpRequisicao", "IsRequisicaoAprovada");
            DropColumn("dbo.CmpRequisicao", "FinFormaPagamentoId");
            DropColumn("dbo.CmpCotacaoItem", "PrazoEntregaEmDias");
            DropColumn("dbo.CmpCotacaoItem", "OpcaoComprador");
            DropColumn("dbo.CmpCotacaoItem", "LaboratorioId");
            DropColumn("dbo.CmpCotacaoItem", "Quantidade");
            DropColumn("dbo.CmpCotacaoItem", "ValorUnitario");
            DropColumn("dbo.CmpCotacaoItem", "CmpCompraCotacaoId");
            DropColumn("dbo.CmpAprovacaoStatus", "IsStatusCotacao");
            DropColumn("dbo.CmpAprovacaoStatus", "IsStatusRequisicao");
            DropTable("dbo.CmpOrdemCompraItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_OrdemCompraItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.CmpCotacao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CompraCotacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.CmpOrdemCompraStatus",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_OrdemCompraStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.CmpOrdemCompra",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CmpOrdemCompra_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            CreateIndex("dbo.CmpRequisicao", "EstEstoqueId");
            CreateIndex("dbo.CmpRequisicao", "SisPacienteId");
            CreateIndex("dbo.CmpRequisicao", "SisMedicoId");
            CreateIndex("dbo.CmpCotacaoItem", "FornecedorId");
            AddForeignKey("dbo.CmpRequisicao", "EstEstoqueId", "dbo.Est_Estoque", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CmpRequisicao", "SisPacienteId", "dbo.SisPaciente", "Id");
            AddForeignKey("dbo.CmpRequisicao", "SisMedicoId", "dbo.SisMedico", "Id");
            AddForeignKey("dbo.CmpCotacaoItem", "FornecedorId", "dbo.Fornecedor", "Id", cascadeDelete: true);
        }
    }
}
