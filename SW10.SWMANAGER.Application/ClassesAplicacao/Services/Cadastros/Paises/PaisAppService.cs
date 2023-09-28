using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Paises;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises
{
    public class PaisAppService : SWMANAGERAppServiceBase, IPaisAppService
    {
        private readonly IRepository<Pais, long> _paisRepository;
        private readonly IListarPaisesExcelExporter _listarPaisesExcelExporter;

        public PaisAppService(IRepository<Pais, long> paisRepository, IListarPaisesExcelExporter listarPaisesExcelExporter)
        {
            _paisRepository = paisRepository;
            _listarPaisesExcelExporter = listarPaisesExcelExporter;
        }

        public async Task CriarOuEditar(CriarOuEditarPais input)
        {
            try
            {
                var pais = input.MapTo<Pais>();
                if (input.Id.Equals(0))
                {
                    await _paisRepository.InsertAsync(pais);
                }
                else
                {
                    var paisEntity = _paisRepository.GetAll()
                                                    .Where(w => w.Id == input.Id)
                                                    .FirstOrDefault();

                    if (paisEntity != null)
                    {
                        paisEntity.Codigo = input.Codigo;
                        paisEntity.Nome = input.Nome;
                        paisEntity.Sigla = input.Sigla;

                        await _paisRepository.UpdateAsync(paisEntity);
                    }


                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarPais input)
        {
            try
            {
                await _paisRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<PaisDto>> Listar(ListarPaisesInput input)
        {
            var contarPaises = 0;
            List<Pais> paises;
            List<PaisDto> paisesDtos = new List<PaisDto>();
            try
            {
                var query = _paisRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
                        m.Sigla.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarPaises = await query
                    .CountAsync();

                paises = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                paisesDtos = paises
                    .MapTo<List<PaisDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<PaisDto>(
                contarPaises,
                paisesDtos
                );
        }

        public async Task<ListResultDto<PaisDto>> ListarTodos()
        {
            var contarPaises = 0;
            List<Pais> paises;
            List<PaisDto> paisesDtos = new List<PaisDto>();
            try
            {
                var query = _paisRepository
                    .GetAll();

                contarPaises = await query
                    .CountAsync();

                paises = await query
                    .AsNoTracking()
                    .ToListAsync();

                paisesDtos = paises
                    .MapTo<List<PaisDto>>();

                return new ListResultDto<PaisDto> { Items = paisesDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input)
        {
            try
            {
                var query = await _paisRepository
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Nome.ToUpper().Contains(input.ToUpper()) ||
                        m.Sigla.ToUpper().Contains(input.ToUpper())
                        )
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Nome })
                    .ToListAsync();

                return new ListResultDto<GenericoIdNome> { Items = query };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarPaisesInput input)
        {
            try
            {
                var result = await Listar(input);
                var paises = result.Items;
                return _listarPaisesExcelExporter.ExportToFile(paises.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<CriarOuEditarPais> Obter(long id)
        {
            try
            {
                var query = await _paisRepository
                    .GetAsync(id);

                var pais = query.MapTo<CriarOuEditarPais>();

                return pais;
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
            List<PaisDto> pacientesDtos = new List<PaisDto>();
            try
            {
                //get com filtro
                var query = from p in _paisRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        //m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                        m.Nome.ToLower().Contains(dropdownInput.search.ToLower()) || m.Sigla.ToLower().Contains(dropdownInput.search.ToLower())
                        )
                            orderby p.Nome ascending
                            select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Nome) };
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
