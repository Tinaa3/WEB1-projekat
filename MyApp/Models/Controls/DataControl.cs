using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApp.Models
{
    public class DataControl
    {
        public static TemplateControl<User> Users { get; set; } = new TemplateControl<User>();
        public static TemplateControl<Comment> Comments { get; set; } = new TemplateControl<Comment>();
        public static TemplateControl<FitnessCenter> FitnessCenter { get; set; } = new TemplateControl<FitnessCenter>();
        public static TemplateControl<Training> Trainings { get; set; } = new TemplateControl<Training>();

    }
}