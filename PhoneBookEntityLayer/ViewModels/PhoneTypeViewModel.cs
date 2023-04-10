using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookEntityLayer.ViewModels
{
    public class PhoneTypeViewModel
    {
        public byte Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Name { get; set; }

    }
}
