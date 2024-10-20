﻿using TripPlanner.Logic.DtoModels;

namespace TripPlanner.Logic.Services
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, string email, string role);
        bool ValidateToken(string key, string issuer, string token);
        string GenerateRefreshToken();
    }
}
