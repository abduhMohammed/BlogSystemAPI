using Microsoft.VisualBasic;

namespace BlogSystemAPI.DTO
{
    public class AuthDTO
    {
        public string message { get; set; }

        public string username { get; set; }

        public bool IsAuthenticated { get; set; }

        public string Email{ get; set; }

        public List<string> Roles { get; set; }

        public string token { get; set; }

        public DateTime ExpiresOn{ get; set; }
    }
}
