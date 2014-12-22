﻿/*
 * Copyright 2014 Dominick Baier, Brock Allen
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.Services;

namespace Thinktecture.IdentityServer.Core.Extensions
{
    public static class ICacheExtensions
    {
        public static async Task<T> GetAsync<T>(this ICache<T> cache, string key, Func<Task<T>> get)
            where T : class
        {
            if (cache == null) throw new ArgumentNullException("cache");
            if (key == null) throw new ArgumentNullException("key");
            if (get == null) throw new ArgumentNullException("get");

            T item = await cache.GetAsync(key);
            if (item == null)
            {
                item = await get();
                await cache.SetAsync(key, item);
            }
            
            return item;
        }
    }
}
