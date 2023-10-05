using System.ComponentModel.DataAnnotations;

namespace Capstone.Models
{
    public class UserProfile
    {
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string SkillLevel { get; set; }
        [Required]
        public int FavoriteBird { get; set; }
        [Required]
        public int MostCommonBird { get; set; }
        public bool ProfileActive { get; set; }

    }

}
