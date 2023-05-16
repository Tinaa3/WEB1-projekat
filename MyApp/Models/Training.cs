using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApp.Models
{
    public enum TrainingType { YOGA, LESMILLSTONE, BODYPUMP, BODYATTACK }
    public class Training:NativeObject
    {

        public string Name { get; set; }
        public TrainingType Type { get; set; }
        public int FitnessCenterID { get; set; }
        public int DurationTime { get; set; }
        public DateTime TrainingTime { get; set; }
        public int MaxVisitors { get; set; }
        public List<int> VisitorsIDs { get; set; }

        public Training()
        {
            FitnessCenterID = -1;
            MaxVisitors = -1;
            VisitorsIDs = new List<int>();
        }

        public Training Clone()
        {
            return (Training)this.MemberwiseClone();
        }
    }
}