using System.ComponentModel.DataAnnotations;

namespace CarReview.API.Auth.Model;

public record RegisterUserDTO([Required] string UserName, [EmailAddress][Required] string Email, [Required] string Password);

public record LoginDTO(string UserName, string Password);

public record UserDTO(string Id, string UserName, string Email);

public record SuccessfulLoginDTO(string accessToken);