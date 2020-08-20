using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApplication_Kaido03.Models
{
    public class Sex
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("性別")]
        public string Name { get; set; }

    }
}