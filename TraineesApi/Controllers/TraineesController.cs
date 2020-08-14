using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TraineesApi.Models;
using TraineesApi.Services;
using System.Linq;
using MongoDB.Driver;

namespace TraineesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TraineesController : ControllerBase
    {
        private readonly TraineeService _traineeService;

        public TraineesController(TraineeService traineeService)
        {
            _traineeService = traineeService;
        }

        // GET: api/Trainees
        [HttpGet]
        public ActionResult<List<Trainee>> Get() =>
            _traineeService.Get();

        // GET: api/Trainees/All
        [HttpGet("All", Name = "GetAll")]        
        public async Task<ActionResult<List<Trainee>>> GetAll()
        {
            var all = await _traineeService.GetAsync();            

            return Ok(all);
        }      

        // GET: api/Trainees/Olivia Pratt
        [HttpGet("{name}", Name = "GetByName")]
        public async Task<ActionResult<List<Trainee>>> GetByName(string name)
        {
            var trainees = await _traineeService.GetByName(name);

            return Ok(trainees);
        }

        // GET: api/Trainees/5
        [HttpGet("{id:length(24)}", Name = "GetTrainee")]
        public ActionResult<Trainee> Get(string id)
        {
            var trainee = _traineeService.Get(id);

            if (trainee == null)
            {
                return NotFound();
            }

            return trainee;
        }

        // POST: api/Trainees
        [HttpPost]
        public ActionResult<Trainee> Create(Trainee trainee)
        {
            _traineeService.Create(trainee);

            return CreatedAtRoute("GetTrainee", new { id = trainee.Id.ToString() }, trainee);
        }

        // PUT: api/Trainees/transact/5        
        [HttpPut("{transact}/{id:length(24)}", Name = "UpdateInTransaction")]       
        public async Task<IActionResult> UpdateInTransaction(string id, Trainee traineeUpdate)
        {
            if (id != traineeUpdate.Id)
            {
                return BadRequest();
            }

            var trainee = _traineeService.Get(id);

            if (trainee == null)
            {
                return NotFound();
            }

            try
            {
                await _traineeService.UpdateInTransaction(id, traineeUpdate);

                return NoContent();
            }
            catch(Exception)
            {
                throw;
            }            
        }

        // PUT: api/Trainees/5
        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Trainee traineeUpdate)
        {
            var trainee = _traineeService.Get(id);

            if (trainee == null)
            {
                return NotFound();
            }

            _traineeService.Update(id, traineeUpdate);

            return NoContent();
        }

        // DELETE: api/Trainees/5
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var trainee = _traineeService.Get(id);

            if (trainee == null)
            {
                return NotFound();
            }

            _traineeService.Remove(trainee.Id);

            return NoContent();
        }

    }
}
