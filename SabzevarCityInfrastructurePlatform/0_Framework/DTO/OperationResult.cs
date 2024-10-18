using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0_Framework.DTO
{
    public class OperationResult
    {
        public bool IsSuccessful { get; set; }

        public Exception? Exception { get; set; }
    }

}
