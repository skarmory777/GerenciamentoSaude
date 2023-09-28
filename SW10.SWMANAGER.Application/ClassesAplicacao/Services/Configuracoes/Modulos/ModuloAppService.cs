using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Modulos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Modulos.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Modulos
{
    public class ModuloAppService : SWMANAGERAppServiceBase, IModuloAppService
    {
        private readonly IRepository<Modulo, long> _moduloRepository;

        public ModuloAppService(
            IRepository<Modulo, long> moduloRepository
            )
        {
            _moduloRepository = moduloRepository;
        }

        public async Task CriarOuEditar(ModuloDto input)
        {
            try
            {
                var modulo = input.MapTo<Modulo>();
                if (input.Id.Equals(0))
                {
                    await _moduloRepository.InsertAsync(modulo);
                }
                else
                {
                    await _moduloRepository.UpdateAsync(modulo);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(ModuloDto input)
        {
            try
            {
                await _moduloRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<ModuloDto>> Listar(ListarInput input)
        {
            var contarModulos = 0;
            List<Modulo> modulos;
            List<ModuloDto> modulosDtos = new List<ModuloDto>();
            try
            {
                var query = _moduloRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.ToUpper().Contains(input.Filtro.ToUpper()) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarModulos = await query
                    .CountAsync();

                modulos = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                modulosDtos = modulos
                    .MapTo<List<ModuloDto>>();

                return new PagedResultDto<ModuloDto>(
                    contarModulos,
                    modulosDtos
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<ModuloDto>> ListarTodos()
        {
            try
            {
                var query = await _moduloRepository
                    .GetAllListAsync();

                var modulos = query.MapTo<List<ModuloDto>>();
                return new ListResultDto<ModuloDto> { Items = modulos };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        //public async Task<FileDto> ListarParaExcel(ListarInput input)
        //{
        //    try
        //    {
        //        var result = await Listar(input);
        //        var modulos = result.Items;
        //        return _listarModulosExcelExporter.ExportToFile(modulos.ToList());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroExportar"));
        //    }
        //}

        public async Task<ModuloDto> Obter(long id)
        {
            try
            {
                var result = await _moduloRepository
                    .GetAllListAsync(m => m.Id == id);

                var modulo = result
                    .FirstOrDefault()
                    .MapTo<ModuloDto>();

                return modulo;
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
            try
            {
                //get com filtro
                var query = from p in _moduloRepository.GetAll()
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

                return new ResultDropdownList() { Items = queryResultPage.Distinct().ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
