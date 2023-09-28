using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasApresentacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasApresentacoes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasApresentacoes.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasApresentacoes
{
    public class FaturamentoBrasApresentacaoAppService : SWMANAGERAppServiceBase, IFaturamentoBrasApresentacaoAppService
    {

        public async Task<PagedResultDto<FaturamentoBrasApresentacaoDto>> Listar(ListarFaturamentoBrasApresentacoesInput input)
        {
            var brasApresentacaorBrasApresentacoes = 0;
            List<FaturamentoBrasApresentacaoDto> brasApresentacoesDtos = new List<FaturamentoBrasApresentacaoDto>();
            try
            {
                using (var _brasApresentacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoBrasApresentacao, long>>())
                {
                    var query = _brasApresentacaoRepository.Object.GetAll();

                    brasApresentacaorBrasApresentacoes = await query.CountAsync();

                    brasApresentacoesDtos = (await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync()
                        )
                        .Select(s => FaturamentoBrasApresentacaoDto.Mapear(s))
                        .ToList();
                    ;

                    // brasApresentacoesDtos = brasApresentacoes.MapTo<List<FaturamentoBrasApresentacaoDto>>();

                    return new PagedResultDto<FaturamentoBrasApresentacaoDto>(
                           brasApresentacaorBrasApresentacoes,
                           brasApresentacoesDtos
                       );
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task CriarOuEditar(FaturamentoBrasApresentacaoDto input)
        {
            try
            {
                using (var brasApresentacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoBrasApresentacao, long>>())
                {
                    var BrasApresentacao = FaturamentoBrasApresentacaoDto.Mapear(input);

                    if (input.Id.Equals(0))
                    {
                        await brasApresentacaoRepository.Object.InsertAsync(BrasApresentacao);
                    }
                    else
                    {
                        await brasApresentacaoRepository.Object.UpdateAsync(BrasApresentacao);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(FaturamentoBrasApresentacaoDto input)
        {
            try
            {
                using (var brasApresentacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoBrasApresentacao, long>>())
                {
                    await brasApresentacaoRepository.Object.DeleteAsync(input.Id);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<FaturamentoBrasApresentacaoDto> Obter(long id)
        {
            try
            {
                using (var brasApresentacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoBrasApresentacao, long>>())
                {
                    var query = await brasApresentacaoRepository.Object
                        .GetAll()
                        .Where(m => m.Id == id)
                        .FirstOrDefaultAsync();

                    var brasApresentacao = FaturamentoBrasApresentacaoDto.Mapear(query);

                    return brasApresentacao;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FaturamentoBrasApresentacaoDto> ObterComEstado(string nome, long estadoId)
        {
            try
            {
                using (var brasApresentacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoBrasApresentacao, long>>())
                {
                    var query = brasApresentacaoRepository.Object.GetAll();

                    var result = await query.FirstOrDefaultAsync();

                    var brasApresentacao = FaturamentoBrasApresentacaoDto.Mapear(result);

                    return brasApresentacao;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FileDto> ListarParaExcel(ListarFaturamentoBrasApresentacoesInput input)
        {
            try
            {
                var result = await Listar(input);
                var brasApresentacoes = result.Items;
                using (var listarBrasApresentacoesExcelExporter = IocManager.Instance.ResolveAsDisposable<IListarBrasApresentacoesExcelExporter>())
                {
                    return listarBrasApresentacoesExcelExporter.Object.ExportToFile(brasApresentacoes.ToList());
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<FaturamentoBrasApresentacaoDto> faturamentoItensDto = new List<FaturamentoBrasApresentacaoDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                using (var brasApresentacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoBrasApresentacao, long>>())
                {
                    var query = from p in brasApresentacaoRepository.Object.GetAll()
                                .WhereIf(!dropdownInput.filtro.IsNullOrEmpty(), m => m.Id.ToString() == dropdownInput.filtro)
                                .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.Descricao.Contains(dropdownInput.search) || m.Codigo.Contains(dropdownInput.search))
                                orderby p.Descricao ascending
                                select new DropdownItems
                                {
                                    id = p.Id,
                                    text = string.Concat(p.Codigo, " - ", p.Descricao)
                                };

                    var queryResultPage = query
                      .Skip(numberOfObjectsPerPage * pageInt)
                      .Take(numberOfObjectsPerPage);

                    var result = await queryResultPage.ToListAsync();
                    int total = await query.CountAsync();

                    return new ResultDropdownList() { Items = result, TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
