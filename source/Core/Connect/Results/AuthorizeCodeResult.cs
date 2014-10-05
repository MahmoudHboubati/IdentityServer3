﻿///*
// * Copyright 2014 Dominick Baier, Brock Allen
// *
// * Licensed under the Apache License, Version 2.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *   http://www.apache.org/licenses/LICENSE-2.0
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

//using System;
//using System.Net;
//using System.Net.Http;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Web.Http;
//using Thinktecture.IdentityServer.Core.Connect.Models;
//using Thinktecture.IdentityServer.Core.Extensions;
//using Thinktecture.IdentityServer.Core.Logging;

//namespace Thinktecture.IdentityServer.Core.Connect.Results
//{
//    public class AuthorizeCodeResult : IHttpActionResult
//    {
//        private readonly static ILog Logger = LogProvider.GetCurrentClassLogger();
//        private readonly AuthorizeResponse _response;

//        public AuthorizeCodeResult(AuthorizeResponse response)
//        {
//            _response = response;
//        }

//        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
//        {
//            return Task.FromResult(Execute());
//        }

//        private HttpResponseMessage Execute()
//        {
//            var responseMessage = new HttpResponseMessage(HttpStatusCode.Redirect);

//            var url = string.Format("{0}?code={1}", _response.RedirectUri.AbsoluteUri, _response.Code);

//            if (_response.State.IsPresent())
//            {
//                url = string.Format("{0}&state={1}", url, _response.State);
//            }

//            responseMessage.Headers.Location = new Uri(url);
//            Logger.Info("Redirecting to: " + url);

//            return responseMessage;
//        }
//    }
//}