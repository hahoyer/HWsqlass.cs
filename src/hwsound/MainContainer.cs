#region Copyright (C) 2013

//     Project hwsound
//     Copyright (C) 2012 - 2013 Harald Hoyer
// 
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>.
//     
//     Comments, bugs and suggestions to hahoyer at yahoo.de

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth2;
using Google.Apis.Authentication;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;

namespace main
{
    sealed class MainContainer
    {
        public const string ClientID = "612134678421.apps.googleusercontent.com";
        public const string ClientSecret = "2uxI5ZQWVESPzjbTF9reewGH";
        public static readonly string ApiKey = "AIzaSyB4obKa_zD8zcDJDq-iJEGH6c4Syb8w-jQ";

        public static void Main()
        {
        }

        OAuth2Authenticator<WebServerClient> CreateAuthenticator()
        {
            // Register the authenticator.
            var provider = new WebServerClient(GoogleAuthenticationServer.Description);
            provider.ClientIdentifier = ClientID;
            provider.ClientSecret = ClientSecret;
            var authenticator =
                new OAuth2Authenticator<WebServerClient>(provider, GetAuthorization) {NoCaching = true};
            return authenticator;
        }
        IAuthorizationState GetAuthorization(WebServerClient client)
        {
            return null;
        }

    }

}