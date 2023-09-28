using System.Collections.Generic;
using Abp.Dependency;
using Abp.RealTime;

namespace SW10.SWMANAGER.Authorization.Users
{
    public interface IUserLogoutInformer
    {
        void InformClients(IReadOnlyList<IOnlineClient> clients);
    }
}
