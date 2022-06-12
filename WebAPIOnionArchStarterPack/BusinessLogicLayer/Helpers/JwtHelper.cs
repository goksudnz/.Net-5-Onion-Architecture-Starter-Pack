// Copyrights(c) Charqe.io. All rights reserved.

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace BusinessLogicLayer.Helpers
{
    internal static class JwtHelper
    {
        /// <summary>
        /// Generating jwt token.
        /// </summary>
        /// <param name="user">Authenticated user.</param>
        /// <param name="secretKey">Secret key of jwt.</param>
        /// <returns></returns>
        internal static string GenerateJwtToken(AppUser user, string secretKey)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Actor, BllConstants.Authorization.Basic)
                }),
                Expires = DateTime.Now.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }

    }
}