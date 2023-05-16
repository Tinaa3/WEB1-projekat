using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApp.Models
{
    public class CommentDTO
    {
        public int ID { get; set; }
        public User Visitor { get; set; }
        public string Text { get; set; }
        public int Mark { get; set; }
        public FitnessCenter FitnessCenter { get; set; }
        public CommentDTO(Comment c)
        {
            ID = c.ID;
            Visitor = DataControl.Users.Get(c.VisitorID);
            Text = c.Text;
            Mark = c.Mark;
            FitnessCenter = DataControl.FitnessCenter.Get(c.FitnessCenterID);
        }
    }
}