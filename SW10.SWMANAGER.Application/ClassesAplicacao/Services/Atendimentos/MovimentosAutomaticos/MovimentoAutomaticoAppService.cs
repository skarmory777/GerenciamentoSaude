using Abp.Application.Services.Dto;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.MovimentosAutomaticos;
using SW10.SWMANAGER.ClassesAplicacao.Repositorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.MovimentosAutomaticos.Dto;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Sessions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.MovimentosAutomaticos
{
    using Abp.Auditing;

    public class MovimentoAutomaticoAppService : SWMANAGERAppServiceBase, IMovimentoAutomaticoAppService
    {
        private readonly ISessionAppService _sessionService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public MovimentoAutomaticoAppService(ISessionAppService sessionService
                                            , IUnitOfWorkManager unitOfWorkManager)
        {
            _sessionService = sessionService;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<MovimentoAutomaticoDto> Obter(long id)
        {
            SWRepository<MovimentoAutomatico> movimentoAutomaticoRepository = new SWRepository<MovimentoAutomatico>(AbpSession, _sessionService);

            var movimentoAutomatico = movimentoAutomaticoRepository.GetAll()
                                                                   .Where(w => w.Id == id)
                                                                   .Include(i => i.Empresa)
                                                                   .Include(i => i.UnidadeOrganizacional)
                                                                   .Include(i => i.Turno)
                                                                   .Include(i => i.CentroCusto)
                                                                   .Include(i => i.TipoAcomodacao)
                                                                   .Include(i => i.Terceirizado)
                                                                   .Include(i => i.Terceirizado.SisPessoa)
                                                                   .Include(i => i.MovimentosAutomaticosConveiosPlanos)
                                                                   .Include(i => i.MovimentosAutomaticosEspecialidades)
                                                                   .Include(i => i.MovimentosAutomaticosFaturamentosItens)
                                                                   .Include(i => i.MovimentosAutomaticosTiposGuias)
                                                                   .Include(i => i.MovimentosAutomaticosTiposGuias.Select(s => s.FaturamentoGuia))
                                                                   .Include(i => i.MovimentosAutomaticosEspecialidades)
                                                                   .Include(i => i.MovimentosAutomaticosEspecialidades.Select(s => s.Especialidade))
                                                                   .Include(i => i.MovimentosAutomaticosFaturamentosItens)
                                                                   .Include(i => i.MovimentosAutomaticosFaturamentosItens.Select(s => s.FaturamentoItem))
                                                                   .Include(i => i.MovimentosAutomaticosConveiosPlanos)
                                                                   .Include(i => i.MovimentosAutomaticosConveiosPlanos.Select(s => s.Convenio))
                                                                   .Include(i => i.MovimentosAutomaticosConveiosPlanos.Select(s => s.Convenio.SisPessoa))
                                                                   .Include(i => i.MovimentosAutomaticosConveiosPlanos.Select(s => s.Plano))
                                                                   .FirstOrDefault();

            return MovimentoAutomaticoDto.Mapear(movimentoAutomatico);
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<MovimentoAutomaticoIndex>> Listar(ListarMovimentoAutomaticoInput input)
        {
            List<MovimentoAutomaticoIndex> listIndex = new List<MovimentoAutomaticoIndex>();

            SWRepository<MovimentoAutomatico> movimentoAutomaticoRepository = new SWRepository<MovimentoAutomatico>(AbpSession, _sessionService);

            var query = movimentoAutomaticoRepository.GetAll()
                                                     .Where(w => string.IsNullOrEmpty(input.Filtro) || (w.Codigo.Contains(input.Filtro) || (w.Descricao.Contains(input.Filtro))));


            var movimentosAutomaticosCount = await query.CountAsync().ConfigureAwait(false);

            var movimentosAutomaticos = await query
                                            .AsNoTracking()
                                            .OrderBy(input.Sorting)
                                            .PageBy(input)
                                            .ToListAsync().ConfigureAwait(false);

            MovimentoAutomaticoIndex movimentoAutomatico;

            foreach (var item in movimentosAutomaticos)
            {
                movimentoAutomatico = new MovimentoAutomaticoIndex();
                movimentoAutomatico.Id = item.Id;
                movimentoAutomatico.Codigo = item.Codigo;
                movimentoAutomatico.Descricao = item.Descricao;

                listIndex.Add(movimentoAutomatico);
            }



            return new PagedResultDto<MovimentoAutomaticoIndex>(
                   movimentosAutomaticosCount,
                   listIndex
                   );


        }

        public DefaultReturn<MovimentoAutomaticoDto> CriarOuEditar(MovimentoAutomaticoDto input)
        {
            SWRepository<MovimentoAutomatico> movimentoAutomaticoRepository = new SWRepository<MovimentoAutomatico>(AbpSession, _sessionService);

            var _retornoPadrao = new DefaultReturn<MovimentoAutomaticoDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    var tiposGuias = JsonConvert.DeserializeObject<List<TipoGuiaIndex>>(input.TiposGuias);
                    var especialidades = JsonConvert.DeserializeObject<List<EspecialidadeIndex>>(input.Especialidades);
                    var conveniosPlanos = JsonConvert.DeserializeObject<List<ConvenioPlanoIndex>>(input.ConveniosPlanos);

                    if (input.Id.Equals(0))
                    {
                        MovimentoAutomatico movimentoAutomatico = new MovimentoAutomatico();

                        movimentoAutomatico.Codigo = input.Codigo;
                        movimentoAutomatico.Descricao = input.Descricao;
                        movimentoAutomatico.EmpresaId = input.EmpresaId;
                        movimentoAutomatico.UnidadeOrganizacionalId = input.UnidadeOrganizacionalId;
                        movimentoAutomatico.IsAmbulatorio = input.IsAmbulatorio;
                        movimentoAutomatico.IsInternacao = input.IsInternacao;
                        movimentoAutomatico.IsNovoAtendimento = input.IsNovoAtendimento;
                        movimentoAutomatico.IsDiaria = input.IsDiaria;
                        movimentoAutomatico.IsCobraPernoite = input.IsCobraPernoite;
                        movimentoAutomatico.IsCobraRefeicao = input.IsCobraRefeicao;
                        movimentoAutomatico.IsCobraFralda = input.IsCobraFralda;

                        movimentoAutomatico.TurnoId = input.TurnoId;
                        movimentoAutomatico.CentroCustoId = input.CentroCustoId;
                        movimentoAutomatico.Quantidade = input.Quantidade;
                        movimentoAutomatico.TipoAcomodacaoId = input.TipoAcomodacaoId;
                        movimentoAutomatico.TerceirizadoId = input.TerceirizadoId;

                        //inclui

                        movimentoAutomatico.MovimentosAutomaticosTiposGuias = new List<MovimentoAutomaticoTipoGuia>();

                        foreach (var tipoGuia in tiposGuias.Where(w => w.Id == 0 || w.Id == null))
                        {
                            movimentoAutomatico.MovimentosAutomaticosTiposGuias.Add(new MovimentoAutomaticoTipoGuia
                            {
                                FaturamentoGuiaId = tipoGuia.TipoGuiaId
                            });
                        }

                        movimentoAutomatico.MovimentosAutomaticosEspecialidades = new List<MovimentoAutomaticoEspecialidade>();

                        foreach (var especialidade in especialidades.Where(w => w.Id == 0 || w.Id == null))
                        {
                            movimentoAutomatico.MovimentosAutomaticosEspecialidades.Add(new MovimentoAutomaticoEspecialidade
                            {
                                EspecialidadeId = especialidade.EspecialidadeId
                            });
                        }

                        movimentoAutomatico.MovimentosAutomaticosFaturamentosItens = new List<MovimentoAutomaticoFaturamentoItem>();

                        movimentoAutomatico.MovimentosAutomaticosFaturamentosItens.Add(new MovimentoAutomaticoFaturamentoItem { FaturamentoItemId = input.FaturamentoItemId });


                        movimentoAutomatico.MovimentosAutomaticosConveiosPlanos = new List<MovimentoAutomaticoConvenioPlano>();

                        foreach (var convenioPlano in conveniosPlanos.Where(w => w.Id == 0 || w.Id == null))
                        {
                            movimentoAutomatico.MovimentosAutomaticosConveiosPlanos.Add(new MovimentoAutomaticoConvenioPlano
                            {
                                ConvenioId = convenioPlano.ConvenioId,
                                PlanoId = convenioPlano.PlanoId
                            });
                        }



                        input.Id = movimentoAutomaticoRepository.InsertAndGetId(movimentoAutomatico);

                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();
                    }
                    else
                    {
                        var movimentoAutomatico = movimentoAutomaticoRepository.GetAll()
                                                                            .Where(w => w.Id == input.Id)
                                                                            .Include(i => i.MovimentosAutomaticosTiposGuias)
                                                                            .Include(i => i.MovimentosAutomaticosEspecialidades)
                                                                            .FirstOrDefault();

                        if (movimentoAutomatico != null)
                        {
                            movimentoAutomatico.Codigo = input.Codigo;
                            movimentoAutomatico.Descricao = input.Descricao;
                            movimentoAutomatico.EmpresaId = input.EmpresaId;
                            movimentoAutomatico.UnidadeOrganizacionalId = input.UnidadeOrganizacionalId;
                            movimentoAutomatico.IsAmbulatorio = input.IsAmbulatorio;
                            movimentoAutomatico.IsInternacao = input.IsInternacao;
                            movimentoAutomatico.IsNovoAtendimento = input.IsNovoAtendimento;
                            movimentoAutomatico.IsDiaria = input.IsDiaria;
                            movimentoAutomatico.IsCobraPernoite = input.IsCobraPernoite;
                            movimentoAutomatico.IsCobraRefeicao = input.IsCobraRefeicao;
                            movimentoAutomatico.IsCobraFralda = input.IsCobraFralda;

                            movimentoAutomatico.TurnoId = input.TurnoId;
                            movimentoAutomatico.CentroCustoId = input.CentroCustoId;
                            movimentoAutomatico.Quantidade = input.Quantidade;
                            movimentoAutomatico.TipoAcomodacaoId = input.TipoAcomodacaoId;
                            movimentoAutomatico.TerceirizadoId = input.TerceirizadoId;


                            #region Tipos Guias
                            //Exclui 

                            ((List<MovimentoAutomaticoTipoGuia>)movimentoAutomatico.MovimentosAutomaticosTiposGuias).RemoveAll(r => !tiposGuias.Any(a => a.Id == r.Id));

                            //Inclui
                            foreach (var tipoGuia in tiposGuias.Where(w => w.Id == 0 || w.Id == null))
                            {
                                movimentoAutomatico.MovimentosAutomaticosTiposGuias.Add(new MovimentoAutomaticoTipoGuia
                                {
                                    FaturamentoGuiaId = tipoGuia.TipoGuiaId
                                });
                            }


                            #endregion

                            #region Especialidades

                            if (movimentoAutomatico.MovimentosAutomaticosEspecialidades == null)
                            {
                                movimentoAutomatico.MovimentosAutomaticosEspecialidades = new List<MovimentoAutomaticoEspecialidade>();

                            }

                              //Exclui 

                              ((List<MovimentoAutomaticoEspecialidade>)movimentoAutomatico.MovimentosAutomaticosEspecialidades).RemoveAll(r => !especialidades.Any(a => a.Id == r.Id));

                            //Inclui
                            foreach (var especialidade in especialidades.Where(w => w.Id == 0 || w.Id == null))
                            {
                                movimentoAutomatico.MovimentosAutomaticosEspecialidades.Add(new MovimentoAutomaticoEspecialidade
                                {
                                    EspecialidadeId = especialidade.EspecialidadeId
                                });
                            }


                            #endregion

                            if (movimentoAutomatico.MovimentosAutomaticosFaturamentosItens == null)
                            {
                                movimentoAutomatico.MovimentosAutomaticosFaturamentosItens = new List<MovimentoAutomaticoFaturamentoItem>(); ;
                            }

                             ((List<MovimentoAutomaticoFaturamentoItem>)movimentoAutomatico.MovimentosAutomaticosFaturamentosItens).RemoveAll(r => r.FaturamentoItemId != input.FaturamentoItemId || input.FaturamentoItemId == 0);


                            if (movimentoAutomatico.MovimentosAutomaticosFaturamentosItens.Count == 0)
                            {
                                movimentoAutomatico.MovimentosAutomaticosFaturamentosItens.Add(new MovimentoAutomaticoFaturamentoItem { FaturamentoItemId = input.FaturamentoItemId });
                            }




                            #region Conveio/Plano
                            //Exclui 

                            if (movimentoAutomatico.MovimentosAutomaticosConveiosPlanos == null)
                            {
                                movimentoAutomatico.MovimentosAutomaticosConveiosPlanos = new List<MovimentoAutomaticoConvenioPlano>();
                            }

                              ((List<MovimentoAutomaticoConvenioPlano>)movimentoAutomatico.MovimentosAutomaticosConveiosPlanos).RemoveAll(r => !conveniosPlanos.Any(a => a.Id == r.Id));

                            //Inclui
                            foreach (var convenioPlano in conveniosPlanos.Where(w => w.Id == 0 || w.Id == null))
                            {
                                movimentoAutomatico.MovimentosAutomaticosConveiosPlanos.Add(new MovimentoAutomaticoConvenioPlano
                                {
                                    ConvenioId = convenioPlano.ConvenioId,
                                    PlanoId = convenioPlano.PlanoId
                                });
                            }


                            #endregion


                            movimentoAutomaticoRepository.Update(movimentoAutomatico);

                        }
                    }

                    _retornoPadrao.ReturnObject = input;

                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();
                }

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    var inner = ex.InnerException;
                    _retornoPadrao.Errors.Add(ErroDto.Criar(inner.HResult.ToString(), inner.Message));
                }
                else
                {
                    _retornoPadrao.Errors.Add(ErroDto.Criar(ex.HResult.ToString(), ex.Message));
                }
            }



            return _retornoPadrao;
        }

        public async Task Excluir(long id)
        {
            SWRepository<MovimentoAutomatico> movimentoAutomaticoRepository = new SWRepository<MovimentoAutomatico>(AbpSession, _sessionService);

            try
            {

                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    var movimentoAutomatico = movimentoAutomaticoRepository.GetAll()
                                                                           .Where(w => w.Id == id)
                                                                           .FirstOrDefault();

                    if (movimentoAutomatico != null)
                    {
                        movimentoAutomaticoRepository.Delete(movimentoAutomatico);
                    }

                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();

                }



            }
            catch (Exception)
            {

            }
        }

        public async Task<MovimentoAutomatico> ObterMovimentoAutomaticoParaAtendimento(long atendimentoId)
        {

            SWRepository<Atendimento> atendimentoRepository = new SWRepository<Atendimento>(AbpSession, _sessionService);
            SWRepository<MovimentoAutomatico> movimentoAutomaticoRepository = new SWRepository<MovimentoAutomatico>(AbpSession, _sessionService);

            var atendimento = atendimentoRepository.GetAll()
                                                   .Where(w => w.Id == atendimentoId)
                                                   .FirstOrDefault();

            if (atendimento != null)
            {
                var query = movimentoAutomaticoRepository.GetAll()
                                                                       .Where(w => w.EmpresaId == atendimento.EmpresaId
                                                                               && w.UnidadeOrganizacionalId == atendimento.UnidadeOrganizacionalId
                                                                               && (w.IsAmbulatorio == atendimento.IsAmbulatorioEmergencia || w.IsInternacao == atendimento.IsInternacao)
                                                                               && w.IsNovoAtendimento
                                                                               && w.MovimentosAutomaticosTiposGuias.Any(a => a.FaturamentoGuiaId == atendimento.FatGuiaId)
                                                                               && w.MovimentosAutomaticosEspecialidades.Any(a => a.EspecialidadeId == atendimento.EspecialidadeId)
                                                                               && (w.MovimentosAutomaticosConveiosPlanos.Any(apc => apc.ConvenioId == atendimento.ConvenioId && apc.PlanoId == atendimento.PlanoId)
                                                                                  || (w.MovimentosAutomaticosConveiosPlanos.Any(apc => apc.ConvenioId == atendimento.ConvenioId && apc.PlanoId == null)
                                                                                     && !w.MovimentosAutomaticosConveiosPlanos.Any(apc => apc.ConvenioId == atendimento.ConvenioId && apc.PlanoId == atendimento.PlanoId))
                                                                                  )

                                                                               )
                                                                       .Include(i => i.MovimentosAutomaticosFaturamentosItens)
                                                                       .Include(i => i.MovimentosAutomaticosFaturamentosItens.Select(s => s.FaturamentoItem))
                                                                   ;//    .FirstOrDefault();

                var movimentoAutomatico = query.FirstOrDefault();

                return movimentoAutomatico;
            }


            return null;
        }
    }
}
