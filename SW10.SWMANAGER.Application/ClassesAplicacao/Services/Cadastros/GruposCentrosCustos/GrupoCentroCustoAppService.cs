using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.GruposCentroCusto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCentrosCustos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCentrosCustos.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCentroCusto
{
    public class GrupoCentroCustoAppService : SWMANAGERAppServiceBase, IGrupoCentroCustoAppService
    {
        #region Cabecalho
        private readonly IRepository<GrupoCentroCusto, long> _grupoCentroCustoRepositorio;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public GrupoCentroCustoAppService(
            IRepository<GrupoCentroCusto, long> grupoCentroCustoRepositorio,
            IUnitOfWorkManager unitOfWorkManager
            )
        {
            _grupoCentroCustoRepositorio = grupoCentroCustoRepositorio;
            _unitOfWorkManager = unitOfWorkManager;
        }
        #endregion cabecalho.

        public async Task<PagedResultDto<GrupoCentroCustoDto>> Listar(ListarGrupoCentroCustoInput input)
        {
            var contarGruposCentroCusto = 0;
            List<GrupoCentroCusto> gruposCentroCusto;
            List<GrupoCentroCustoDto> gruposCentroCustoDtos = new List<GrupoCentroCustoDto>();
            try
            {
                var query = _grupoCentroCustoRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );


                contarGruposCentroCusto = await query.CountAsync();

                gruposCentroCusto = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                gruposCentroCustoDtos = gruposCentroCusto.MapTo<List<GrupoCentroCustoDto>>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<GrupoCentroCustoDto>(
                contarGruposCentroCusto,
                gruposCentroCustoDtos
                );
        }

        [UnitOfWork]
        public async Task CriarOuEditar(CriarOuEditarGrupoCentroCusto input)
        {
            try
            {
                var grupoCentroCusto = input.MapTo<GrupoCentroCusto>();
                if (input.Id.Equals(0))
                {
                    await _grupoCentroCustoRepositorio.InsertAsync(grupoCentroCusto);
                }
                else
                {
                    await _grupoCentroCustoRepositorio.UpdateAsync(grupoCentroCusto);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task<CriarOuEditarGrupoCentroCusto> Obter(long id)
        {
            try
            {
                try
                {
                    var result = await _grupoCentroCustoRepositorio.GetAsync(id);
                    return result.MapTo<CriarOuEditarGrupoCentroCusto>();
                }
                catch { }
                return new CriarOuEditarGrupoCentroCusto();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task Excluir(CriarOuEditarGrupoCentroCusto input)
        {
            try
            {
                await _grupoCentroCustoRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {

            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<GrupoCentroCustoDto> unidadeOrganizacionaisDtos = new List<GrupoCentroCustoDto>();
            try
            {
                //get com filtro
                var query = from p in _grupoCentroCustoRepositorio.GetAll()
                        //  .WhereIf(dropdownInput.filtro == "inter", m => m.IsInternacao == true)
                        //   .WhereIf(dropdownInput.filtro == "ambEmr", m => m.IsAmbulatorioEmergencia == true)
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        m.Codigo.ToLower().Contains(dropdownInput.search.ToLower()) ||
                        m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                        )
                            orderby p.Descricao ascending
                            select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Descricao) };
                //paginação 
                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                int total = await query.CountAsync();

                return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


    }
}
