using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookEntityLayer.Entities
{
    public abstract class Base<T>
    {
        [Column(Order = 1)] //Tablodak 1.kolon ıd
        [Key]  //Idnin Primary key olmasını sağlar
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //identity(1,1) 1er 1 er artsın
        public T Id { get; set; }

        [Column(Order = 2)]
        public DateTime CreatedDate { get; set; }
        public bool IsRemoved { get; set; }
    }
}
