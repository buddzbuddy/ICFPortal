using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CISSAPortal.Models
{
    public class QuestionViewModel
    {
        public Question Question { get; set; }
        public List<Answer1> Answers1 { get; set; }
        public List<Answer2> Answers2 { get; set; }
        public List<Answer3> Answers3 { get; set; }
        public List<Answer4> Answers4 { get; set; }
    }
}