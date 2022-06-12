// Copyrights(c) Charqe.io. All rights reserved.

namespace Domain.DTOs
{
    public class LoginResponseDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
    }
}