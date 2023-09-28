using Abp.Domain.Services;
using Dapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.DomainServices._3_05_00;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.DomainServices
{
    public class GeraLoteDomainService : SWMANAGERDomainServiceBase, IGeraLoteDomainService
    {
        public async Task<EntregaTissLoteGerado> GerarLote(long fatEntregaLoteId)
        {
            var query = @"
                EXEC sp_FatTISSCabec @fatEntregaLoteId;
                EXEC sp_FatTISSGuias @fatEntregaLoteId;
                EXEC sp_FatTISSHonInd @fatEntregaLoteId;
                EXEC sp_FatTISSOutrasDespesas @fatEntregaLoteId, null;
                EXEC sp_FatTISSProcRealizados @fatEntregaLoteId, null;
                EXEC sp_FatTISSMembroEquipe @fatEntregaLoteId, null, null;
            ";
            using (var connection = new SqlConnection(this.GetConnection()))
            {
                var entrega = new SpEntrega();
                try
                {
                    using (var multi = await connection.QueryMultipleAsync(query, new { FatEntregaLoteId = fatEntregaLoteId }))
                    {
                        entrega.Lote = await multi.ReadFirstAsync<SpEntregaLoteDto>();

                        entrega.Guias = await multi.ReadAsync<SpEntregaLoteGuiasDto>();

                        var honorarios = await multi.ReadAsync<SpEntregaLoteHonIndDto>();

                        var outrasDespesas = await multi.ReadAsync<SpEntregaLoteGuiasOutrasDespesasDto>();
                        var procRealizados = await multi.ReadAsync<SpEntregaLoteGuiasProcRealizadosDto>();
                        var membroEquipes = await multi.ReadAsync<SpEntregaLoteGuiasMembroEquipeDto>();

                        foreach (var guia in entrega.Guias)
                        {
                            guia.Honorarios = honorarios.Where(x => x.FatContaMedicaId == guia.FatContaId);
                            guia.ProcRealizados = procRealizados.Where(x => x.FatContaMedicaId == guia.FatContaId);
                            guia.OutrasDespesas = outrasDespesas.Where(x => x.FatContaMedicaId == guia.FatContaId);
                            guia.MembroEquipes = membroEquipes.Where(x => x.FatContaMedicaId == guia.FatContaId);
                        }

                        var tissConverter = new SpEntregaConvertTiss3_05_00();
                        return tissConverter.ConvertMensagemTISS(entrega);
                        
                    }
                }
                catch (Exception e)
                {

                }


                return null;
            }
        }
    }

    


    public interface IGeraLoteDomainService : IDomainService
    {
        public Task<EntregaTissLoteGerado> GerarLote(long fatEntregaLoteId);
    }

    public class SpEntrega
    {
        public SpEntregaLoteDto Lote { get; set; }
        public IEnumerable<SpEntregaLoteGuiasDto> Guias { get; set; }

        public string EpilogoHash { get; set; }
        public StringBuilder Hash { get; set; } = new StringBuilder();

        
    }

    public class SpEntregaLoteDto
    {
        public string TipoTransacao { get; set; }
        public string SequencialTransacao { get; set; }

        public string DataRegistroTransacao { get; set; }

        public string HoraRegistroTransacao { get; set; }

        public int? FatGuiaId { get; set; }
        public string ContratadoDados { get; set; }
        public string CodigoPrestadorNaOperadora { get; set; }

        public string RegistroANS { get; set; }

        public string NumeroLote { get; set; }
        public string NomeAplicativo { get; set; }

        public string VersaoAplicativo { get; set; }

        public string FabricanteAplicativo { get; set; }

        public string VersaoPadrao { get; set; }

        public string Url { get; set; }
    }

    public class SpEntregaLoteGuiasDto
    {
        public long FatContaId { get; set; }

        public long AteAtendimentoId { get; set; }

        

        public string RegistroANS { get; set; }
        public string CnpjFontePagadora { get; set; }
        public string DataEmissaoGuia { get; set; }
        public string NumeroGuiaPrestador { get; set; }
        public string NumeroGuiaPrincipal { get; set; }
        public string NumeroCarteira { get; set; }
        public string NomeBeneficiario { get; set; }

        public string NomePlano { get; set; }

        public string ValidadeCarteira { get; set; }

        public string NomeContratado { get; set; }

        public string TipoLogradouroContratado { get; set; }

        public string LogradouroContratado { get; set; }

        public string NumeroContratado { get; set; }

        public string ComplementoContratado { get; set; }

        public string CodIBGEContratado { get; set; }

        public string MunicipioContratado { get; set; }

        public string CodUFContratado { get; set; }

        public string CepContratado { get; set; }

        public string NumeroConselho { get; set; }

        public string UfConselho { get; set; }

        public string CodigoDiagnostico { get; set; }
        public string Cbos { get; set; }

        public string CbosSus { get; set; }
        public string DataRegistro { get; set; }

        public string TipoConsulta { get; set; }

        public string DataAutorizacao { get; set; }

        public string SenhaAutorizacao { get; set; }

        public string ValidadeSenha { get; set; }

        public string DataHoraAtendimento { get; set; }

        public string CaraterAtendimento { get; set; }

        public string IndicacaoClinica { get; set; }

        public string CodigoCBOSSUS { get; set; }

        public string CodigoCBOS { get; set; }

        public string NomeTabela { get; set; }
        public string TipoDoenca { get; set; }

        public string TipoSaida { get; set; }

        public string DataHoraSaidaInternacao { get; set; }

        public string CaraterInternacao { get; set; }

        public string NumeroGuiaSolicitacao { get; set; }

        public string Acomodacao { get; set; }

        public string TipoFaturamento { get; set; }

        public string TipoInternacao { get; set; }

        public string RegimeInternacao { get; set; }

        public bool IsInternacaoObstetrica { get; set; }

        public bool EmGestacao { get; set; }

        public bool Aborto { get; set; }

        public bool TranstornoMaternoRelGravidez { get; set; }

        public bool ComplicacaoPeriodoPuerperio { get; set; }

        public bool AtendimentoRNSalaParto { get; set; }

        public bool ComplicacaoNeonatal { get; set; }

        public bool BaixoPeso { get; set; }

        public bool PartoCesareo { get; set; }

        public bool PartoNormal { get; set; }

        public bool ObitoMulher { get; set; }

        public string NumeroDN { get; set; }

        public int QtdNascidosVivosTermo { get; set; }

        public int QtdNascidosMortos { get; set; }

        public int QtdVivosPrematuros { get; set; }

        public int QtdeobitoPrecoce { get; set; }

        public int QtdeobitoTardio { get; set; }

        public string MotivoSaidaInternacao { get; set; }

        public string NumeroDeclaracao { get; set; }

        public string Identificacao { get; set; }

        public string NumeroCNES { get; set; }

        public string NomeProfissional { get; set; }
        public string SiglaConselho { get; set; }

        public string Observacao { get; set; }

        public string CGC { get; set; }

        public string CIDObito { get; set; }

        public string CIDObs { get; set; }
        public string IndicadorAcidente { get; set; }

        public string DataInicioFaturamento { get; set; }
        public string HoraInicioFaturamento { get; set; }

        public string DataFinalFaturamento { get; set; }

        public string HoraFinalFaturamento { get; set; }

        public string AtendimentoRN { get; set; }


        public IEnumerable<SpEntregaLoteGuiasOutrasDespesasDto> OutrasDespesas { get; set; }
        public IEnumerable<SpEntregaLoteGuiasProcRealizadosDto> ProcRealizados { get; set; }
        public IEnumerable<SpEntregaLoteGuiasMembroEquipeDto> MembroEquipes { get; set; }
        public IEnumerable<SpEntregaLoteHonIndDto> Honorarios { get;  set; }

        public SpEntregaLoteGuiaValorTotalDto GuiaValorTotal { get; set; } = new SpEntregaLoteGuiaValorTotalDto();
    }

    public class SpEntregaLoteHonIndDto
    {
        public long FatEntregaLoteID { get; set; }

        public long FatContaMedicaId { get; set; }

        public long FatContaItemID { get; set; }

        public string TagXML { get; set; }

        public string RegistroANS { get; set; }
        public string CnpjFontePagadora { get; set; }
        public string DataEmissaoGuia { get; set; }
        public string NumeroGuiaPrestador { get; set; }
        public string NumeroGuiaPrincipal { get; set; }
        public string NumeroCarteira { get; set; }
        public string NomeBeneficiario { get; set; }
        public string NomePlano { get; set; }
        public string ValidadeCarteira { get; set; }
        public string Data { get; set; }
        public string ViaAcesso { get; set; }
        public string TecnicaUtilizada { get; set; }

        public string HoraInicio { get; set; }

        public string HoraFim { get; set; }

        public string NomeContratado { get; set; }

        public string NumeroCNES { get; set; }
        public string NomeExecutante { get; set; }
        public string SiglaConselho { get; set; }

        public string NumeroConselho { get; set; }

        public string UfConselho { get; set; }

        public string CodigoCBOS { get; set; }

        public string CodigoCBOSSUS { get; set; }

        public string PosicaoProfissional { get; set; }

        public string TipoAcomodacao { get; set; }

        public string TipoTabela { get; set; }

        public string Codigo { get; set; }

        public string Descricao { get; set; }

        public int QuantidadeRealizada { get; set; }

        public long SisMedicoID { get; set; }

        public double IsPercTaxaMultiplic { get; set; }

        public string CodCredenciado { get; set; }

        public string RefMed { get; set; }

        public string TagCodCredenciado { get; set; }

        public string Empresa { get; set; }

        public string CNES { get; set; }

        public double valor { get; set; }

        public double valorTotal { get; set; }

        public double ReducaoAcrescimo { get; set; }

        public string SenhaAutorizacao { get; set; }

        public string DataInicioFaturamento { get; set; }

        public string DataFinalFaturamento { get; set; }

        public string IsFontePagadora { get; set; }

        public string GuiaOperadora { get; set; }
        public string IsImpObsGuia { get; set; }

        public string Observacao { get; set; }

        public string CIDObs { get; set; }
    }

    public class SpEntregaLoteGuiasOutrasDespesasDto
    {
        public long FatEntregaLoteId { get; set; }
        public long FatContaMedicaId { get; set; }

        public string Codigo { get; set; }

        public string DataRealizacao { get; set; }

        public string HoraInicial { get; set; }

        public string HoraFinal { get; set; }

        public string TipoDespesa { get; set; }

        public double ValorUnitario { get; set; }

        public double Quantidade { get; set; }

        public double ValorTotal { get; set; }

        public string PercTaxas { get; set; }

        public string TipoTabela { get; set; }

        public string Descricao { get; set; }

        public string LocalOrdem { get; set; }

        public bool IsOpm { get; set; }

        public string CodUnidadeANS { get; set; }

        public string RegistroAnvisa { get; set; }

        public string CodigoRefFabricante { get; set; }

        public string AutorizacaoFuncionamento { get; set; }
        public int SequencialItem { get; internal set; }
    }

    public class SpEntregaLoteGuiasProcRealizadosDto
    {
        public long FatEntregaLoteID { get; set; }

        public long FatContaMedicaId { get; set; }

        public string Data { get; set; }

        public string ViaAcesso { get; set; }

        public string TecnicaUtilizada { get; set; }

        public string ViaAcessoV3 { get; set; }

        public string TecnicaUtilizadaV3 { get; set; }

        public string HoraInicio { get; set; }

        public string HoraFim { get; set; }

        public string CodigoCBOSSUS { get; set; }

        public double Valor { get; set; }

        public double Quantidade { get; set; }

        public double ValorTotal { get; set; }

        public string PercTaxas { get; set; }

        public string TipoTabela { get; set; }

        public string CodigoProcedimento { get; set; }

        public string Descricao { get; set; }

        public string PosicaoProfissional { get; set; }

        public long IDMedico { get; set; }

        public long IDAuxiliar1 { get; set; }

        public long IDAuxiliar2 { get; set; }

        public long IDAuxiliar3 { get; set; }

        public long IDInstrumentador { get; set; }

        public double ValUnitAux1 { get; set; }

        public double ValUnitAux2 { get; set; }

        public double ValUnitAux3 { get; set; }

        public double ValUnitInst { get; set; }

        public double ValTotAux1 { get; set; }

        public double ValTotAux2 { get; set; }

        public double ValTotAux3 { get; set; }

        public double ValTotInst { get; set; }

        public double ValorTaxas { get; set; }

        public int? Ordem { get; set; }

        public int? LocalOrdem { get; set; }

        public long SisConvenioId { get; set; }

        public long FatGuiaId { get; set; }
        public string CodigoCBOSAux1 { get; set; }

        public string CodigoCBOSAux2 { get; set; }

        public string CodigoCBOSAux3 { get; set; }

        public string CodigoCBOSInst { get; set; }

        public long SequencialItem { get; set; }


    }

    public class SpEntregaLoteGuiasMembroEquipeDto
    {
        public long FatEntregaLoteId { get; set; }
        public long FatContaMedicaId { get; set; }
        public long SisMedicoId { get; set; }

        public long FatContaItemID { get; set; }

        public string TipoProfissional { get; set; }

        public string SeqTipoProfissional { get; set; }

        public string NomeExecutante { get; set; }

        public string NumeroConselho { get; set; }

        public string SiglaConselho { get; set; }

        public string UfConselho { get; set; }

        public string Cpf { get; set; }

        public string CodCredenciadoMed { get; set; }

        public bool IsCodCredenciadoMed { get; set; }
    }

    public class SpEntregaLoteGuiaValorTotalDto
    {
        public double ValorProcedimentos { get; set; }

        public double ValorDiarias { get; set; }

        public double ValorTaxasAlugueis { get; set; }

        public double ValorMateriais { get; set; }

        public double ValorMedicamentos { get; set; }

        public double ValorOPME { get; set; }

        public double ValorGasesMedicinais { get; set; }

        public double ValorTotalGeral { 
            get {
                return ValorProcedimentos + ValorDiarias + ValorTaxasAlugueis + ValorMateriais + ValorMedicamentos + ValorOPME + ValorGasesMedicinais;
            } 
        }
    }
}

