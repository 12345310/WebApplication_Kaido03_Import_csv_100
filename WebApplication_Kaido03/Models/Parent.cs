using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApplication_Kaido03.Models
{
    public class Parent
    {
            public int Id { get; set; }

            [Required]
　　        [DisplayName("親の名前")]
            public string Name { get; set; }

            public int SexId { get; set; }

            public virtual Sex Sex { get; set; }

            [Required]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }

            public virtual ICollection<Child> Children { get; set; }
    }

}
