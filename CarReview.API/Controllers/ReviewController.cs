using AutoMapper;
using CarReview.API.Auth.Model;
using CarReview.API.DTOs;
using CarReview.API.Models;
using CarReview.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CarReview.API.Controllers
{
    [ApiController]
    [Route("API/Reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly CarReviewDbContext _context;
        private readonly UserManager<CarReviewUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public ReviewController(CarReviewDbContext context, IMapper mapper, IAuthorizationService authorizationService, UserManager<CarReviewUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        //[HttpPost]
        //[Authorize(Roles = CarReviewRoles.ReviewUser)]
        //public async Task<IActionResult> Create(CreateReviewDTO newReview)
        //{
        //    if (_context.Cars.Count() == 0)
        //    {
        //        return NotFound("Currently there are no cars added!");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id.Equals(newReview.CarId));

        //        if (car == null)
        //        {
        //            return NotFound("Car by this id does not exist!");
        //        }

        //        Review review = new()
        //        {
        //            Text = newReview.Text,
        //            CreationDate = DateTime.Now,
        //            EngineDisplacement = newReview.EngineDisplacement,
        //            EnginePower = newReview.EnginePower,
        //            Likes = 0,
        //            Dislikes = 0,
        //            Positives = newReview.Positives,
        //            Negatives = newReview.Negatives,
        //            FinalScore = newReview.FinalScore,
        //            UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub),
        //            FkCarId = newReview.CarId,
        //        };

        //        await _context.Reviews.AddAsync(review);
        //        await _context.SaveChangesAsync();

        //        return StatusCode(201);
        //    }

        //    return BadRequest("Model is not valid!");
        //}

        [HttpPost]
        [Authorize(Roles = CarReviewRoles.ReviewUser)]
        public async Task<IActionResult> Create(AddReviewDTO newReview)
        {
            if (_context.Cars.Count() == 0)
            {
                return NotFound("Currently there are no cars added!");
            }

            if (ModelState.IsValid)
            {
                var carId = await GetCarByParameters(newReview.Brand, newReview.Model, newReview.Generation);

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
                    FkCarId = carId,
                };

                await _context.Reviews.AddAsync(review);
                await _context.SaveChangesAsync();

                var mappedReview = new GetReviewDTO2
                {
                    Id = review.Id,
                    Brand = newReview.Brand,
                    Model = newReview.Model,
                    Generation = newReview.Generation,
                    Username = User.FindFirstValue(ClaimTypes.Name),
                    UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub),
                    Text = review.Text,
                    CreationDate = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString(),
                    EngineDisplacement = review.EngineDisplacement,
                    EnginePower = review.EnginePower,
                    Likes = review.Likes,
                    Dislikes = review.Dislikes,
                    Positives = review.Positives,
                    Negatives = review.Negatives,
                    FinalScore = review.FinalScore,
                };

                return CreatedAtAction(nameof(Create), mappedReview);
            }

            return BadRequest("Model is not valid!");
        }

        private async Task<int> GetCarByParameters(string brand, string model, string generation)
        {
            var car = await _context.Cars.SingleOrDefaultAsync(x => x.Brand.Equals(brand) && x.Model.Equals(model) && x.Generation.Equals(generation));
            return car.Id;
        }

        [HttpGet("{id}/GetByCarId")]
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

        //[HttpGet]
        //[Authorize(Roles = CarReviewRoles.ReviewUser)]
        //public async Task<IActionResult> Get()
        //{
        //    if (_context.Reviews.Count() == 0)
        //    {
        //        return NotFound("Currently there are no reviews added!");
        //    }

        //    var reviews = await _context.Reviews.ToListAsync();

        //    return Ok(reviews.Select((p) => _mapper.Map<GetReviewDTO>(p)));
        //}

        [HttpGet("{userName}/review")]
        [Authorize(Roles = CarReviewRoles.ReviewUser)]
        public async Task<IActionResult> GetByUsername(string userName)
        {
            if (_context.Reviews.Count() == 0)
            {
                return NotFound("Currently there are no reviews added!");
            }

            var reviews = await _context.Reviews.ToListAsync();

            var mappedReviews = new List<GetReviewDTO2>();

            foreach (var review in reviews)
            {
                var car = await GetCarById(review.FkCarId);
                var user = await _userManager.FindByIdAsync(review.UserId);

                mappedReviews.Add(new GetReviewDTO2
                {
                    Id = review.Id,
                    Brand = car.Brand,
                    Model = car.Model,
                    Generation = car.Generation,
                    Username = user.UserName,
                    UserId = review.UserId,
                    Text = review.Text,
                    CreationDate = review.CreationDate.Year.ToString() + "-" + review.CreationDate.Month.ToString() + "-" + review.CreationDate.Day.ToString(),
                    EngineDisplacement = review.EngineDisplacement,
                    EnginePower = review.EnginePower,
                    Likes = review.Likes,
                    Dislikes = review.Dislikes,
                    Positives = review.Positives,
                    Negatives = review.Negatives,
                    FinalScore = review.FinalScore
                });
            }

            return Ok(mappedReviews.Where(x => x.Username.Equals(userName)));
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

            var mappedReviews = new List<GetReviewDTO2>();

            foreach (var review in reviews)
            {
                var car = await GetCarById(review.FkCarId);
                var user = await _userManager.FindByIdAsync(review.UserId);

                mappedReviews.Add(new GetReviewDTO2
                {
                    Id = review.Id,
                    Brand = car.Brand,
                    Model = car.Model,
                    Generation = car.Generation,
                    Username = user.UserName,
                    UserId = review.UserId,
                    Text = review.Text,
                    CreationDate = review.CreationDate.Year.ToString() + "-" + review.CreationDate.Month.ToString() + "-" + review.CreationDate.Day.ToString(),
                    EngineDisplacement = review.EngineDisplacement,
                    EnginePower = review.EnginePower,
                    Likes = review.Likes,
                    Dislikes = review.Dislikes,
                    Positives = review.Positives,
                    Negatives = review.Negatives,
                    FinalScore = review.FinalScore
                });
            }

            return Ok(mappedReviews);
        }

        private async Task<Car> GetCarById(int id)
        {
            return _context.Cars.FirstOrDefault(m => m.Id == id);
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
        public async Task<IActionResult> Update(int id, AddReviewDTO updatedReview)
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

                //var car = _context.Cars.FirstOrDefault(m => m.Id == updatedReview.CarId);

                //if (car == null)
                //{
                //    return NotFound("Car by this id does not exist!");
                //}

                review.Text = updatedReview.Text;
                review.EngineDisplacement = updatedReview.EngineDisplacement;
                review.EnginePower = updatedReview.EnginePower;
                review.Positives = updatedReview.Positives;
                review.Negatives = updatedReview.Negatives;
                review.FinalScore = updatedReview.FinalScore;

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
                var authorizationResult = await _authorizationService.AuthorizeAsync(User, review, policyName: PolicyNames.ResourceOwner);
                if (!authorizationResult.Succeeded)
                {
                    return Forbid();
                }

                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            return NotFound("Review by this id does not exist!");
        }

        [HttpGet("GetResponsesResult")]
        [Authorize(Roles = CarReviewRoles.ReviewUser)]
        public async Task<IActionResult> GetResponsesResult()
        {
            if (_context.Reviews.Count() == 0)
            {
                return NotFound("Currently there are no reviews added!");
            }

            var reviews = await _context.Reviews.Where(x => x.UserId.Equals(User.FindFirstValue(JwtRegisteredClaimNames.Sub))).ToListAsync();

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

        [HttpGet("{userId}/GetReviewCount")]
        [Authorize(Roles = CarReviewRoles.ReviewUser)]
        public async Task<IActionResult> GetReviewCount(string userId)
        {
            var allReviews = await _context.Reviews.ToListAsync();

            if (allReviews.Count() == 0)
            {
                return NotFound("Currently there are no reviews added!");
            }

            var reviews = await _context.Reviews.Where(x => x.UserId.Equals(userId)).ToListAsync();

            var count = reviews.Count();

            if (count == 0)
            {
                return NotFound("Currently this user has no reviews!");
            }

            return Ok(count);
        }

        [HttpGet("{userId}/GetByUserId")]
        [Authorize(Roles = CarReviewRoles.ReviewUser)]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            if (_context.Reviews.Count() == 0)
            {
                return NotFound("Currently there are no reviews added!");
            }

            var reviews = await _context.Reviews.Where(x => x.UserId.Equals(userId)).ToListAsync();

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
    }
}
