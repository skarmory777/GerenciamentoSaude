// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImpressoraArquivosAppService.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the IImpressoraArquivosAppService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Abp.Application.Services;
using Abp.Auditing;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using SW10.SWMANAGER.ClassesAplicacao.Impressoras;
using SW10.SWMANAGER.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Impressora
{
    /// <summary>
    /// The impressora arquivos app service.
    /// </summary>
    [AbpAllowAnonymous]
    public class ImpressoraArquivosAppService : ApplicationService, IImpressoraArquivosAppService
    {

        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<IEnumerable<ImpressoraArquivoDto>> ListarArquivosPendentesImpressao(string tenancyName)
        {
            using (var tenantManager = IocManager.Instance.ResolveAsDisposable<TenantManager>())
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var impressoraArquivosRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ImpressoraArquivo, long>>())
            {
                var tenant = await tenantManager.Object.Tenants.FirstOrDefaultAsync(x => x.TenancyName == tenancyName)
                                 .ConfigureAwait(false);
                if (tenant == null)
                {
                    return null;
                }

                try
                {
                    using (unitOfWorkManager.Object.Current.SetTenantId(tenant.Id))
                    {
                        return impressoraArquivosRepository.Object.GetAll().AsNoTracking().Where(x => !x.IsPrinted)
                            .MapTo<List<ImpressoraArquivoDto>>();
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Error(ex.Message);
                }

                return null;
            }
        }

        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<ImpressoraArquivoDto> ArquivoImpressoSucesso(string tenancyName, long id)
        {
            using (var tenantManager = IocManager.Instance.ResolveAsDisposable<TenantManager>())
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var impressoraArquivosRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ImpressoraArquivo, long>>())
            {
                var tenant = await tenantManager.Object.Tenants.FirstOrDefaultAsync(x => x.TenancyName == tenancyName)
                             .ConfigureAwait(false);
                if (tenant == null)
                {
                    return null;
                }

                using (unitOfWorkManager.Object.Current.SetTenantId(tenant.Id))
                {
                    try
                    {
                        var arquivo = await impressoraArquivosRepository.Object.FirstOrDefaultAsync(id)
                                          .ConfigureAwait(false);

                        if (arquivo != null)
                        {
                            arquivo.IsPrinted = true;
                            arquivo.IsDeleted = true;

                            await impressoraArquivosRepository.Object.UpdateAsync(arquivo).ConfigureAwait(false);

                            //var baseDir = Path.Combine(
                            //    HttpRuntime.AppDomainAppPath,
                            //    ConfigurationManager.AppSettings["App.UploadFilesPath"]);
                            //var pathDir = Path.Combine(baseDir, "impressora");

                            //if (!Directory.Exists(baseDir))
                            //{
                            //    Directory.CreateDirectory(baseDir);
                            //}

                            //if (!Directory.Exists(pathDir))
                            //{
                            //    Directory.CreateDirectory(pathDir);
                            //}

                            //File.Delete(Path.Combine(pathDir, arquivo.FilePath));

                            return arquivo.MapTo<ImpressoraArquivoDto>();
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Error(ex.Message);
                    }
                }
            }

            return null;
        }

        /// <inheritdoc/>
        public ImpressoraArquivo EnviarParaImprimir(
            string printerName,
            byte[] file,
            string fileName,
            long numberOfCopies = 1)
        {
            using (var impressoraArquivosRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ImpressoraArquivo, long>>())
            {
                var baseDir = Path.Combine(HttpRuntime.AppDomainAppPath, ConfigurationManager.AppSettings["App.UploadFilesPath"]);
                var pathDir = Path.Combine(baseDir, "impressora");

                if (!Directory.Exists(baseDir))
                {
                    Directory.CreateDirectory(baseDir);
                }

                if (!Directory.Exists(pathDir))
                {
                    Directory.CreateDirectory(pathDir);
                }


                File.WriteAllBytes(Path.Combine(pathDir, fileName), file);

                var model = new ImpressoraArquivo
                {
                    Id = 0,
                    FilePath = fileName,
                    IsPrinted = false,
                    PrinterName = printerName,
                    NumberOfCopies = numberOfCopies
                };

                impressoraArquivosRepository.Object.Insert(model);

                return model;
            }
        }
    }
}
