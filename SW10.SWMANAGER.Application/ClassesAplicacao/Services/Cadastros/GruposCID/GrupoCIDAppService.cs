
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.GruposCID;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCID.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCID.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCID
{
    public class GrupoCIDAppService : SWMANAGERAppServiceBase, IGrupoCIDAppService
    {
        private readonly IRepository<GrupoCID, long> _indicacaoRepository;
        private readonly IListarGruposCIDExcelExporter _listarGruposCIDExcelExporter;


        public async Task<ListResultDto<GrupoCIDDto>> ListarTodos()
        {
            List<GrupoCID> indicacoes;
            List<GrupoCIDDto> indicacoesDtos = new List<GrupoCIDDto>();
            try
            {
                indicacoes = await _indicacaoRepository
                  .GetAll()
                  .AsNoTracking()
                  .ToListAsync();

                indicacoesDtos = indicacoes
                    .MapTo<List<GrupoCIDDto>>();

                return new ListResultDto<GrupoCIDDto> { Items = indicacoesDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public GrupoCIDAppService(IRepository<GrupoCID, long> indicacaoRepository, IListarGruposCIDExcelExporter listarGruposCIDExcelExporter)
        {
            _indicacaoRepository = indicacaoRepository;
            _listarGruposCIDExcelExporter = listarGruposCIDExcelExporter;
        }

        public async Task CriarOuEditar(GrupoCIDDto input)
        {
            try
            {
                var indicacao = input.MapTo<GrupoCID>();
                if (input.Id.Equals(0))
                {
                    await _indicacaoRepository.InsertAsync(indicacao);
                }
                else
                {
                    await _indicacaoRepository.UpdateAsync(indicacao);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(GrupoCIDDto input)
        {
            try
            {
                await _indicacaoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<GrupoCIDDto>> Listar(ListarGruposCIDInput input)
        {
            var contarGruposCID = 0;
            List<GrupoCID> indicacao = new List<GrupoCID>();
            List<GrupoCIDDto> indecacaoDtos = new List<GrupoCIDDto>();
            try
            {
                var query = _indicacaoRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarGruposCID = await query
                    .CountAsync();

                indicacao = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                indecacaoDtos = indicacao.MapTo<List<GrupoCIDDto>>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<GrupoCIDDto>(
                contarGruposCID,
                indecacaoDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarGruposCIDInput input)
        {
            try
            {
                var result = await Listar(input);
                var indicacoes = result.Items;
                return _listarGruposCIDExcelExporter.ExportToFile(indicacoes.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<GrupoCIDDto> Obter(long id)
        {
            var query = await _indicacaoRepository
                .GetAsync(id);

            var indicacao = query.MapTo<GrupoCIDDto>();

            return indicacao;
        }
        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<GrupoCIDDto> grupoCID = new List<GrupoCIDDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                //   bool isLaudo = (!dropdownInput.filtro.IsNullOrEmpty()) ? dropdownInput.filtro.Equals("IsLaudo") : false;

                var query = from p in _indicacaoRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                            m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) ||
                            m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                            )
                                //.Where(f => f.IsLaudo.Equals(isLaudo))
                            orderby p.Descricao ascending
                            select new DropdownItems
                            {
                                id = p.Id,
                                text = string.Concat(p.Codigo, " - ", p.Descricao)
                            };

                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                var result = queryResultPage.ToList();

                int total = await Task.Run(() => query.Count());

                return new ResultDropdownList() { Items = result, TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
