using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApp.Models
{
    public enum Role { VISITOR, OWNER, TRAINER };
    public enum Gender { MALE, FEMALE }
    public class User:NativeObject
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public DateTime Birthday { get; set; }
        public Role Role { get; set; }
        public List<int> Visitor_TrainingsIDs { get; set; }
        public List<int> Trainer_TrainingsIDs { get; set; }
        public int Trainer_FitnessCenterID { get; set; }
        public List<int> Owner_FitnessCentersIDs { get; set; }
        
        public User()
        {
            Role = Role.VISITOR;
            Visitor_TrainingsIDs = new List<int>();
            Trainer_TrainingsIDs = new List<int>();
            Trainer_FitnessCenterID = -1;
            Owner_FitnessCentersIDs = new List<int>();
        }

    }
}