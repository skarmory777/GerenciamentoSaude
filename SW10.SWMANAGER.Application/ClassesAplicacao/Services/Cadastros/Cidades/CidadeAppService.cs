using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades
{
    public class CidadeAppService : SWMANAGERAppServiceBase, ICidadeAppService
    {
        #region Cabecalho
        private readonly IRepository<Cidade, long> _cidadeRepository;
        private readonly IListarCidadesExcelExporter _listarCidadesExcelExporter;
        private readonly IEstadoAppService _estadoAppService;

        public CidadeAppService(IRepository<Cidade, long> cidadeRepository, IListarCidadesExcelExporter listarCidadesExcelExporter, IEstadoAppService estadoAppService)
        {
            _cidadeRepository = cidadeRepository;
            _listarCidadesExcelExporter = listarCidadesExcelExporter;
            _estadoAppService = estadoAppService;
        }
        #endregion cabecalho.

        public async Task<PagedResultDto<CidadeDto>> Listar(ListarCidadesInput input)
        {
            var contarCidades = 0;
            List<Cidade> cidades;
            List<CidadeDto> cidadesDtos = new List<CidadeDto>();
            try
            {
                var query = _cidadeRepository
                    .GetAll()
                    .Include(m => m.Estado)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Nome.Contains(input.Filtro)
                    )
                    .WhereIf(!input.EstadoId.Equals(0), m =>
                        m.EstadoId == input.EstadoId
                    );

                contarCidades = await query
                    .CountAsync();

                cidades = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                cidadesDtos = cidades
                    .MapTo<List<CidadeDto>>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<CidadeDto>(
                contarCidades,
                cidadesDtos
                );
        }

        public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input, long? estadoId)
        {
            try
            {
                var query = await _cidadeRepository
                    .GetAll()
                    .Include(m => m.Estado)
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Nome.Contains(input)
                    )
                    .WhereIf(estadoId.HasValue, m =>
                     m.EstadoId == estadoId)
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Nome })
                    .ToListAsync();

                var cidades = new ListResultDto<GenericoIdNome> { Items = query };

                List<CidadeDto> cidadesList = new List<CidadeDto>();

                //foreach (var cidade in cidades.Items)
                //{
                //    cidadesList.Add(cidade.MapTo<CidadeDto>());
                //}

                //ListResultDto<CidadeDto> cidadesDto = new ListResultDto<CidadeDto> { Items = cidadesList };

                return cidades;

                //	return new ListResultDto<CidadeDto> { Items = query };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task CriarOuEditar(CidadeDto input)
        {
            try
            {
                var Cidade = input.MapTo<Cidade>();
                if (input.Id.Equals(0))
                {
                    await _cidadeRepository.InsertAsync(Cidade);
                }
                else
                {
                    var cidadeEntity = _cidadeRepository.GetAll()
                                                        .Where(w => w.Id == input.Id)
                                                        .FirstOrDefault();

                    if (cidadeEntity != null)
                    {

                        cidadeEntity.Nome = input.Nome;
                        cidadeEntity.EstadoId = input.EstadoId;
                        cidadeEntity.IsCapital = input.Capital;

                        await _cidadeRepository.UpdateAsync(cidadeEntity);
                    }




                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(CidadeDto input)
        {
            try
            {
                await _cidadeRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<CidadeDto> Obter(long id)
        {
            try
            {
                var query = await _cidadeRepository
                    .GetAll()
                    .Include(m => m.Estado)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var cidade = query
                    .MapTo<CidadeDto>();

                return cidade;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<CidadeDto> ObterComEstado(string nome, long estadoId)
        {
            try
            {
                var query = _cidadeRepository
                    .GetAll()
                    .Include(m => m.Estado)
                    .Where(m =>
                        m.Nome == (nome) &&
                        m.EstadoId == estadoId
                    );

                var result = await query.FirstOrDefaultAsync();

                var cidade = result
                    .MapTo<CidadeDto>();

                return cidade;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FileDto> ListarParaExcel(ListarCidadesInput input)
        {
            try
            {
                var result = await Listar(input);
                var cidades = result.Items;
                return _listarCidadesExcelExporter.ExportToFile(cidades.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<ListResultDto<CidadeDto>> ListarPorEstado(long? id)
        {
            try
            {
                var query = _cidadeRepository
                    .GetAll()
                    .Include(m => m.Estado)
                    .WhereIf(id.HasValue && id > 0,
                        m => m.EstadoId == id
                    );

                var result = await query
                    .ToListAsync();

                var cidades = new ListResultDto<CidadeDto>
                {
                    Items = result.MapTo<List<CidadeDto>>()
                };

                return cidades;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {

            //int pageInt = int.Parse(dropdownInput.page) - 1;
            //var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            //List<EstadoDto> pacientesDtos = new List<EstadoDto>();

            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<CidadeDto> pacientesDtos = new List<CidadeDto>();
            try
            {
                //se um estado estiver selecionado, filtra apenas pelos suas cidades
                if (dropdownInput.filtro == "Digite um nome")
                {
                    dropdownInput.filtro = null;
                }
                long estadoId;
                long.TryParse(dropdownInput.filtro, out estadoId);

                //get com filtro
                var query = from p in _cidadeRepository.GetAll()
                        // .Where(m => m.Descricao != null && m.Descricao.Trim() != "")
                        .WhereIf(!dropdownInput.filtro.IsNullOrEmpty(), m => m.EstadoId == estadoId)
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        //m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                        m.Nome.ToLower().Contains(dropdownInput.search.ToLower())

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
