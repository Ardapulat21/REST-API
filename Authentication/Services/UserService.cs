using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using QualifiedAuthentication.Interfaces;
using QualifiedAuthentication.Models.Data;
using QualifiedAuthentication.Models.Register;
using QualifiedAuthentication.Models.User;
using System;
using System.Diagnostics.Metrics;

namespace QualifiedAuthentication.Services
{
    public class UserService : IUserService
    {
        private readonly IDatabaseService _databaseService;
        public UserService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        public async Task<UserResponse?> LoginAsync([FromBody] UserLogin credential)
        {
            var user = _databaseService.GetUser(credential);

            return user;
        }
        public async Task<UserResponse?> RegisterAsync([FromBody] UserRequest request)
        {
            var credentials = new UserLogin()
            {
                Username = request.Username,
                Password = request.Password
            };

            var user = _databaseService.GetUser(credentials);

            if (user != null)
                return null;

            _databaseService.InsertUser(request);
            return _databaseService.GetUser(credentials);
        } 
    }
}
