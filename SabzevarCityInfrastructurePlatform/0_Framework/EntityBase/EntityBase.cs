using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0_Framework.EntityBase
{
    public class EntityBase
    {
        [Key]
        public long Id { get; set; }

        public DateTime CreationDate { get; set; }


        public EntityBase()
        {
            CreationDate = DateTime.Now;
        }
    }
}
