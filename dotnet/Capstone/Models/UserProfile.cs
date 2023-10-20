using System.ComponentModel.DataAnnotations;

namespace Capstone.Models
{
    public class UserProfile
    {
        [Required]
        [MaxLength(9)]
        [MinLength(5)]
        public string ZipCode { get; set; }
        [Required]
        [RegularExpression("(beginner|intermediate|advanced)")]
        public string SkillLevel { get; set; }
        [Required]
        [MaxLength(20)]
        public string FavoriteBird { get; set; }
        [Required]
        [MaxLength(20)]
        public string MostCommonBird { get; set; }
        public bool ProfileActive { get; set; }

    }

}
