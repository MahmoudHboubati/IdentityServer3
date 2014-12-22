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

using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Thinktecture.IdentityServer.Core.Events;
using Thinktecture.IdentityServer.Core.Logging;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services;

namespace Thinktecture.IdentityServer.Core.Extensions
{
    internal static class IEventServiceExtensions
    {
        public static void RaiseLocalLoginSuccessEvent(this IEventService events, string username, SignInMessage signInMessage, AuthenticateResult authResult)
        {
            if (events == null) throw new ArgumentNullException("events");

            var evt = new LocalAuthenticationEvent()
            {
                Id = EventConstants.Ids.SuccessfulLocalLogin,
                EventType = EventType.Success,
                Message = Resources.Events.LocalLoginSuccess,
                SubjectId = authResult.User.GetSubjectId(),
                SignInMessage = signInMessage,
                LoginUserName = username
            };
            events.Raise(evt);
        }
    }
}
