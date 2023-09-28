namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Threading.Tasks;

    using Abp.Application.Services.Dto;
    using Abp.Dependency;
    using Abp.Domain.Repositories;
    using Abp.Extensions;
    using Abp.Linq.Extensions;
    using Abp.UI;

    using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;

    public class ColMultiOptionAppService : SWMANAGERAppServiceBase, IColMultiOptionAppService
    {
        public async Task CriarOuEditar(ColMultiOption input)
        {
            try
            {
                using (var colMultiOptionRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<ColMultiOption, long>>())
                {
                    //var formConfig = input.MapTo<ColMultiOption>();
                    if (input.Id.Equals(0))
                    {
                        await colMultiOptionRepository.Object.InsertAsync(input).ConfigureAwait(false);
                    }
                    else
                    {
                        await colMultiOptionRepository.Object.UpdateAsync(input).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(ColMultiOption input)
        {
            try
            {
                using (var colMultiOptionRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<ColMultiOption, long>>())
                {
                    await colMultiOptionRepository.Object.DeleteAsync(input.Id).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<ColMultiOption>> Listar(ListarInput input)
        {
            try
            {
                using (var colMultiOptionRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<ColMultiOption, long>>())
                {
                    var query = colMultiOptionRepository.Object.GetAll().WhereIf(
                        !input.Filtro.IsNullOrEmpty(),
                        m => m.Opcao.ToUpper().Contains(input.Filtro.ToUpper()));

                    var contarGeradorFormularios = await query.CountAsync().ConfigureAwait(false);

                    var formsConfig =
                        await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync().ConfigureAwait(false);

                    return new PagedResultDto<ColMultiOption>(contarGeradorFormularios, formsConfig);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<ColMultiOption>> ListarTodos()
        {
            try
            {
                using (var colMultiOptionRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<ColMultiOption, long>>())
                {
                    var query = await colMultiOptionRepository.Object.GetAllListAsync().ConfigureAwait(false);

                    //var formsConfig = query.MapTo<List<ColMultiOptionDto>>();
                    return new ListResultDto<ColMultiOption> { Items = query };
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ColMultiOption> Obter(long id)
        {
            try
            {
                using (var colMultiOptionRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<ColMultiOption, long>>())
                {
                    var result = await colMultiOptionRepository.Object.GetAllListAsync(m => m.Id == id).ConfigureAwait(false);

                    var formConfig = result.FirstOrDefault();
                    //.MapTo<ColMultiOptionDto>();

                    return formConfig;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
