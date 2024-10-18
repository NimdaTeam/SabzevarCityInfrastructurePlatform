using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.EntityBase;

namespace AuthenticationSystem.Domain.User
{
    public class OTPCodes : EntityBase
    {
        [ForeignKey("Users")]
        public long UserId { get; set; }
        public string PhoneNumber { get; set; }

        public string OTPCode { get; set; }

        public DateTime ExpireDate { get; set; }

        public bool IsActive { get; set; }


        #region Relations
        public virtual Users Users { get; set; }
        #endregion


    }
}
