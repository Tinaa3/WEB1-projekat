using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApp.Models
{
    public abstract class NativeObject
    {
        public int ID { get; set; } = -1;
        public bool Deleted { get; set; } = false;
    }
}