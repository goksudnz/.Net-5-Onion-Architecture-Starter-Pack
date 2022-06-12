// Copyrights(c) Charqe.io. All rights reserved.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.ModelServices;
using PresentationLayer.ViewModels;

namespace PresentationLayer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserModelService _modelService;

        public UsersController(UserModelService modelService) => _modelService = modelService;

        /// <summary>
        /// Login action.
        /// </summary>
        /// <param name="model">Login view model.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginVM model)
        {
            var res = await _modelService.Login(model);
            if (!res.Status) return BadRequest(res);
            return Ok(res);
        }

        /// <summary>
        /// Register action.
        /// </summary>
        /// <param name="model">Register view model.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            var res = await _modelService.Register(model);
            if (!res.Status) return BadRequest(res);
            return Ok(res);
        }
    }
}