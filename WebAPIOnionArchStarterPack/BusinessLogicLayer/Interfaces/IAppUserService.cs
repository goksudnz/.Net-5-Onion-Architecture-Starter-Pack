// Copyrights(c) Charqe.io. All rights reserved.

using System.Threading.Tasks;
using BusinessLogicLayer.Generic;
using Domain.DTOs;
using Domain.Entities;
using Domain.Models.General;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogicLayer.Interfaces
{
    public interface IAppUserService<TEntity> : IGenericService<TEntity> where TEntity : class, new()
    {
        Task<ServiceResultExt<LoginResponseDTO>> Login(string email, string password);
        Task<ServiceResultExt<IdentityResult>> Register(AppUser user, string password);
    }
}