using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApplication_Kaido03.Models
{
    public class Child
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("名前")]
        public string Name { get; set; }

        public int SexId { get; set; }

        public virtual Sex Sex { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("誕生日")]
        public DateTime Birthday { get; set; }

        [Required]
        public int ParentId { get; set; }

        public virtual Parent Parent { get; set; }
    }
}