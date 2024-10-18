using _0_Framework.EntityBase;
using AuthenticationSystem.Domain.Role;

namespace AuthenticationSystem.Domain.User
{
    public class Users:EntityBase
    {
        public string PhoneNumber { get; set; }

        public string FullName { get; set; }



        #region Relations

        public virtual List<Roles> Roles { get; set; }

        public virtual List<UserRoles> UserRoles { get; set; }

        public virtual List<OTPCodes> OtpCodes { get; set; }

        #endregion
    }
}
