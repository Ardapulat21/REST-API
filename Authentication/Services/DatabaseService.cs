using Azure.Core;
using QualifiedAuthentication.Interfaces;
using QualifiedAuthentication.Models.Data;
using QualifiedAuthentication.Models.Register;
using QualifiedAuthentication.Models.Token;
using QualifiedAuthentication.Models.User;
using System;

namespace QualifiedAuthentication.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly DataBase _dataBase;
        public DatabaseService(DataBase dataBase)
        {
            _dataBase = dataBase;
        }
        public UserResponse? GetUser(UserLogin? user)
        {
            return _dataBase.Users.SingleOrDefault(u => u.Username == user.Username && u.Password == user.Password);
        }
        public UserResponse? GetUser(string? refreshToken)
        {
            var token = _dataBase.RefreshTokens.SingleOrDefault(t => t.RefreshToken == refreshToken);
            return _dataBase.Users.SingleOrDefault(u => u.Id == token.Id);
        }
        public UserRefreshToken? GetUserRefreshToken(UserRefreshToken refreshToken)
        {
            return _dataBase.RefreshTokens.SingleOrDefault(t => t.Id == refreshToken.Id);
        }
        public UserRefreshToken? GetUserRefreshToken(string? refreshToken)
        {
            return _dataBase.RefreshTokens.SingleOrDefault(x => x.RefreshToken == refreshToken);
        }
        public void InsertRefreshToken(UserRefreshToken userRefreshToken)
        {
            _dataBase.RefreshTokens.RemoveAll(t => t.Id == userRefreshToken.Id);

            _dataBase.RefreshTokens.Add(userRefreshToken);
        }
        public void InsertUser(UserRequest request)
        {
            _dataBase.Users.Add(
                new UserResponse()
                {
                    Id = UserResponse.Counter++,
                    Username = request.Username,
                    Email = request.Email,
                    Password = request.Password
                });
        }
        public List<UserResponse> GetAllCredentials()
        {
            return _dataBase.Users;
        }
    }
}
