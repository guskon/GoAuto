using AutoMapper;
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
    [Route("Reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly CarReviewDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public ReviewController(CarReviewDbContext context, IMapper mapper, IAuthorizationService authorizationService)
        {
            _context = context;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [Authorize(Roles = CarReviewRoles.ReviewUser)]
        public async Task<IActionResult> Create(CreateReviewDTO newReview)
        {
            if (_context.Cars.Count() == 0)
            {
                return NotFound("Currently there are no cars added!");
            }

            if (_context.Users.Count() == 0)
            {
                return NotFound("Currently there are no users added!");
            }

            if (ModelState.IsValid)
            {
                var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id.Equals(newReview.CarId));

                if (car == null)
                {
                    return NotFound("Car by this id does not exist!");
                }

                //var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(User.FindFirstValue(JwtRegisteredClaimNames.Sub)));

                //if (user == null)
                //{
                //    return NotFound("User by this id does not exist!");
                //}

                Review review = new()
                {
                    Text = newReview.Text,
                    CreationDate = DateTime.Now,
                    EngineDisplacement = newReview.EngineDisplacement,
                    EnginePower = newReview.EnginePower,
                    Likes = 0,
                    Dislikes = 0,
                    Positives = newReview.Positives,
                    Negatives = newReview.Negatives,
                    FinalScore = newReview.FinalScore,
                    UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub),
                    FkCarId = newReview.CarId,
                };

                await _context.Reviews.AddAsync(review);
                await _context.SaveChangesAsync();

                return StatusCode(201);
            }

            return BadRequest("Model is not valid!");
        }

        [HttpGet("{id}/Get by car id")]
        [Authorize(Roles = CarReviewRoles.ReviewUser)]
        public async Task<IActionResult> GetByCarId(int id)
        {
            if (_context.Reviews.Count() == 0)
            {
                return NotFound("Currently there are no reviews added!");
            }

            if (_context.Cars.Count() == 0)
            {
                return NotFound("Currently there are no cars added!");
            }

            var car = _context.Cars.FirstOrDefault(m => m.Id == id);

            if (car == null)
            {
                return NotFound("Car by this id does not exist!");
            }

            var reviews = _context.Reviews.Where(x => x.FkCarId == id).ToList();

            if (reviews.Count() == 0)
            {
                return NotFound("Currently this car does not have any reviews!");
            }

            return Ok(reviews.Select((p) => _mapper.Map<GetReviewDTO>(p)));
        }

        [HttpGet]
        [Authorize(Roles = CarReviewRoles.ReviewUser)]
        public async Task<IActionResult> Get()
        {
            if (_context.Reviews.Count() == 0)
            {
                return NotFound("Currently there are no reviews added!");
            }

            var reviews = await _context.Reviews.ToListAsync();

            return Ok(reviews.Select((p) => _mapper.Map<GetReviewDTO>(p)));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = CarReviewRoles.ReviewUser)]
        public async Task<IActionResult> GetById(int id)
        {
            if (_context.Reviews.Count() == 0)
            {
                return NotFound("Currently there are no reviews added!");
            }

            var review = GetReview(id);

            if (review == null)
            {
                return NotFound("Review by this id does not exist!");
            }

            return Ok(_mapper.Map<GetReviewDTO>(review));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = CarReviewRoles.ReviewUser)]
        public async Task<IActionResult> Update(int id, UpdateReviewDTO updatedReview)
        {
            if (_context.Reviews.Count() == 0)
            {
                return NotFound("Currently there are no reviews added!");
            }

            if (ModelState.IsValid)
            {
                Review review = GetReview(id);

                if (review == null)
                {
                    return NotFound("Review by this id does not exist!");
                }

                var authorizationResult = await _authorizationService.AuthorizeAsync(User, review, policyName: PolicyNames.ResourceOwner);
                if (!authorizationResult.Succeeded)
                {
                    return Forbid();
                }

                var car = _context.Cars.FirstOrDefault(m => m.Id == updatedReview.CarId);

                if (car == null)
                {
                    return NotFound("Car by this id does not exist!");
                }

                review.Text = updatedReview.Text;
                review.EngineDisplacement = updatedReview.EngineDisplacement;
                review.EnginePower = updatedReview.EnginePower;
                review.Positives = updatedReview.Positives;
                review.Negatives = updatedReview.Negatives;
                review.FinalScore = updatedReview.FinalScore;
                review.FinalScore = updatedReview.FinalScore;
                review.FkCarId = updatedReview.CarId;

                _context.Update(review);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest("Model is not valid!");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = CarReviewRoles.ReviewUser)]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Reviews.Count() == 0)
            {
                return NotFound("Currently there are no reviews added!");
            }

            var review = GetReview(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            return NotFound("Review by this id does not exist!");
        }

        [HttpGet("GetResponsesResult")]
        [Authorize(Roles = CarReviewRoles.ReviewUser)]
        public async Task<IActionResult> GetResponsesResult(int userId)
        {
            if (_context.Users.Count() == 0)
            {
                return NotFound("Currently there are no users added!");
            }

            var user = await _context.Users.FirstOrDefaultAsync<CarReviewUser>(x => x.Id.Equals(userId));

            if (user == null)
            {
                return NotFound("User by this id does not exist!");
            }

            if (_context.Reviews.Count() == 0)
            {
                return NotFound("Currently there are no reviews added!");
            }

            var reviews = await _context.Reviews.Where(x => x.UserId.Equals(userId)).ToListAsync();

            if (reviews.Count() == 0)
            {
                return NotFound("User does not have any reviews!");
            }

            var sumLikes = 0;
            var sumDislikes = 0;

            foreach (var review in reviews)
            {
                sumLikes += review.Likes;
                sumDislikes += review.Dislikes;
            }

            return Ok(new ResponsesResultDTO() { Likes = sumLikes, Dislikes = sumDislikes });
        }

        [HttpGet("{id}/GetReviewCount")]
        [Authorize(Roles = CarReviewRoles.ReviewUser)]
        public async Task<IActionResult> GetReviewCount(int id)
        {
            if (_context.Users.Count() == 0)
            {
                return NotFound("Currently there are no users added!");
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (user == null)
            {
                return NotFound("User by this id does not exist!");
            }

            var allReviews = await _context.Reviews.ToListAsync();

            if (allReviews.Count() == 0)
            {
                return NotFound("Currently there are no reviews added!");
            }

            var reviews = await _context.Reviews.Where(x => x.UserId.Equals(id)).ToListAsync();

            var count = reviews.Count();

            if (count == 0)
            {
                return NotFound("Currently this user has no reviews!");
            }

            return Ok(count);
        }

        [HttpGet("{id}/GetByUserId")]
        [Authorize(Roles = CarReviewRoles.ReviewUser)]
        public async Task<IActionResult> GetByUserId(int id)
        {
            if (_context.Users.Count() == 0)
            {
                return NotFound("Currently there are no users added!");
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (user == null)
            {
                return NotFound("User by this id does not exist!");
            }

            if (_context.Reviews.Count() == 0)
            {
                return NotFound("Currently there are no reviews added!");
            }

            var reviews = await _context.Reviews.Where(x => x.UserId.Equals(id)).ToListAsync();

            if (reviews.Count() == 0)
            {
                return NotFound("User does not have any reviews!");
            }

            return Ok(reviews.Select((p) => _mapper.Map<GetReviewDTO>(p)));
        }

        private Review GetReview(int id)
        {
            return _context.Reviews.FirstOrDefault(m => m.Id == id);
        }




        //[HttpPost("Filter reviews")]
        //public async Task<IActionResult> FilterReviews(FilterReviewsDTO filter)
        //{
        //    if (_context.Cars.Count() == 0)
        //    {
        //        return BadRequest("Currently there are no cars added!");
        //    }

        //    if (_context.Reviews.Count() == 0)
        //    {
        //        return BadRequest("Currently there are no reviews added!");
        //    }

        //    var filteredByBrand = await _context.Cars.Where(x => x.Brand.Equals(filter.Brand)).ToListAsync();

        //    if (filteredByBrand.Count() == 0)
        //    {
        //        return NotFound("Car brand was not found!");
        //    }

        //    var filteredByModel = filteredByBrand.Where(x => x.Model.Equals(filter.Model)).ToList();

        //    if (filteredByModel.Count() == 0)
        //    {
        //        return NotFound("Car model was not found!");
        //    }

        //    var filteredByGeneration = filteredByModel.Where(x => x.Generation.Equals(filter.Generation)).ToList();

        //    if (filteredByGeneration.Count() == 0)
        //    {
        //        return NotFound("Car model generation was not found!");
        //    }

        //    var filteredReviews = await _context.Reviews.Where(x => x.FkCarId.Equals(filteredByGeneration[0].Id)).ToListAsync();

        //    if (filteredReviews.Count() == 0)
        //    {
        //        return NotFound("Reviews not found!");
        //    }

        //    return Ok(filteredReviews.Select((p) => _mapper.Map<GetReviewDTO>(p)));
        //}
    }
}
