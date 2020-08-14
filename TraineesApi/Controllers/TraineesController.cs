using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TraineesApi.Models;
using TraineesApi.Services;

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

        // DELETE: api/ApiWithActions/5
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

        //The function takes as input a binary string and outputs whether it’s a a good binary string.
        //A non-empty binary string is to be good if the following two conditions are true:
        //1. The number of 0's is equal to the number of 1's.
        //2. For every prefix of the binary string, the number of 1's should not be less than the number of 0's.
        private bool IsBinaryStringGood(string binaryStr)
        {
            try
            {
                bool isGood = true;

                //non-empty binary string
                if (binaryStr == null || binaryStr.Length == 0) return false;

                //The number of 0's is equal to the number of 1's
                if (binaryStr.Length % 2 > 0) return false;
                int count = binaryStr.Count(chr => chr == '0');
                if (count != binaryStr.Length / 2) return false;

                //For every prefix of the binary string, the number of 1's should not be less than the number of 0's
                if (binaryStr.Substring(0, 1) != "1") return false;
                
                var innerList = new List<int>();
                int length = binaryStr.Length;
                int i = 0;

                while (isGood && i < length)
                {
                    innerList.Add(int.Parse(binaryStr[i].ToString()));
                    isGood = innerList.Sum() >= Math.Ceiling((double)innerList.Count / 2);
                    i++;
                }

                return isGood;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
