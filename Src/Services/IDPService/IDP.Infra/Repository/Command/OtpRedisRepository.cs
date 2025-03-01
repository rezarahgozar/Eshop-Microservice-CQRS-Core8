﻿using IDP.Domain.DTO;
using IDP.Domain.IRepository.Command;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace IDP.Infra.Repository.Command
{
    public class OtpRedisRepository : IOtpRedisRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConfiguration _configuration;

        public OtpRedisRepository(IDistributedCache distributedCache,IConfiguration configuration)
        {
            _distributedCache = distributedCache;
            _configuration = configuration;
        }
        public async Task<bool> DeleteAsync(Otp entity)
        {
            await _distributedCache.RemoveAsync(entity.UserName.ToString());
            return true;
        }

        public async Task<Otp> GetDataAsync(string mobile)
        {
            var data = _distributedCache.GetString(mobile);
            if (data == null) return null;
            var otpObj = JsonConvert.DeserializeObject<Otp>(data);
            return otpObj;
        }

        public async Task<Otp> InsertAsync(Otp entity)
        {
            int time = Convert.ToInt32(_configuration.GetSection("Otp:OtpTime").Value);
            _distributedCache.SetString(entity.UserName.ToString(), JsonConvert.SerializeObject(entity),new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(time)));
            return entity;
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Otp entity)
        {
            throw new NotImplementedException();
        }
    }
}
