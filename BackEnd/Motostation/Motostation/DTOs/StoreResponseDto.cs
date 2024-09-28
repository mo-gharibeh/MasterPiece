namespace Motostation.DTOs
{
    public class StoreResponseDto
    {
        //public int StoreId { get; set; }

        public string StoreName { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string? Email { get; set; }

        public string? WorkingHours { get; set; }

        public IFormFile? StoreImageUrl { get; set; }

        public string? Location { get; set; }
        public int ManagerId { get; set; }

        //public UserDto User { get; set; }
    }
    //public class UserDto
    //{
    //    public int UserId { get; set; }

        //    public string UserName { get; set; }
        //}
    }
