using Microsoft.AspNetCore.Mvc;
using WebDockerAPI.Interface;
using WebDockerAPI.Models;
using WebDockerAPI.Repository;

namespace WebDockerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepo repo;

        public CountryController(ICountryRepo repo)
        {
            this.repo = repo;
        }

        [HttpGet("All")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ICountryRepo>))]
        public IActionResult GetCountries()
        {
            var countries = repo.GetCountries();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(countries);
        }

        [HttpPost("Add")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddCountry([FromBody] Country country)
        {
            if (country == null)
            {
                return BadRequest(ModelState);
            }

            var ct = this.repo.GetCountries()
                .Where(c => c.Name.Trim().ToLower() == country.Name.ToLower())
                .FirstOrDefault();

            if(ct != null)
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            this.repo.AddCountry(country);

            return Ok("Successfully added");
        }

        [HttpPut("{cid}")]
        [ProducesResponseType(400)] //bad request
        [ProducesResponseType(404)] //not found
        [ProducesResponseType(204)] //no content
        public IActionResult UpdateCountry(int cid, [FromBody] Country country)
        {
            if (country == null)
                return BadRequest(ModelState);

            if (cid != country.Id)
                return BadRequest(ModelState);

            if(!this.repo.IsExist(cid))
            {
                ModelState.AddModelError("", "Item is not exist");
                return StatusCode(500, ModelState); //internal server error
            }

            this.repo.UpdateCountry(country);

            return Ok("Update successfully");
        }

        [HttpDelete("{cid}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public IActionResult DeleteCountry(int cid)
        {
            if (!this.repo.IsExist(cid))
            {
                ModelState.AddModelError("", "Item is not exist");
                return StatusCode(500, ModelState); //internal server error
            }

            var country = this.repo.GetCountry(cid);

            if (country == null)
            {
                ModelState.AddModelError("", "Item is not exist");
                return StatusCode(500, ModelState); //internal server error
            }
                
            this.repo.DeleteCountry(country);

            return Ok("Deleted");
        }
    }
}
