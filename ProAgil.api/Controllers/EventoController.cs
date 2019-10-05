using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IProAgilRepository _repository;

        public EventoController(IProAgilRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
         public async Task<IActionResult> Get()
        {
            try
            {
                var results = await _repository.GetAllEventoAsync(true);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou!");
            }
        }

        [HttpGet("{EventoId}")]
         public async Task<IActionResult> Get(int eventoId)
        {
            try
            {
                var results = await _repository.GetAllEventoAsyncById(eventoId, true);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou!");
            }
        }

        [HttpGet("getByTema/{Tema}")]
         public async Task<IActionResult> Get(string tema)
        {
            try
            {
                var results = await _repository.GetAllEventoAsyncByTema(tema, true);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou!");
            }
        }

        [HttpPost]
         public async Task<IActionResult> Post(Evento evento)
        {
            try
            {
                _repository.Add(evento);

                if(await _repository.SaveChangesAsync()) {
                    return Created($"/api/evento/{evento.Id}", evento);
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou!");
            }

            return BadRequest();
        }

        [HttpPut]
         public async Task<IActionResult> Put(int eventoId, Evento evento)
        {
            try
            {
                var response = await _repository.GetAllEventoAsyncById(eventoId, false);

                if(response == null) return NotFound();

                _repository.Update(evento);

                if(await _repository.SaveChangesAsync()) {
                    return Created($"/api/evento/{evento.Id}", evento);
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou!");
            }

            return BadRequest();
        }

        [HttpDelete]
         public async Task<IActionResult> Delete(int eventoId)
        {
            try
            {
                var response = await _repository.GetAllEventoAsyncById(eventoId, false);

                if(response == null) return NotFound();

                _repository.Delete(response);

                if(await _repository.SaveChangesAsync()) {
                    return Ok();
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou!");
            }

            return BadRequest();
        }
    }
}