using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CISSAPortal.Models
{
    public class DiagramViewModel
    {
        [Display(Name = "С")]
        public DateTime? From { get; set; }
        [Display(Name = "По")]
        public DateTime? To { get; set; }
        [Display(Name = "Область")]
        public int? AreaId { get; set; }

        [Display(Name = "Поиск:")]
        public EntityType? EntityType { get; set; }// = EntityType.Certificate;

        public List<BarDiagramItem> BarDiagramItems { get; set; } = new List<BarDiagramItem>();
        public List<PieDiagramItem> PieDiagramItems { get; set; } = new List<PieDiagramItem>();
    }

    public enum EntityType
    {
        Produt,
        Certificate
    }


    public class BarDiagramItem
    {
        public string CategoryName { get; set; }
        public int Received { get; set; }
        public int Issued { get; set; }

        private int? _Balance = null;
        public int Balance
        {
            get
            {
                return _Balance != null ? _Balance.Value : Received - Issued;
            }
            set { _Balance = value; }
        }
    }

    public class PieDiagramItem
    {
        public string Name { get; set; }
        public int Amount { get; set; }
    }
}