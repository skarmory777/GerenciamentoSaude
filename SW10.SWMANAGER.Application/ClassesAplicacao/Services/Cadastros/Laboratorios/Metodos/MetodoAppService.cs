using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Metodos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Metodos.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Metodos
{
    public class MetodoAppService : SWMANAGERAppServiceBase, IMetodoAppService
    {
        #region Cabecalho
        private readonly IRepository<Metodo, long> _MetodoRepository;
        private readonly IListarMetodosExcelExporter _listarMetodosExcelExporter;

        public MetodoAppService(IRepository<Metodo, long> MetodoRepository, IListarMetodosExcelExporter listarMetodosExcelExporter)
        {
            _MetodoRepository = MetodoRepository;
            _listarMetodosExcelExporter = listarMetodosExcelExporter;
        }
        #endregion cabecalho.

        public async Task<PagedResultDto<MetodoDto>> Listar(ListarMetodosInput input)
        {
            var contarMetodos = 0;
            List<Metodo> Metodos;
            List<MetodoDto> MetodosDtos = new List<MetodoDto>();
            try
            {
                var query = _MetodoRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarMetodos = await query
                    .CountAsync();

                Metodos = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                MetodosDtos = Metodos
                    .MapTo<List<MetodoDto>>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<MetodoDto>(
                contarMetodos,
                MetodosDtos
                );
        }

        public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input)
        {
            try
            {
                var query = await _MetodoRepository
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.ToUpper())
                    )
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                    .ToListAsync();

                var Metodos = new ListResultDto<GenericoIdNome> { Items = query };

                List<MetodoDto> MetodosList = new List<MetodoDto>();

                //foreach (var Metodo in Metodos.Items)
                //{
                //    MetodosList.Add(Metodo.MapTo<MetodoDto>());
                //}

                //ListResultDto<MetodoDto> MetodosDto = new ListResultDto<MetodoDto> { Items = MetodosList };

                return Metodos;

                //	return new ListResultDto<MetodoDto> { Items = query };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task CriarOuEditar(MetodoDto input)
        {
            try
            {
                var Metodo = input.MapTo<Metodo>();
                if (input.Id.Equals(0))
                {
                    await _MetodoRepository.InsertAsync(Metodo);
                }
                else
                {
                    await _MetodoRepository.UpdateAsync(Metodo);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(MetodoDto input)
        {
            try
            {
                await _MetodoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<MetodoDto> Obter(long id)
        {
            try
            {
                var query = await _MetodoRepository
                    .GetAll()
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var Metodo = query
                    .MapTo<MetodoDto>();

                return Metodo;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FileDto> ListarParaExcel(ListarMetodosInput input)
        {
            try
            {
                var result = await Listar(input);
                var Metodos = result.Items;
                return _listarMetodosExcelExporter.ExportToFile(Metodos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<MetodoDto> pacientesDtos = new List<MetodoDto>();
            try
            {
                //get com filtro
                var query = from p in _MetodoRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                        //||
                        //  m.NomeCompleto.ToLower().Contains(dropdownInput.search.ToLower())
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
