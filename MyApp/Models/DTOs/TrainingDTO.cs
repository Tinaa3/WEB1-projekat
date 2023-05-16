using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApp.Models
{
    public class TrainingDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public FitnessCenter FitnessCenter { get; set; }
        public int DurationTime { get; set; }
        public string TrainingTime { get; set; }
        public int MaxVisitors { get; set; }
        public List<User> Visitors { get; set; } = new List<User>();

        public TrainingDTO(Training t)
        {
            ID = t.ID;
            Name = t.Name;
            Type = Enum.GetName(typeof(TrainingType), t.Type);
            FitnessCenter = DataControl.FitnessCenter.Get(t.FitnessCenterID);
            DurationTime = t.DurationTime;
            TrainingTime = t.TrainingTime.ToString("g");
            MaxVisitors = t.MaxVisitors;
            Visitors = DataControl.Users.Get().FindAll(u => t.VisitorsIDs.Contains(u.ID));
        }
    }
}