using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios
{
    using Abp.Dependency;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto;

    public class ColConfigAppService : SWMANAGERAppServiceBase, IColConfigAppService
    {
        public async Task CriarOuEditar(ColConfig input)
        {
            try
            {
                using (var colConfigRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ColConfig, long>>())
                using (var multiOptionRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ColMultiOption, long>>())
                {
                    //var formConfig = input.MapTo<ColConfig>();
                    if (input.Id.Equals(0))
                    {
                        await colConfigRepository.Object.InsertAsync(input).ConfigureAwait(false);
                    }
                    else
                    {

                        if (input.Type == "checkbox" || input.Type == "radio" || input.Type == "select")
                        {
                            foreach (var multiOption in input.MultiOption)
                            {
                                if (multiOption.Id == 0)
                                {
                                    await multiOptionRepository.Object.InsertAsync(multiOption).ConfigureAwait(false);
                                }
                                else
                                {
                                    await multiOptionRepository.Object.UpdateAsync(multiOption).ConfigureAwait(false);
                                }
                            }
                        }
                        else
                        {
                            input.MultiOption = new List<ColMultiOption>();
                        }

                        await colConfigRepository.Object.UpdateAsync(input).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(ColConfig input)
        {
            try
            {
                using (var colConfigRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ColConfig, long>>())
                {
                    await colConfigRepository.Object.DeleteAsync(input.Id).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<ColConfigDto>> Listar(ListarInput input)
        {
            try
            {
                using (var colConfigRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ColConfig, long>>())
                {
                    var query = colConfigRepository.Object.GetAll().AsNoTracking().WhereIf(
                        !input.Filtro.IsNullOrEmpty(),
                        m => m.Label.Contains(input.Filtro) || m.Name.Contains(input.Filtro));

                    var contarGeradorFormularios = await query.CountAsync().ConfigureAwait(false);

                    var formsConfig = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync().ConfigureAwait(false);
                    return new PagedResultDto<ColConfigDto>(
                        contarGeradorFormularios,
                        formsConfig.Select(x => ColConfigDto.Mapear(x)).ToList());
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<ColConfigDto>> ListarTodos()
        {
            try
            {
                using (var colConfigRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ColConfig, long>>())
                {
                    var query = await colConfigRepository.Object.GetAll().AsNoTracking().ToListAsync()
                                    .ConfigureAwait(false);

                    return new ListResultDto<ColConfigDto>
                    {
                        Items = query.ToList().Select(x => ColConfigDto.Mapear(x)).ToList()
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
        
        public async Task<ListResultDto<ColConfigDto>> ListarTodos(IEnumerable<long> colunaIds)
        {
            try
            {
                using (var colConfigRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ColConfig, long>>())
                {
                    var query = await colConfigRepository.Object.GetAll().AsNoTracking()
                        .Where(m => colunaIds.Contains(m.Id)).ToListAsync()
                        .ConfigureAwait(false);

                    return new ListResultDto<ColConfigDto>
                    {
                        Items = query.ToList().Select(x => ColConfigDto.Mapear(x)).ToList()
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ColConfigDto> Obter(long id)
        {
            try
            {
                using (var colConfigRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<ColConfig, long>>())
                {
                    return ColConfigDto.Mapear(
                        await colConfigRepository.Object.GetAll().AsNoTracking().FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false));
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
