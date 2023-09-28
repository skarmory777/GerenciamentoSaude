using Abp.AutoMapper;
using SW10.SWMANAGER.Friendships.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;

namespace SW10.SWMANAGER.Friendships.Dto
{
    [AutoMapFrom(typeof(FriendCacheItem), typeof(Friendship))]
    public class FriendDto
    {
        public long FriendUserId { get; set; }

        public int? FriendTenantId { get; set; }

        public string FriendUserName { get; set; }

        public string FriendTenancyName { get; set; }

        public Guid? FriendProfilePictureId { get; set; }

        public int UnreadMessageCount { get; set; }

        public bool IsOnline { get; set; }

        public FriendshipState State { get; set; }

        
        public static IEnumerable<FriendDto>MapearList(IEnumerable<FriendCacheItem> cacheItems)
        {
            foreach (var cacheItem in cacheItems)
            {
                yield return Mapear(cacheItem);
            }
        }
        public static FriendDto Mapear(FriendCacheItem cacheItem)
        {
            if (cacheItem == null)
            {
                return null;
            }

            return new FriendDto
            {
                FriendTenancyName = cacheItem.FriendTenancyName,
                FriendUserId = cacheItem.FriendUserId,
                FriendTenantId = cacheItem.FriendTenantId,
                FriendUserName = cacheItem.FriendUserName,
                UnreadMessageCount = cacheItem.UnreadMessageCount,
                FriendProfilePictureId = cacheItem.FriendProfilePictureId,
                State = cacheItem.State
            };
        }
        
        public static FriendDto Mapear(Friendship friendship)
        {
            if (friendship == null)
            {
                return null;
            }

            return new FriendDto
            {
                FriendTenancyName = friendship.FriendTenancyName,
                FriendUserId = friendship.FriendUserId,
                FriendTenantId = friendship.FriendTenantId,
                FriendUserName = friendship.FriendUserName,
                FriendProfilePictureId = friendship.FriendProfilePictureId,
                State = friendship.State
            };
        }
    }
    
}
