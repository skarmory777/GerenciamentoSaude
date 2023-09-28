using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Estados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados
{
    public class EstadoAppService : SWMANAGERAppServiceBase, IEstadoAppService
    {
        private readonly IRepository<Estado, long> _estadoRepository;
        private readonly IListarEstadosExcelExporter _listarEstadosExcelExporter;
        private readonly IPaisAppService _paisAppService;

        public EstadoAppService(IRepository<Estado, long> estadoRepository, IListarEstadosExcelExporter listarEstadosExcelExporter, IPaisAppService paisAppService)
        {
            _estadoRepository = estadoRepository;
            _listarEstadosExcelExporter = listarEstadosExcelExporter;
            _paisAppService = paisAppService;
        }

        public async Task CriarOuEditar(EstadoDto input)
        {
            try
            {
                var estado = input.MapTo<Estado>();
                if (input.Id.Equals(0))
                {
                    await _estadoRepository.InsertAsync(estado);
                }
                else
                {
                    var estadoEntity = _estadoRepository.GetAll()
                                                        .Where(w => w.Id == input.Id)
                                                        .FirstOrDefault();

                    if (estadoEntity != null)
                    {
                        estadoEntity.Codigo = input.Codigo;
                        estadoEntity.Nome = input.Nome;
                        estadoEntity.PaisId = input.PaisId;
                        estadoEntity.Uf = input.Uf;

                        await _estadoRepository.UpdateAsync(estadoEntity);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(EstadoDto input)
        {
            try
            {
                await _estadoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<EstadoDto>> Listar(ListarEstadosInput input)
        {
            var contarEstados = 0;
            List<Estado> estados = new List<Estado>();
            List<EstadoDto> estadosDtos = new List<EstadoDto>();
            try
            {
                var query = _estadoRepository
                    .GetAll()
                    .Include(m => m.Pais)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
                        m.Uf.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarEstados = await query
                    .CountAsync();

                estados = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                estadosDtos = estados.MapTo<List<EstadoDto>>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<EstadoDto>(
                contarEstados,
                estadosDtos
                );
        }

        public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input, long? paisId)
        {
            try
            {
                var query = await _estadoRepository
                    .GetAll()
                    .Include(m => m.Pais)
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                       m.Nome.ToUpper().Contains(input.ToUpper()) ||
                       m.Uf.ToUpper().Contains(input.ToUpper()))
                    .WhereIf(paisId.HasValue, m =>
                        m.PaisId == paisId)
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Nome })
                    .ToListAsync();

                return new ListResultDto<GenericoIdNome> { Items = query };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarEstadosInput input)
        {
            try
            {
                var result = await Listar(input);
                var estados = result.Items;
                return _listarEstadosExcelExporter.ExportToFile(estados.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<ListResultDto<EstadoDto>> ListarPorPais(long? id)
        {
            try
            {
                var query = _estadoRepository
                    .GetAll()
                    .Include(m => m.Pais);


                var result = await query
                    .WhereIf(id.HasValue && id > 0, m =>
                          m.PaisId == id
                    )
                    .ToListAsync();

                var estados = new ListResultDto<EstadoDto>
                {
                    Items = result.MapTo<List<EstadoDto>>()
                };

                return estados;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<EstadoDto> Obter(long id)
        {
            var query = await _estadoRepository
                .GetAll()
                .Include(m => m.Pais)
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();


            var estado = query.MapTo<EstadoDto>();

            return estado;
        }

        public async Task<EstadoDto> Obter(string uf)
        {
            EstadoDto estado;
            try
            {
                var query = await _estadoRepository
                    .GetAll()
                    .Include(m => m.Pais)
                    .Where(m => m.Uf.ToUpper().Equals(uf.ToUpper()))
                    .ToListAsync();

                estado = query
                   .FirstOrDefault()
                   .MapTo<EstadoDto>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

            return estado;
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<EstadoDto> pacientesDtos = new List<EstadoDto>();
            try
            {
                //se um país estiver selecionado, filtra apenas pelos seus estados
                long paisId;
                long.TryParse(dropdownInput.filtro, out paisId);

                //get com filtro
                var query = from p in _estadoRepository.GetAll()
                        //.Where(m => m.Descricao != null && m.Descricao.Trim() != "")
                        .WhereIf(!dropdownInput.filtro.IsNullOrEmpty(), m => m.PaisId == paisId)
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        //m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                        m.Nome.ToLower().Contains(dropdownInput.search.ToLower()) || m.Uf.ToLower().Contains(dropdownInput.search.ToLower())

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
