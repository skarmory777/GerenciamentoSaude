using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLeito.Dto;
using Abp.Domain.Repositories;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Authorization;
using SW10.SWMANAGER.Authorization;
using System.Data.Entity;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLeito.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposLeito;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposTiposGrupoCentroCustoLeitos.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLeito
{
    public class TipoLeitoAppService : SWMANAGERAppServiceBase, ITipoLeitoAppService
    {
        private readonly IRepository<TipoLeito, long> _tipoLeitoRepositorio;
   //     private readonly IListarTipoLeitoExcelExporter _listarTipoLeitoExcelExporter;

        public TipoLeitoAppService(
            IRepository<TipoLeito, long> tipoLeitoRepositorio
          //  , 
          //  IListarTipoLeitoExcelExporter listarTipoLeitoExcelExporter
            )

        {
            _tipoLeitoRepositorio = tipoLeitoRepositorio;
         //   _listarTipoLeitoExcelExporter = listarTipoLeitoExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TiposLeito_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TiposLeito_Edit)]
        public async Task CriarOuEditar(CriarOuEditarTipoLeito input)
        {
            try
            {
                var tipoLeito = input.MapTo<TipoLeito>();
                if (input.Id.Equals(0))
                {
                    await _tipoLeitoRepositorio.InsertOrUpdateAsync(tipoLeito);
                }
                else
                {
                    await _tipoLeitoRepositorio.UpdateAsync(tipoLeito);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarTipoLeito input)
        {
            try
            {
                await _tipoLeitoRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<TipoLeitoDto>> Listar(ListarTiposLeitoInput input)
        {
            var contarTiposLeito = 0;
            List<TipoLeito> tiposLeito;
            List<TipoLeitoDto> tiposLeitoDtos = new List<TipoLeitoDto>();
            try
            {
                var query = _tipoLeitoRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarTiposLeito = await query
                    .CountAsync();

                tiposLeito = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                tiposLeitoDtos = tiposLeito
                    .MapTo<List<TipoLeitoDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<TipoLeitoDto>(
                contarTiposLeito,
                tiposLeitoDtos
                );
        }

        public async Task<ListResultDto<TipoLeitoDto>> ListarTodos()
        {
            List<TipoLeitoDto> tiposLeitosDtos = new List<TipoLeitoDto>();
            try
            {
                var tiposLeitos = await _tipoLeitoRepositorio
                    .GetAll()
                    .AsNoTracking()
                    .ToListAsync();

                tiposLeitosDtos = tiposLeitos
                    .MapTo<List<TipoLeitoDto>>();

                return new ListResultDto<TipoLeitoDto> { Items = tiposLeitosDtos };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        //public async Task<FileDto> ListarParaExcel(ListarTiposLeitoInput input)
        //{
        //    try
        //    {
        //        var query = await Listar(input);

        //        var tiposLeitoDtos = query.Items;

        //        return _listarTipoLeitoExcelExporter.ExportToFile(tiposLeitoDtos.ToList());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroExportar"));
        //    }

        //}

        public async Task<CriarOuEditarTipoLeito> Obter(long id)
        {
            try
            {
                var result = await _tipoLeitoRepositorio.GetAsync(id);
                var tipoLeito = result.MapTo<CriarOuEditarTipoLeito>();
                return tipoLeito;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<TipoLeitoDto> tipoLeitosDtos = new List<TipoLeitoDto>();
            try
            {
                //get com filtro
                var query = from p in _tipoLeitoRepositorio.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower()) ||
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
