using Application.EntityUser.Query;
using Application.Interfaces;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace Application.EntityUser.Query.QueryHandler
{
    public class GetUserListHandler(IUserManagerWrapper userManager, /*IMemoryCache memoryCache, */IDistributedCache distributedCache) : IRequestHandler<GetUserListQuery, List<User>>
    {
        private readonly IUserManagerWrapper _userManager = userManager;
        private readonly IDistributedCache _distributedCache = distributedCache;

        public async Task<List<User>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            string key = "GetUserListResult";
            //for distributedCache
            var userListJson = _distributedCache.GetString(key);

            if (userListJson.IsNullOrEmpty())
            {
                Thread.Sleep(5000);
                var userList = await _userManager.ToListAsync(request, cancellationToken);
                userListJson = JsonSerializer.Serialize(userList);
                _distributedCache.SetString(key, userListJson, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });
            }
            return JsonSerializer.Deserialize<List<User>>(userListJson!)!;
        }
    }
}