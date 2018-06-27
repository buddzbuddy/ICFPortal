using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CISSAPortal.Models
{
    public class CreateIPRViewModel
    {
        public IPR IPR { get; set; }
        public string[] TargetSelected { get; set; }
        public List<RehabilitationTargetToIPR> Targets { get; set; }
        public QuestionViewModel QuestionViewModel { get; set; }

        public List<AnswerTarget1> AnswerTargets1 { get; set; }
        public List<AnswerTarget2> AnswerTargets2 { get; set; }
        public List<AnswerTarget3> AnswerTargets3 { get; set; }
        public List<AnswerTarget4> AnswerTargets4 { get; set; }
        
        public List<SelectedTarget1> SelectedTargets1 { get; set; }
        public List<SelectedTarget2> SelectedTargets2 { get; set; }
        public List<SelectedTarget3> SelectedTargets3 { get; set; }
        public List<SelectedTarget4> SelectedTargets4 { get; set; }

        public List<ServiceType> ServiceTypes { get; set; }

        public List<SelectedService1> SelectedServices1 { get; set; }
        public List<SelectedService2> SelectedServices2 { get; set; }
        public List<SelectedService3> SelectedServices3 { get; set; }
        public List<SelectedService4> SelectedServices4 { get; set; }
    }
}