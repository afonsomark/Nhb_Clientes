﻿namespace Nhb_Clientes.Models.Caching
{
    public interface ICachingService
    {
        Task SetAsync(string key, string value);
        Task<string> GetAsync(string key);
    }
}
