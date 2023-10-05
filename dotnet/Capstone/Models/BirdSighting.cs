using System;

namespace Capstone.Models
{
    public class BirdSighting
    {
        public int id {  get; set; }
        public int userId { get; set; }
        public int birdId {  get; set; }
        public DateTime dateSighted { get; set; }
    }


}
