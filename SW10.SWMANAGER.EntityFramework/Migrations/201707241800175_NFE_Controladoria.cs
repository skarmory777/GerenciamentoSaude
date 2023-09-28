namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class NFE_Controladoria : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.NotaFiscalautXML", "NotaFiscalId", "dbo.NotaFiscal");
            DropForeignKey("dbo.NotaFiscal", "Avulsa_Id", "dbo.NotaFiscalAvulsa");
            DropForeignKey("dbo.NotaFiscalcanadeduc", "NotaFiscalcanaId", "dbo.NotaFiscalcana");
            DropForeignKey("dbo.NotaFiscalcanaforDia", "NotaFiscalcanaId", "dbo.NotaFiscalcana");
            DropForeignKey("dbo.NotaFiscal", "Cana_Id", "dbo.NotaFiscalcana");
            DropForeignKey("dbo.NotaFiscalCobrancaDuplicata", "NotaFiscalCobrancaId", "dbo.NotaFiscalCobranca");
            DropForeignKey("dbo.NotaFiscalCobranca", "NotaFiscalCobrancaFaturaId", "dbo.NotaFiscalCobrancaFatura");
            DropForeignKey("dbo.NotaFiscalCobranca", "NotaFiscalId", "dbo.NotaFiscal");
            DropForeignKey("dbo.NotaFiscal", "Compra_Id", "dbo.NotaFiscalcompra");
            DropForeignKey("dbo.NotaFiscalDestinatario", "NotaFiscalEnderecoDestinatarioId", "dbo.NotaFiscalEnderecoDestinatario");
            DropForeignKey("dbo.NotaFiscal", "Destinatario_Id", "dbo.NotaFiscalDestinatario");
            DropForeignKey("dbo.NotaFiscalCOFINS", "TipoCOFINS_Id", "dbo.NotaFiscalCOFINSBasico");
            DropForeignKey("dbo.NotaFiscalImposto", "COFINS_Id", "dbo.NotaFiscalCOFINS");
            DropForeignKey("dbo.NotaFiscalImposto", "COFINSST_Id", "dbo.NotaFiscalCOFINSST");
            DropForeignKey("dbo.NotaFiscalICMS", "TipoICMS_Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalImposto", "ICMS_Id", "dbo.NotaFiscalICMS");
            DropForeignKey("dbo.NotaFiscalImposto", "ICMSUFDest_Id", "dbo.NotaFiscalICMSUFDest");
            DropForeignKey("dbo.NotaFiscalImposto", "II_Id", "dbo.NotaFiscalICMSImpostoImportacao");
            DropForeignKey("dbo.NotaFiscalIPI", "TipoIPI_Id", "dbo.NotaFiscalIPIBasico");
            DropForeignKey("dbo.NotaFiscalImposto", "IPI_Id", "dbo.NotaFiscalIPI");
            DropForeignKey("dbo.NotaFiscalImposto", "ISSQN_Id", "dbo.NotaFiscalISSQN");
            DropForeignKey("dbo.NotaFiscalPIS", "TipoPIS_Id", "dbo.NotaFiscalPISBasico");
            DropForeignKey("dbo.NotaFiscalImposto", "PIS_Id", "dbo.NotaFiscalPIS");
            DropForeignKey("dbo.NotaFiscalImposto", "PISST_Id", "dbo.NotaFiscalPISST");
            DropForeignKey("dbo.NotaFiscalDetalhe", "imposto_Id", "dbo.NotaFiscalImposto");
            DropForeignKey("dbo.NotaFiscalImpostoDevolvido", "IPI_Id", "dbo.NotaFiscalIPIDevolvido");
            DropForeignKey("dbo.NotaFiscalDetalhe", "impostoDevol_Id", "dbo.NotaFiscalImpostoDevolvido");
            DropForeignKey("dbo.NotaFiscaldetExport", "NotaFiscalexportIndId", "dbo.NotaFiscalexportInd");
            DropForeignKey("dbo.NotaFiscaldetExport", "NotaFiscalProdutoId", "dbo.NotaFiscalProduto");
            DropForeignKey("dbo.NotaFiscaladi", "NotaFiscalDI_Id", "dbo.NotaFiscalDI");
            DropForeignKey("dbo.NotaFiscalDI", "NotaFiscalProdutoId", "dbo.NotaFiscalProduto");
            DropForeignKey("dbo.NotaFiscalProduto", "NotaFiscalProdutoEspecificoId", "dbo.NotaFiscalProdutoEspecifico");
            DropForeignKey("dbo.NotaFiscalDetalhe", "prod_Id", "dbo.NotaFiscalProduto");
            DropForeignKey("dbo.NotaFiscalDetalhe", "NotaFiscalId", "dbo.NotaFiscal");
            DropForeignKey("dbo.NotaFiscalEmitente", "NotaFiscalEnderecoEmitenteId", "dbo.NotaFiscalEnderecoEmitente");
            DropForeignKey("dbo.NotaFiscal", "Emitente_Id", "dbo.NotaFiscalEmitente");
            DropForeignKey("dbo.NotaFiscal", "Entrega_Id", "dbo.NotaFiscalEntrega");
            DropForeignKey("dbo.NotaFiscal", "Exporta_Id", "dbo.NotaFiscalexporta");
            DropForeignKey("dbo.NotaFiscalNFref", "refECF_Id", "dbo.NotaFiscalrefECF");
            DropForeignKey("dbo.NotaFiscalNFref", "refNF_Id", "dbo.NotaFiscalrefNF");
            DropForeignKey("dbo.NotaFiscalNFref", "refNFP_Id", "dbo.NotaFiscalrefNFP");
            DropForeignKey("dbo.NotaFiscalNFref", "NotaFiscalIdentificacaoId", "dbo.NotaFiscalIdentificacao");
            DropForeignKey("dbo.NotaFiscal", "Ide_Id", "dbo.NotaFiscalIdentificacao");
            DropForeignKey("dbo.NotaFiscalInformacaoAdicionalCont", "NotaFiscalInformacaoAdicionalId", "dbo.NotaFiscalInformacaoAdicional");
            DropForeignKey("dbo.NotaFiscalInformacaoAdicionalobsFisco", "NotaFiscalInformacaoAdicionalId", "dbo.NotaFiscalInformacaoAdicional");
            DropForeignKey("dbo.NotaFiscalInformacaoAdicionalprocRef", "NotaFiscalInformacaoAdicionalId", "dbo.NotaFiscalInformacaoAdicional");
            DropForeignKey("dbo.NotaFiscal", "InformacaoAdicional_Id", "dbo.NotaFiscalInformacaoAdicional");
            DropForeignKey("dbo.NotaFiscalPagamento", "card_Id", "dbo.NotaFiscalPagamentoCartao");
            DropForeignKey("dbo.NotaFiscalPagamento", "NotaFiscalId", "dbo.NotaFiscal");
            DropForeignKey("dbo.NotaFiscal", "Retirada_Id", "dbo.NotaFiscalRetirada");
            DropForeignKey("dbo.NotaFiscalTotal", "ICMSTot_Id", "dbo.NotaFiscalICMSTot");
            DropForeignKey("dbo.NotaFiscalTotal", "ISSQNtot_Id", "dbo.NotaFiscalISSQNTot");
            DropForeignKey("dbo.NotaFiscalTotal", "retTrib_Id", "dbo.NotaFiscalretTrib");
            DropForeignKey("dbo.NotaFiscal", "TotalNota_Id", "dbo.NotaFiscalTotal");
            DropForeignKey("dbo.NotaFiscalManifestacaoDestinatario", "NotaFiscalId", "dbo.NotaFiscal");
            DropForeignKey("dbo.NotaFiscalCOFINSAliq", "Id", "dbo.NotaFiscalCOFINSBasico");
            DropForeignKey("dbo.NotaFiscalCOFINSNT", "Id", "dbo.NotaFiscalCOFINSBasico");
            DropForeignKey("dbo.NotaFiscalCOFINSOutr", "Id", "dbo.NotaFiscalCOFINSBasico");
            DropForeignKey("dbo.NotaFiscalCOFINSQtde", "Id", "dbo.NotaFiscalCOFINSBasico");
            DropForeignKey("dbo.NotaFiscalCOFINSST", "Id", "dbo.NotaFiscalCOFINSBasico");
            DropForeignKey("dbo.NotaFiscalICMS00", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMS10", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMS20", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMS30", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMS40", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMS51", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMS60", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMS70", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMS90", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMSPart", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMSSN101", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMSSN102", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMSSN201", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMSSN202", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMSSN500", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMSSN900", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMSST", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalIPINT", "Id", "dbo.NotaFiscalIPIBasico");
            DropForeignKey("dbo.NotaFiscalIPITrib", "Id", "dbo.NotaFiscalIPIBasico");
            DropForeignKey("dbo.NotaFiscalPISAliq", "Id", "dbo.NotaFiscalPISBasico");
            DropForeignKey("dbo.NotaFiscalPISNT", "Id", "dbo.NotaFiscalPISBasico");
            DropForeignKey("dbo.NotaFiscalPISOutr", "Id", "dbo.NotaFiscalPISBasico");
            DropForeignKey("dbo.NotaFiscalPISQtde", "Id", "dbo.NotaFiscalPISBasico");
            DropForeignKey("dbo.NotaFiscalPISST", "Id", "dbo.NotaFiscalPISBasico");
            DropForeignKey("dbo.NotaFiscalarma", "Id", "dbo.NotaFiscalProdutoEspecifico");
            DropForeignKey("dbo.NotaFiscalarma", "NotaFiscalProdutoId", "dbo.NotaFiscalProduto");
            DropForeignKey("dbo.NotaFiscalcomb", "Id", "dbo.NotaFiscalProdutoEspecifico");
            DropForeignKey("dbo.NotaFiscalcomb", "NotaFiscalProdutoId", "dbo.NotaFiscalProduto");
            DropForeignKey("dbo.NotaFiscalcomb", "NotaFiscalCIDEId", "dbo.NotaFiscalCIDE");
            DropForeignKey("dbo.NotaFiscalcomb", "NotaFiscalEncerranteId", "dbo.NotaFiscalEncerrante");
            DropForeignKey("dbo.NotaFiscalmed", "Id", "dbo.NotaFiscalProdutoEspecifico");
            DropForeignKey("dbo.NotaFiscalmed", "NotaFiscalProdutoId", "dbo.NotaFiscalProduto");
            DropForeignKey("dbo.NotaFiscalveicProd", "Id", "dbo.NotaFiscalProdutoEspecifico");
            DropForeignKey("dbo.NotaFiscalveicProd", "NotaFiscalProdutoId", "dbo.NotaFiscalProduto");
            DropIndex("dbo.NotaFiscalManifestacaoDestinatario", new[] { "NotaFiscalId" });
            DropIndex("dbo.NotaFiscal", new[] { "Avulsa_Id" });
            DropIndex("dbo.NotaFiscal", new[] { "Cana_Id" });
            DropIndex("dbo.NotaFiscal", new[] { "Compra_Id" });
            DropIndex("dbo.NotaFiscal", new[] { "Destinatario_Id" });
            DropIndex("dbo.NotaFiscal", new[] { "Emitente_Id" });
            DropIndex("dbo.NotaFiscal", new[] { "Entrega_Id" });
            DropIndex("dbo.NotaFiscal", new[] { "Exporta_Id" });
            DropIndex("dbo.NotaFiscal", new[] { "Ide_Id" });
            DropIndex("dbo.NotaFiscal", new[] { "InformacaoAdicional_Id" });
            DropIndex("dbo.NotaFiscal", new[] { "Retirada_Id" });
            DropIndex("dbo.NotaFiscal", new[] { "TotalNota_Id" });
            DropIndex("dbo.NotaFiscalautXML", new[] { "NotaFiscalId" });
            DropIndex("dbo.NotaFiscalcanadeduc", new[] { "NotaFiscalcanaId" });
            DropIndex("dbo.NotaFiscalcanaforDia", new[] { "NotaFiscalcanaId" });
            DropIndex("dbo.NotaFiscalCobranca", new[] { "NotaFiscalId" });
            DropIndex("dbo.NotaFiscalCobranca", new[] { "NotaFiscalCobrancaFaturaId" });
            DropIndex("dbo.NotaFiscalCobrancaDuplicata", new[] { "NotaFiscalCobrancaId" });
            DropIndex("dbo.NotaFiscalDestinatario", new[] { "NotaFiscalEnderecoDestinatarioId" });
            DropIndex("dbo.NotaFiscalDetalhe", new[] { "NotaFiscalId" });
            DropIndex("dbo.NotaFiscalDetalhe", new[] { "imposto_Id" });
            DropIndex("dbo.NotaFiscalDetalhe", new[] { "impostoDevol_Id" });
            DropIndex("dbo.NotaFiscalDetalhe", new[] { "prod_Id" });
            DropIndex("dbo.NotaFiscalImposto", new[] { "COFINS_Id" });
            DropIndex("dbo.NotaFiscalImposto", new[] { "COFINSST_Id" });
            DropIndex("dbo.NotaFiscalImposto", new[] { "ICMS_Id" });
            DropIndex("dbo.NotaFiscalImposto", new[] { "ICMSUFDest_Id" });
            DropIndex("dbo.NotaFiscalImposto", new[] { "II_Id" });
            DropIndex("dbo.NotaFiscalImposto", new[] { "IPI_Id" });
            DropIndex("dbo.NotaFiscalImposto", new[] { "ISSQN_Id" });
            DropIndex("dbo.NotaFiscalImposto", new[] { "PIS_Id" });
            DropIndex("dbo.NotaFiscalImposto", new[] { "PISST_Id" });
            DropIndex("dbo.NotaFiscalCOFINS", new[] { "TipoCOFINS_Id" });
            DropIndex("dbo.NotaFiscalICMS", new[] { "TipoICMS_Id" });
            DropIndex("dbo.NotaFiscalIPI", new[] { "TipoIPI_Id" });
            DropIndex("dbo.NotaFiscalPIS", new[] { "TipoPIS_Id" });
            DropIndex("dbo.NotaFiscalImpostoDevolvido", new[] { "IPI_Id" });
            DropIndex("dbo.NotaFiscalProduto", new[] { "NotaFiscalProdutoEspecificoId" });
            DropIndex("dbo.NotaFiscaldetExport", new[] { "NotaFiscalProdutoId" });
            DropIndex("dbo.NotaFiscaldetExport", new[] { "NotaFiscalexportIndId" });
            DropIndex("dbo.NotaFiscalDI", new[] { "NotaFiscalProdutoId" });
            DropIndex("dbo.NotaFiscaladi", new[] { "NotaFiscalDI_Id" });
            DropIndex("dbo.NotaFiscalEmitente", new[] { "NotaFiscalEnderecoEmitenteId" });
            DropIndex("dbo.NotaFiscalNFref", new[] { "NotaFiscalIdentificacaoId" });
            DropIndex("dbo.NotaFiscalNFref", new[] { "refECF_Id" });
            DropIndex("dbo.NotaFiscalNFref", new[] { "refNF_Id" });
            DropIndex("dbo.NotaFiscalNFref", new[] { "refNFP_Id" });
            DropIndex("dbo.NotaFiscalInformacaoAdicionalCont", new[] { "NotaFiscalInformacaoAdicionalId" });
            DropIndex("dbo.NotaFiscalInformacaoAdicionalobsFisco", new[] { "NotaFiscalInformacaoAdicionalId" });
            DropIndex("dbo.NotaFiscalInformacaoAdicionalprocRef", new[] { "NotaFiscalInformacaoAdicionalId" });
            DropIndex("dbo.NotaFiscalPagamento", new[] { "NotaFiscalId" });
            DropIndex("dbo.NotaFiscalPagamento", new[] { "card_Id" });
            DropIndex("dbo.NotaFiscalTotal", new[] { "ICMSTot_Id" });
            DropIndex("dbo.NotaFiscalTotal", new[] { "ISSQNtot_Id" });
            DropIndex("dbo.NotaFiscalTotal", new[] { "retTrib_Id" });
            DropIndex("dbo.NotaFiscalCOFINSAliq", new[] { "Id" });
            DropIndex("dbo.NotaFiscalCOFINSNT", new[] { "Id" });
            DropIndex("dbo.NotaFiscalCOFINSOutr", new[] { "Id" });
            DropIndex("dbo.NotaFiscalCOFINSQtde", new[] { "Id" });
            DropIndex("dbo.NotaFiscalCOFINSST", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMS00", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMS10", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMS20", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMS30", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMS40", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMS51", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMS60", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMS70", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMS90", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMSPart", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMSSN101", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMSSN102", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMSSN201", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMSSN202", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMSSN500", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMSSN900", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMSST", new[] { "Id" });
            DropIndex("dbo.NotaFiscalIPINT", new[] { "Id" });
            DropIndex("dbo.NotaFiscalIPITrib", new[] { "Id" });
            DropIndex("dbo.NotaFiscalPISAliq", new[] { "Id" });
            DropIndex("dbo.NotaFiscalPISNT", new[] { "Id" });
            DropIndex("dbo.NotaFiscalPISOutr", new[] { "Id" });
            DropIndex("dbo.NotaFiscalPISQtde", new[] { "Id" });
            DropIndex("dbo.NotaFiscalPISST", new[] { "Id" });
            DropIndex("dbo.NotaFiscalarma", new[] { "Id" });
            DropIndex("dbo.NotaFiscalarma", new[] { "NotaFiscalProdutoId" });
            DropIndex("dbo.NotaFiscalcomb", new[] { "Id" });
            DropIndex("dbo.NotaFiscalcomb", new[] { "NotaFiscalProdutoId" });
            DropIndex("dbo.NotaFiscalcomb", new[] { "NotaFiscalCIDEId" });
            DropIndex("dbo.NotaFiscalcomb", new[] { "NotaFiscalEncerranteId" });
            DropIndex("dbo.NotaFiscalmed", new[] { "Id" });
            DropIndex("dbo.NotaFiscalmed", new[] { "NotaFiscalProdutoId" });
            DropIndex("dbo.NotaFiscalveicProd", new[] { "Id" });
            DropIndex("dbo.NotaFiscalveicProd", new[] { "NotaFiscalProdutoId" });
            AlterColumn("dbo.NotaFiscal", "DataEmissao", c => c.DateTime());
            DropColumn("dbo.NotaFiscal", "NotaFiscalCobrancaId");
            DropColumn("dbo.NotaFiscal", "Avulsa_Id");
            DropColumn("dbo.NotaFiscal", "Cana_Id");
            DropColumn("dbo.NotaFiscal", "Compra_Id");
            DropColumn("dbo.NotaFiscal", "Destinatario_Id");
            DropColumn("dbo.NotaFiscal", "Emitente_Id");
            DropColumn("dbo.NotaFiscal", "Entrega_Id");
            DropColumn("dbo.NotaFiscal", "Exporta_Id");
            DropColumn("dbo.NotaFiscal", "Ide_Id");
            DropColumn("dbo.NotaFiscal", "InformacaoAdicional_Id");
            DropColumn("dbo.NotaFiscal", "Retirada_Id");
            DropColumn("dbo.NotaFiscal", "TotalNota_Id");
            DropTable("dbo.NotaFiscalManifestacaoDestinatario",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_NotaFiscalManifestacaoDestinatario_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.NotaFiscalautXML");
            DropTable("dbo.NotaFiscalAvulsa");
            DropTable("dbo.NotaFiscalcana");
            DropTable("dbo.NotaFiscalcanadeduc");
            DropTable("dbo.NotaFiscalcanaforDia");
            DropTable("dbo.NotaFiscalCobranca");
            DropTable("dbo.NotaFiscalCobrancaDuplicata");
            DropTable("dbo.NotaFiscalCobrancaFatura");
            DropTable("dbo.NotaFiscalcompra");
            DropTable("dbo.NotaFiscalDestinatario");
            DropTable("dbo.NotaFiscalEnderecoDestinatario");
            DropTable("dbo.NotaFiscalDetalhe");
            DropTable("dbo.NotaFiscalImposto");
            DropTable("dbo.NotaFiscalCOFINS");
            DropTable("dbo.NotaFiscalCOFINSBasico");
            DropTable("dbo.NotaFiscalICMS");
            DropTable("dbo.NotaFiscalICMSBasico");
            DropTable("dbo.NotaFiscalICMSUFDest");
            DropTable("dbo.NotaFiscalICMSImpostoImportacao");
            DropTable("dbo.NotaFiscalIPI");
            DropTable("dbo.NotaFiscalIPIBasico");
            DropTable("dbo.NotaFiscalISSQN");
            DropTable("dbo.NotaFiscalPIS");
            DropTable("dbo.NotaFiscalPISBasico");
            DropTable("dbo.NotaFiscalImpostoDevolvido");
            DropTable("dbo.NotaFiscalIPIDevolvido");
            DropTable("dbo.NotaFiscalProduto");
            DropTable("dbo.NotaFiscaldetExport");
            DropTable("dbo.NotaFiscalexportInd");
            DropTable("dbo.NotaFiscalDI");
            DropTable("dbo.NotaFiscaladi");
            DropTable("dbo.NotaFiscalProdutoEspecifico");
            DropTable("dbo.NotaFiscalCIDE");
            DropTable("dbo.NotaFiscalEncerrante");
            DropTable("dbo.NotaFiscalEmitente");
            DropTable("dbo.NotaFiscalEnderecoEmitente");
            DropTable("dbo.NotaFiscalEntrega");
            DropTable("dbo.NotaFiscalexporta");
            DropTable("dbo.NotaFiscalIdentificacao");
            DropTable("dbo.NotaFiscalNFref");
            DropTable("dbo.NotaFiscalrefECF");
            DropTable("dbo.NotaFiscalrefNF");
            DropTable("dbo.NotaFiscalrefNFP");
            DropTable("dbo.NotaFiscalInformacaoAdicional");
            DropTable("dbo.NotaFiscalInformacaoAdicionalCont");
            DropTable("dbo.NotaFiscalInformacaoAdicionalobsFisco");
            DropTable("dbo.NotaFiscalInformacaoAdicionalprocRef");
            DropTable("dbo.NotaFiscalPagamento");
            DropTable("dbo.NotaFiscalPagamentoCartao");
            DropTable("dbo.NotaFiscalRetirada");
            DropTable("dbo.NotaFiscalTotal");
            DropTable("dbo.NotaFiscalICMSTot");
            DropTable("dbo.NotaFiscalISSQNTot");
            DropTable("dbo.NotaFiscalretTrib");
            DropTable("dbo.NotaFiscalCOFINSAliq");
            DropTable("dbo.NotaFiscalCOFINSNT");
            DropTable("dbo.NotaFiscalCOFINSOutr");
            DropTable("dbo.NotaFiscalCOFINSQtde");
            DropTable("dbo.NotaFiscalCOFINSST");
            DropTable("dbo.NotaFiscalICMS00");
            DropTable("dbo.NotaFiscalICMS10");
            DropTable("dbo.NotaFiscalICMS20");
            DropTable("dbo.NotaFiscalICMS30");
            DropTable("dbo.NotaFiscalICMS40");
            DropTable("dbo.NotaFiscalICMS51");
            DropTable("dbo.NotaFiscalICMS60");
            DropTable("dbo.NotaFiscalICMS70");
            DropTable("dbo.NotaFiscalICMS90");
            DropTable("dbo.NotaFiscalICMSPart");
            DropTable("dbo.NotaFiscalICMSSN101");
            DropTable("dbo.NotaFiscalICMSSN102");
            DropTable("dbo.NotaFiscalICMSSN201");
            DropTable("dbo.NotaFiscalICMSSN202");
            DropTable("dbo.NotaFiscalICMSSN500");
            DropTable("dbo.NotaFiscalICMSSN900");
            DropTable("dbo.NotaFiscalICMSST");
            DropTable("dbo.NotaFiscalIPINT");
            DropTable("dbo.NotaFiscalIPITrib");
            DropTable("dbo.NotaFiscalPISAliq");
            DropTable("dbo.NotaFiscalPISNT");
            DropTable("dbo.NotaFiscalPISOutr");
            DropTable("dbo.NotaFiscalPISQtde");
            DropTable("dbo.NotaFiscalPISST");
            DropTable("dbo.NotaFiscalarma");
            DropTable("dbo.NotaFiscalcomb");
            DropTable("dbo.NotaFiscalmed");
            DropTable("dbo.NotaFiscalveicProd");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.NotaFiscalveicProd",
                c => new
                {
                    Id = c.Long(nullable: false),
                    NotaFiscalProdutoId = c.Long(nullable: false),
                    tpOp = c.Int(nullable: false),
                    chassi = c.String(),
                    cCor = c.String(),
                    xCor = c.String(),
                    pot = c.String(),
                    cilin = c.String(),
                    pesoL = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pesoB = c.Decimal(nullable: false, precision: 18, scale: 2),
                    nSerie = c.String(),
                    tpComb = c.String(),
                    nMotor = c.String(),
                    CMT = c.Decimal(nullable: false, precision: 18, scale: 2),
                    dist = c.String(),
                    anoMod = c.Int(nullable: false),
                    anoFab = c.Int(nullable: false),
                    tpPint = c.String(),
                    tpVeic = c.String(),
                    espVeic = c.Int(nullable: false),
                    VIN = c.Int(nullable: false),
                    condVeic = c.Int(nullable: false),
                    cMod = c.String(),
                    cCorDENATRAN = c.String(),
                    lota = c.Int(nullable: false),
                    tpRest = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalmed",
                c => new
                {
                    Id = c.Long(nullable: false),
                    NotaFiscalProdutoId = c.Long(nullable: false),
                    nLote = c.String(),
                    qLote = c.Decimal(nullable: false, precision: 18, scale: 2),
                    dFab = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ProxydFab = c.String(),
                    dVal = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ProxydVal = c.String(),
                    vPMC = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalcomb",
                c => new
                {
                    Id = c.Long(nullable: false),
                    NotaFiscalProdutoId = c.Long(nullable: false),
                    cProdANP = c.String(),
                    pMixGN = c.Decimal(precision: 18, scale: 2),
                    CODIF = c.String(),
                    qTemp = c.Decimal(precision: 18, scale: 2),
                    UFCons = c.String(),
                    NotaFiscalCIDEId = c.Long(),
                    NotaFiscalEncerranteId = c.Long(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalarma",
                c => new
                {
                    Id = c.Long(nullable: false),
                    NotaFiscalProdutoId = c.Long(nullable: false),
                    tpArma = c.Int(nullable: false),
                    nSerie = c.String(),
                    nCano = c.String(),
                    descr = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalPISST",
                c => new
                {
                    Id = c.Long(nullable: false),
                    NotaFiscalImpostoId = c.Long(nullable: false),
                    vBC = c.Decimal(precision: 18, scale: 2),
                    pPIS = c.Decimal(precision: 18, scale: 2),
                    qBCProd = c.Decimal(precision: 18, scale: 2),
                    vAliqProd = c.Decimal(precision: 18, scale: 2),
                    vPIS = c.Decimal(precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalPISQtde",
                c => new
                {
                    Id = c.Long(nullable: false),
                    CST = c.Int(nullable: false),
                    NotaFiscalPISId = c.Long(nullable: false),
                    qBCProd = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vAliqProd = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vPIS = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalPISOutr",
                c => new
                {
                    Id = c.Long(nullable: false),
                    CST = c.Int(nullable: false),
                    vBC = c.Decimal(precision: 18, scale: 2),
                    pPIS = c.Decimal(precision: 18, scale: 2),
                    qBCProd = c.Decimal(precision: 18, scale: 2),
                    vAliqProd = c.Decimal(precision: 18, scale: 2),
                    vPIS = c.Decimal(precision: 18, scale: 2),
                    NotaFiscalPISId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalPISNT",
                c => new
                {
                    Id = c.Long(nullable: false),
                    CST = c.Int(nullable: false),
                    NotaFiscalPISId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalPISAliq",
                c => new
                {
                    Id = c.Long(nullable: false),
                    CST = c.Int(nullable: false),
                    vBC = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pPIS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vPIS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    NotaFiscalPISId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalIPITrib",
                c => new
                {
                    Id = c.Long(nullable: false),
                    CST = c.Int(nullable: false),
                    vBC = c.Decimal(precision: 18, scale: 2),
                    pIPI = c.Decimal(precision: 18, scale: 2),
                    qUnid = c.Decimal(precision: 18, scale: 2),
                    vUnid = c.Decimal(precision: 18, scale: 2),
                    vIPI = c.Decimal(precision: 18, scale: 2),
                    NotaFiscalIPIId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalIPINT",
                c => new
                {
                    Id = c.Long(nullable: false),
                    CST = c.Int(nullable: false),
                    NotaFiscalIPIId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMSST",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CST = c.Int(nullable: false),
                    vBCSTRet = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSSTRet = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vBCSTDest = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSSTDest = c.Decimal(nullable: false, precision: 18, scale: 2),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMSSN900",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CSOSN = c.Int(nullable: false),
                    modBC = c.Int(),
                    vBC = c.Decimal(precision: 18, scale: 2),
                    pRedBC = c.Decimal(precision: 18, scale: 2),
                    pICMS = c.Decimal(precision: 18, scale: 2),
                    vICMS = c.Decimal(precision: 18, scale: 2),
                    modBCST = c.Int(),
                    pMVAST = c.Decimal(precision: 18, scale: 2),
                    pRedBCST = c.Decimal(precision: 18, scale: 2),
                    vBCST = c.Decimal(precision: 18, scale: 2),
                    pICMSST = c.Decimal(precision: 18, scale: 2),
                    vICMSST = c.Decimal(precision: 18, scale: 2),
                    pCredSN = c.Decimal(precision: 18, scale: 2),
                    vCredICMSSN = c.Decimal(precision: 18, scale: 2),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMSSN500",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CSOSN = c.Int(nullable: false),
                    vBCSTRet = c.Decimal(precision: 18, scale: 2),
                    vICMSSTRet = c.Decimal(precision: 18, scale: 2),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMSSN202",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CSOSN = c.Int(nullable: false),
                    modBCST = c.Int(nullable: false),
                    pMVAST = c.Decimal(precision: 18, scale: 2),
                    pRedBCST = c.Decimal(precision: 18, scale: 2),
                    vBCST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMSST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMSSN201",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CSOSN = c.Int(nullable: false),
                    modBCST = c.Int(nullable: false),
                    pMVAST = c.Decimal(precision: 18, scale: 2),
                    pRedBCST = c.Decimal(precision: 18, scale: 2),
                    vBCST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMSST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pCredSN = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vCredICMSSN = c.Decimal(nullable: false, precision: 18, scale: 2),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMSSN102",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CSOSN = c.Int(nullable: false),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMSSN101",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CSOSN = c.Int(nullable: false),
                    pCredSN = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vCredICMSSN = c.Decimal(nullable: false, precision: 18, scale: 2),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMSPart",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CST = c.Int(nullable: false),
                    modBC = c.Int(nullable: false),
                    vBC = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pRedBC = c.Decimal(precision: 18, scale: 2),
                    pICMS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    modBCST = c.Int(nullable: false),
                    pMVAST = c.Decimal(precision: 18, scale: 2),
                    pRedBCST = c.Decimal(precision: 18, scale: 2),
                    vBCST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMSST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pBCOp = c.Decimal(nullable: false, precision: 18, scale: 2),
                    UFST = c.String(),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMS90",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CST = c.Int(nullable: false),
                    modBC = c.Int(),
                    vBC = c.Decimal(precision: 18, scale: 2),
                    pRedBC = c.Decimal(precision: 18, scale: 2),
                    pICMS = c.Decimal(precision: 18, scale: 2),
                    vICMS = c.Decimal(precision: 18, scale: 2),
                    modBCST = c.Int(),
                    pMVAST = c.Decimal(precision: 18, scale: 2),
                    pRedBCST = c.Decimal(precision: 18, scale: 2),
                    vBCST = c.Decimal(precision: 18, scale: 2),
                    pICMSST = c.Decimal(precision: 18, scale: 2),
                    vICMSST = c.Decimal(precision: 18, scale: 2),
                    vICMSDeson = c.Decimal(precision: 18, scale: 2),
                    motDesICMS = c.Int(),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMS70",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CST = c.Int(nullable: false),
                    modBC = c.Int(nullable: false),
                    pRedBC = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vBC = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    modBCST = c.Int(nullable: false),
                    pMVAST = c.Decimal(precision: 18, scale: 2),
                    pRedBCST = c.Decimal(precision: 18, scale: 2),
                    vBCST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMSST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSDeson = c.Decimal(precision: 18, scale: 2),
                    motDesICMS = c.Int(),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMS60",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CST = c.Int(nullable: false),
                    vBCSTRet = c.Decimal(precision: 18, scale: 2),
                    vICMSSTRet = c.Decimal(precision: 18, scale: 2),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMS51",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CST = c.Int(nullable: false),
                    modBC = c.Int(),
                    pRedBC = c.Decimal(precision: 18, scale: 2),
                    vBC = c.Decimal(precision: 18, scale: 2),
                    pICMS = c.Decimal(precision: 18, scale: 2),
                    vICMSOp = c.Decimal(precision: 18, scale: 2),
                    pDif = c.Decimal(precision: 18, scale: 2),
                    vICMSDif = c.Decimal(precision: 18, scale: 2),
                    vICMS = c.Decimal(precision: 18, scale: 2),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMS40",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CST = c.Int(nullable: false),
                    vICMSDeson = c.Decimal(precision: 18, scale: 2),
                    motDesICMS = c.Int(),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMS30",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CST = c.Int(nullable: false),
                    modBCST = c.Int(nullable: false),
                    pMVAST = c.Decimal(precision: 18, scale: 2),
                    pRedBCST = c.Decimal(precision: 18, scale: 2),
                    vBCST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMSST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSDeson = c.Decimal(precision: 18, scale: 2),
                    motDesICMS = c.Int(),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMS20",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CST = c.Int(nullable: false),
                    modBC = c.Int(nullable: false),
                    pRedBC = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vBC = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSDeson = c.Decimal(precision: 18, scale: 2),
                    motDesICMS = c.Int(),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMS10",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CST = c.Int(nullable: false),
                    modBC = c.Int(nullable: false),
                    vBC = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    modBCST = c.Int(nullable: false),
                    pMVAST = c.Decimal(precision: 18, scale: 2),
                    pRedBCST = c.Decimal(precision: 18, scale: 2),
                    vBCST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMSST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMS00",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CST = c.Int(nullable: false),
                    modBC = c.Int(nullable: false),
                    vBC = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalCOFINSST",
                c => new
                {
                    Id = c.Long(nullable: false),
                    NotaFiscalImpostoId = c.Long(nullable: false),
                    vBC = c.Decimal(precision: 18, scale: 2),
                    pCOFINS = c.Decimal(precision: 18, scale: 2),
                    qBCProd = c.Decimal(precision: 18, scale: 2),
                    vAliqProd = c.Decimal(precision: 18, scale: 2),
                    vCOFINS = c.Decimal(precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalCOFINSQtde",
                c => new
                {
                    Id = c.Long(nullable: false),
                    CST = c.Int(nullable: false),
                    NotaFiscalCOFINSId = c.Long(nullable: false),
                    qBCProd = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vAliqProd = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vCOFINS = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalCOFINSOutr",
                c => new
                {
                    Id = c.Long(nullable: false),
                    CST = c.Int(nullable: false),
                    vBC = c.Decimal(precision: 18, scale: 2),
                    pCOFINS = c.Decimal(precision: 18, scale: 2),
                    qBCProd = c.Decimal(precision: 18, scale: 2),
                    vAliqProd = c.Decimal(precision: 18, scale: 2),
                    vCOFINS = c.Decimal(precision: 18, scale: 2),
                    NotaFiscalCOFINSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalCOFINSNT",
                c => new
                {
                    Id = c.Long(nullable: false),
                    CST = c.Int(nullable: false),
                    NotaFiscalCOFINSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalCOFINSAliq",
                c => new
                {
                    Id = c.Long(nullable: false),
                    CST = c.Int(nullable: false),
                    vBC = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pCOFINS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vCOFINS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    NotaFiscalCOFINSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalretTrib",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalTotalId = c.Long(nullable: false),
                    vRetPIS = c.Decimal(precision: 18, scale: 2),
                    vRetCOFINS = c.Decimal(precision: 18, scale: 2),
                    vRetCSLL = c.Decimal(precision: 18, scale: 2),
                    vBCIRRF = c.Decimal(precision: 18, scale: 2),
                    vIRRF = c.Decimal(precision: 18, scale: 2),
                    vBCRetPrev = c.Decimal(precision: 18, scale: 2),
                    vRetPrev = c.Decimal(precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalISSQNTot",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalTotalId = c.Long(nullable: false),
                    vServ = c.Decimal(precision: 18, scale: 2),
                    vBC = c.Decimal(precision: 18, scale: 2),
                    vISS = c.Decimal(precision: 18, scale: 2),
                    vPIS = c.Decimal(precision: 18, scale: 2),
                    vCOFINS = c.Decimal(precision: 18, scale: 2),
                    dCompet = c.String(),
                    vDeducao = c.Decimal(precision: 18, scale: 2),
                    vOutro = c.Decimal(precision: 18, scale: 2),
                    vDescIncond = c.Decimal(precision: 18, scale: 2),
                    vDescCond = c.Decimal(precision: 18, scale: 2),
                    vISSRet = c.Decimal(precision: 18, scale: 2),
                    cRegTrib = c.Int(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMSTot",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalTotalId = c.Long(nullable: false),
                    vBC = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSDeson = c.Decimal(precision: 18, scale: 2),
                    vFCPUFDest = c.Decimal(precision: 18, scale: 2),
                    vICMSUFDest = c.Decimal(precision: 18, scale: 2),
                    vICMSUFRemet = c.Decimal(precision: 18, scale: 2),
                    vBCST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vProd = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vFrete = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vSeg = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vDesc = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vII = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vIPI = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vPIS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vCOFINS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vOutro = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vNF = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vTotTrib = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalTotal",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    ICMSTot_Id = c.Long(),
                    ISSQNtot_Id = c.Long(),
                    retTrib_Id = c.Long(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalRetirada",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    CNPJ = c.String(),
                    CPF = c.String(),
                    xLgr = c.String(),
                    nro = c.String(),
                    xCpl = c.String(),
                    xBairro = c.String(),
                    cMun = c.Long(nullable: false),
                    xMun = c.String(),
                    UF = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalPagamentoCartao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalPagamentoId = c.Long(nullable: false),
                    tpIntegra = c.Int(),
                    CNPJ = c.String(),
                    tBand = c.Int(),
                    cAut = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalPagamento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    tPag = c.Int(nullable: false),
                    vPag = c.Decimal(nullable: false, precision: 18, scale: 2),
                    card_Id = c.Long(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalInformacaoAdicionalprocRef",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalInformacaoAdicionalId = c.Long(nullable: false),
                    nProc = c.String(),
                    indProc = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalInformacaoAdicionalobsFisco",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalInformacaoAdicionalId = c.Long(nullable: false),
                    xCampo = c.String(),
                    xTexto = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalInformacaoAdicionalCont",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalInformacaoAdicionalId = c.Long(nullable: false),
                    xCampo = c.String(),
                    xTexto = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalInformacaoAdicional",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    infAdFisco = c.String(),
                    infCpl = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalrefNFP",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalNFrefId = c.Long(nullable: false),
                    cUF = c.Int(nullable: false),
                    AAMM = c.String(),
                    CNPJ = c.String(),
                    CPF = c.String(),
                    IE = c.String(),
                    mod = c.String(),
                    serie = c.Int(nullable: false),
                    nNF = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalrefNF",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalNFrefId = c.Long(nullable: false),
                    cUF = c.Int(nullable: false),
                    AAMM = c.String(),
                    CNPJ = c.String(),
                    mod = c.String(),
                    serie = c.Int(nullable: false),
                    nNF = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalrefECF",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalNFrefId = c.Long(nullable: false),
                    mod = c.String(),
                    nECF = c.Int(nullable: false),
                    nCOO = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalNFref",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalIdentificacaoId = c.Long(nullable: false),
                    refNFe = c.String(),
                    refECF_Id = c.Long(),
                    refNF_Id = c.Long(),
                    refNFP_Id = c.Long(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalIdentificacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    cUF = c.Int(nullable: false),
                    cNF = c.String(),
                    natOp = c.String(),
                    indPag = c.Int(nullable: false),
                    mod = c.Int(nullable: false),
                    serie = c.Int(nullable: false),
                    nNF = c.Long(nullable: false),
                    dEmi = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ProxydEmi = c.String(),
                    dSaiEnt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ProxydSaiEnt = c.String(),
                    dhEmi = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ProxyDhEmi = c.String(),
                    dhSaiEnt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ProxydhSaiEnt = c.String(),
                    tpNF = c.Int(nullable: false),
                    idDest = c.Int(),
                    cMunFG = c.Long(nullable: false),
                    tpImp = c.Int(nullable: false),
                    tpEmis = c.Int(nullable: false),
                    cDV = c.Int(nullable: false),
                    tpAmb = c.Int(nullable: false),
                    finNFe = c.Int(nullable: false),
                    indFinal = c.Int(),
                    indPres = c.Int(),
                    procEmi = c.Int(nullable: false),
                    verProc = c.String(),
                    dhCont = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ProxydhCont = c.String(),
                    xJust = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalexporta",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    UFSaidaPais = c.String(),
                    xLocExporta = c.String(),
                    xLocDespacho = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalEntrega",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    CNPJ = c.String(),
                    CPF = c.String(),
                    xLgr = c.String(),
                    nro = c.String(),
                    xCpl = c.String(),
                    xBairro = c.String(),
                    cMun = c.Long(nullable: false),
                    xMun = c.String(),
                    UF = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalEnderecoEmitente",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalEmitenteId = c.Long(nullable: false),
                    xLgr = c.String(),
                    nro = c.String(),
                    xCpl = c.String(),
                    xBairro = c.String(),
                    cMun = c.Long(nullable: false),
                    xMun = c.String(),
                    UF = c.String(),
                    CEP = c.String(),
                    cPais = c.Int(),
                    xPais = c.String(),
                    fone = c.Long(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalEmitente",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    CNPJ = c.String(),
                    CPF = c.String(),
                    xNome = c.String(),
                    xFant = c.String(),
                    NotaFiscalEnderecoEmitenteId = c.Long(),
                    IE = c.String(),
                    IEST = c.String(),
                    IM = c.String(),
                    CNAE = c.String(),
                    CRT = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalEncerrante",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalcombId = c.Long(nullable: false),
                    nBico = c.Int(nullable: false),
                    nBomba = c.Int(),
                    nTanque = c.Int(nullable: false),
                    vEncIni = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vEncFin = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalCIDE",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalcombId = c.Long(nullable: false),
                    qBCProd = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vAliqProd = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vCIDE = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalProdutoEspecifico",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscaladi",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    nAdicao = c.Int(nullable: false),
                    nSeqAdic = c.Int(nullable: false),
                    cFabricante = c.String(),
                    vDescDI = c.Decimal(precision: 18, scale: 2),
                    nDraw = c.String(),
                    NotaFiscalDI_Id = c.Long(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalDI",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalProdutoId = c.Long(nullable: false),
                    nDI = c.String(),
                    dDI = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ProxydDI = c.String(),
                    xLocDesemb = c.String(),
                    UFDesemb = c.String(),
                    dDesemb = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ProxydDesemb = c.String(),
                    tpViaTransp = c.Int(nullable: false),
                    vAFRMM = c.Decimal(precision: 18, scale: 2),
                    tpIntermedio = c.Int(nullable: false),
                    CNPJ = c.String(),
                    UFTerceiro = c.String(),
                    cExportador = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalexportInd",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    nRE = c.String(),
                    chNFe = c.String(),
                    qExport = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscaldetExport",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalProdutoId = c.Long(nullable: false),
                    nDraw = c.String(),
                    NotaFiscalexportIndId = c.Long(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalProduto",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalDetalheId = c.Long(nullable: false),
                    cProd = c.String(),
                    cEAN = c.String(),
                    xProd = c.String(),
                    NCM = c.String(),
                    CEST = c.String(),
                    EXTIPI = c.String(),
                    CFOP = c.Int(nullable: false),
                    uCom = c.String(),
                    qCom = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vUnCom = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vProd = c.Decimal(nullable: false, precision: 18, scale: 2),
                    cEANTrib = c.String(),
                    uTrib = c.String(),
                    qTrib = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vUnTrib = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vFrete = c.Decimal(precision: 18, scale: 2),
                    vSeg = c.Decimal(precision: 18, scale: 2),
                    vDesc = c.Decimal(precision: 18, scale: 2),
                    vOutro = c.Decimal(precision: 18, scale: 2),
                    indTot = c.Int(nullable: false),
                    xPed = c.String(),
                    nItemPed = c.Int(),
                    nFCI = c.String(),
                    NotaFiscalProdutoEspecificoId = c.Long(),
                    nRECOPI = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalIPIDevolvido",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalImpostoDevolvidoId = c.Long(nullable: false),
                    vIPIDevol = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalImpostoDevolvido",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalDetalheId = c.Long(nullable: false),
                    pDevol = c.Decimal(nullable: false, precision: 18, scale: 2),
                    IPI_Id = c.Long(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalPISBasico",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalPIS",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalImpostoId = c.Long(nullable: false),
                    TipoPIS_Id = c.Long(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalISSQN",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalImpostoId = c.Long(nullable: false),
                    vBC = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vAliq = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vISSQN = c.Decimal(nullable: false, precision: 18, scale: 2),
                    cMunFG = c.Long(nullable: false),
                    cListServ = c.String(),
                    vDeducao = c.Decimal(precision: 18, scale: 2),
                    vOutro = c.Decimal(precision: 18, scale: 2),
                    vDescIncond = c.Decimal(precision: 18, scale: 2),
                    vDescCond = c.Decimal(precision: 18, scale: 2),
                    vISSRet = c.Decimal(precision: 18, scale: 2),
                    indISS = c.Int(nullable: false),
                    cServico = c.String(),
                    cMun = c.Long(),
                    cPais = c.Int(),
                    nProcesso = c.String(),
                    indIncentivo = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalIPIBasico",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalIPI",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalImpostoId = c.Long(nullable: false),
                    clEnq = c.String(),
                    CNPJProd = c.String(),
                    cSelo = c.String(),
                    qSelo = c.Int(),
                    cEnq = c.Int(nullable: false),
                    TipoIPI_Id = c.Long(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMSImpostoImportacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalImpostoId = c.Long(nullable: false),
                    vBC = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vDespAdu = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vII = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vIOF = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMSUFDest",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalImpostoId = c.Long(nullable: false),
                    vBCUFDest = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pFCPUFDest = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMSUFDest = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMSInter = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMSInterPart = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vFCPUFDest = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSUFDest = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSUFRemet = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMSBasico",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMS",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalImpostoId = c.Long(nullable: false),
                    TipoICMS_Id = c.Long(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalCOFINSBasico",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalCOFINS",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalImpostoId = c.Long(nullable: false),
                    TipoCOFINS_Id = c.Long(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalImposto",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalDetalheId = c.Long(nullable: false),
                    vTotTrib = c.Decimal(precision: 18, scale: 2),
                    COFINS_Id = c.Long(),
                    COFINSST_Id = c.Long(),
                    ICMS_Id = c.Long(),
                    ICMSUFDest_Id = c.Long(),
                    II_Id = c.Long(),
                    IPI_Id = c.Long(),
                    ISSQN_Id = c.Long(),
                    PIS_Id = c.Long(),
                    PISST_Id = c.Long(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalDetalhe",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    nItem = c.Int(nullable: false),
                    infAdProd = c.String(),
                    imposto_Id = c.Long(),
                    impostoDevol_Id = c.Long(),
                    prod_Id = c.Long(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalEnderecoDestinatario",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalDestinatarioId = c.Long(nullable: false),
                    xLgr = c.String(),
                    nro = c.String(),
                    xCpl = c.String(),
                    xBairro = c.String(),
                    cMun = c.Long(nullable: false),
                    xMun = c.String(),
                    UF = c.String(),
                    CEP = c.String(),
                    cPais = c.Int(),
                    xPais = c.String(),
                    fone = c.Long(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalDestinatario",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    CNPJ = c.String(),
                    CPF = c.String(),
                    idEstrangeiro = c.String(),
                    xNome = c.String(),
                    NotaFiscalEnderecoDestinatarioId = c.Long(),
                    indIEDest = c.Int(),
                    IE = c.String(),
                    ISUF = c.String(),
                    IM = c.String(),
                    email = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalcompra",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    xNEmp = c.String(),
                    xPed = c.String(),
                    xCont = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalCobrancaFatura",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalCobrancaId = c.Long(nullable: false),
                    nFat = c.String(),
                    vOrig = c.Decimal(precision: 18, scale: 2),
                    vDesc = c.Decimal(precision: 18, scale: 2),
                    vLiq = c.Decimal(precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalCobrancaDuplicata",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalCobrancaId = c.Long(nullable: false),
                    nDup = c.String(),
                    dVenc = c.String(),
                    vDup = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalCobranca",
                c => new
                {
                    NotaFiscalId = c.Long(nullable: false),
                    NotaFiscalCobrancaFaturaId = c.Long(),
                })
                .PrimaryKey(t => t.NotaFiscalId);

            CreateTable(
                "dbo.NotaFiscalcanaforDia",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalcanaId = c.Long(nullable: false),
                    dia = c.Int(nullable: false),
                    qtde = c.Decimal(nullable: false, precision: 18, scale: 2),
                    qTotMes = c.Decimal(nullable: false, precision: 18, scale: 2),
                    qTotAnt = c.Decimal(nullable: false, precision: 18, scale: 2),
                    qTotGer = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalcanadeduc",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalcanaId = c.Long(nullable: false),
                    xDed = c.String(),
                    vDed = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vFor = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vTotDed = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vLiqFor = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalcana",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    safra = c.String(),
                    _ref = c.String(name: "ref"),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalAvulsa",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    CNPJ = c.String(),
                    xOrgao = c.String(),
                    matr = c.String(),
                    xAgente = c.String(),
                    fone = c.String(),
                    UF = c.String(),
                    nDAR = c.String(),
                    dEmi = c.String(),
                    vDAR = c.Decimal(nullable: false, precision: 18, scale: 2),
                    repEmi = c.String(),
                    dPag = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalautXML",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    CNPJ = c.String(),
                    CPF = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalManifestacaoDestinatario",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ChaveAcesso = c.String(),
                    DataOperacao = c.DateTime(nullable: false),
                    NumeroProtocolo = c.String(),
                    TipoEvento = c.Int(nullable: false),
                    DescricaoTipoEvento = c.String(),
                    DescricaoRetorno = c.String(),
                    NotaFiscalId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
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
                    { "DynamicFilter_NotaFiscalManifestacaoDestinatario_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.NotaFiscal", "TotalNota_Id", c => c.Long());
            AddColumn("dbo.NotaFiscal", "Retirada_Id", c => c.Long());
            AddColumn("dbo.NotaFiscal", "InformacaoAdicional_Id", c => c.Long());
            AddColumn("dbo.NotaFiscal", "Ide_Id", c => c.Long());
            AddColumn("dbo.NotaFiscal", "Exporta_Id", c => c.Long());
            AddColumn("dbo.NotaFiscal", "Entrega_Id", c => c.Long());
            AddColumn("dbo.NotaFiscal", "Emitente_Id", c => c.Long());
            AddColumn("dbo.NotaFiscal", "Destinatario_Id", c => c.Long());
            AddColumn("dbo.NotaFiscal", "Compra_Id", c => c.Long());
            AddColumn("dbo.NotaFiscal", "Cana_Id", c => c.Long());
            AddColumn("dbo.NotaFiscal", "Avulsa_Id", c => c.Long());
            AddColumn("dbo.NotaFiscal", "NotaFiscalCobrancaId", c => c.Long());
            AlterColumn("dbo.NotaFiscal", "DataEmissao", c => c.DateTime(nullable: false));
            CreateIndex("dbo.NotaFiscalveicProd", "NotaFiscalProdutoId");
            CreateIndex("dbo.NotaFiscalveicProd", "Id");
            CreateIndex("dbo.NotaFiscalmed", "NotaFiscalProdutoId");
            CreateIndex("dbo.NotaFiscalmed", "Id");
            CreateIndex("dbo.NotaFiscalcomb", "NotaFiscalEncerranteId");
            CreateIndex("dbo.NotaFiscalcomb", "NotaFiscalCIDEId");
            CreateIndex("dbo.NotaFiscalcomb", "NotaFiscalProdutoId");
            CreateIndex("dbo.NotaFiscalcomb", "Id");
            CreateIndex("dbo.NotaFiscalarma", "NotaFiscalProdutoId");
            CreateIndex("dbo.NotaFiscalarma", "Id");
            CreateIndex("dbo.NotaFiscalPISST", "Id");
            CreateIndex("dbo.NotaFiscalPISQtde", "Id");
            CreateIndex("dbo.NotaFiscalPISOutr", "Id");
            CreateIndex("dbo.NotaFiscalPISNT", "Id");
            CreateIndex("dbo.NotaFiscalPISAliq", "Id");
            CreateIndex("dbo.NotaFiscalIPITrib", "Id");
            CreateIndex("dbo.NotaFiscalIPINT", "Id");
            CreateIndex("dbo.NotaFiscalICMSST", "Id");
            CreateIndex("dbo.NotaFiscalICMSSN900", "Id");
            CreateIndex("dbo.NotaFiscalICMSSN500", "Id");
            CreateIndex("dbo.NotaFiscalICMSSN202", "Id");
            CreateIndex("dbo.NotaFiscalICMSSN201", "Id");
            CreateIndex("dbo.NotaFiscalICMSSN102", "Id");
            CreateIndex("dbo.NotaFiscalICMSSN101", "Id");
            CreateIndex("dbo.NotaFiscalICMSPart", "Id");
            CreateIndex("dbo.NotaFiscalICMS90", "Id");
            CreateIndex("dbo.NotaFiscalICMS70", "Id");
            CreateIndex("dbo.NotaFiscalICMS60", "Id");
            CreateIndex("dbo.NotaFiscalICMS51", "Id");
            CreateIndex("dbo.NotaFiscalICMS40", "Id");
            CreateIndex("dbo.NotaFiscalICMS30", "Id");
            CreateIndex("dbo.NotaFiscalICMS20", "Id");
            CreateIndex("dbo.NotaFiscalICMS10", "Id");
            CreateIndex("dbo.NotaFiscalICMS00", "Id");
            CreateIndex("dbo.NotaFiscalCOFINSST", "Id");
            CreateIndex("dbo.NotaFiscalCOFINSQtde", "Id");
            CreateIndex("dbo.NotaFiscalCOFINSOutr", "Id");
            CreateIndex("dbo.NotaFiscalCOFINSNT", "Id");
            CreateIndex("dbo.NotaFiscalCOFINSAliq", "Id");
            CreateIndex("dbo.NotaFiscalTotal", "retTrib_Id");
            CreateIndex("dbo.NotaFiscalTotal", "ISSQNtot_Id");
            CreateIndex("dbo.NotaFiscalTotal", "ICMSTot_Id");
            CreateIndex("dbo.NotaFiscalPagamento", "card_Id");
            CreateIndex("dbo.NotaFiscalPagamento", "NotaFiscalId");
            CreateIndex("dbo.NotaFiscalInformacaoAdicionalprocRef", "NotaFiscalInformacaoAdicionalId");
            CreateIndex("dbo.NotaFiscalInformacaoAdicionalobsFisco", "NotaFiscalInformacaoAdicionalId");
            CreateIndex("dbo.NotaFiscalInformacaoAdicionalCont", "NotaFiscalInformacaoAdicionalId");
            CreateIndex("dbo.NotaFiscalNFref", "refNFP_Id");
            CreateIndex("dbo.NotaFiscalNFref", "refNF_Id");
            CreateIndex("dbo.NotaFiscalNFref", "refECF_Id");
            CreateIndex("dbo.NotaFiscalNFref", "NotaFiscalIdentificacaoId");
            CreateIndex("dbo.NotaFiscalEmitente", "NotaFiscalEnderecoEmitenteId");
            CreateIndex("dbo.NotaFiscaladi", "NotaFiscalDI_Id");
            CreateIndex("dbo.NotaFiscalDI", "NotaFiscalProdutoId");
            CreateIndex("dbo.NotaFiscaldetExport", "NotaFiscalexportIndId");
            CreateIndex("dbo.NotaFiscaldetExport", "NotaFiscalProdutoId");
            CreateIndex("dbo.NotaFiscalProduto", "NotaFiscalProdutoEspecificoId");
            CreateIndex("dbo.NotaFiscalImpostoDevolvido", "IPI_Id");
            CreateIndex("dbo.NotaFiscalPIS", "TipoPIS_Id");
            CreateIndex("dbo.NotaFiscalIPI", "TipoIPI_Id");
            CreateIndex("dbo.NotaFiscalICMS", "TipoICMS_Id");
            CreateIndex("dbo.NotaFiscalCOFINS", "TipoCOFINS_Id");
            CreateIndex("dbo.NotaFiscalImposto", "PISST_Id");
            CreateIndex("dbo.NotaFiscalImposto", "PIS_Id");
            CreateIndex("dbo.NotaFiscalImposto", "ISSQN_Id");
            CreateIndex("dbo.NotaFiscalImposto", "IPI_Id");
            CreateIndex("dbo.NotaFiscalImposto", "II_Id");
            CreateIndex("dbo.NotaFiscalImposto", "ICMSUFDest_Id");
            CreateIndex("dbo.NotaFiscalImposto", "ICMS_Id");
            CreateIndex("dbo.NotaFiscalImposto", "COFINSST_Id");
            CreateIndex("dbo.NotaFiscalImposto", "COFINS_Id");
            CreateIndex("dbo.NotaFiscalDetalhe", "prod_Id");
            CreateIndex("dbo.NotaFiscalDetalhe", "impostoDevol_Id");
            CreateIndex("dbo.NotaFiscalDetalhe", "imposto_Id");
            CreateIndex("dbo.NotaFiscalDetalhe", "NotaFiscalId");
            CreateIndex("dbo.NotaFiscalDestinatario", "NotaFiscalEnderecoDestinatarioId");
            CreateIndex("dbo.NotaFiscalCobrancaDuplicata", "NotaFiscalCobrancaId");
            CreateIndex("dbo.NotaFiscalCobranca", "NotaFiscalCobrancaFaturaId");
            CreateIndex("dbo.NotaFiscalCobranca", "NotaFiscalId");
            CreateIndex("dbo.NotaFiscalcanaforDia", "NotaFiscalcanaId");
            CreateIndex("dbo.NotaFiscalcanadeduc", "NotaFiscalcanaId");
            CreateIndex("dbo.NotaFiscalautXML", "NotaFiscalId");
            CreateIndex("dbo.NotaFiscal", "TotalNota_Id");
            CreateIndex("dbo.NotaFiscal", "Retirada_Id");
            CreateIndex("dbo.NotaFiscal", "InformacaoAdicional_Id");
            CreateIndex("dbo.NotaFiscal", "Ide_Id");
            CreateIndex("dbo.NotaFiscal", "Exporta_Id");
            CreateIndex("dbo.NotaFiscal", "Entrega_Id");
            CreateIndex("dbo.NotaFiscal", "Emitente_Id");
            CreateIndex("dbo.NotaFiscal", "Destinatario_Id");
            CreateIndex("dbo.NotaFiscal", "Compra_Id");
            CreateIndex("dbo.NotaFiscal", "Cana_Id");
            CreateIndex("dbo.NotaFiscal", "Avulsa_Id");
            CreateIndex("dbo.NotaFiscalManifestacaoDestinatario", "NotaFiscalId");
            AddForeignKey("dbo.NotaFiscalveicProd", "NotaFiscalProdutoId", "dbo.NotaFiscalProduto", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NotaFiscalveicProd", "Id", "dbo.NotaFiscalProdutoEspecifico", "Id");
            AddForeignKey("dbo.NotaFiscalmed", "NotaFiscalProdutoId", "dbo.NotaFiscalProduto", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NotaFiscalmed", "Id", "dbo.NotaFiscalProdutoEspecifico", "Id");
            AddForeignKey("dbo.NotaFiscalcomb", "NotaFiscalEncerranteId", "dbo.NotaFiscalEncerrante", "Id");
            AddForeignKey("dbo.NotaFiscalcomb", "NotaFiscalCIDEId", "dbo.NotaFiscalCIDE", "Id");
            AddForeignKey("dbo.NotaFiscalcomb", "NotaFiscalProdutoId", "dbo.NotaFiscalProduto", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NotaFiscalcomb", "Id", "dbo.NotaFiscalProdutoEspecifico", "Id");
            AddForeignKey("dbo.NotaFiscalarma", "NotaFiscalProdutoId", "dbo.NotaFiscalProduto", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NotaFiscalarma", "Id", "dbo.NotaFiscalProdutoEspecifico", "Id");
            AddForeignKey("dbo.NotaFiscalPISST", "Id", "dbo.NotaFiscalPISBasico", "Id");
            AddForeignKey("dbo.NotaFiscalPISQtde", "Id", "dbo.NotaFiscalPISBasico", "Id");
            AddForeignKey("dbo.NotaFiscalPISOutr", "Id", "dbo.NotaFiscalPISBasico", "Id");
            AddForeignKey("dbo.NotaFiscalPISNT", "Id", "dbo.NotaFiscalPISBasico", "Id");
            AddForeignKey("dbo.NotaFiscalPISAliq", "Id", "dbo.NotaFiscalPISBasico", "Id");
            AddForeignKey("dbo.NotaFiscalIPITrib", "Id", "dbo.NotaFiscalIPIBasico", "Id");
            AddForeignKey("dbo.NotaFiscalIPINT", "Id", "dbo.NotaFiscalIPIBasico", "Id");
            AddForeignKey("dbo.NotaFiscalICMSST", "Id", "dbo.NotaFiscalICMSBasico", "Id");
            AddForeignKey("dbo.NotaFiscalICMSSN900", "Id", "dbo.NotaFiscalICMSBasico", "Id");
            AddForeignKey("dbo.NotaFiscalICMSSN500", "Id", "dbo.NotaFiscalICMSBasico", "Id");
            AddForeignKey("dbo.NotaFiscalICMSSN202", "Id", "dbo.NotaFiscalICMSBasico", "Id");
            AddForeignKey("dbo.NotaFiscalICMSSN201", "Id", "dbo.NotaFiscalICMSBasico", "Id");
            AddForeignKey("dbo.NotaFiscalICMSSN102", "Id", "dbo.NotaFiscalICMSBasico", "Id");
            AddForeignKey("dbo.NotaFiscalICMSSN101", "Id", "dbo.NotaFiscalICMSBasico", "Id");
            AddForeignKey("dbo.NotaFiscalICMSPart", "Id", "dbo.NotaFiscalICMSBasico", "Id");
            AddForeignKey("dbo.NotaFiscalICMS90", "Id", "dbo.NotaFiscalICMSBasico", "Id");
            AddForeignKey("dbo.NotaFiscalICMS70", "Id", "dbo.NotaFiscalICMSBasico", "Id");
            AddForeignKey("dbo.NotaFiscalICMS60", "Id", "dbo.NotaFiscalICMSBasico", "Id");
            AddForeignKey("dbo.NotaFiscalICMS51", "Id", "dbo.NotaFiscalICMSBasico", "Id");
            AddForeignKey("dbo.NotaFiscalICMS40", "Id", "dbo.NotaFiscalICMSBasico", "Id");
            AddForeignKey("dbo.NotaFiscalICMS30", "Id", "dbo.NotaFiscalICMSBasico", "Id");
            AddForeignKey("dbo.NotaFiscalICMS20", "Id", "dbo.NotaFiscalICMSBasico", "Id");
            AddForeignKey("dbo.NotaFiscalICMS10", "Id", "dbo.NotaFiscalICMSBasico", "Id");
            AddForeignKey("dbo.NotaFiscalICMS00", "Id", "dbo.NotaFiscalICMSBasico", "Id");
            AddForeignKey("dbo.NotaFiscalCOFINSST", "Id", "dbo.NotaFiscalCOFINSBasico", "Id");
            AddForeignKey("dbo.NotaFiscalCOFINSQtde", "Id", "dbo.NotaFiscalCOFINSBasico", "Id");
            AddForeignKey("dbo.NotaFiscalCOFINSOutr", "Id", "dbo.NotaFiscalCOFINSBasico", "Id");
            AddForeignKey("dbo.NotaFiscalCOFINSNT", "Id", "dbo.NotaFiscalCOFINSBasico", "Id");
            AddForeignKey("dbo.NotaFiscalCOFINSAliq", "Id", "dbo.NotaFiscalCOFINSBasico", "Id");
            AddForeignKey("dbo.NotaFiscalManifestacaoDestinatario", "NotaFiscalId", "dbo.NotaFiscal", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NotaFiscal", "TotalNota_Id", "dbo.NotaFiscalTotal", "Id");
            AddForeignKey("dbo.NotaFiscalTotal", "retTrib_Id", "dbo.NotaFiscalretTrib", "Id");
            AddForeignKey("dbo.NotaFiscalTotal", "ISSQNtot_Id", "dbo.NotaFiscalISSQNTot", "Id");
            AddForeignKey("dbo.NotaFiscalTotal", "ICMSTot_Id", "dbo.NotaFiscalICMSTot", "Id");
            AddForeignKey("dbo.NotaFiscal", "Retirada_Id", "dbo.NotaFiscalRetirada", "Id");
            AddForeignKey("dbo.NotaFiscalPagamento", "NotaFiscalId", "dbo.NotaFiscal", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NotaFiscalPagamento", "card_Id", "dbo.NotaFiscalPagamentoCartao", "Id");
            AddForeignKey("dbo.NotaFiscal", "InformacaoAdicional_Id", "dbo.NotaFiscalInformacaoAdicional", "Id");
            AddForeignKey("dbo.NotaFiscalInformacaoAdicionalprocRef", "NotaFiscalInformacaoAdicionalId", "dbo.NotaFiscalInformacaoAdicional", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NotaFiscalInformacaoAdicionalobsFisco", "NotaFiscalInformacaoAdicionalId", "dbo.NotaFiscalInformacaoAdicional", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NotaFiscalInformacaoAdicionalCont", "NotaFiscalInformacaoAdicionalId", "dbo.NotaFiscalInformacaoAdicional", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NotaFiscal", "Ide_Id", "dbo.NotaFiscalIdentificacao", "Id");
            AddForeignKey("dbo.NotaFiscalNFref", "NotaFiscalIdentificacaoId", "dbo.NotaFiscalIdentificacao", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NotaFiscalNFref", "refNFP_Id", "dbo.NotaFiscalrefNFP", "Id");
            AddForeignKey("dbo.NotaFiscalNFref", "refNF_Id", "dbo.NotaFiscalrefNF", "Id");
            AddForeignKey("dbo.NotaFiscalNFref", "refECF_Id", "dbo.NotaFiscalrefECF", "Id");
            AddForeignKey("dbo.NotaFiscal", "Exporta_Id", "dbo.NotaFiscalexporta", "Id");
            AddForeignKey("dbo.NotaFiscal", "Entrega_Id", "dbo.NotaFiscalEntrega", "Id");
            AddForeignKey("dbo.NotaFiscal", "Emitente_Id", "dbo.NotaFiscalEmitente", "Id");
            AddForeignKey("dbo.NotaFiscalEmitente", "NotaFiscalEnderecoEmitenteId", "dbo.NotaFiscalEnderecoEmitente", "Id");
            AddForeignKey("dbo.NotaFiscalDetalhe", "NotaFiscalId", "dbo.NotaFiscal", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NotaFiscalDetalhe", "prod_Id", "dbo.NotaFiscalProduto", "Id");
            AddForeignKey("dbo.NotaFiscalProduto", "NotaFiscalProdutoEspecificoId", "dbo.NotaFiscalProdutoEspecifico", "Id");
            AddForeignKey("dbo.NotaFiscalDI", "NotaFiscalProdutoId", "dbo.NotaFiscalProduto", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NotaFiscaladi", "NotaFiscalDI_Id", "dbo.NotaFiscalDI", "Id");
            AddForeignKey("dbo.NotaFiscaldetExport", "NotaFiscalProdutoId", "dbo.NotaFiscalProduto", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NotaFiscaldetExport", "NotaFiscalexportIndId", "dbo.NotaFiscalexportInd", "Id");
            AddForeignKey("dbo.NotaFiscalDetalhe", "impostoDevol_Id", "dbo.NotaFiscalImpostoDevolvido", "Id");
            AddForeignKey("dbo.NotaFiscalImpostoDevolvido", "IPI_Id", "dbo.NotaFiscalIPIDevolvido", "Id");
            AddForeignKey("dbo.NotaFiscalDetalhe", "imposto_Id", "dbo.NotaFiscalImposto", "Id");
            AddForeignKey("dbo.NotaFiscalImposto", "PISST_Id", "dbo.NotaFiscalPISST", "Id");
            AddForeignKey("dbo.NotaFiscalImposto", "PIS_Id", "dbo.NotaFiscalPIS", "Id");
            AddForeignKey("dbo.NotaFiscalPIS", "TipoPIS_Id", "dbo.NotaFiscalPISBasico", "Id");
            AddForeignKey("dbo.NotaFiscalImposto", "ISSQN_Id", "dbo.NotaFiscalISSQN", "Id");
            AddForeignKey("dbo.NotaFiscalImposto", "IPI_Id", "dbo.NotaFiscalIPI", "Id");
            AddForeignKey("dbo.NotaFiscalIPI", "TipoIPI_Id", "dbo.NotaFiscalIPIBasico", "Id");
            AddForeignKey("dbo.NotaFiscalImposto", "II_Id", "dbo.NotaFiscalICMSImpostoImportacao", "Id");
            AddForeignKey("dbo.NotaFiscalImposto", "ICMSUFDest_Id", "dbo.NotaFiscalICMSUFDest", "Id");
            AddForeignKey("dbo.NotaFiscalImposto", "ICMS_Id", "dbo.NotaFiscalICMS", "Id");
            AddForeignKey("dbo.NotaFiscalICMS", "TipoICMS_Id", "dbo.NotaFiscalICMSBasico", "Id");
            AddForeignKey("dbo.NotaFiscalImposto", "COFINSST_Id", "dbo.NotaFiscalCOFINSST", "Id");
            AddForeignKey("dbo.NotaFiscalImposto", "COFINS_Id", "dbo.NotaFiscalCOFINS", "Id");
            AddForeignKey("dbo.NotaFiscalCOFINS", "TipoCOFINS_Id", "dbo.NotaFiscalCOFINSBasico", "Id");
            AddForeignKey("dbo.NotaFiscal", "Destinatario_Id", "dbo.NotaFiscalDestinatario", "Id");
            AddForeignKey("dbo.NotaFiscalDestinatario", "NotaFiscalEnderecoDestinatarioId", "dbo.NotaFiscalEnderecoDestinatario", "Id");
            AddForeignKey("dbo.NotaFiscal", "Compra_Id", "dbo.NotaFiscalcompra", "Id");
            AddForeignKey("dbo.NotaFiscalCobranca", "NotaFiscalId", "dbo.NotaFiscal", "Id");
            AddForeignKey("dbo.NotaFiscalCobranca", "NotaFiscalCobrancaFaturaId", "dbo.NotaFiscalCobrancaFatura", "Id");
            AddForeignKey("dbo.NotaFiscalCobrancaDuplicata", "NotaFiscalCobrancaId", "dbo.NotaFiscalCobranca", "NotaFiscalId", cascadeDelete: true);
            AddForeignKey("dbo.NotaFiscal", "Cana_Id", "dbo.NotaFiscalcana", "Id");
            AddForeignKey("dbo.NotaFiscalcanaforDia", "NotaFiscalcanaId", "dbo.NotaFiscalcana", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NotaFiscalcanadeduc", "NotaFiscalcanaId", "dbo.NotaFiscalcana", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NotaFiscal", "Avulsa_Id", "dbo.NotaFiscalAvulsa", "Id");
            AddForeignKey("dbo.NotaFiscalautXML", "NotaFiscalId", "dbo.NotaFiscal", "Id", cascadeDelete: true);
        }
    }
}
