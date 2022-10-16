using AutoMapper;
using CarReview.API.Models;
using CarReview.API.DTOs;
namespace CarReview.API.Profiles
{
    public class MappingProfile : Profile
    {
       public MappingProfile()
       {
            CreateMap<Car, CarDTO>();
            CreateMap<CarDTO, Car>();
            CreateMap<Car, GetCarDTO>();

            CreateMap<Review, GetReviewDTO>();

            CreateMap<CreateResponseDTO, Response>();
            CreateMap<Response, CreateResponseDTO>();
        }
    }
}
