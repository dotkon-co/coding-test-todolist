using Core.Todo.Entities;
using Core.Todo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Todo.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly IJobRepository _jobRepository;
        private readonly IUserRepository _userRepository;

        public JobController(IJobRepository jobRepository, IUserRepository userRepository)
        {
            _jobRepository = jobRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Job job)
        {
            try
            {
                var userId = long.Parse(User.FindFirst("id").Value);
                job.UserId = userId;
                await _jobRepository.Create(job);
                return Ok(new { message = "Tarefa criada com sucesso"});
            }
            catch (Exception ex) 
            {
                return BadRequest("Ocorreu um erro ao criar a tarefa.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(Job job)
        {
            try
            {
                await _jobRepository.Update(job);
                return Ok(new { message = "Tarefa atualizada com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao atualizar a tarefa.");
            }
        }

        [HttpPut("Donejob")]
        public async Task<IActionResult> DoneJobAsync(Job job)
        {
            try
            {
                await _jobRepository.DoneJob(job);
                return Ok(new { message = "Tarefa finalizada com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao atualizar a tarefa.");
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            try
            {
                Job job = await _jobRepository.GetById(id);
                if (job is not null)
                {
                    await _jobRepository.Delete(id);
                    return Ok(new { message = "Tarefa excluída com sucesso" });
                }
                return NotFound(new { message = "Tarefa não encontrada" });  

            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao excluir a tarefa.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                return Ok(await _jobRepository.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao listar as tarefas.");
            }
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            try
            {
                Job job = await _jobRepository.GetById(id);
                if (job is not null) return Ok(job);
                else return NotFound(new { message = "Tarefa não encontrada"});
                
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao buscar a tarefa.");
            }
        }

        [HttpGet("GetAllDoneByUser")]
        public async Task<IActionResult> GetAllDoneByUserAsync()
        {
            try
            {
                long userId = long.Parse(User.FindFirst("id").Value);
                return Ok(await _jobRepository.GetAllDoneByUser(userId));
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao listar as tarefas.");
            }
        }

        [HttpGet("GetAllNotDoneByUser")]
        public async Task<IActionResult> GetAllNotDoneAsync()
        {
            try
            {
                long userId = long.Parse(User.FindFirst("id").Value);

                return Ok(await _jobRepository.GetAllNotDoneByUser(userId));
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao listar as tarefas.");
            }
        }
    }
}
