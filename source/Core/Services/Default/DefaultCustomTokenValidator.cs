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

using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Thinktecture.IdentityModel;
using Thinktecture.IdentityServer.Core.Extensions;
using Thinktecture.IdentityServer.Core.Validation;

namespace Thinktecture.IdentityServer.Core.Services.Default
{
    /// <summary>
    /// Default custom token validator
    /// </summary>
    public class DefaultCustomTokenValidator : ICustomTokenValidator
    {
        /// <summary>
        /// The user service
        /// </summary>
        protected readonly IUserService _users;

        /// <summary>
        /// The client store
        /// </summary>
        private readonly IClientStore _clients;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCustomTokenValidator"/> class.
        /// </summary>
        /// <param name="users">The users store.</param>
        /// <param name="clients">The client store.</param>
        public DefaultCustomTokenValidator(IUserService users, IClientStore clients)
        {
            _users = users;
            _clients = clients;
        }

        /// <summary>
        /// Custom validation logic for access tokens.
        /// </summary>
        /// <param name="result">The validation result so far.</param>
        /// <returns>
        /// The validation result
        /// </returns>
        public async Task<TokenValidationResult> ValidateAccessTokenAsync(TokenValidationResult result)
        {
            if (result.IsError)
            {
                return result;
            }

            // make sure user is still active (if sub claim is present)
            var subClaim = result.Claims.FirstOrDefault(c => c.Type == Constants.ClaimTypes.Subject);
            if (subClaim != null)
            {
                var principal = Principal.Create("tokenvalidator", result.Claims.ToArray());

                if (result.ReferenceTokenId.IsPresent())
                {
                    principal.Identities.First().AddClaim(new Claim(Constants.ClaimTypes.ReferenceTokenId, result.ReferenceTokenId));
                }

                if (await _users.IsActiveAsync(principal) == false)
                {
                    result.IsError = true;
                    result.Error = Constants.ProtectedResourceErrors.ExpiredToken;
                    result.Claims = null;

                    return result;
                }
            }

            // make sure client is still active (if client_id claim is present)
            var clientClaim = result.Claims.FirstOrDefault(c => c.Type == Constants.ClaimTypes.ClientId);
            if (clientClaim != null)
            {
                var client = await _clients.FindClientByIdAsync(clientClaim.Value);
                if (client == null || client.Enabled == false)
                {
                    result.IsError = true;
                    result.Error = Constants.ProtectedResourceErrors.ExpiredToken;
                    result.Claims = null;

                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Custom validation logic for identity tokens.
        /// </summary>
        /// <param name="result">The validation result so far.</param>
        /// <returns>
        /// The validation result
        /// </returns>
        public async Task<TokenValidationResult> ValidateIdentityTokenAsync(TokenValidationResult result)
        {
            // make sure user is still active (if sub claim is present)
            var subClaim = result.Claims.FirstOrDefault(c => c.Type == Constants.ClaimTypes.Subject);
            if (subClaim != null)
            {
                var principal = Principal.Create("tokenvalidator", result.Claims.ToArray());

                if (await _users.IsActiveAsync(principal) == false)
                {
                    result.IsError = true;
                    result.Error = Constants.ProtectedResourceErrors.ExpiredToken;
                    result.Claims = null;

                    return result;
                }
            }

            return result;
        }
    }
}