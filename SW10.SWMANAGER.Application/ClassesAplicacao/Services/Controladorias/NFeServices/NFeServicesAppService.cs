using Abp.Authorization;
using Abp.AutoMapper;
using Abp.UI;
using DFe.Classes.Flags;
using Newtonsoft.Json;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using NFe.Classes.Servicos.Tipos;
using NFe.Servicos;
using NFe.Utils;
using NFe.Utils.Email;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Controladorias.NFeServices.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Controladorias.NFeServices.Ferramentas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Controladorias.NFeServices.Ferramentas.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Controladorias.NFeServices
{
    /// <summary>
    /// Gerenciador Online para Serviço Notas Fiscais
    /// </summary>
    [AbpAuthorize(AppPermissions.Pages_Tenant_Controladoria, AppPermissions.Pages_Tenant_Suprimentos)]
    public class NFeServicesAppService : SWMANAGERAppServiceBase, INFeServicesAppService
    {
        private ConfiguracaoApp CarregarConfiguracao(string CNPJ)
        {
            var fullPath = @"c:\nfe\config_" + CNPJ + ".xml"; // /configuracao.xml");
            try
            {
                ConfiguracaoApp file = new ConfiguracaoApp();
                if (System.IO.File.Exists(fullPath))
                {
                    file = FuncoesXmlSW.ArquivoXmlParaClasse<ConfiguracaoApp>(fullPath);
                    //_configuracoes = file;
                    if (file.CfgServico.TimeOut == 0)
                    {
                        file.CfgServico.TimeOut = 999999; //mínimo
                    }
                    file.CfgServico.DiretorioSchemas = @"c:\nfe\Schemas";
                    file.CfgServico.DiretorioSalvarXml = @"c:\nfe\Xmls";
                    //throw new Exception("acessou pasta " + file.CfgServico.Certificado.Arquivo);
                    return file;
                }
                else
                {
                    throw new Exception("Não foi possivel localizar as configurações para este CNPJ: " + CNPJ);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }
        }

        public NFeServicesAppService()
        {

        }

        private ServicosNFe InicializaServico(string CNPJ)
        {
            ConfiguracaoApp _configuracoes = new ConfiguracaoApp();
            _configuracoes = CarregarConfiguracao(CNPJ);
            return new ServicosNFe(_configuracoes.CfgServico);
        }

        /// <summary>
        /// Consulta descrição do evento
        /// </summary>
        /// <param name="tpEventoCodigo">Código do evento</param>
        /// <returns>Retorna descrição do evento</returns>
        public async Task<string> DescOperacao(string tpEventoCodigo)
        {
            try
            {
                return await Task.Run(() =>
                {
                    TipoEventoSW _tpEvento;
                    Enum.TryParse(tpEventoCodigo, out _tpEvento);
                    return _tpEvento.Descricao();
                });
            }
            catch (Exception)
            {
                throw new UserFriendlyException(0, "Código não encontrado!");
            }
        }

        /// <summary>
        ///     Consulta o status do Serviço de NFe
        /// </summary>
        /// <returns>Retorna true</returns>
        public async Task<bool> isOnline(string CNPJ)
        {
            return await Task.Run(() =>
            {
                using (ServicosNFe service = this.InicializaServico(CNPJ))
                {
                    var retorno = service.NfeStatusServico().Retorno;
                    if (retorno.cStat == 107)
                    {
                        return true;
                    }
                    else
                    {
                        throw new UserFriendlyException(retorno.cStat, retorno.xMotivo);
                    }
                }
            });
        }

        public async Task<RetEnvEventoOutput> RecepcaoEventoManifestacaoDestinatario(int idlote, int sequenciaEvento,
            string chaveNFe, string tipoEventoManifestacaoDestinatario, string CNPJ,
            string justificativa = null)
        {
            return await Task.Run(() =>
            {
                using (ServicosNFe service = this.InicializaServico(CNPJ))
                {
                    RetEnvEventoOutput ret = new RetEnvEventoOutput();

                    NFeTipoEvento _tipoEventoManifestacaoDestinatario;
                    Enum.TryParse(tipoEventoManifestacaoDestinatario, out _tipoEventoManifestacaoDestinatario);
                    var retorno = service.RecepcaoEventoManifestacaoDestinatario(idlote, sequenciaEvento, chaveNFe, _tipoEventoManifestacaoDestinatario, CNPJ, justificativa = null).Retorno;

                    ret = retorno.MapTo<RetEnvEventoOutput>();

                    if (retorno.cStat == 107)
                    {
                        return ret;
                    }
                    else
                    {
                        throw new UserFriendlyException(retorno.cStat, retorno.xMotivo, JsonConvert.SerializeObject(ret));
                    }

                }
            });
        }

        /// <summary>
        /// Serviço destinado à distribuição de informações resumidas e documentos fiscais eletrônicos de interesse de um ator, seja este pessoa física ou jurídica.
        /// </summary>
        /// <param name="CNPJ">CNPJ/CPF do interessado no DF-e</param>
        /// <param name="ufAutor">Código da UF do Autor</param>
        /// <param name="ultNSU">Último NSU recebido pelo Interessado</param>
        /// <param name ="nSU"> Número Sequencial Único</param>
        /// <returns>Retorna lista de documentos de interesse do CNPJ informado</returns>
        public async Task<RetDistDFeIntOutput> NfeDistDFeInteresse(string CNPJ, string ufAutor, string ultNSU, string nSU = "0")
        {
            return await Task.Run(() =>
            {
                using (ServicosNFe service = this.InicializaServico(CNPJ))
                {
                    NFe.Classes.Servicos.DistribuicaoDFe.retDistDFeInt retorno;
                    retorno = service.NfeDistDFeInteresse(ufAutor, CNPJ, ultNSU, nSU).Retorno;
                    RetDistDFeIntOutput _retDistDFeIntOutput = retorno.MapTo<RetDistDFeIntOutput>();

                    if (retorno.cStat == 138)
                    {
                        for (int i = 0; i < retorno.loteDistDFeInt.Length; i++)
                        {
                            string xmlNfe = DFe.Utils.Compressao.Unzip(retorno.loteDistDFeInt[i].XmlNfe);
                            if (xmlNfe.StartsWith("<resNFe"))
                            {
                                NFe.Classes.Servicos.DistribuicaoDFe.Schemas.resNFe _resNFe = FuncoesXmlSW.XmlStringParaClasse<NFe.Classes.Servicos.DistribuicaoDFe.Schemas.resNFe>(xmlNfe);
                                ResNFeOutput _resNFeOutput = _resNFe.MapTo<ResNFeOutput>();
                                _retDistDFeIntOutput.resNFe.Add(_resNFeOutput);

                            }
                            else if (xmlNfe.StartsWith("<procEventoNFe"))
                            {
                                NFe.Classes.Servicos.DistribuicaoDFe.Schemas.procEventoNFe _procEventoNFe = FuncoesXmlSW.XmlStringParaClasse<NFe.Classes.Servicos.DistribuicaoDFe.Schemas.procEventoNFe>(xmlNfe);
                                ProcEventoNFeOutput _resNFeOutput;
                                if (_procEventoNFe.evento.infEvento != null)
                                    _resNFeOutput = _procEventoNFe.evento.infEvento.MapTo<ProcEventoNFeOutput>();
                                _resNFeOutput = _procEventoNFe.retEvento.infEvento.MapTo<ProcEventoNFeOutput>();

                                _retDistDFeIntOutput.procEventoNFe.Add(_resNFeOutput);

                            }
                            else if (xmlNfe.StartsWith("<resEvento"))
                            {
                                NFe.Classes.Servicos.DistribuicaoDFe.Schemas.resEvento _resEvento = FuncoesXmlSW.XmlStringParaClasse<NFe.Classes.Servicos.DistribuicaoDFe.Schemas.resEvento>(xmlNfe);
                                ResEventoOutput _resEventoOutput = _resEvento.MapTo<ResEventoOutput>();
                                _retDistDFeIntOutput.resEvento.Add(_resEventoOutput);
                            }
                            else //<nfeProc
                            {
                                NFe.Classes.nfeProc _nfeProc = FuncoesXmlSW.XmlStringParaClasse<NFe.Classes.nfeProc>(xmlNfe);
                                NfeProcOutput _nfeProcOutput = new NfeProcOutput();
                                _nfeProcOutput.xmlNfe = xmlNfe;

                                #region Informações do Protocolo de resposta -> nfeProc.protNFe.infProt
                                _nfeProcOutput.chNFe_infProt = _nfeProc.protNFe.infProt.chNFe;
                                _nfeProcOutput.dhRecbto_infProt = _nfeProc.protNFe.infProt.dhRecbto.DateTime;
                                _nfeProcOutput.nProt_infProt = _nfeProc.protNFe.infProt.nProt;
                                _nfeProcOutput.cStat_infProt = _nfeProc.protNFe.infProt.cStat;
                                _nfeProcOutput.xMotivo_infProt = _nfeProc.protNFe.infProt.xMotivo;
                                #endregion

                                #region Identificação da Nota Fiscal eletrônica -> nfeProc.NFe.infNFe.ide
                                _nfeProcOutput.cUF_ide = (EstadoNfeSW)((int)_nfeProc.NFe.infNFe.ide.cUF);
                                _nfeProcOutput.cNF_ide = _nfeProc.NFe.infNFe.ide.cNF;
                                _nfeProcOutput.natOp_ide = _nfeProc.NFe.infNFe.ide.natOp;
                                _nfeProcOutput.indPag_ide = (IndicadorPagamentoSW)((int)_nfeProc.NFe.infNFe.ide.indPag);
                                _nfeProcOutput.mod_ide = (ModeloDocumentoSW)((int)_nfeProc.NFe.infNFe.ide.mod);
                                _nfeProcOutput.serie_ide = _nfeProc.NFe.infNFe.ide.serie;
                                _nfeProcOutput.nNF_ide = _nfeProc.NFe.infNFe.ide.nNF;
                                _nfeProcOutput.dhEmi_ide = _nfeProc.NFe.infNFe.ide.dhEmi;
                                _nfeProcOutput.dhSaiEnt_ide = _nfeProc.NFe.infNFe.ide.dhSaiEnt;
                                #endregion

                                #region Identificação do emitente da NF-e -> nfeProc.NFe.infNFe.emit
                                _nfeProcOutput.cnpj_emit = _nfeProc.NFe.infNFe.emit.CNPJ;
                                _nfeProcOutput.cpf_emit = _nfeProc.NFe.infNFe.emit.CPF;
                                _nfeProcOutput.xNome_emit = _nfeProc.NFe.infNFe.emit.xNome;
                                _nfeProcOutput.xFant_emit = _nfeProc.NFe.infNFe.emit.xFant;
                                _nfeProcOutput.xLgr_emit = _nfeProc.NFe.infNFe.emit.enderEmit.xLgr;
                                _nfeProcOutput.nro_emit = _nfeProc.NFe.infNFe.emit.enderEmit.nro;
                                _nfeProcOutput.xBairro_emit = _nfeProc.NFe.infNFe.emit.enderEmit.xBairro;
                                _nfeProcOutput.cMun_emit = _nfeProc.NFe.infNFe.emit.enderEmit.cMun;
                                _nfeProcOutput.xMun_emit = _nfeProc.NFe.infNFe.emit.enderEmit.xMun;
                                _nfeProcOutput.uf_emit = _nfeProc.NFe.infNFe.emit.enderEmit.UF;
                                _nfeProcOutput.cep_emit = _nfeProc.NFe.infNFe.emit.enderEmit.CEP;
                                _nfeProcOutput.cPais_emit = _nfeProc.NFe.infNFe.emit.enderEmit.cPais;
                                _nfeProcOutput.xPais_emit = _nfeProc.NFe.infNFe.emit.enderEmit.xPais;
                                _nfeProcOutput.fone_emit = _nfeProc.NFe.infNFe.emit.enderEmit.fone;
                                _nfeProcOutput.ie_emit = _nfeProc.NFe.infNFe.emit.IE;
                                _nfeProcOutput.iest_emit = _nfeProc.NFe.infNFe.emit.IEST;
                                _nfeProcOutput.im_emit = _nfeProc.NFe.infNFe.emit.IM;
                                _nfeProcOutput.cnae_emit = _nfeProc.NFe.infNFe.emit.CNAE;
                                _nfeProcOutput.crt_emit = (CRTSW)((int)_nfeProc.NFe.infNFe.emit.CRT);
                                #endregion

                                #region Informações do fisco emitente (uso exclusivo do fisco) -> nfeProc.NFe.infNFe.avulsa
                                if (_nfeProc.NFe.infNFe.avulsa != null)
                                {
                                    _nfeProcOutput.cnpj_avulsa = _nfeProc.NFe.infNFe.avulsa.CNPJ;
                                    _nfeProcOutput.xOrgao_avulsa = _nfeProc.NFe.infNFe.avulsa.xOrgao;
                                    _nfeProcOutput.matr_avulsa = _nfeProc.NFe.infNFe.avulsa.matr;
                                    _nfeProcOutput.xAgente_avulsa = _nfeProc.NFe.infNFe.avulsa.xAgente;
                                    _nfeProcOutput.fone_avulsa = _nfeProc.NFe.infNFe.avulsa.fone;
                                    _nfeProcOutput.UF_avulsa = _nfeProc.NFe.infNFe.avulsa.UF;
                                    _nfeProcOutput.nDAR_avulsa = _nfeProc.NFe.infNFe.avulsa.nDAR;
                                    _nfeProcOutput.dEmi_avulsa = _nfeProc.NFe.infNFe.avulsa.dEmi;
                                    _nfeProcOutput.vDAR_avulsa = _nfeProc.NFe.infNFe.avulsa.vDAR;
                                    _nfeProcOutput.repEmi_avulsa = _nfeProc.NFe.infNFe.avulsa.repEmi;
                                    _nfeProcOutput.dPag_avulsa = _nfeProc.NFe.infNFe.avulsa.dPag;
                                }
                                #endregion

                                #region Identificação do destinatário da NF-e -> nfeProc.NFe.infNFe.dest
                                _nfeProcOutput.cnpj_dest = _nfeProc.NFe.infNFe.dest.CNPJ;
                                _nfeProcOutput.cpf_dest = _nfeProc.NFe.infNFe.dest.CPF;
                                _nfeProcOutput.idEstrangeiro_dest = _nfeProc.NFe.infNFe.dest.idEstrangeiro;
                                _nfeProcOutput.xNome_dest = _nfeProc.NFe.infNFe.dest.xNome;
                                _nfeProcOutput.xLgr_dest = _nfeProc.NFe.infNFe.dest.enderDest.xLgr;
                                _nfeProcOutput.nro_dest = _nfeProc.NFe.infNFe.dest.enderDest.nro;
                                _nfeProcOutput.xBairro_dest = _nfeProc.NFe.infNFe.dest.enderDest.xBairro;
                                _nfeProcOutput.cMun_dest = _nfeProc.NFe.infNFe.dest.enderDest.cMun;
                                _nfeProcOutput.xMun_dest = _nfeProc.NFe.infNFe.dest.enderDest.xMun;
                                _nfeProcOutput.uf_dest = _nfeProc.NFe.infNFe.dest.enderDest.UF;
                                _nfeProcOutput.cep_dest = _nfeProc.NFe.infNFe.dest.enderDest.CEP;
                                _nfeProcOutput.cPais_dest = _nfeProc.NFe.infNFe.dest.enderDest.cPais;
                                _nfeProcOutput.xPais_dest = _nfeProc.NFe.infNFe.dest.enderDest.xPais;
                                _nfeProcOutput.fone_dest = _nfeProc.NFe.infNFe.dest.enderDest.fone;
                                #endregion

                                #region Grupo Totais da NF-e -> nfeProc.NFe.infNFe.total
                                //_nfeProcOutput.issqNtot_total = _nfeProc.NFe.infNFe.total.ISSQNtot;
                                //_nfeProcOutput.retTrib_total = _nfeProc.NFe.infNFe.total.retTrib;
                                _nfeProcOutput.vBC_total = _nfeProc.NFe.infNFe.total.ICMSTot.vBC;
                                _nfeProcOutput.vICMS_total = _nfeProc.NFe.infNFe.total.ICMSTot.vICMS;
                                _nfeProcOutput.vICMSDeson_total = _nfeProc.NFe.infNFe.total.ICMSTot.vICMSDeson;
                                _nfeProcOutput.vBCST_total = _nfeProc.NFe.infNFe.total.ICMSTot.vBCST;
                                _nfeProcOutput.vST_total = _nfeProc.NFe.infNFe.total.ICMSTot.vST;
                                _nfeProcOutput.vProd_total = _nfeProc.NFe.infNFe.total.ICMSTot.vProd;
                                _nfeProcOutput.vFrete_total = _nfeProc.NFe.infNFe.total.ICMSTot.vFrete;
                                _nfeProcOutput.vSeg_total = _nfeProc.NFe.infNFe.total.ICMSTot.vSeg;
                                _nfeProcOutput.vDesc_total = _nfeProc.NFe.infNFe.total.ICMSTot.vDesc;
                                _nfeProcOutput.vII_total = _nfeProc.NFe.infNFe.total.ICMSTot.vII;
                                _nfeProcOutput.vIPI_total = _nfeProc.NFe.infNFe.total.ICMSTot.vIPI;
                                _nfeProcOutput.vPIS_total = _nfeProc.NFe.infNFe.total.ICMSTot.vPIS;
                                _nfeProcOutput.vCOFINS_total = _nfeProc.NFe.infNFe.total.ICMSTot.vCOFINS;
                                _nfeProcOutput.vOutro_total = _nfeProc.NFe.infNFe.total.ICMSTot.vOutro;
                                _nfeProcOutput.vNF_total = _nfeProc.NFe.infNFe.total.ICMSTot.vNF;
                                _nfeProcOutput.vTotTrib_total = _nfeProc.NFe.infNFe.total.ICMSTot.vTotTrib;
                                #endregion

                                #region Grupo Informações do Transporte -> nfeProc.NFe.infNFe.transp
                                _nfeProcOutput.modFrete_transp = (ModalidadeFreteSW)((int)_nfeProc.NFe.infNFe.transp.modFrete);
                                if (_nfeProc.NFe.infNFe.transp.transporta != null)
                                {
                                    _nfeProcOutput.CNPJ_transp = _nfeProc.NFe.infNFe.transp.transporta.CNPJ;
                                    _nfeProcOutput.CPF_transp = _nfeProc.NFe.infNFe.transp.transporta.CPF;
                                    _nfeProcOutput.xNome = _nfeProc.NFe.infNFe.transp.transporta.xNome;
                                    _nfeProcOutput.IE_transp = _nfeProc.NFe.infNFe.transp.transporta.IE;
                                    _nfeProcOutput.xEnder_transp = _nfeProc.NFe.infNFe.transp.transporta.xEnder;
                                    _nfeProcOutput.xMun_transp = _nfeProc.NFe.infNFe.transp.transporta.xMun;
                                    _nfeProcOutput.UF_transp = _nfeProc.NFe.infNFe.transp.transporta.UF;
                                }
                                if (_nfeProc.NFe.infNFe.transp.retTransp != null)
                                {
                                    _nfeProcOutput.vServ_retTransp_transp = _nfeProc.NFe.infNFe.transp.retTransp.vServ;
                                    _nfeProcOutput.vBCRet_retTransp_transp = _nfeProc.NFe.infNFe.transp.retTransp.vBCRet;
                                    _nfeProcOutput.pICMSRet_retTransp_transp = _nfeProc.NFe.infNFe.transp.retTransp.pICMSRet;
                                    _nfeProcOutput.vICMSRet_retTransp_transp = _nfeProc.NFe.infNFe.transp.retTransp.vICMSRet;
                                    _nfeProcOutput.CFOP_retTransp_transp = _nfeProc.NFe.infNFe.transp.retTransp.CFOP;
                                    _nfeProcOutput.cMunFG_retTransp_transp = _nfeProc.NFe.infNFe.transp.retTransp.cMunFG;
                                }
                                if (_nfeProc.NFe.infNFe.transp.veicTransp != null)
                                {
                                    _nfeProcOutput.placa_veicTransp_transp = _nfeProc.NFe.infNFe.transp.veicTransp.placa;
                                    _nfeProcOutput.UF_veicTransp_transp = _nfeProc.NFe.infNFe.transp.veicTransp.UF;
                                    _nfeProcOutput.RNTC_veicTransp_transp = _nfeProc.NFe.infNFe.transp.veicTransp.RNTC;
                                }
                                _nfeProcOutput.vagao_transp = _nfeProc.NFe.infNFe.transp.vagao;
                                _nfeProcOutput.balsa_transp = _nfeProc.NFe.infNFe.transp.balsa;
                                foreach (NFe.Classes.Informacoes.Transporte.vol _vol in _nfeProc.NFe.infNFe.transp.vol)
                                {
                                    VolOutput _volOutput = new VolOutput();

                                    if (_nfeProcOutput.vol_transp == null)
                                    {
                                        _nfeProcOutput.vol_transp = new List<VolOutput>();
                                    }

                                    _volOutput.qVol = _vol.qVol;
                                    _volOutput.esp = _vol.esp;
                                    _volOutput.marca = _vol.marca;
                                    _volOutput.nVol = _vol.nVol;
                                    _volOutput.pesoL = _vol.pesoL;
                                    _volOutput.pesoB = _vol.pesoB;

                                    if (_volOutput.lacres == null)
                                    {
                                        _volOutput.lacres = new List<LacresOutput>();
                                        foreach (NFe.Classes.Informacoes.Transporte.lacres _lacres in _vol.lacres)
                                        {
                                            _volOutput.lacres.Add(new LacresOutput() { nLacre = _lacres.nLacre });
                                        }
                                    }

                                    _nfeProcOutput.vol_transp.Add(_volOutput);
                                }

                                foreach (NFe.Classes.Informacoes.Transporte.reboque _reboque in _nfeProc.NFe.infNFe.transp.reboque)
                                {
                                    if (_nfeProcOutput.reboque_transp == null)
                                    {
                                        _nfeProcOutput.reboque_transp = new List<ReboqueOutput>();
                                    }

                                    _nfeProcOutput.reboque_transp.Add(new ReboqueOutput() { placa = _reboque.placa, RNTC = _reboque.RNTC, UF = _reboque.UF });
                                }
                                #endregion

                                #region Grupo Cobrança -> nfeProc.NFe.infNFe.cobr
                                _nfeProcOutput.nFat_fat_cobr = _nfeProc.NFe.infNFe.cobr.fat.nFat;
                                _nfeProcOutput.vOrig_fat_cobr = _nfeProc.NFe.infNFe.cobr.fat.vOrig;
                                _nfeProcOutput.vLiq_fat_cobr = _nfeProc.NFe.infNFe.cobr.fat.vLiq;
                                foreach (NFe.Classes.Informacoes.Cobranca.dup _dup in _nfeProc.NFe.infNFe.cobr.dup)
                                {
                                    //if (_nfeProcOutput.dup_cobr == null)
                                    //    _nfeProcOutput.dup_cobr = new List<DupOutput>();

                                    //_nfeProcOutput.dup_cobr.Add(new DupOutput() { dVenc = _dup.dVenc, nDup = _dup.nDup, vDup = _dup.vDup });
                                }
                                #endregion

                                #region Grupo de Informações Adicionais -> nfeProc.NFe.infNFe.infAdic
                                _nfeProcOutput.infCpl_infAdic = _nfeProc.NFe.infNFe.infAdic.infCpl;
                                #endregion


                                _retDistDFeIntOutput.nfeProc.Add(_nfeProcOutput);
                            }
                        }
                        return _retDistDFeIntOutput;
                    }
                    else
                    {
                        throw new UserFriendlyException(retorno.cStat, retorno.xMotivo);
                    }
                }
            });
        }


        /// <summary>
        /// Serviço destinado à distribuição de informações resumidas e documentos fiscais eletrônicos de interesse de um ator, seja este pessoa física ou jurídica.
        /// </summary>
        /// <param name="CNPJ">CNPJ/CPF do interessado no DF-e</param>
        /// <param name="ufAutor">Código da UF do Autor</param>
        /// <param name="chNFe">Chave da Nota Fiscal</param>
        /// <returns>Retorna NF de interesse do CNPJ informado</returns>
        public async Task<RetDistDFeIntOutput> BuscaPorChave(string CNPJ, string ufAutor, string chNFe)
        {
            return await Task.Run(() =>
            {
                using (var service = this.InicializaServico(CNPJ))
                {
                    var retorno = service.NfeDistDFeInteresse(ufAutor, CNPJ, chNFE: chNFe).Retorno;
                    RetDistDFeIntOutput _retDistDFeIntOutput = retorno.MapTo<RetDistDFeIntOutput>();
                    if (retorno.cStat == 138)
                    {
                        for (int i = 0; i < retorno.loteDistDFeInt.Length; i++)
                        {
                            string xmlNfe = DFe.Utils.Compressao.Unzip(retorno.loteDistDFeInt[i].XmlNfe);
                            var _nfeProc = FuncoesXmlSW.XmlStringParaClasse<NFe.Classes.nfeProc>(xmlNfe);
                            var _nfeProcOutput = new NfeProcOutput
                            {
                                xmlNfe = xmlNfe
                            };
                            _retDistDFeIntOutput.nfeProc.Add(_nfeProcOutput);
                        }
                        return _retDistDFeIntOutput;
                    }
                    else
                    {
                        throw new UserFriendlyException(retorno.cStat, retorno.xMotivo);
                    }
                }
            });
        }
    }

    public class ConfiguracaoApp
    {
        private ConfiguracaoServico _cfgServico;

        public ConfiguracaoApp()
        {
            CfgServico = ConfiguracaoServico.Instancia;
            CfgServico.tpAmb = TipoAmbiente.Producao;
            CfgServico.tpEmis = TipoEmissao.teNormal;
            CfgServico.ProtocoloDeSeguranca = ServicePointManager.SecurityProtocol;
            Emitente = new emit { CPF = "", CRT = CRT.SimplesNacional };
            EnderecoEmitente = new NFe.Classes.Informacoes.Emitente.enderEmit();
            ConfiguracaoEmail = new ConfiguracaoEmail("email@dominio.com", "senha", "Envio de NFE", "Resources.MensagemHtml", "smtp.gmail.com", 587, true, true);
            ConfiguracaoCsc = new ConfiguracaoCsc("000001", "");
        }

        public ConfiguracaoServico CfgServico
        {
            get
            {
                ConfiguracaoServico.Instancia.CopiarPropriedades(_cfgServico);
                return _cfgServico;
            }
            set
            {
                _cfgServico = value;
                ConfiguracaoServico.Instancia.CopiarPropriedades(value);
            }
        }

        public emit Emitente { get; set; }
        public enderEmit EnderecoEmitente { get; set; }
        public ConfiguracaoEmail ConfiguracaoEmail { get; set; }
        public ConfiguracaoCsc ConfiguracaoCsc { get; set; }

        /// <summary>
        ///     Salva os dados de CfgServico em um arquivo XML
        /// </summary>
        /// <param name="arquivo">Arquivo XML onde será salvo os dados</param>
        public void SalvarParaAqruivo(string arquivo)
        {
            var camposEmBranco = CfgServico.ObterPropriedadesEmBranco();

            var propinfo = _cfgServico.ObterPropriedadeInfo(c => c.DiretorioSalvarXml);
            camposEmBranco.Remove(propinfo.Name);

            if (camposEmBranco.Count > 0)
                throw new Exception("Informe os dados abaixo antes de salvar as Configurações:" + Environment.NewLine + string.Join(", ", camposEmBranco.ToArray()));

            var dir = Path.GetDirectoryName(arquivo);
            if (dir != null && !Directory.Exists(dir))
            {
                throw new DirectoryNotFoundException("Diretório " + dir + " não encontrado!");
            }
            FuncoesXmlSW.ClasseParaArquivoXml(this, arquivo);
        }
    }

    public class ConfiguracaoCsc
    {
        public ConfiguracaoCsc(string cIdToken, string csc)
        {
            this.CIdToken = cIdToken;
            Csc = csc;
        }

        /// <summary>
        /// Construtor sem parâmetros para serialização
        /// </summary>
        private ConfiguracaoCsc()
        {
        }

        /// <summary>
        /// Identificador do CSC – Código de Segurança do Contribuinte no Banco de Dados da SEFAZ
        /// </summary>
        public string CIdToken { get; set; }

        /// <summary>
        /// Código de Segurança do Contribuinte(antigo Token)
        /// </summary>
        public string Csc { get; set; }
    }



}
