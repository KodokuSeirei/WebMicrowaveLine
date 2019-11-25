using Microsoft.AspNetCore.Identity;

namespace WebMicrowaveLine.Models
{
    public class User : IdentityUser
    {
        
       
        public string FullName { get; set; }
        public string Adress { get; set; }
        public string Birthday { get; set; }
        public string RelaxRoomName { get; set; }
        public string MicrowaveName { get; set; }
        public int Position { get; set; } = 0;

    }
}
