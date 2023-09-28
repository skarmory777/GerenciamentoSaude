namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha.HangfireJob
{
    using Abp.BackgroundJobs;
    using Abp.Dependency;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha;
    using System;
    using System.Linq;

    public class ZerarFilaJob : BackgroundJob<ZerarFilaJobArgs>, ITransientDependency
    {
        [UnitOfWork]
        public override void Execute(ZerarFilaJobArgs args)
        {
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var filaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Fila, long>>())
            {
                unitOfWorkManager.Object.Current.SetTenantId(args.TenantId);
                var fila = filaRepository.Object.GetAll().FirstOrDefault(x => x.Id == args.FilaId);

                fila.UltimaSenha = fila.NumeroInicial;
                fila.UltimaZera = DateTime.Now;

                filaRepository.Object.Update(fila);
            }
        }

        public static string GeraZeraFilaJobId(long filaId, int? tenantId)
        {
            return $"ZerarFila_TenantId_{tenantId ?? 0}_FilaId_{filaId}";
        }
    }
}
