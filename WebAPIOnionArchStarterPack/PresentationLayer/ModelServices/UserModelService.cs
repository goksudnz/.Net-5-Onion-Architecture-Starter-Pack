// Copyrights(c) Charqe.io. All rights reserved.

using System.Threading.Tasks;
using AutoMapper;
using BusinessLogicLayer.Interfaces;
using Domain.DTOs;
using Domain.Entities;
using Domain.Models.General;
using Microsoft.AspNetCore.Identity;
using PresentationLayer.ViewModels;

namespace PresentationLayer.ModelServices
{
    public class UserModelService
    {
        private readonly IAppUserService<AppUser> _userService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="userService">App user service injection.</param>
        /// <param name="mapper">Automapper injection.</param>
        public UserModelService(IAppUserService<AppUser> userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Login process of model service layer.
        /// </summary>
        /// <param name="model">Login view model.</param>
        /// <returns>Service result with referenced object.</returns>
        internal Task<ServiceResultExt<LoginResponseDTO>> Login(LoginVM model)
        {
            var user = _userService.Login(model.Email, model.Password);
            return user;
        }

        /// <summary>
        /// Register process of model service layer.
        /// </summary>
        /// <param name="model">Register view model.</param>
        /// <returns>Service result with represents result of an identity operation.</returns>
        internal Task<ServiceResultExt<IdentityResult>> Register(RegisterVM model)
        {
            var user = _mapper.Map<AppUser>(model);
            return _userService.Register(user, model.Password);
        }
    }
}