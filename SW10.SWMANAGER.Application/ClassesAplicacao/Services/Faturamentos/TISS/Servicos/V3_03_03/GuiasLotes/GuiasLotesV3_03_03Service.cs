using Abp.Runtime.Session;
using SW10.SWMANAGER.ClassesAplicaca.Services.Faturamentos.VersoesTISS.V3_03_03;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Repositorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Guias;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Sessions;

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TISS.Servicos.V3_03_03.GuiasLotes
{
    public class GuiasLotesV3_03_03Service
    {
        // private readonly IRepository<Lote>
        private readonly IAbpSession AbpSession;
        private readonly ISessionAppService _sessionService;

        public GuiasLotesV3_03_03Service(IAbpSession abpSession, ISessionAppService sessionService)
        {
            AbpSession = abpSession;
            _sessionService = sessionService;
        }

        public DefaultReturn<ctm_guiaLote> GerarGuiasLote(long loteId, string codigoPrestadorNaOperadora)
        {
            var _retornoPadrao = new DefaultReturn<ctm_guiaLote>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            var _faturamentoEntregaContaRepository = new SWRepository<FaturamentoEntregaConta>(AbpSession, _sessionService);

            var contasLotes = _faturamentoEntregaContaRepository.GetAll()
                                                                .Where(w => w.EntregaLoteId == loteId)
                                                                .Include(i => i.ContaMedica)
                                                                .Include(i => i.ContaMedica.Atendimento)
                                                                .Include(i => i.ContaMedica.Atendimento.Convenio)
                                                                .Include(i => i.ContaMedica.Atendimento.Paciente)
                                                                .Include(i => i.ContaMedica.Atendimento.Paciente.SisPessoa)
                                                                .Include(i => i.ContaMedica.Atendimento.AtendimentoTipo.TabelaDominio)
                                                                .Include(i => i.ContaMedica.Atendimento.FatGuia)
                                                                .Include(i => i.ContaMedica.Atendimento.MotivoAlta)
                                                                .Include(i => i.ContaMedica.Atendimento.MotivoAlta.MotivoAltaTipoAlta)
                                                                .Include(i => i.ContaMedica.Atendimento.AltaGrupoCID)
                                                                .Include(i => i.ContaMedica.Atendimento.Empresa)
                                                                .Include(i => i.ContaMedica.Atendimento.Medico)
                                                                .Include(i => i.ContaMedica.Atendimento.Medico.SisPessoa)
                                                                .Include(i => i.ContaMedica.Atendimento.Medico.Conselho)
                                                                .Include(i => i.ContaMedica.Atendimento.Especialidade)
                                                                .Include(i => i.EntregaLote)
                                                                .Include(i => i.ContaMedica.Atendimento.CaraterAtendimento)
                                                                .Include(i => i.ContaMedica.Atendimento.IndicacaoAcidente)
                                                                .Include(i => i.ContaMedica.Atendimento.ServicoMedicoPrestado)
                                                                .Include(i => i.ContaMedica.Convenio)
                                                                ;

            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.FaturamentoItem))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.FaturamentoItem.Grupo))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.FaturamentoItem.Grupo.FaturamentoCodigoDespesa))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.Anestesista))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.Anestesista.Conselho))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.AnestesistaEspecialidade))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.AnestesistaEspecialidade.Especialidade.SisCbo))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.Auxiliar1))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.Auxiliar1.Conselho))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.Auxiliar1Especialidade))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.Auxiliar1Especialidade.Especialidade.SisCbo))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.Auxiliar2))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.Auxiliar2.Conselho))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.Auxiliar2Especialidade))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.Auxiliar2Especialidade.Especialidade.SisCbo))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.Auxiliar3))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.Auxiliar3.Conselho))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.Auxiliar3Especialidade))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.Auxiliar3Especialidade.Especialidade.SisCbo))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.Instrumentador))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.Instrumentador.Conselho))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.InstrumentadorEspecialidade))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.InstrumentadorEspecialidade.Especialidade.SisCbo))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.Medico))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.Medico.Conselho))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.MedicoEspecialidade))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.MedicoEspecialidade.Especialidade.SisCbo))
            //.Include(i => i.ContaMedica.ContaItens.Select(s => s.FaturamentoConfigConvenio));
            var listaContas = contasLotes.ToList();

            ctm_guiaLote guiaLote = new ctm_guiaLote();

            guiaLote.guiasTISS = new ctm_guiaLoteGuiasTISS();



            guiaLote.guiasTISS.Items = new object[listaContas.Count];



            long codigoGuia = 0;

            if (listaContas.Count > 0)
            {
                long.TryParse(listaContas[0].ContaMedica.Atendimento.FatGuia.Codigo, out codigoGuia);
                guiaLote.numeroLote = listaContas[0].EntregaLote.CodEntregaLote;
            }

            int posicao = 0;

            var listaContasDto = FaturamentoEntregaContaDto.Mapear(listaContas);


            switch (codigoGuia)
            {
                case (long)EnumCodigoTipoGuia.Consulta:

                    GuiaConsultaV3_03_03Service guiaConsulta = new GuiaConsultaV3_03_03Service(AbpSession, _sessionService);
                    var retorno = guiaConsulta.GerarGuiaConsulta(listaContasDto);
                    guiaLote.guiasTISS.Items = retorno.ReturnObject;
                    _retornoPadrao.Errors.AddRange(retorno.Errors);
                    break;

                case (long)EnumCodigoTipoGuia.SPSADT:

                    GuiaSPSADTV3_03_03Service guiaSPADT = new GuiaSPSADTV3_03_03Service(AbpSession, _sessionService);
                    var retornoSADT = guiaSPADT.GerarGuiaSPSADT(listaContasDto, codigoPrestadorNaOperadora);
                    guiaLote.guiasTISS.Items = retornoSADT.ReturnObject;
                    _retornoPadrao.Errors.AddRange(retornoSADT.Errors);
                    break;

                case (long)EnumCodigoTipoGuia.ResumoInternacao:

                    GuiaResumoInternacaoV3_03_03Service guiaResumoInternacao = new GuiaResumoInternacaoV3_03_03Service(AbpSession, _sessionService);
                    var retornoResumoInternacao = guiaResumoInternacao.GerarGuiaResumoInternacoes(listaContasDto);
                    guiaLote.guiasTISS.Items = retornoResumoInternacao.ReturnObject;
                    _retornoPadrao.Errors.AddRange(retornoResumoInternacao.Errors);
                    break;

                case (long)EnumCodigoTipoGuia.HonorarioIndividual:

                    GuiaHonorarioIndividualV3_03_03Service guiaHonorarioIndividual = new GuiaHonorarioIndividualV3_03_03Service(AbpSession, _sessionService);
                    var retornoHonorarioIndividual = guiaHonorarioIndividual.GerarGuiaHonorarioIndividual(listaContasDto, codigoPrestadorNaOperadora);
                    guiaLote.guiasTISS.Items = retornoHonorarioIndividual.ReturnObject;
                    _retornoPadrao.Errors.AddRange(retornoHonorarioIndividual.Errors);

                    break;

                case (long)EnumCodigoTipoGuia.Particular:
                    break;
            }

            _retornoPadrao.ReturnObject = guiaLote;

            return _retornoPadrao;
        }

    }
}
