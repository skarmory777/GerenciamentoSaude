using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Abp;
using Abp.Authorization.Users;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using SW10.SWMANAGER.Chat;
using SW10.SWMANAGER.Notifications;

namespace SW10.SWMANAGER.ClassesAplicacao.Avisos
{
    public class AvisoJob : BackgroundJob<AvisoJobArgs>, ITransientDependency
    {
        [UnitOfWork]
        public override void Execute(AvisoJobArgs args)
        {
            try
            {
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var avisoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Aviso, long>>())
                using (var userRoleRepository = IocManager.Instance.ResolveAsDisposable<IRepository<UserRole, long>>())
                using (var notificationManager = IocManager.Instance.ResolveAsDisposable<INotificationManager>())
                {
                    unitOfWorkManager.Object.Current.SetTenantId(args.TenantId);
                    var notificationsToSend = new List<AppUserNotificationMessage>();
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        var aviso = avisoRepository.Object.GetAll().Include(x => x.Grupos).FirstOrDefault(x => x.Id == args.Id);

                        if (aviso == null || aviso.DisparoAtivo || aviso.DataFinalDisparo.HasValue)
                        {
                            return;
                        }

                        aviso.DisparoAtivo = true;
                        if (!aviso.DataInicioDisparo.HasValue)
                        {
                            aviso.DataInicioDisparo = DateTime.Now;
                        }

                        avisoRepository.Object.Update(aviso);
                        var roleIds = aviso.Grupos.Select(z => z.RoleId).ToList();
                        var users = userRoleRepository.Object.GetAll()
                            .Where(x => roleIds.Contains(x.RoleId) && x.TenantId == args.TenantId)
                            .Select(x => x.UserId).Distinct().ToList();
                        var notificationUsers = notificationManager.Object
                            .GetAllNotificationsBySource(typeof(Aviso).FullName, args.Id.ToString())
                            .Select(x => x.UserId);
                        
                        foreach (var userId in users.Except(notificationUsers))
                        {
                            var user = new UserIdentifier(args.TenantId, userId);
                            var userNotification = new AppUserNotificationMessage(user, aviso.Titulo, aviso.Mensagem,
                                typeof(Aviso).FullName, args.Id.ToString(), aviso.Bloquear,
                                ChatMessageReadState.Unread);
                            notificationManager.Object.Save(user, userNotification);
                            // notificationManager.Object.SendMessageAsync(user, userNotification);
                            aviso.DataFinalDisparo = DateTime.Now;
                            notificationsToSend.Add(userNotification);
                        }

                        aviso.DataFinalDisparo = DateTime.Now;
                        aviso.DisparoAtivo = false;
                        aviso.TotalEnviado = users.Except(notificationUsers).Count();

                        avisoRepository.Object.Update(aviso);
                        
                        unitOfWork.Complete();
                        unitOfWork.Dispose();
                    }
                    // Descomentar quando o signalR voltar a funcionar - Migrar versão abp
                    // notificationManager.Object.SendNotificationToOnline(notificationsToSend);
                }
            }
            catch (Exception e)
            {
                using (var avisoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Aviso, long>>())
                {
                    var aviso = avisoRepository.Object.Get(args.Id);

                    aviso.DisparoAtivo = false;
                    avisoRepository.Object.Update(aviso);
                }
            }
        }
    }
}