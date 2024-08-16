using API.Todo.Models;
using Core.Todo.Account;
using Core.Todo.Entities;
using Core.Todo.Repositories;
using Infrastructure.Todo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Todo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticate _authenticateService;

        public UserController(IUserRepository userRepository, IAuthenticate authenticateService)
        {
            _userRepository = userRepository;
            _authenticateService = authenticateService;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(User user)
        {
            try
            {
                await _userRepository.Create(user);
                return Ok(new { message = "Usuário criado com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao criar o usuário.");
            }
        }
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(User user)
        {
            try
            {
                await _userRepository.Update(user);
                return Ok(new { message = "Usuário atualizado com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao atualizar o usuário.");
            }
        }
        [Authorize]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            try
            {
                User user = await _userRepository.GetById(id);
                if (user is not null)
                {
                    await _userRepository.Delete(id);
                    return Ok(new { message = "Usuário excluído com sucesso" });
                }
                return NotFound(new { message = "Usuário não encontrada" });

            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao excluir a usuário.");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                return Ok(await _userRepository.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao listar os usuários.");
            }
        }
        [Authorize]
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                User user = await _userRepository.GetById(id);
                if (user is not null) return Ok(user);
                else return NotFound(new { message = "Usuário não encontrado" });

            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao buscar o usuário.");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserToken>> Login(LoginModel loginModel)
        {
            var existe = await _authenticateService.UserExists(loginModel.Email);
            if (!existe)
            {
                return Unauthorized("Usuário não existe.");
            }

            var result = await _authenticateService.AuthenticateAsync(loginModel.Email, loginModel.Password);
            if (!result)
            {
                return Unauthorized("Usuário ou senha inválido.");
            }

            var usuario = await _authenticateService.GetUserByEmail(loginModel.Email);

            var token = _authenticateService.GenerateToken(usuario.Id, usuario.Email);

            return Ok(new UserToken
            {
                Token = token,
                Email = usuario.Email
            });
        }

    }
}
