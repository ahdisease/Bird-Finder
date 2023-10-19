using System;

namespace Capstone.Models
{
    public class BirdNote
    {
        public int NoteId {  get; set; }
        public int BirdId {  get; set; }
        public DateTime DateSpotted { get; set; }
        public int NumMales { get; set; }   
        public int NumFemales { get; set; }
        public string FeederType {  get; set; }
        public string FoodBlend { get; set; }
        public string Notes {  get; set; }
        
    }


}
