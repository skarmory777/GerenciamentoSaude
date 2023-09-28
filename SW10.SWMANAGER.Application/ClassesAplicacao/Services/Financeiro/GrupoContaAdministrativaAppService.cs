using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading;
using Abp.UI;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    public class GrupoContaAdministrativaAppService : SWMANAGERAppServiceBase, IGrupoContaAdministrativaAppService
    {
        private readonly IRepository<GrupoContaAdministrativa, long> _grupoContaAdministrativarepositorory;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public GrupoContaAdministrativaAppService(IRepository<GrupoContaAdministrativa, long> grupoContaAdministrativarepositorory
                                                , IUnitOfWorkManager unitOfWorkManager)
        {
            _grupoContaAdministrativarepositorory = grupoContaAdministrativarepositorory;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<ListResultDto<GrupoContaAdministrativaDto>> Listar(ListarGrupoContaAdministrativaInput input)
        {
            try
            {
                List<GrupoContaAdministrativaDto> grupoContasAdministrativasDto = new List<GrupoContaAdministrativaDto>();


                grupoContasAdministrativasDto = _grupoContaAdministrativarepositorory.GetAll()
                                                     .Where(w => (input.Filtro == "" || input.Filtro == null)
                                                                || w.Descricao.ToString().ToUpper().Contains(input.Filtro.ToUpper()))
                                                      .Select(s => new GrupoContaAdministrativaDto
                                                      {
                                                          Id = s.Id,
                                                          Codigo = s.Codigo,
                                                          Descricao = s.Descricao,
                                                          GrupoDRE = new GrupoDREDto { Codigo = s.GrupoDRE.Codigo, Descricao = s.GrupoDRE.Descricao }
                                                      }).ToList();

                return new PagedResultDto<GrupoContaAdministrativaDto>(
                  grupoContasAdministrativasDto.Count,
                  grupoContasAdministrativasDto
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<GrupoContaAdministrativaDto> Obter(long id)
        {
            try
            {
                var query = _grupoContaAdministrativarepositorory
                    .GetAll()
                    .Where(m => m.Id == id)
                    .Include(i => i.GrupoDRE)
                    .Include(i => i.SubGruposContasAdministrativa)
                    .FirstOrDefault();

                var grupoContaAdministrativaDto = query.MapTo<GrupoContaAdministrativaDto>();
                grupoContaAdministrativaDto.SubGruposCntAdm = new List<SubGrupoContaAdministrativaDto>();

                long idGrid = 0;
                foreach (var subGrupo in query.SubGruposContasAdministrativa)
                {
                    var subGrupoDto = subGrupo.MapTo<SubGrupoContaAdministrativaDto>();

                    subGrupoDto.IdGrid = idGrid++;
                    grupoContaAdministrativaDto.SubGruposCntAdm.Add(subGrupoDto);
                }


                return grupoContaAdministrativaDto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public DefaultReturn<GrupoContaAdministrativaDto> CriarOuEditar(GrupoContaAdministrativaDto input)
        {
            var _retornoPadrao = new DefaultReturn<GrupoContaAdministrativaDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    var subGruposDto = JsonConvert.DeserializeObject<List<SubGrupoContaAdministrativaDto>>(input.SubGrupos);

                    var subGrupos = new List<SubGrupoContaAdministrativa>();

                    if (input.Id == 0)
                    {
                        GrupoContaAdministrativa grupoContaAdministrativa = input.MapTo<GrupoContaAdministrativa>();

                        grupoContaAdministrativa.SubGruposContasAdministrativa = new List<SubGrupoContaAdministrativa>();


                        foreach (var subGrupoDto in subGruposDto)
                        {
                            var subGrupo = new SubGrupoContaAdministrativa();

                            subGrupo.Id = subGrupoDto.Id;
                            subGrupo.Codigo = subGrupoDto.Codigo;
                            subGrupo.Descricao = subGrupoDto.Descricao;
                            subGrupo.IsNaoDetalharContaAdministrativa = subGrupoDto.IsNaoDetalharContaAdministrativa;
                            subGrupo.IsSomandoDespesas = subGrupoDto.IsSomandoDespesas;
                            subGrupo.IsSubGrupoContaNaoOperacional = subGrupoDto.IsSubGrupoContaNaoOperacional;
                            subGrupo.IsUsarFormula = subGrupoDto.IsUsarFormula;
                            subGrupo.IsUtilizadoCalculoSalario = subGrupoDto.IsUtilizadoCalculoSalario;
                            grupoContaAdministrativa.SubGruposContasAdministrativa.Add(subGrupo);
                        }

                        AsyncHelper.RunSync(() => _grupoContaAdministrativarepositorory.InsertAsync(grupoContaAdministrativa));

                        _retornoPadrao.ReturnObject = grupoContaAdministrativa.MapTo<GrupoContaAdministrativaDto>();

                    }
                    else
                    {
                        var grupoContaAdministrativa = _grupoContaAdministrativarepositorory.GetAll()
                                                                     .Where(w => w.Id == input.Id)
                                                                     .Include(i => i.SubGruposContasAdministrativa)
                                                                     .FirstOrDefault();

                        if (grupoContaAdministrativa != null)
                        {
                            grupoContaAdministrativa.Codigo = input.Codigo;
                            grupoContaAdministrativa.Descricao = input.Descricao;
                            grupoContaAdministrativa.GrupoDREId = input.GrupoDREId;
                            grupoContaAdministrativa.IsValorIncideResultadoOperacionalEmpresa = input.IsValorIncideResultadoOperacionalEmpresa;



                            if (grupoContaAdministrativa.SubGruposContasAdministrativa == null)
                            {
                                grupoContaAdministrativa.SubGruposContasAdministrativa = new List<SubGrupoContaAdministrativa>();
                            }


                            //Exclui endereços
                            grupoContaAdministrativa.SubGruposContasAdministrativa.ToList().RemoveAll(r => !subGruposDto.Any(a => a.Id == r.Id));

                            //atuliza endereços
                            foreach (var subGrupo in grupoContaAdministrativa.SubGruposContasAdministrativa)
                            {
                                var novoSubGrupo = subGruposDto.Where(w => w.Id == subGrupo.Id)
                                                               .First();

                                subGrupo.Codigo = novoSubGrupo.Codigo;
                                subGrupo.Descricao = novoSubGrupo.Descricao;
                                subGrupo.IsNaoDetalharContaAdministrativa = novoSubGrupo.IsNaoDetalharContaAdministrativa;
                                subGrupo.IsSomandoDespesas = novoSubGrupo.IsSomandoDespesas;
                                subGrupo.IsSubGrupoContaNaoOperacional = novoSubGrupo.IsSubGrupoContaNaoOperacional;
                                subGrupo.IsUsarFormula = novoSubGrupo.IsUsarFormula;
                                subGrupo.IsUtilizadoCalculoSalario = novoSubGrupo.IsUtilizadoCalculoSalario;


                            }

                            //inclui novos endereços
                            foreach (var subGrupo in subGruposDto.Where(w => w.Id == 0))
                            {
                                grupoContaAdministrativa.SubGruposContasAdministrativa.Add(subGrupo.MapTo<SubGrupoContaAdministrativa>());

                            }


                            _grupoContaAdministrativarepositorory.Update(grupoContaAdministrativa);

                            _retornoPadrao.ReturnObject = grupoContaAdministrativa.MapTo<GrupoContaAdministrativaDto>();
                        }
                    }

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

        public async Task Excluir(GrupoContaAdministrativaDto input)
        {
            try
            {
                var grupo = _grupoContaAdministrativarepositorory.GetAll()
                                                                 .Where(w => w.Id == input.Id)
                                                                 .Include(i => i.SubGruposContasAdministrativa)
                                                                 .FirstOrDefault();

                // await _grupoContaAdministrativarepositorory.DeleteAsync(input.Id);
                await _grupoContaAdministrativarepositorory.DeleteAsync(grupo);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }
    }
}
