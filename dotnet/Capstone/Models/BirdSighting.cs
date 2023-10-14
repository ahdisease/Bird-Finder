using System;

namespace Capstone.Models
{
    public class BirdSighting
    {
        public int Id {  get; set; }
        public int BirdId {  get; set; }
        public DateTime DateSighted { get; set; }
        public int MalesSpotted { get; set; }   
        public int FemalesSpotted { get; set; }
        public string FeederType {  get; set; }
        public string FoodBlend { get; set; }
        public string Notes {  get; set; }
        
    }


}
