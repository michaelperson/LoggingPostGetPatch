using LoggingAndErrors.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingAndErrors.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        #region Fields
        private IConfiguration _configuration = null;
        #endregion
        #region Constructors
        public TokenController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        #endregion
        #region Public methods
        /// <summary>
        /// Action de login
        /// </summary>
        /// <param name="model">un login model avec le mot de passe</param>
        /// <returns>un usermodel avec le token ou un code 401</returns>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            IActionResult result = this.Unauthorized();

            if (model != null)
            {
                UserModel resultModel = new UserModel();

                resultModel.Token = this.CreateToken(model);

                result = this.Ok(resultModel);
            }

            return result;
        }
        #endregion
        #region Private methods
        private string CreateToken(LoginModel model)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration["jwt:key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(this._configuration["jwt:issuer"],
                                             this._configuration["jwt:audience"],
                                             expires: DateTime.Now.AddMinutes(30),
                                             signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}
