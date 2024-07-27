using Microsoft.AspNetCore.Identity;

namespace GR30321.Alexandrov.UI.Models
{
    public class AppUser:IdentityUser
    {
        //сможем дописать свои поля
        public byte[] Avatar { get; set; }
        public string MimeType { get; set; } = string.Empty;  //мультимедиа тип

    }
}
