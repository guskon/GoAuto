using AutoMapper;
using AutoMapper.Configuration.Conventions;
using CarReview.API.DTOs;
using CarReview.API.Models;
using CarReview.API.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarReview.API.Controllers
{
    [ApiController]
    [Route("Responses")]
    public class ResponseController : Controller
    {
        private readonly CarReviewDbContext _context;
        private readonly IMapper _mapper;

        public ResponseController(CarReviewDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateResponse(CreateResponseDTO newResponse)
        {
            if (_context.Users.Count() == 0)
            {
                return NotFound("Currently there are no users added!");
            }

            if (_context.Reviews.Count() == 0)
            {
                return NotFound("Currently there are no reviews added!");
            }

            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync<User>(x => x.Id.Equals(newResponse.FkUserId));

                if (user == null)
                {
                    return NotFound("User by this id does not exist!");
                }

                var foundReview = await _context.Reviews.FirstOrDefaultAsync<Review>(x => x.Id.Equals(newResponse.FkReviewId));

                if (foundReview == null)
                {
                    return NotFound("Review by this id does not exist!");
                }

                var existingResponse = await _context.Responses.FirstOrDefaultAsync(x => x.FkUserId.Equals(newResponse.FkUserId)
                    && x.FkReviewId.Equals(newResponse.FkReviewId));

                if (existingResponse == null)
                {
                    await _context.Responses.AddAsync(_mapper.Map<Response>(newResponse));

                    if (newResponse.Status == 1)
                    {
                        foundReview.Likes++;

                        _context.Reviews.Update(foundReview);
                    }
                    else if (newResponse.Status == 0)
                    {
                        foundReview.Dislikes++;

                        _context.Reviews.Update(foundReview);
                    }
                    await _context.SaveChangesAsync();
                    return StatusCode(201);
                }
                else if (existingResponse != null)
                {
                    if (existingResponse.Status == newResponse.Status)
                    {
                        if (existingResponse.Status == 1)
                        {
                            foundReview.Likes--;

                            _context.Reviews.Update(foundReview);
                        }
                        else if (existingResponse.Status == 0)
                        {
                            foundReview.Dislikes--;

                            _context.Reviews.Update(foundReview);
                        }
                        _context.Responses.Remove(existingResponse);
                        await _context.SaveChangesAsync();
                    }
                    else if (existingResponse.Status != newResponse.Status)
                    {
                        if (existingResponse.Status == 1)
                        {
                            foundReview.Likes--;

                            foundReview.Dislikes++;

                            existingResponse.Status = newResponse.Status;
                            _context.Responses.Update(existingResponse);
                            _context.Reviews.Update(foundReview);
                        }
                        else if (existingResponse.Status == 0)
                        {
                            foundReview.Dislikes--;

                            foundReview.Likes++;

                            existingResponse.Status = newResponse.Status;
                            _context.Responses.Update(existingResponse);
                            _context.Reviews.Update(foundReview);
                        }
                    }
                }
            }
            else
            {
                return BadRequest("Model is not valid!");
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var responses = await _context.Responses.ToListAsync();

            if (_context.Responses.Count() == 0)
            {
                return NotFound("Currently there are no responses added!");
            }

            return Ok(responses.Select((p) => _mapper.Map<CreateResponseDTO>(p)));
        }
    }
}
