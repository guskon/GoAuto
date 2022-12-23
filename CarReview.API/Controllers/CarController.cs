using CarReview.API.Auth.Model;
using CarReview.API.DTOs;
using CarReview.API.Models;
using CarReview.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CarReview.API.Controllers
{
    [ApiController]
    [Route("API/Cars")]
    public class CarController : ControllerBase
    {
        private readonly CarReviewDbContext _context;

        public CarController(CarReviewDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = CarReviewRoles.ReviewUser)]
        public async Task<IActionResult> Get()
        {
            if (_context.Cars.Count() == 0)
            {
                return BadRequest("Currently there are no cars added!");
            }

            var cars = await _context.Cars.ToListAsync();

            var mappedCars = new List<GetCarDTO>();

            foreach (var car in cars)
            {
                mappedCars.Add(new GetCarDTO
                {
                    Id = car.Id,
                    Brand = car.Brand,
                    Model = car.Model,
                    Generation = car.Generation,
                    StartYear = car.StartYear.Year.ToString(),
                    EndYear = car.EndYear.Year.ToString(),
                    UserId = car.UserId
                });
            }

            return Ok(mappedCars);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = CarReviewRoles.ReviewUser)]
        public async Task<IActionResult> GetById(int id)
        {
            if (_context.Cars.Count() == 0)
            {
                return NotFound("Currently there are no cars added!");
            }

            var car = GetCarById(id);

            if (car == null)
            {
                return NotFound("Car by this id does not exist!");
            }

            var mappedCar = new GetCarDTO
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Generation = car.Generation,
                StartYear = car.StartYear.Year.ToString(),
                EndYear = car.EndYear.Year.ToString(),
                UserId = car.UserId
            };

            return Ok(mappedCar);
        }

        [HttpPost]
        [Authorize(Roles = CarReviewRoles.Admin)]
        public async Task<IActionResult> Create(CarDTO car)
        {
            if (ModelState.IsValid)
            {
                if (await GetCar(car) != null)
                {
                    return BadRequest("This car already exists!");
                }

                int startYear;
                int endYear;

                if (!int.TryParse(car.StartYear, out startYear))
                {
                    return BadRequest("Start year has to be an integer!");
                }

                if (!int.TryParse(car.EndYear, out endYear))
                {
                    return BadRequest("End year has to be an integer!");
                }

                Car newCar = new()
                {
                    Brand = car.Brand,
                    Model = car.Model,
                    Generation = car.Generation,
                    StartYear = new DateTime(startYear, 1, 1),
                    EndYear = new DateTime(endYear, 1, 1),
                    UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                };

                await _context.AddAsync(newCar);
                await _context.SaveChangesAsync();

                var returnCar = new GetCarDTO
                {
                    Id = newCar.Id,
                    Brand = newCar.Brand,
                    Model = newCar.Model,
                    Generation = newCar.Generation,
                    StartYear = newCar.StartYear.Year.ToString(),
                    EndYear = newCar.EndYear.Year.ToString(),
                    UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                };
            
                return CreatedAtAction(nameof(Create), returnCar);
            }
            
            return BadRequest("Model is not valid!");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = CarReviewRoles.Admin)]
        public async Task<IActionResult> Update(int id, CarDTO updatedCar)
        {
            if (_context.Cars.Count() == 0)
            {
                return NotFound("Currently there are no cars added!");
            }

            if (ModelState.IsValid)
            {
                Car car = GetCarById(id);

                if (car == null)
                {
                    return NotFound("Car by this id does not exist!");
                }

                int startYear;
                int endYear;

                if (!int.TryParse(updatedCar.StartYear, out startYear))
                {
                    return BadRequest("Start year has to be an integer!");
                }

                if (!int.TryParse(updatedCar.EndYear, out endYear))
                {
                    return BadRequest("End year has to be an integer!");
                }

                car.Brand = updatedCar.Brand;
                car.Model = updatedCar.Model;
                car.Generation = updatedCar.Generation;
                car.StartYear = new DateTime(startYear, 1, 1);
                car.EndYear = new DateTime(endYear, 1, 1);

                if (await GetCarForUpdate(updatedCar) != null)
                {
                    return BadRequest("This car already exists!");
                }

                _context.Update(car);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest("Model is not valid!");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = CarReviewRoles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Cars.Count() == 0)
            {
                return NotFound("Currently there are no cars added!");
            }

            var car = GetCarById(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            return NotFound("Car by this id does not exist!");
        }

        [HttpGet("{brand}/FilterByBrand")]
        [Authorize(Roles = CarReviewRoles.ReviewUser)]
        public async Task<IActionResult> FilterByBrand(string brand)
        {
            if (_context.Cars.Count() == 0)
            {
                return NotFound("Currently there are no cars added!");
            }

            var filteredByBrand = await _context.Cars.Where(x => x.Brand.Equals(brand)).ToListAsync();

            if (filteredByBrand.Count() == 0)
            {
                return NotFound("Car brand was not found!");
            }

            var mappedCars = new List<GetCarDTO>();

            foreach (var car in filteredByBrand)
            {
                mappedCars.Add(new GetCarDTO
                {
                    Id = car.Id,
                    Brand = car.Brand,
                    Model = car.Model,
                    Generation = car.Generation,
                    StartYear = car.StartYear.Year.ToString(),
                    EndYear = car.EndYear.Year.ToString(),
                });
            }

            return Ok(mappedCars);
        }

        [HttpPost("FilterByModel")]
        [Authorize(Roles = CarReviewRoles.ReviewUser)]
        public async Task<IActionResult> FilterByModel(FilterByModelDTO dto)
        {
            if (_context.Cars.Count() == 0)
            {
                return NotFound("Currently there are no cars added!");
            }

            if (ModelState.IsValid)
            {
                var filteredByModel = dto.FilteredCars.Where(x => x.Model.Equals(dto.Model)).ToList();

                if (filteredByModel.Count() == 0)
                {
                    return NotFound("Car model was not found!");
                }

                return Ok(filteredByModel);
            }
            else
            {
                return BadRequest("Model is not valid!");
            }
        }

        [HttpPost("GetIdByGeneration")]
        [Authorize(Roles = CarReviewRoles.ReviewUser)]
        public async Task<IActionResult> GetCarIdByGeneration(FilterByGenerationDTO dto)
        {
            if (_context.Cars.Count() == 0)
            {
                return NotFound("Currently there are no cars added!");
            }

            if (ModelState.IsValid)
            {
                var filteredByGeneration = dto.FilteredCars.Where(x => x.Generation.Equals(dto.Generation)).ToList();

                if (filteredByGeneration.Count() == 0)
                {
                    return NotFound("Car model generation was not found!");
                }

                return Ok(filteredByGeneration[0].Id);
            }
            else
            {
                return BadRequest("Model is not valid!");
            }
        }

        private Car GetCarById(int id)
        {
            return _context.Cars.FirstOrDefault(m => m.Id == id);
        }

        private async Task<Car> GetCar(CarDTO car)
        {
            return await _context.Cars.FirstOrDefaultAsync(x => x.Brand.Equals(car.Brand) && x.Model.Equals(car.Model)
                 && x.Generation.Equals(car.Generation));
        }

        private async Task<Car> GetCarForUpdate(CarDTO car)
        {
            return await _context.Cars.FirstOrDefaultAsync(x => x.Brand.Equals(car.Brand) && x.Model.Equals(car.Model)
                 && x.Generation.Equals(car.Generation) && x.StartYear.Equals(new DateTime(Convert.ToInt32(car.StartYear), 1, 1))
                 && x.EndYear.Equals(new DateTime(Convert.ToInt32(car.EndYear), 1, 1)));
        }
    }
}
