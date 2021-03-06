﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MES.Mvc.Models
{
    public class WorkOrderDetailsModels
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Workorder Id")]
        public int WorkOrderId { get; set; }
        [Required]
        [DisplayName("Workorder")]
        public string Number { get; set; }
        [DisplayName("Target Quantity")]
        public int? Quantity { get; set; }
        [DisplayName("Date")]
        public DateTime DateTime { get; set; }
        [DisplayName("Generated By Machine")]
        public string EntryThroughMachine { get; set; }
        [DisplayName("Front Line")]
        public string FrontLine { get; set; }
        [DisplayName("Reference")]
        public string Reference { get; set; }
        [DisplayName("Product Sequence")]
        public string UseProductSequence { get; set; }
        public string Machine { get; set; }
        public int MachineId { get; set; }
        [DisplayName("Qty Generated")]
        public int? GeneratedQty { get; set; }
        [DisplayName("Qty Processed")]
        public int? ProcessQty { get; set; }
        [DisplayName("Qty Pass")]
        public int? PassQty { get; set; }
        [DisplayName("Qty Fail")]
        public int? FailQty { get; set; }
        [DisplayName("Qty Pass After Dismantle")]
        public int? PosDismantlePassQty { get; set; }
        [DisplayName("Qty Fail  After Dismantle")]
        public int? PosDismantleFailQty { get; set; }
        [DisplayName("Qty Dismantle")]
        public int? DismantleQty { get; set; }
    }
}