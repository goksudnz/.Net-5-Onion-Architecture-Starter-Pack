// Copyrights(c) Charqe.io. All rights reserved.

using System.Threading.Tasks;
using BusinessLogicLayer.Generic;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Generic;
using Domain.DTOs;
using Domain.Entities;
using Domain.Models.General;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace BusinessLogicLayer.Services
{
    public class AppUserService : GenericService<AppUser>, IAppUserService<AppUser>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        private readonly IPasswordHasher<AppUser> _passwordHasher;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="jwtConfig">Jwt config file.</param>
        /// <param name="userManager">User manager of microsoft identity.</param>
        /// <param name="passwordHasher">Password hasher.</param>
        public AppUserService(IUnitOfWork unitOfWork, IOptions<JwtConfig> jwtConfig, UserManager<AppUser> userManager, IPasswordHasher<AppUser> passwordHasher) : base(unitOfWork)
        {
            _userManager = userManager;
            _jwtConfig = jwtConfig.Value;
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Using for login.
        /// </summary>
        /// <param name="email">User's email.</param>
        /// <param name="password">User's password.</param>
        /// <returns>Service result with referenced object.</returns>
        public Task<ServiceResultExt<LoginResponseDTO>> Login(string email, string password)
        {
            // TODO: User can implement his/her own method.
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Using for register.
        /// </summary>
        /// <param name="user">User object.</param>
        /// <param name="password">User's password.</param>
        /// <returns>Service result with represent the result of an identity operation.</returns>
        public Task<ServiceResultExt<IdentityResult>> Register(AppUser user, string password)
        {
            // TODO: User can implement his/her own method.
            throw new System.NotImplementedException();
        }
    }
}