using DFe.Classes.Entidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Controladorias.NFeServices.Ferramentas.Enums;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Controladorias.NFeServices.Dto
{
    public class NfeProcOutput
    {
        #region Informações do Protocolo de resposta -> nfeProc.protNFe.infProt
        public string chNFe_infProt { get; set; }
        public DateTime dhRecbto_infProt { get; set; }
        public string nProt_infProt { get; set; }
        public int cStat_infProt { get; set; }
        public string xMotivo_infProt { get; set; }
        #endregion

        #region Identificação da Nota Fiscal eletrônica -> nfeProc.NFe.infNFe.ide
        public EstadoNfeSW cUF_ide { get; set; }
        public string cNF_ide { get; set; }
        public string natOp_ide { get; set; }
        public IndicadorPagamentoSW indPag_ide { get; set; }
        public ModeloDocumentoSW mod_ide { get; set; }
        public int serie_ide { get; set; }
        public long nNF_ide { get; set; }
        public DateTimeOffset dhEmi_ide { get; set; }
        public DateTimeOffset? dhSaiEnt_ide { get; set; }
        #endregion

        #region Identificação do emitente da NF-e -> nfeProc.NFe.infNFe.emit
        public string cnpj_emit { get; set; }
        public string cpf_emit { get; set; }
        public string xNome_emit { get; set; }
        public string xFant_emit { get; set; }
        public string xLgr_emit { get; set; }
        public string nro_emit { get; set; }
        public string xBairro_emit { get; set; }
        public long cMun_emit { get; set; }
        public string xMun_emit { get; set; }
        public Estado uf_emit { get; set; }
        public string cep_emit { get; set; }
        public int? cPais_emit { get; set; }
        public string xPais_emit { get; set; }
        public long? fone_emit { get; set; }
        public string ie_emit { get; set; }
        public string iest_emit { get; set; }
        public string im_emit { get; set; }
        public string cnae_emit { get; set; }
        public CRTSW crt_emit { get; set; }
        #endregion

        #region Informações do fisco emitente (uso exclusivo do fisco) -> nfeProc.NFe.infNFe.avulsa
        public string cnpj_avulsa { get; set; }
        public string xOrgao_avulsa { get; set; }
        public string matr_avulsa { get; set; }
        public string xAgente_avulsa { get; set; }
        public string fone_avulsa { get; set; }
        public string UF_avulsa { get; set; }
        public string nDAR_avulsa { get; set; }
        public string dEmi_avulsa { get; set; }
        public decimal vDAR_avulsa { get; set; }
        public string repEmi_avulsa { get; set; }
        public string dPag_avulsa { get; set; }
        #endregion

        #region Identificação do destinatário da NF-e -> nfeProc.NFe.infNFe.dest
        public string cnpj_dest { get; set; }
        public string cpf_dest { get; set; }
        public string idEstrangeiro_dest { get; set; }
        public string xNome_dest { get; set; }
        public string xLgr_dest { get; set; }
        public string nro_dest { get; set; }
        public string xBairro_dest { get; set; }
        public long cMun_dest { get; set; }
        public string xMun_dest { get; set; }
        public string uf_dest { get; set; }
        public string cep_dest { get; set; }
        public int? cPais_dest { get; set; }
        public string xPais_dest { get; set; }
        public long? fone_dest { get; set; }
        #endregion

        #region Grupo Totais da NF-e -> nfeProc.NFe.infNFe.total
        public string issqNtot_total { get; set; }
        public string retTrib_total { get; set; }
        public decimal vBC_total { get; set; }
        public decimal vICMS_total { get; set; }
        public decimal? vICMSDeson_total { get; set; }
        public decimal vBCST_total { get; set; }
        public decimal vST_total { get; set; }
        public decimal vProd_total { get; set; }
        public decimal vFrete_total { get; set; }
        public decimal vSeg_total { get; set; }
        public decimal vDesc_total { get; set; }
        public decimal vII_total { get; set; }
        public decimal vIPI_total { get; set; }
        public decimal vPIS_total { get; set; }
        public decimal vCOFINS_total { get; set; }
        public decimal vOutro_total { get; set; }
        public decimal vNF_total { get; set; }
        public decimal vTotTrib_total { get; set; }
        #endregion

        #region Grupo Informações do Transporte -> nfeProc.NFe.infNFe.transp
        public ModalidadeFreteSW modFrete_transp { get; set; }
        public string CNPJ_transp { get; set; }
        public string CPF_transp { get; set; }
        public string xNome { get; set; }
        public string IE_transp { get; set; }
        public string xEnder_transp { get; set; }
        public string xMun_transp { get; set; }
        public string UF_transp { get; set; }
        public decimal vServ_retTransp_transp { get; set; }
        public decimal vBCRet_retTransp_transp { get; set; }
        public decimal pICMSRet_retTransp_transp { get; set; }
        public decimal vICMSRet_retTransp_transp { get; set; }
        public int CFOP_retTransp_transp { get; set; }
        public long cMunFG_retTransp_transp { get; set; }
        public string placa_veicTransp_transp { get; set; }
        public string UF_veicTransp_transp { get; set; }
        public string RNTC_veicTransp_transp { get; set; }
        public string vagao_transp { get; set; }
        public string balsa_transp { get; set; }
        public ICollection<VolOutput> vol_transp { get; set; }
        public ICollection<ReboqueOutput> reboque_transp { get; set; }
        #endregion

        #region Grupo Cobrança -> nfeProc.NFe.infNFe.cobr
        public string nFat_fat_cobr { get; set; }
        public decimal? vOrig_fat_cobr { get; set; }
        public decimal? vLiq_fat_cobr { get; set; }
        //public ICollection<DupOutput> dup_cobr { get; set; }
        #endregion

        #region Grupo de Informações Adicionais -> nfeProc.NFe.infNFe.infAdic
        public string infCpl_infAdic { get; set; }
        #endregion

        #region Grupo Exportação -> nfeProc.NFe.infNFe.exporta
        //não implementado
        #endregion

        #region Grupo Compra -> nfeProc.NFe.infNFe.compra
        //não implementado
        #endregion

        #region Grupo Cana -> nfeProc.NFe.infNFe.cana
        //não implementado
        #endregion

        public string xmlNfe { get; set; }
    }
}
