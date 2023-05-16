using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApp.Models
{
    public class Comment : NativeObject
    {
        public int VisitorID { get; set; }
        public string Text { get; set; }
        public int Mark { get; set; }
        public int FitnessCenterID { get; set; }
        public bool Visible { get; set; }

        public Comment()
        {
            VisitorID = FitnessCenterID = -1;
            Visible = false;
        }
    }
}