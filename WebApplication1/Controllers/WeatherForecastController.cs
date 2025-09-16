using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using System.Collections;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static List<string> Summaries = new()
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        //[HttpGet]
        //public List<string> Get()
        //{
        //    return Summaries;
        //}
        [HttpPost]
        public IActionResult Add(string name)
        {
            if (name == "Igor")
            {
                return BadRequest("Ты не игор!!!");
            }
            if (2 >= name.Length)
            {
                return BadRequest("слишком коротко!");
            }

            Summaries.Add(name);
            return Ok();
        }
        [HttpPut]
        public IActionResult Update(int index, string name)
        {
            if (index < 0 || index >= Summaries.Count)
            {
                return BadRequest("Нет такого индекса!!!");
            }


            Summaries[index] = name;
            return Ok();
        }
        [HttpDelete]
        public IActionResult Delete(int index)
        {
            Summaries.RemoveAt(index);
            return Ok();
        }
        [HttpGet("{index}")]
        public ActionResult<string> GetItem(int index)
        {
            if (index < 0 || index >= Summaries.Count)
            {
                return BadRequest("Данный индекс неверен!!!");
            }
            return Summaries[index];
        }
        [HttpGet("find-by-name /{name}")]
        public ActionResult<IEnumerable<string>> GetName(string name)
        {
            var foundItems = Summaries.Where(i => i.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            return Ok(foundItems);
        }
        [HttpGet]
        public IActionResult GettAll([FromQuery] int? sortStrategy = null)
        {
            try
            {
                var result = sortStrategy switch
                {
                    null => Summaries,
                    1 => Summaries.OrderBy(x => x).ToList(),
                    -1 =>Summaries.OrderByDescending(x => x).ToList(),
                    _ => throw new ArgumentException("Некоректное значение параметра")
                };
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
