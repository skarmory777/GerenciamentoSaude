using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Exames.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Exames
{
    public class ExameAppService : SWMANAGERAppServiceBase, IExameAppService
    {
        private readonly IRepository<FaturamentoItem, long> _faturamentoItemRepository;

        public ExameAppService(IRepository<FaturamentoItem, long> faturamentoItemRepository)
        {
            _faturamentoItemRepository = faturamentoItemRepository;
        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Create, AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Edit)]
        public async Task Editar(ExameDto input)
        {
            try
            {
                var exame = _faturamentoItemRepository.GetAll()
                                                     .Where(w => w.Id == input.Id)
                                                     .FirstOrDefault();
                if (exame != null)
                {
                    exame.MaterialId = input.MaterialId;
                    exame.FormataId = input.FormataId;
                    exame.Mneumonico = input.Mneumonico;
                    exame.QtdFatura = input.QtdFatura;
                    exame.IsExameSimples = input.IsExameSimples;
                    exame.IsPeso = input.IsPeso;
                    exame.IsTesta100 = input.IsTesta100;
                    exame.IsAltura = input.IsAltura;
                    exame.IsCor = input.IsCor;
                    exame.IsMestruacao = input.IsMestruacao;
                    exame.IsNacionalidade = input.IsNacionalidade;
                    exame.IsNaturalidade = input.IsNaturalidade;
                    exame.IsImpReferencia = input.IsImpReferencia;
                    exame.IsCultura = input.IsCultura;
                    exame.IsPendente = input.IsPendente;
                    exame.IsRepete = input.IsRepete;
                    exame.IsLibera = input.IsLibera;
                    exame.OrdemImp = input.OrdemImp;
                    exame.Prazo = input.Prazo;
                    if (!input.InterpretacaoStr.IsNullOrWhiteSpace())
                    {
                        exame.Interpretacao = FuncoesGlobais.StrToBlob(input.InterpretacaoStr);
                    }
                    if (!input.Extra1Str.IsNullOrWhiteSpace())
                    {
                        exame.Extra1 = FuncoesGlobais.StrToBlob(input.Extra1Str);
                    }
                    if (!input.Extra2Str.IsNullOrWhiteSpace())
                    {
                        exame.Extra2 = FuncoesGlobais.StrToBlob(input.Extra2Str);
                    }
                    exame.MapaExame = input.MapaExame;
                    exame.OrdemResul = input.OrdemResul;
                    exame.OrdemMapaResultado = input.OrdemMapaResultado;
                    exame.OrdemResumo = input.OrdemResumo;
                    exame.EquipamentoId = input.EquipamentoId;
                    exame.ExameIncluiId = input.ExameIncluiId;
                    exame.SetorId = input.SetorId;
                    exame.MetodoId = input.MetodoId;
                    exame.UnidadeId = input.UnidadeId;
                    exame.MapaId = input.MapaId;

                    await _faturamentoItemRepository.UpdateAsync(exame);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task<PagedResultDto<ExameDto>> Listar(ListarExamesInput input)
        {
            var contarTiposTabelaDominio = 0;
            List<FaturamentoItem> exames;
            List<ExameDto> examesDtos = new List<ExameDto>();
            try
            {
                var query = _faturamentoItemRepository
                    .GetAll()
                    .Where(w => w.IsLaboratorio || w.Grupo.IsLaboratorio)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarTiposTabelaDominio = await query
                    .CountAsync();

                exames = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                examesDtos = ExameDto.Mapear(exames);

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<ExameDto>(
                contarTiposTabelaDominio,
                examesDtos
                );
        }

        public async Task<FaturamentoItemDto> Obter(long id)
        {
            try
            {
                FaturamentoItem result = _faturamentoItemRepository.GetAll()
                                                             .Include(i => i.Formata)
                                                             .Include(i => i.Material)
                                                             .Include(i => i.ExameInclui)
                                                             .Include(i => i.BrasItem)
                                                             .Include(i => i.Equipamento)
                                                             .Include(i => i.Grupo)
                                                             .Include(i => i.LaudoGrupo)
                                                             .Include(i => i.Mapa)
                                                             .Include(i => i.Metodo)
                                                             .Include(i => i.Setor)
                                                             .Include(i => i.SubGrupo)
                                                             .Include(i => i.Unidade)
                                                             .Where(w => w.Id == id)
                                                             .FirstOrDefault();
                if (result.ExameIncluiId > 0 && result.ExameInclui == null)
                {
                    result.ExameInclui = await _faturamentoItemRepository.GetAsync(result.ExameIncluiId.Value);
                }
                var exame = FaturamentoItemDto.Mapear(result);
                return exame;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<ExameDto> solicitacaoExameDtos = new List<ExameDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                bool isLaudo = (!dropdownInput.filtro.IsNullOrEmpty()) ? dropdownInput.filtro.Equals("IsLaudo") : false;

                var query = from p in _faturamentoItemRepository.GetAll()
                            .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                                m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) ||
                                m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                            )
                                //.Where(f => f.IsLaudo.Equals(isLaudo))
                            where p.IsLaboratorio || p.Grupo.IsLaboratorio || p.SubGrupo.IsLaboratorio
                            orderby p.Descricao ascending
                            select new DropdownItems
                            {
                                id = p.Id,
                                text = string.Concat(p.Codigo, " - ", p.Descricao)
                            };

                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);


                var result = await queryResultPage.Distinct().ToListAsync();

                int total = await query.Distinct().CountAsync();

                return new ResultDropdownList() { Items = result, TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}

