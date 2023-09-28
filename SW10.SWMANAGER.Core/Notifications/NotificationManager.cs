using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Notifications;
using Castle.Core.Internal;
using SW10.SWMANAGER.Chat;
using SW10.SWMANAGER.Notifications.Dto;

namespace SW10.SWMANAGER.Notifications
{
    [AbpAuthorize]
    public class NotificationManager : SWMANAGERDomainServiceBase, INotificationManager
    {
        private readonly INotificationPublisher _notificationPublisher;

        private readonly INotificationCommunicator _notificationCommunicator;
        
        public NotificationManager(INotificationPublisher notificationPublisher, INotificationCommunicator notificationCommunicator)
        {
            _notificationPublisher = notificationPublisher;
            _notificationCommunicator = notificationCommunicator;
        }

        public void SendMessage(UserIdentifier user, AppUserNotificationMessage message, NotificationSeverity severity = NotificationSeverity.Info)
        {
            _notificationCommunicator.SendMessage(user, message);
        }

        public async Task SendMessageAsync(UserIdentifier user, string title, string message, NotificationSeverity severity = NotificationSeverity.Info, string source = null, string sourceId = null)
        {
            _notificationCommunicator.SendMessage(user, new AppUserNotificationMessage(user, title, message, source, sourceId, true, ChatMessageReadState.Unread));
        }

        [UnitOfWork]
        [AbpAllowAnonymous]
        public Guid Save(IUserIdentifier userIdentifier, AppUserNotificationMessage message)
        {
            using (CurrentUnitOfWork.SetTenantId(userIdentifier.TenantId))
            using (var userNotificationMessageRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AppUserNotificationMessage,Guid>>())
            {
                return userNotificationMessageRepository.Object.InsertAndGetId(message);
            }
        }
        
        [UnitOfWork]
        public IEnumerable<AppUserNotificationMessageDto>GetUnreadNotifications(IUserIdentifier userIdentifier)
        {
            using (CurrentUnitOfWork.SetTenantId(userIdentifier.TenantId))
            using (var userNotificationMessageRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AppUserNotificationMessage, Guid>>())
            {
                return AppUserNotificationMessageDto.Mapear(userNotificationMessageRepository.Object.GetAll().AsNoTracking()
                    .Where(x => x.UserId == userIdentifier.UserId && x.ReadState == ChatMessageReadState.Unread).ToList());
            }
            
        }

        [UnitOfWork]
        public void SetReadMessage(IUserIdentifier userIdentifier, Guid messageId)
        {
            using (var userNotificationMessageRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AppUserNotificationMessage, Guid>>())
            {
                var message = userNotificationMessageRepository.Object.FirstOrDefault(messageId);
                if (message != null && !message.IsTransient())
                {
                    SetReadMessage(userIdentifier, message);
                }
            }
        }

        public UserNotificationsOutputDto SendUserNotificationToOnline(IUserIdentifier user, IEnumerable<AppUserNotificationMessageDto> message)
        {
            throw new NotImplementedException();
        }

        [UnitOfWork]
        [AbpAllowAnonymous]
        public void SendNotificationToOnline(IEnumerable<AppUserNotificationMessage> notificationMessages)
        {
            if (notificationMessages.IsNullOrEmpty())
            {
                return;
            }
            foreach (var onlineClient in _notificationCommunicator.GetOnlineClients().Where(x => x.UserId.HasValue))
            {
                var userNotifications = notificationMessages.Where(x=> x.UserId == onlineClient.UserId.Value);
                if (userNotifications.IsNullOrEmpty())
                {
                    continue;
                }
                    
                _notificationCommunicator.SendMessage(onlineClient, userNotifications );
            }
        }

        [UnitOfWork]
        public void SetReadMessage(IUserIdentifier userIdentifier, AppUserNotificationMessage message)
        {
            message.ChangeReadState(ChatMessageReadState.Read);
            using (CurrentUnitOfWork.SetTenantId(userIdentifier.TenantId))
            using (var userNotificationMessageRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AppUserNotificationMessage,Guid>>())
            {
                userNotificationMessageRepository.Object.InsertOrUpdate(message);
            }
        }
        
        [AbpAllowAnonymous]
        public async Task SendMessageAsync(UserIdentifier user, AppUserNotificationMessage message, NotificationSeverity severity = NotificationSeverity.Info)
        {
            _notificationCommunicator.SendMessages(user, new List<AppUserNotificationMessage> {message});
        }

        [UnitOfWork]
        [AbpAllowAnonymous]
        public IEnumerable<AppUserNotificationMessageDto> GetAllNotificationsBySource(string source, string sourceId)
        {
            using (var userNotificationMessageRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<AppUserNotificationMessage, Guid>>())
            {
                return AppUserNotificationMessageDto.Mapear(userNotificationMessageRepository.Object.GetAll().AsNoTracking().Where(x => x.Source == source && x.SourceId == sourceId));
            }
        }
    }
}