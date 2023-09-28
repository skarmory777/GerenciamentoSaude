using Castle.Core.Internal;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Threading.Tasks;

    using Abp.Application.Services.Dto;
    using Abp.Dependency;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Abp.Extensions;
    using Abp.Linq.Extensions;
    using Abp.UI;

    using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;

    public class FormDataAppService : SWMANAGERAppServiceBase, IFormDataAppService
    {
        [UnitOfWork]
        public async Task CriarOuEditar(FormData input)
        {
            try
            {
                using (var formDataRepository = IocManager.Instance
                    .ResolveAsDisposable<IRepository<FormData, long>>())
                using (var unitOfWorkManager = IocManager.Instance
                    .ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        await formDataRepository.Object.InsertOrUpdateAsync(input).ConfigureAwait(false);
                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork]
        public async Task Excluir(long id)
        {
            try
            {
                using (var formDataRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FormData, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        await formDataRepository.Object.DeleteAsync(m => m.Id == id).ConfigureAwait(false);
                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }
        
        [UnitOfWork]
        public void Excluir(IEnumerable<long> ids)
        {
            try
            {
                if (ids.IsNullOrEmpty())
                {
                    return;
                }
                
                using (var formDataRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FormData, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        foreach (var id in ids.Distinct().ToList())
                        {
                            formDataRepository.Object.Delete(m => m.Id == id);
                        }
                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<PagedResultDto<FormData>> Listar(ListarInput input)
        {
            //List<FormDataDto> formsConfigDtos = new List<FormDataDto>();
            try
            {
                using (var formDataRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FormData, long>>())
                {
                    var query = formDataRepository.Object.GetAll().WhereIf(
                        !input.Filtro.IsNullOrEmpty(),
                        m => m.Valor.Contains(input.Filtro));

                    var contarGeradorFormularios = await query.CountAsync().ConfigureAwait(false);

                    var formsConfig = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync()
                                          .ConfigureAwait(false);

                    //formsConfigDtos = formsConfig
                    //    .MapTo<List<FormDataDto>>();

                    return new PagedResultDto<FormData>(contarGeradorFormularios, formsConfig);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<FormData>> ListarTodos()
        {
            try
            {
                using (var formDataRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FormData, long>>())
                {
                    var query = await formDataRepository.Object.GetAll().AsNoTracking().ToListAsync().ConfigureAwait(false);

                    //var formsConfig = query.MapTo<List<FormDataDto>>();
                    return new ListResultDto<FormData> { Items = query };
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FormData> Obter(long id)
        {
            try
            {
                using (var formDataRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FormData, long>>())
                {
                    return await formDataRepository.Object.FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<List<FormData>> ListarNoLazy(long id)
        {
            try
            {
                using (var formDataRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FormData, long>>())
                {
                    var result = await formDataRepository.Object.GetAll().Where(m => m.FormRespostaId == id).Select(
                                     item => new FormData
                                     {
                                         ColConfigId = item.ColConfigId,
                                         CreationTime = item.CreationTime,
                                         CreatorUserId = item.CreatorUserId,
                                         DeleterUserId = item.DeleterUserId,
                                         DeletionTime = item.DeletionTime,
                                         FormRespostaId = item.FormRespostaId,
                                         Id = item.Id,
                                         IsDeleted = item.IsDeleted,
                                         IsSistema = item.IsSistema,
                                         LastModificationTime = item.LastModificationTime,
                                         LastModifierUserId = item.LastModifierUserId,
                                         Valor = item.Valor
                                     }).ToListAsync().ConfigureAwait(false);

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
