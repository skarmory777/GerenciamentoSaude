using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Repositorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItenss;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.Sessions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentosSalaCirurgicas
{
    using Abp.Auditing;

    public class AgendamentoSalaCirurgicaAppService : SWMANAGERAppServiceBase, IAgendamentoSalaCirurgicaAppService
    {
        public async Task<ICollection<AgendamentoCirurgico>> ListarPorSala(long? salaId, long? tipoCirurgiaId, DateTime start, DateTime end, long? empresaId, long? medicoId)
        {
            try
            {
                using (var agendamentoCirurgicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoCirurgico, long>>())
                using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
                {
                    var userId = AbpSession.UserId;
                    var userEmpresas = await userAppService.Object.GetUserEmpresas(userId.Value);

                    var empresaIds = (userEmpresas != null && userEmpresas.Items != null) ? userEmpresas.Items.Select(s => s.Id) : new List<long>();

                    //var query = await _agendamentoConsultaRepository
                    return await agendamentoCirurgicoRepository.Object
                                    .GetAll()
                                    .AsNoTracking()
                                    .Include(m => m.AgendamentoSalaCirurgicaDisponibilidade)
                                    .Include(m => m.AgendamentoSalaCirurgicaDisponibilidade.SalaCirurgica)
                                    .Include(m => m.AgendamentoSalaCirurgicaDisponibilidade.Intervalo)
                                    .Include(m => m.AgendamentoConsulta.Convenio)
                                    .Include(i => i.AgendamentoConsulta.Convenio.SisPessoa)
                                    .Include(m => m.AgendamentoConsulta.Medico)
                                    .Include(m => m.AgendamentoConsulta.Medico.SisPessoa)
                                    .Include(m => m.AgendamentoConsulta.MedicoEspecialidade.Especialidade)
                                    .Include(m => m.AgendamentoConsulta.Paciente)
                                    .Include(m => m.AgendamentoConsulta.Paciente.SisPessoa)
                                    .Include(m => m.AgendamentoConsulta.Plano)
                                    .Include(m => m.AgendamentoConsulta.AgendamentoStatus)
                                    //.WhereIf(medicoId.HasValue && medicoId >= 0, m => m.MedicoId == medicoId.Value)
                                    //.WhereIf(medicoEspecialidadeId.HasValue && medicoEspecialidadeId >= 0, m => m.MedicoEspecialidade.EspecialidadeId == medicoEspecialidadeId.Value)
                                    .Where(m => m.AgendamentoConsulta.DataAgendamento >= start
                                                && m.AgendamentoConsulta.DataAgendamento <= end
                                                && (salaId == null || m.AgendamentoSalaCirurgicaDisponibilidade.SalaCirurgicaId == salaId)
                                                && (tipoCirurgiaId == null || m.AgendamentoSalaCirurgicaDisponibilidade.TipoCirurgiaId == tipoCirurgiaId)
                                                && m.AgendamentoConsulta.IsCirurgia

                                                && (medicoId == null || m.AgendamentoConsulta.MedicoId == medicoId)
                                                && (empresaId == null || m.AgendamentoSalaCirurgicaDisponibilidade.EmpresaId == empresaId)
                                                && (empresaIds.Any(a => a == m.AgendamentoSalaCirurgicaDisponibilidade.EmpresaId))


                                    //&& tipoAgendamento == EnumTipoAgendamento.Consulta)
                                    ) // start <= m.DataAgendamento && end >= m.DataAgendamento)
                                      // .Select(s=> s.AgendamentoConsulta)
                                    .ToListAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ICollection<AgendamentoCirurgico>> ListarPorData(DateTime start, DateTime end)
        {
            using (var agendamentoCirurgicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoCirurgico, long>>())
            {

                var query = await agendamentoCirurgicoRepository.Object
                                .GetAll()
                                .AsNoTracking()
                                .Include(m => m.AgendamentoSalaCirurgicaDisponibilidade.Intervalo)
                                .Include(m => m.AgendamentoConsulta.Convenio)
                                .Include(i => i.AgendamentoConsulta.Convenio.SisPessoa)
                                .Include(m => m.AgendamentoConsulta.Medico)
                                .Include(m => m.AgendamentoConsulta.Medico.SisPessoa)
                                .Include(m => m.AgendamentoConsulta.MedicoEspecialidade.Especialidade)
                                .Include(m => m.AgendamentoConsulta.Paciente)
                                .Include(m => m.AgendamentoConsulta.Paciente.SisPessoa)
                                .Include(m => m.AgendamentoConsulta.Plano)
                                .Where(m => m.AgendamentoConsulta.DataAgendamento >= start && m.AgendamentoConsulta.DataAgendamento <= end)
                                .ToListAsync().ConfigureAwait(false);

                // return AgendamentoCirurgicoDto.Mapear(query);

                return query;
            }
        }

        public async Task CriarOuEditar(AgendamentoCirurgicoDto input)
        {
            // input.MedicoId = 1;
            try
            {
                using (var agendamentoCirurgicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoCirurgico, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    var cirurgias = JsonConvert.DeserializeObject<List<GenericoRelacionamento>>(input.CirurgiasJson);
                    var MateriaisOPME = JsonConvert.DeserializeObject<List<AgendamentoMaterialOPMEJson>>(input.MateriaisOPMEJson, new IsoDateTimeConverter() { DateTimeFormat = "dd/MM/yyyy" });
                    var materiais = JsonConvert.DeserializeObject<List<AgendamentoMaterialJson>>(input.MateriaisJson);


                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {

                        var agendamentoConsulta = AgendamentoCirurgicoDto.Mapear(input);


                        agendamentoConsulta.AgendamentoConsulta.IsCirurgia = true;

                        agendamentoConsulta.Cirurgias = new List<AgendamentoItemFaturamento>();

                        foreach (var item in cirurgias)
                        {
                            agendamentoConsulta.Cirurgias.Add(new AgendamentoItemFaturamento { FaturamentoItemId = item.RelacionadoId ?? 0, Id = item.Id, Quantidade = 1, IsCirurgica = true });
                        }

                        foreach (var item in materiais)
                        {
                            agendamentoConsulta.Cirurgias.Add(new AgendamentoItemFaturamento { FaturamentoItemId = item.FaturamentoItemId ?? 0, Quantidade = item.QuantidadeMaterial, IsCirurgica = false });
                        }


                        agendamentoConsulta.MateriaisOPME = new List<AgendamentoMaterial>();
                        foreach (var item in MateriaisOPME)
                        {
                            agendamentoConsulta.MateriaisOPME.Add(new AgendamentoMaterial
                            {
                                DataPrevista = item.DataPrevista
                            ,
                                DataRecebimento = item.DataRecebimento
                            ,
                                FornecedorId = item.FornecedorId ?? 0
                            ,
                                Material = item.Material
                            ,
                                NumeroNotaFiscal = item.NumeroNota
                            ,
                                Quantidade = item.QuantidadeMaterial ?? 0
                            ,
                                ValorNotaFiscal = item.ValorNota ?? 0
                            ,
                                IsCobrarPeloHospital = item.IsCobraPeloHospital
                            });
                        }


                        if (input.Id.Equals(0))
                        {
                            await agendamentoCirurgicoRepository.Object.InsertAsync(agendamentoConsulta).ConfigureAwait(false);
                        }
                        else
                        {
                            var agendamentoAtual = agendamentoCirurgicoRepository.Object
                                                                                  .GetAll()
                                                                                  .Include(i => i.AgendamentoConsulta)
                                                                                  .Include(i => i.Cirurgias)
                                                                                  .Include(i => Enumerable.Select<AgendamentoItemFaturamento, FaturamentoItem>(i.Cirurgias, s => s.FaturamentoItem))
                                                                                  .Include(i => i.MateriaisOPME)
                                                                                  .FirstOrDefault(w => w.AgendamentoConsulta.Id == input.Id);

                            if (agendamentoAtual != null)
                            {
                                agendamentoAtual.OPMESolicitada = input.OPMESolicitada;
                                agendamentoAtual.OPMEAutorizada = input.OPMEAutorizada;
                                agendamentoAtual.IsPossuiAlergia = input.IsPossuiAlergia;
                                agendamentoAtual.Alergias = input.Alergias;
                                agendamentoAtual.IsPossuiPrecaucoes = input.IsPossuiPrecaucoes;
                                agendamentoAtual.Precaucoes = input.Precaucoes;

                                agendamentoAtual.AgendamentoSalaCirurgicaDisponibilidadeId = agendamentoConsulta.AgendamentoSalaCirurgicaDisponibilidadeId;
                                agendamentoAtual.AgendamentoConsulta.ConvenioId = agendamentoConsulta.AgendamentoConsulta.ConvenioId;
                                agendamentoAtual.AgendamentoConsulta.DataAgendamento = agendamentoConsulta.AgendamentoConsulta.DataAgendamento;
                                agendamentoAtual.AgendamentoConsulta.HoraAgendamento = agendamentoConsulta.AgendamentoConsulta.HoraAgendamento;
                                agendamentoAtual.AgendamentoConsulta.QuantidadeHorarios = agendamentoConsulta.AgendamentoConsulta.QuantidadeHorarios;
                                agendamentoAtual.AgendamentoConsulta.PacienteId = agendamentoConsulta.AgendamentoConsulta.PacienteId;
                                agendamentoAtual.AgendamentoConsulta.ConvenioId = agendamentoConsulta.AgendamentoConsulta.ConvenioId;
                                agendamentoAtual.AgendamentoConsulta.PlanoId = agendamentoConsulta.AgendamentoConsulta.PlanoId;
                                agendamentoAtual.AgendamentoConsulta.Notas = agendamentoConsulta.AgendamentoConsulta.Notas;
                                agendamentoAtual.AgendamentoConsulta.MedicoId = input.MedicoId;
                                agendamentoAtual.AgendamentoConsulta.MedicoEspecialidadeId = input.MedicoEspecialidadeId;

                                agendamentoAtual.AgendamentoConsulta.NomeReservante = input.NomeReservante;
                                agendamentoAtual.AgendamentoConsulta.TelefoneReservante = input.TelefoneReservante;
                                agendamentoAtual.AgendamentoConsulta.DataNascimentoReservante = input.DataNascimentoReservante;
                                agendamentoAtual.AgendamentoConsulta.CPF = input.CPF;
                                agendamentoAtual.AgendamentoConsulta.StatusId = input.StatusId;

                                //Excluir

                                agendamentoAtual.Cirurgias.RemoveAll(r => (!agendamentoConsulta.Cirurgias.Any(a => a.Id == r.Id) && !materiais.Any(a => a.Id == r.Id)));

                                //Incluir
                                foreach (var agendamentoItemFaturamentoDto in agendamentoConsulta.Cirurgias.Where(w => (w.Id == 0)))
                                {
                                    var agendamentoItemFaturamento = new AgendamentoItemFaturamento();

                                    agendamentoItemFaturamento.FaturamentoItemId = agendamentoItemFaturamentoDto.FaturamentoItemId;
                                    agendamentoItemFaturamento.Quantidade = agendamentoItemFaturamentoDto.Quantidade;
                                    agendamentoItemFaturamento.IsCirurgica = agendamentoItemFaturamentoDto.IsCirurgica;


                                    agendamentoAtual.Cirurgias.Add(agendamentoItemFaturamento);
                                }



                                //Editar
                                foreach (var material in agendamentoAtual.Cirurgias.Where(w => w.FaturamentoItem != null && w.FaturamentoItem.IsAgendaMaterial))
                                {
                                    var materialDto = materiais.First(w => w.Id == material.Id);

                                    if (materialDto != null)
                                    {
                                        material.Id = materialDto.Id ?? 0;
                                        material.FaturamentoItemId = materialDto.FaturamentoItemId;
                                        material.Quantidade = materialDto.QuantidadeMaterial;
                                    }
                                }



                                foreach (var agendamentoItemFaturamentoDto in materiais.Where(w => (w.Id == 0 || w.Id == null)))
                                {
                                    var agendamentoItemFaturamento = new AgendamentoItemFaturamento
                                    {
                                        FaturamentoItemId = agendamentoItemFaturamentoDto.FaturamentoItemId,
                                        Quantidade = agendamentoItemFaturamentoDto.QuantidadeMaterial,
                                        IsCirurgica = false
                                    };
                                    agendamentoAtual.Cirurgias.Add(agendamentoItemFaturamento);
                                }







                                #region Materiais OPME

                                agendamentoConsulta.MateriaisOPME = new List<AgendamentoMaterial>();


                                //Excluir
                                agendamentoAtual.MateriaisOPME.RemoveAll(r => !MateriaisOPME.Any(a => a.Id == r.Id));


                                //Editar

                                foreach (var material in agendamentoAtual.MateriaisOPME)
                                {
                                    var rateioDto = MateriaisOPME.Where(w => w.Id == material.Id)
                                                                   .First();

                                    material.Id = rateioDto.Id ?? 0;
                                    material.DataPrevista = rateioDto.DataPrevista;
                                    material.DataRecebimento = rateioDto.DataRecebimento;
                                    material.FornecedorId = rateioDto.FornecedorId ?? 0;
                                    material.Material = rateioDto.Material;
                                    material.NumeroNotaFiscal = rateioDto.NumeroNota;
                                    material.Quantidade = rateioDto.QuantidadeMaterial ?? 0;
                                    material.ValorNotaFiscal = rateioDto.ValorNota ?? 0;
                                    material.IsCobrarPeloHospital = rateioDto.IsCobraPeloHospital;

                                }

                                //Incluir

                                foreach (var item in MateriaisOPME.Where(w => (w.Id == 0 || w.Id == null)))
                                {
                                    agendamentoAtual.MateriaisOPME.Add(new AgendamentoMaterial
                                    {
                                        Id = item.Id ?? 0,
                                        DataPrevista = item.DataPrevista,
                                        DataRecebimento = item.DataRecebimento,
                                        FornecedorId = item.FornecedorId ?? 0,
                                        Material = item.Material,
                                        NumeroNotaFiscal = item.NumeroNota,
                                        Quantidade = item.QuantidadeMaterial ?? 0,
                                        ValorNotaFiscal = item.ValorNota ?? 0,
                                        IsCobrarPeloHospital = item.IsCobraPeloHospital,
                                    });
                                }

                                #endregion

                                await agendamentoCirurgicoRepository.Object.UpdateAsync(agendamentoAtual).ConfigureAwait(false);
                            }
                        }

                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task<AgendamentoCirurgicoDto> Obter(long id)
        {
            using (var agendamentoCirurgicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoCirurgico, long>>())
            {
                var agendamentoCirurgico = await agendamentoCirurgicoRepository.Object.GetAll()
                                                                          .Include(i => i.AgendamentoConsulta)
                                                                          .Include(i => i.AgendamentoConsulta.AgendamentoStatus)
                                                                          .Include(i => i.AgendamentoConsulta.Paciente)
                                                                          .Include(i => i.AgendamentoConsulta.Paciente.SisPessoa)
                                                                          .Include(i => i.AgendamentoConsulta.Paciente.SisPessoa.Sexo)
                                                                          .Include(i => i.AgendamentoConsulta.Plano)
                                                                          .Include(i => i.AgendamentoConsulta.Convenio)
                                                                          .Include(i => i.AgendamentoConsulta.Medico)
                                                                          .Include(i => i.AgendamentoConsulta.Medico.SisPessoa)
                                                                          .Include(i => i.AgendamentoConsulta.MedicoEspecialidade)
                                                                          .Include(i => i.AgendamentoConsulta.MedicoEspecialidade.Especialidade)
                                                                          .Include(i => i.AgendamentoConsulta.Convenio.SisPessoa)
                                                                          .Include(i => i.AgendamentoSalaCirurgicaDisponibilidade)
                                                                          .Include(i => i.AgendamentoSalaCirurgicaDisponibilidade.SalaCirurgica)
                                                                          .Include(i => i.AgendamentoSalaCirurgicaDisponibilidade.TipoCirurgia)
                                                                          .Include(i => i.Cirurgias.Select(s => s.FaturamentoItem))
                                                                          .Include(i => i.MateriaisOPME)
                                                                          .Include(i => i.MateriaisOPME.Select(s => s.Fornecedor))
                                                                          .Include(i => i.MateriaisOPME.Select(s => s.Fornecedor.SisPessoa))
                                                                          .Where(w => w.AgendamentoConsultaId == id)
                                                                          .FirstOrDefaultAsync().ConfigureAwait(false);

                return AgendamentoCirurgicoDto.Mapear(agendamentoCirurgico);
            }
        }

        public async Task<AgendamentoCirurgicoDto> ObterCirurgico(long id)
        {
            using (var agendamentoCirurgicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoCirurgico, long>>())
            {
                var agendamentoCirurgico = agendamentoCirurgicoRepository.Object
                                                                            .GetAll()
                                                                          .AsNoTracking()
                                                                          .Include(i => i.AgendamentoConsulta)
                                                                          .Include(i => i.AgendamentoConsulta.Paciente)
                                                                          .Include(i => i.AgendamentoConsulta.Paciente.Sexo)
                                                                          .Include(i => i.AgendamentoConsulta.Paciente.SisPessoa)
                                                                          .Include(i => i.AgendamentoConsulta.Plano)
                                                                          .Include(i => i.AgendamentoConsulta.Convenio)
                                                                          .Include(i => i.AgendamentoConsulta.Convenio.SisPessoa)
                                                                          .Include(i => i.AgendamentoConsulta.Medico.SisPessoa)
                                                                          .Include(i => i.AgendamentoConsulta.MedicoEspecialidade.Especialidade)
                                                                          .Include(i => i.AgendamentoSalaCirurgicaDisponibilidade)
                                                                          .Include(i => i.AgendamentoSalaCirurgicaDisponibilidade.SalaCirurgica)
                                                                          .Include(i => i.AgendamentoSalaCirurgicaDisponibilidade.TipoCirurgia)
                                                                          .Include(i => Enumerable.Select(i.Cirurgias, s => s.FaturamentoItem))
                                                                          .FirstOrDefault(w => w.AgendamentoConsulta.Id == id);

                return AgendamentoCirurgicoDto.Mapear(agendamentoCirurgico);
            }
        }

        public async Task<DefaultReturn<AgendamentoCirurgicoDto>> RecalcularQuantidadeHorarios(string cirurgiasJson, long agendamentoId, long? disponibilidadeId, DateTime dataAgendamento, DateTime horaAgendamento, long? salaId)
        {
            var retorno =
                new DefaultReturn<AgendamentoCirurgicoDto>
                {
                    ReturnObject = new AgendamentoCirurgicoDto(),
                    Warnings = new List<Dto.ErroDto>()
                };

            using (var agendamentoCirurgicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoCirurgico, long>>())
            using (var sessionService = IocManager.Instance.ResolveAsDisposable<ISessionAppService>())
            {
                var faturamentoItemRepository = new SWRepository<FaturamentoItem>(AbpSession, sessionService.Object);
                var agendamentoConsultaRepository = new SWRepository<AgendamentoConsulta>(AbpSession, sessionService.Object);
                var disponibilidadeRepository = new SWRepository<AgendamentoSalaCirurgicaDisponibilidade>(AbpSession, sessionService.Object);

                //var agendamento = agendamentoConsultaRepository.GetAll()
                //                                               .Where(w => w.Id == agendamentoId)
                //                                               .FirstOrDefault();


                //var agendamento = _agendamentoCirurgicoRepository.GetAll()
                //                                                 .Include(i => i.AgendamentoConsulta)
                //                                                 .Include(i => i.AgendamentoSalaCirurgicaDisponibilidade.Intervalo)
                //                                                 .Where(w => w.AgendamentoConsultaId == agendamentoId)
                //                                                 .FirstOrDefault();

                var disponibilidade = disponibilidadeRepository
                                                               .GetAll()
                                                               .Include(i => i.Intervalo)
                                                               .FirstOrDefault(w => w.Id == disponibilidadeId);


                if (disponibilidade != null)
                {
                    var cirurgias = JsonConvert.DeserializeObject<List<GenericoRelacionamento>>(cirurgiasJson);

                    var tempoTotalAgendamento = 0;

                    foreach (var item in cirurgias)
                    {
                        var faturamentoItem = faturamentoItemRepository
                                                                       .GetAll()
                                                                       .FirstOrDefault(w => w.Id == item.RelacionadoId);

                        if (faturamentoItem != null)
                        {
                            tempoTotalAgendamento += faturamentoItem.QuantidadeMinutos;
                        }
                    }

                    var quantidadeHorarios = ((decimal)tempoTotalAgendamento) / ((decimal)disponibilidade.Intervalo.IntervaloMinutos);

                    if (quantidadeHorarios % 1 != 0)
                    {
                        quantidadeHorarios = Math.Truncate(quantidadeHorarios);
                        quantidadeHorarios++;
                    }



                    var horaProximoAgendamento = await ObterProximoAgendamento(dataAgendamento
                                                                   , disponibilidade.SalaCirurgicaId
                                                                   , horaAgendamento);

                    if (horaProximoAgendamento != null && horaProximoAgendamento != DateTime.MinValue)
                    {
                        var quantidadeMinutos = disponibilidade.Intervalo.IntervaloMinutos * quantidadeHorarios;

                        var finalAgenamento = horaAgendamento.AddMinutes((double)quantidadeMinutos);

                        if (finalAgenamento > horaProximoAgendamento)
                        {
                            retorno.Warnings.Add(new Dto.ErroDto { CodigoErro = "AGE0001" });

                            var dif = (horaProximoAgendamento - horaAgendamento);


                            var diferenca = (dif.Days * 24 * 60) + (dif.Hours * 60) + dif.Minutes;

                            retorno.ReturnObject.QuantidadeHorarios = (int)(diferenca / disponibilidade.Intervalo.IntervaloMinutos);
                        }
                        else
                        {
                            retorno.ReturnObject.QuantidadeHorarios = (int)quantidadeHorarios;
                        }
                    }
                    else
                    {
                        retorno.ReturnObject.QuantidadeHorarios = (int)quantidadeHorarios;
                    }
                }

                return retorno;
            }
        }

        public async Task<DateTime> ObterProximoAgendamento(DateTime date, long salaId, DateTime hora)
        {
            using (var agendamentoCirurgicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoCirurgico, long>>())
            using (var sessionService = IocManager.Instance.ResolveAsDisposable<ISessionAppService>())
            {
                var agendamentoConsultaRepository = new SWRepository<AgendamentoConsulta>(AbpSession, sessionService.Object);

                var horaProximoAgendamento = agendamentoCirurgicoRepository.Object.GetAll().AsNoTracking()
                                                                        .Where(w => w.AgendamentoConsulta.DataAgendamento == date
                                                                                && w.AgendamentoSalaCirurgicaDisponibilidade.SalaCirurgicaId == salaId
                                                                                && w.AgendamentoConsulta.HoraAgendamento > hora)
                                                                        .OrderBy(o => o.AgendamentoConsulta.HoraAgendamento)
                                                                        .Select(s => s.AgendamentoConsulta.HoraAgendamento)
                                                                        .FirstOrDefault();

                return horaProximoAgendamento;
            }
        }

        public async Task<RelatorioAgendamentoDto> ObterAgendamentosDia(DateTime? inicio, DateTime? fim, long? tipoCirurgiaId, long? medicoId)
        {
            using (var agendamentoCirurgicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoCirurgico, long>>())
            {
                var query = await agendamentoCirurgicoRepository.Object.GetAll()
                                .AsNoTracking()
                                .Include(i => i.AgendamentoConsulta.Medico)
                                .Include(i => i.AgendamentoConsulta.Medico.SisPessoa)
                                .Include(i => i.AgendamentoConsulta.Paciente)
                                .Include(i => i.AgendamentoConsulta.Paciente.SisPessoa)
                                .Include(i => i.AgendamentoConsulta.Convenio)
                                .Include(i => i.AgendamentoConsulta.Convenio.SisPessoa)
                                .Include(i => i.Cirurgias)
                                .Include(i => i.AgendamentoSalaCirurgicaDisponibilidade)
                                .Include(i => i.AgendamentoSalaCirurgicaDisponibilidade.SalaCirurgica)
                                .Include(i => i.AgendamentoSalaCirurgicaDisponibilidade.Intervalo)
                                .Include(i => i.Cirurgias.Select(s => s.FaturamentoItem))
                                .Where(w => (inicio == null || w.AgendamentoConsulta.DataAgendamento >= ((DateTime)inicio).Date)
                                            && (fim == null || w.AgendamentoConsulta.DataAgendamento <= ((DateTime)fim).Date)
                                            && (medicoId == null || w.AgendamentoConsulta.MedicoId == medicoId)
                                            && (tipoCirurgiaId == null || w.AgendamentoSalaCirurgicaDisponibilidade.TipoCirurgiaId == tipoCirurgiaId)
                                            && w.AgendamentoConsulta.StatusId != 4 // status 4 = Cancelado
                                ).ToListAsync().ConfigureAwait(false);

                var relatorioAgendamentoDto =
                    new RelatorioAgendamentoDto { AgendamentosDia = new List<AgendamentoDiaDto>() };



                foreach (var item in query)
                {
                    if (item.Cirurgias != null && item.Cirurgias.Count != 0)
                    {
                        var horaFinal = CalcularHoraFinal(item);

                        foreach (var procedimento in item.Cirurgias)
                        {
                            var agendamentoDiaDto = new AgendamentoDiaDto
                            {
                                Hora =
                                                                $" {item.AgendamentoConsulta.HoraAgendamento:HH:mm} / {horaFinal}",
                                Medico = item.AgendamentoConsulta.Medico.NomeCompleto,
                                Paciente =
                                                                item.AgendamentoConsulta.Paciente != null
                                                                    ? item.AgendamentoConsulta.Paciente?.NomeCompleto
                                                                    : item.AgendamentoConsulta.NomeReservante,
                                Procedimento = procedimento.FaturamentoItem?.Descricao,
                                Sala = item.AgendamentoSalaCirurgicaDisponibilidade
                                                                .SalaCirurgica.Descricao,
                                EmpresaId = item.AgendamentoSalaCirurgicaDisponibilidade
                                                                .EmpresaId,
                                Data = $"{item.AgendamentoConsulta.DataAgendamento:dd/MM/yyyy}",
                                Convenio = item.AgendamentoConsulta.Convenio?.NomeFantasia
                            };



                            var maxlength = item.AgendamentoConsulta.Notas.Length > 20 ? 19 : item.AgendamentoConsulta.Notas.Length;
                            agendamentoDiaDto.Notas = item.AgendamentoConsulta.Notas.Substring(0, maxlength);

                            relatorioAgendamentoDto.AgendamentosDia.Add(agendamentoDiaDto);
                        }
                    }
                    else
                    {
                        var agendamentoDiaDto = new AgendamentoDiaDto
                        {
                            Hora =
                                                            $" {item.AgendamentoConsulta.HoraAgendamento:HH:mm} / {this.CalcularHoraFinal(item)}",
                            Medico = item.AgendamentoConsulta.Medico.NomeCompleto,
                            Paciente =
                                                            item.AgendamentoConsulta.Paciente != null
                                                                ? item.AgendamentoConsulta.Paciente.NomeCompleto
                                                                : item.AgendamentoConsulta.NomeReservante,
                            Sala = item.AgendamentoSalaCirurgicaDisponibilidade.SalaCirurgica
                                                            .Descricao,
                            EmpresaId = item.AgendamentoSalaCirurgicaDisponibilidade.EmpresaId,
                            Data = $"{item.AgendamentoConsulta.DataAgendamento:dd/MM/yyyy}",
                            Convenio = item.AgendamentoConsulta.Convenio?.NomeFantasia
                        };

                        //agendamentoDiaDto.Procedimento = procedimento.FaturamentoItem.Descricao;

                        relatorioAgendamentoDto.AgendamentosDia.Add(agendamentoDiaDto);
                    }
                }

                return relatorioAgendamentoDto;
            }

        }

        string CalcularHoraFinal(AgendamentoCirurgico agendamento)
        {
            var horaInicial = agendamento.AgendamentoConsulta.HoraAgendamento;

            var qtdMinutos = agendamento.AgendamentoSalaCirurgicaDisponibilidade.Intervalo.IntervaloMinutos * agendamento.AgendamentoConsulta.QuantidadeHorarios;


            var horaFinal = horaInicial.AddMinutes(qtdMinutos);


            return $"{horaFinal:HH:mm}";
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<ListagemAgendamentoDto>> obterListagem(ListarAgendamentoInput input)
        {
            using (var agendamentoCirurgicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoCirurgico, long>>())
            {
                var query = agendamentoCirurgicoRepository.Object.GetAll()
                                                           .AsNoTracking()
                                                           .Include(i => i.AgendamentoConsulta)
                                                           .Include(i => i.AgendamentoConsulta.AgendamentoStatus)
                                                           .Include(i => i.AgendamentoSalaCirurgicaDisponibilidade.SalaCirurgica)
                                                           .Include(i => i.AgendamentoSalaCirurgicaDisponibilidade.TipoCirurgia)
                                                           .Include(i => i.AgendamentoConsulta.Medico)
                                                           .Include(i => i.AgendamentoConsulta.Medico.SisPessoa)
                                                           .Include(i => i.AgendamentoConsulta.Paciente)
                                                           .Include(i => i.AgendamentoConsulta.Paciente.SisPessoa)
                                                           .Where(w => (input.PacienteId == null || w.AgendamentoConsulta.PacienteId == input.PacienteId)
                                                                   && (input.MedicoId == null || w.AgendamentoConsulta.MedicoId == input.MedicoId)
                                                                   && (input.SalaId == null || w.AgendamentoSalaCirurgicaDisponibilidade.SalaCirurgicaId == input.SalaId)
                                                                   && (input.ProcedimentoId == null || w.Cirurgias.Any(a => a.FaturamentoItemId == input.ProcedimentoId))
                                                                   && (input.Inicio == null || w.AgendamentoConsulta.DataAgendamento >= input.Inicio)
                                                                   && (input.Fim == null || w.AgendamentoConsulta.DataAgendamento <= input.Fim)
                                                                   && (input.TipoCirurgiaId == null || w.AgendamentoSalaCirurgicaDisponibilidade.TipoCirurgiaId == input.TipoCirurgiaId));

                var contaragendamentos = await query
                                             .CountAsync().ConfigureAwait(false);

                var agendamentos = await query
                                       .OrderBy(input.Sorting)
                                       .PageBy(input)
                                       .ToListAsync().ConfigureAwait(false);

                var agendamentosDtos = new List<ListagemAgendamentoDto>();//.Mapear(agendamentos);

                foreach (var item in agendamentos)
                {
                    if (item.AgendamentoConsulta != null)
                    {
                        var agendamento = new ListagemAgendamentoDto
                        {
                            Id = item.AgendamentoConsultaId,
                            Data = item.AgendamentoConsulta.DataAgendamento,
                            Hora = item.AgendamentoConsulta.HoraAgendamento
                        };

                        if (item.AgendamentoConsulta.Paciente != null)
                        {
                            agendamento.Paciente = item.AgendamentoConsulta.Paciente.NomeCompleto;
                        }

                        if (item.AgendamentoConsulta.Medico != null)
                        {
                            agendamento.Medico = item.AgendamentoConsulta.Medico.NomeCompleto;
                        }

                        if (item.AgendamentoSalaCirurgicaDisponibilidade != null && item.AgendamentoSalaCirurgicaDisponibilidade.SalaCirurgica != null)
                        {
                            agendamento.Sala = item.AgendamentoSalaCirurgicaDisponibilidade.SalaCirurgica.Descricao;
                        }
                        if (item.AgendamentoSalaCirurgicaDisponibilidade != null && item.AgendamentoSalaCirurgicaDisponibilidade.TipoCirurgia != null)
                        {
                            agendamento.TipoCirurgia = item.AgendamentoSalaCirurgicaDisponibilidade.TipoCirurgia.Descricao;
                        }

                        if (item.AgendamentoConsulta != null && item.AgendamentoConsulta.AgendamentoStatus != null)
                        {
                            agendamento.Status = item.AgendamentoConsulta.AgendamentoStatus.Descricao;
                            agendamento.StatusId = item.AgendamentoConsulta.AgendamentoStatus.Id;
                        }

                        agendamentosDtos.Add(agendamento);
                    }
                }
                //.MapTo<List<ConvenioDto>>();

                return new PagedResultDto<ListagemAgendamentoDto>(contaragendamentos, agendamentosDtos);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<ProcedimentoDescontoDto>> ObterProcedimentos(
            AgendamentoProcedimentoInput input)
        {
            using (var faturamentoContaItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaItemAppService>())
            using (var agendamentoItemFaturamentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoItemFaturamento, long>>())
            {
                var query = agendamentoItemFaturamentoRepository.Object.GetAll()
                    .AsNoTracking()
                    .Include(i => i.FaturamentoItem)
                    .Include(i => i.AgendamentoCirurgico.AgendamentoSalaCirurgicaDisponibilidade)
                    .Include(i => i.AgendamentoCirurgico.AgendamentoConsulta)
                    .Where(w => w.AgendamentoCirurgico.AgendamentoConsultaId == input.Id);

                var contaragendamentos = await query
                                             .CountAsync().ConfigureAwait(false);

                var agendamentos = await query
                                       .OrderBy(input.Sorting)
                                       .PageBy(input)
                                       .ToListAsync().ConfigureAwait(false);


                var procedimentosDtos = new List<ProcedimentoDescontoDto>();

                if (agendamentos != null && agendamentos.Count > 0)
                {
                    foreach (var item in agendamentos)
                    {
                        var procedimento = new ProcedimentoDescontoDto
                        {
                            Id = item.Id,
                            FaturamentoItemId = item.FaturamentoItemId ?? 0,
                            Procedimento = item.FaturamentoItem?.Descricao
                        };

                        var faturamentoContaItemDto = new FaturamentoContaItemDto
                        {
                            FaturamentoItem = FaturamentoItemDto.Mapear(item.FaturamentoItem),
                            FaturamentoItemId = item.FaturamentoItemId
                        };

                        procedimento.ValorSemDesconto = (decimal)await faturamentoContaItemAppService.Object.CalcularValorUnitarioItem(item.AgendamentoCirurgico.AgendamentoSalaCirurgicaDisponibilidade.EmpresaId ?? 0
                                                                     , item.AgendamentoCirurgico.AgendamentoConsulta.ConvenioId ?? 0
                                                                     , item.AgendamentoCirurgico.AgendamentoConsulta.PlanoId ?? 0
                                                                     , faturamentoContaItemDto).ConfigureAwait(false);


                        procedimento.ValorComDesconto = (procedimento.ValorSemDesconto ?? 0) - (item.ValorDesconto ?? 0);

                        procedimentosDtos.Add(procedimento);
                    }
                }

                return new PagedResultDto<ProcedimentoDescontoDto>(contaragendamentos, procedimentosDtos);
            }
        }

        public async Task<DefaultReturn<AgendamentoCirurgicoDto>> AtualizarDesconto(long agendamentoId, decimal? valorDesconto) // List<ProcedimentoDescontoDto> procedimentos)
        {
            var retorno = new DefaultReturn<AgendamentoCirurgicoDto> { Errors = new List<Dto.ErroDto>() };

            using (var agendamentoCirurgicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoCirurgico, long>>())
            {
                var agendamento = await agendamentoCirurgicoRepository.Object
                                      .GetAll()
                                      .AsNoTracking()
                                      .Include(i => i.Cirurgias)
                                      .FirstOrDefaultAsync(w => w.AgendamentoConsulta.Id == agendamentoId).ConfigureAwait(false);

                if (agendamento.Cirurgias.Any())
                {
                    var valorDescontoParcela = valorDesconto / agendamento.Cirurgias.Count();
                    agendamento.Cirurgias.ForEach(f => { f.ValorDesconto = valorDescontoParcela; f.UsuarioDescontoId = AbpSession.UserId; });
                }

                return retorno;
            }
        }
    }
}
