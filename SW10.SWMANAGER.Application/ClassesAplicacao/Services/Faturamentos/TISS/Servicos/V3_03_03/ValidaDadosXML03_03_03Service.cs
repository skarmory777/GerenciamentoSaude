using Abp.Runtime.Session;
using SW10.SWMANAGER.ClassesAplicaca.Services.Faturamentos.VersoesTISS.V3_03_03;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TabelasDominio;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposTabelaDominio;
using SW10.SWMANAGER.ClassesAplicacao.Repositorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas.Dto;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TISS.Servicos.V3_03_03
{
    public class ValidaDadosXML03_03_03Service
    {
        DefaultReturn<string> _retornoPadrao = new DefaultReturn<string>();
        SWRepository<TabelaDominio> tabelaDominioRepository;

        private readonly ISessionAppService _sessionService;
        private readonly IAbpSession _abpSession;


        public ValidaDadosXML03_03_03Service() { }

        public ValidaDadosXML03_03_03Service(IAbpSession abpSession, ISessionAppService sessionService)
        {
            _sessionService = sessionService;
            _abpSession = abpSession;
        }


        public DefaultReturn<string> Validar()
        {
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            return _retornoPadrao;

        }

        public DefaultReturn<string> ValidarCabecalho(FaturamentoEntregaLoteDto faturamentoEntregaLote)
        {

            _retornoPadrao.Warnings = _retornoPadrao.Warnings ?? new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();


            if (string.IsNullOrEmpty(faturamentoEntregaLote.IdentificacaoPrestadorNaOperadora))
            {
                _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTC0001", Parametros = new List<object> { faturamentoEntregaLote.Convenio.NomeFantasia } });
            }

            if (string.IsNullOrEmpty(faturamentoEntregaLote.Convenio.RegistroANS))
            {
                _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTC0002", Parametros = new List<object> { faturamentoEntregaLote.Convenio.NomeFantasia } });
            }



            return _retornoPadrao;
        }

        public DefaultReturn<string> ValidarGuias(FaturamentoContaDto faturamentoConta)
        {
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            tabelaDominioRepository = new SWRepository<TabelaDominio>(_abpSession, _sessionService);

            if (string.IsNullOrEmpty(faturamentoConta.Atendimento.GuiaNumero))
            {
                _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTG0002", Parametros = new List<object> { faturamentoConta.Atendimento.Codigo } });
            }

            ValidarMedico(faturamentoConta.Atendimento.Medico);
            ValidarEspecialidade(faturamentoConta.Atendimento.Especialidade, faturamentoConta.Atendimento.Medico);

            if (faturamentoConta.Atendimento.MotivoAlta != null)
            {
                var motivoAlta = tabelaDominioRepository.GetAll()
                                                        .Where(w => w.Codigo == faturamentoConta.Atendimento.MotivoAlta.Codigo
                                                                 && w.TipoTabelaDominioId == (long)EnumTipoTabelaDominio.MotivoEncerramento)
                                                        .FirstOrDefault();

                if (motivoAlta != null)
                {
                    _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTG0009", Parametros = new List<object> { faturamentoConta.Atendimento.MotivoAlta.Descricao } });
                }
            }

            if (faturamentoConta.Itens == null || faturamentoConta.Itens.Count == 0)
            {
                _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTG0012", Parametros = new List<object> { faturamentoConta.Atendimento.Codigo } });
            }
            else
            {

                foreach (var item in faturamentoConta.Itens)//.Where(w=> w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == null))
                {
                    if (item.Qtde == 0)
                    {
                        _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTG0015", Parametros = new List<object> { item.FaturamentoItem.Descricao, item.FaturamentoConta.Atendimento.Codigo } });
                    }



                    if (item.FaturamentoConfigConvenioDto == null || string.IsNullOrEmpty(item.FaturamentoConfigConvenioDto.Codigo))
                    {
                        _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTG0004", Parametros = new List<object> { faturamentoConta.Convenio.NomeFantasia } });
                    }
                    else
                    {
                        try
                        {
                            var tabela = FuncoesGlobais.ObterValueEnum(typeof(dm_tabela), item.FaturamentoConfigConvenioDto.Codigo, false);
                            if (tabela == null)
                            {
                                _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTG0013", Parametros = new List<object> { faturamentoConta.Convenio.NomeFantasia } });
                            }

                        }
                        catch (Exception)
                        {
                            _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTG0013", Parametros = new List<object> { faturamentoConta.Convenio.NomeFantasia } });
                        }
                    }

                    if (string.IsNullOrEmpty(item.FaturamentoItem.DescricaoTuss))
                    {
                        _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTG0014", Parametros = new List<object> { item.FaturamentoItem.Descricao } });
                    }

                    if (string.IsNullOrEmpty(item.FaturamentoItem.CodTuss))
                    {
                        _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTG0005", Parametros = new List<object> { item.FaturamentoItem.Descricao } });
                    }
                    //else
                    //{

                    //    if (string.IsNullOrEmpty(item.FaturamentoItem.Grupo.CodTipoOutraDespesa.TrimEnd()))
                    //    {
                    //        var procedimento = tabelaDominioRepository.GetAll()
                    //                                                  .Where(w => w.Codigo == item.FaturamentoItem.CodTuss
                    //                                                          && w.TipoTabelaDominioId == (long)EnumTipoTabelaDominio.ProcedimentosEventosEmSaude)
                    //                                                  .FirstOrDefault();

                    //        if (procedimento == null)
                    //        {
                    //            _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTG0006", Parametros = new List<object> { item.FaturamentoItem.Descricao } });
                    //        }
                    //    }
                    //    else
                    //    {
                    //        var medicamento = tabelaDominioRepository.GetAll()
                    //                                              .Where(w => w.Codigo == item.FaturamentoItem.CodTuss
                    //                                                      && w.TipoTabelaDominioId == (long)EnumTipoTabelaDominio.Medicamento)
                    //                                              .FirstOrDefault();

                    //        if (medicamento == null)
                    //        {
                    //            _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTG0006", Parametros = new List<object> { item.FaturamentoItem.Descricao } });
                    //        }
                    //    }

                    //}

                    if (!string.IsNullOrEmpty(item.ViaAcesso))
                    {
                        var viaAcesso = tabelaDominioRepository.GetAll()
                                                                  .Where(w => w.Codigo == item.ViaAcesso
                                                                          && w.TipoTabelaDominioId == (long)EnumTipoTabelaDominio.ViaAcesso)
                                                                  .FirstOrDefault();

                        if (viaAcesso != null)
                        {
                            _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTG0010", Parametros = new List<object> { item.ViaAcesso } });
                        }
                    }

                    if (!string.IsNullOrEmpty(item.Tecnica))
                    {
                        var tecnica = tabelaDominioRepository.GetAll()
                                                                  .Where(w => w.Codigo == item.Tecnica
                                                                          && w.TipoTabelaDominioId == (long)EnumTipoTabelaDominio.TecnicaUtilizada)
                                                                  .FirstOrDefault();

                        if (tecnica != null)
                        {
                            _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTG0011", Parametros = new List<object> { item.Tecnica } });
                        }
                    }

                    ValidarMedico(item.Anestesista);
                    ValidarMedico(item.Auxiliar1);
                    ValidarMedico(item.Auxiliar2);
                    ValidarMedico(item.Auxiliar3);
                    ValidarMedico(item.Medico);
                    ValidarMedico(item.Instrumentador);

                    ValidarEspecialidade(item.EspecialidadeAnestesista?.Especialidade, item.Anestesista);
                    ValidarEspecialidade(item.Auxiliar1Especialidade?.Especialidade, item.Auxiliar1);
                    ValidarEspecialidade(item.Auxiliar2Especialidade?.Especialidade, item.Auxiliar2);
                    ValidarEspecialidade(item.Auxiliar3Especialidade?.Especialidade, item.Auxiliar2);
                    ValidarEspecialidade(item.MedicoEspecialidade?.Especialidade, item.Medico);
                    ValidarEspecialidade(item.InstrumentadorEspecialidade?.Especialidade, item.Instrumentador);
                }

            }



            //if (faturamentoConta.Atendimento.Medico.Conselho == null || string.IsNullOrEmpty(faturamentoConta.Atendimento.Medico.Conselho.Codigo))
            //{
            //    _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTG0001", Parametros = new List<object> { faturamentoConta.Atendimento.Medico.NomeCompleto } });
            //}
            //else
            //{
            //    var conselho = tabelaDominioRepository.GetAll()
            //                                          .Where(w => w.Codigo == faturamentoConta.Atendimento.Medico.Conselho.Codigo
            //                                                  && w.TipoTabelaDominioId == (long)EnumTipoTabelaDominio.ConselhoProfissional)
            //                                          .FirstOrDefault();

            //    if (conselho != null)
            //    {
            //        _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTG0007", Parametros = new List<object> { faturamentoConta.Atendimento.Medico.NomeCompleto } });
            //    }
            //}

            //if (!string.IsNullOrEmpty(faturamentoConta.Atendimento.Especialidade.Cbo))
            //{
            //    var cbo = tabelaDominioRepository.GetAll()
            //                                     .Where(w => w.Codigo == faturamentoConta.Atendimento.Especialidade.Cbo
            //                                              && w.TipoTabelaDominioId == (long)EnumTipoTabelaDominio.ClassificaçãoBrasileiraOcupações_CBO)
            //                                     .FirstOrDefault();

            //    if (cbo == null)
            //    {
            //        _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTG0008", Parametros = new List<object> { faturamentoConta.Atendimento.Medico.NomeCompleto } });
            //    }
            //}



            return _retornoPadrao;
        }

        private void ValidarMedico(MedicoDto medico)
        {
            if (medico != null)
            {

                if (medico.Conselho == null || string.IsNullOrEmpty(medico.Conselho.Codigo))
                {
                    _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTG0001", Parametros = new List<object> { medico.NomeCompleto } });
                }
                else
                {
                    var conselho = tabelaDominioRepository.GetAll()
                                                          .Where(w => w.Codigo == medico.Conselho.Codigo
                                                                  && w.TipoTabelaDominioId == (long)EnumTipoTabelaDominio.ConselhoProfissional)
                                                          .FirstOrDefault();

                    if (conselho == null)
                    {
                        _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTG0007", Parametros = new List<object> { medico.NomeCompleto } });
                    }
                }
            }
        }

        private void ValidarEspecialidade(EspecialidadeDto especialidade, MedicoDto medico)
        {
            if (medico != null && especialidade != null)
            {
                if (!string.IsNullOrEmpty(especialidade.Cbo))
                {
                    var cbo = tabelaDominioRepository.GetAll()
                                                     .Where(w => w.Codigo == especialidade.Cbo
                                                              && w.TipoTabelaDominioId == (long)EnumTipoTabelaDominio.ClassificaçãoBrasileiraOcupações_CBO)
                                                     .FirstOrDefault();
                    if (cbo == null)
                    {
                        _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTG0008", Parametros = new List<object> { medico.NomeCompleto } });
                    }
                }
            }
        }



    }
}
