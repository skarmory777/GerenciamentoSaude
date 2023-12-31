﻿using Abp;
using Abp.Domain.Services;

namespace SW10.SWMANAGER.Friendships
{
    public interface IFriendshipManager : IDomainService
    {
        void CreateFriendship(Friendship friendship);

        void UpdateFriendship(Friendship friendship);

        Friendship GetFriendshipOrNull(UserIdentifier user, UserIdentifier probableFriend);

        void BanFriend(UserIdentifier userIdentifier, UserIdentifier probableFriend);

        void AcceptFriendshipRequest(UserIdentifier userIdentifier, UserIdentifier probableFriend);
    }
}
