using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using RestSharp;
using Sefaz;
using Sefaz.Entities;
using SW10.SWMANAGER.ClassesAplicacao.Sefaz;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais;
using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos.Enumeradores;

namespace SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos
{
    public class AtualizaArquivoPrescricaoMedicaJob : BackgroundJob<AtualizaArquivoPrescricaoMedicaJobArgs>, ITransientDependency
    {
        [UnitOfWork(IsDisabled = false)]
        public override void Execute(AtualizaArquivoPrescricaoMedicaJobArgs args)
        {
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                unitOfWorkManager.Object.Current.SetTenantId(args.TenantId);
                using (var prescricaoMedicaAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoMedicaAppService>())
                using (var prescricaoMedicaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoMedica, long>>())
                using (var registroArquivoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<RegistroArquivo, long>>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var prescricao = prescricaoMedicaRepository.Object.GetAll().AsNoTracking().Select(x => new {x.Id, x.AtendimentoId}).FirstOrDefault(x => x.Id == args.PrescricaoMedicaId);
                    if (!prescricao.AtendimentoId.HasValue)
                    {
                        return;
                    }
                    
                    var pdfBytes = prescricaoMedicaAppService.Object.RetornaArquivoPrescricaoMedica(args.PrescricaoMedicaId, false, args.DataAgrupamento, args.TenantId);
                    var registroArquivo = new RegistroArquivo
                    {
                        Arquivo = pdfBytes,
                        RegistroTabelaId = (long) EnumArquivoTabela.PrescricaoMedica,
                        RegistroId = (long) (prescricao?.Id ?? 0),
                        AtendimentoId = prescricao?.AtendimentoId ?? 0
                    };
                    registroArquivoRepository.Object.InsertAndGetId(registroArquivo);
                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
        }
    }
}