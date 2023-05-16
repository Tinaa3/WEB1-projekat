using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApp.Models
{
    public class FitnessCenter:NativeObject
    {
        public string Name { get; set; }
        public Address Address { get; set; }
        public int OpeningYear { get; set; }
        public int OwnerID { get; set; }
        public int MonthFee { get; set; }
        public int YearFee { get; set; }
        public int TrainingFee { get; set; }
        public int GroupFee { get; set; }
        public int PersonalFee { get; set; }

        public FitnessCenter()
        {
            OwnerID = -1;
            Address = new Address();
        }

        public FitnessCenter Clone()
        {
            return (FitnessCenter)this.MemberwiseClone();
        }
    }
}