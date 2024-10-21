
namespace KoiMuseum.Data.Dtos.Requests.User
{
    public class CreateUserResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Role { get; set; }

        public string? Description { get; set; }

        public string? AvatarUrl { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }
    }
}
