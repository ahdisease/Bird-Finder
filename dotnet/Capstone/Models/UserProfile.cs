using System.ComponentModel.DataAnnotations;

namespace Capstone.Models
{
    public class UserProfile
    {
        [Required]
        [RegularExpression("^\\d{5}(?:[-\\s]\\d{4})?$",ErrorMessage ="Parameter zipCode must be a valid zip or zip+4 format.")]
        public string ZipCode { get; set; }
        [Required]
        [RegularExpression("(beginner|intermediate|advanced)",ErrorMessage = "Parameter skillLevel only accepts 'beginner', 'intermediate' and 'advanced' as parameters.")]
        public string SkillLevel { get; set; }
        [Required]
        [MaxLength(20,ErrorMessage = "Parameter favoriteBird must be 20 characters or less.")]
        public string FavoriteBird { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "Parameter mostCommonBird must be 20 characters or less.")]
        public string MostCommonBird { get; set; }
        public bool ProfileActive { get; set; }

    }

}
