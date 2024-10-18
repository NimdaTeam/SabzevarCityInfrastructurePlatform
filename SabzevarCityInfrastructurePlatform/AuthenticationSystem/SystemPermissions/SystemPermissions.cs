using _0_Framework.DTO;

namespace AuthenticationSystem.SystemPermissions
{
    public static class SystemPermissions
    {
        public static List<PermissionViewModel> UserManagementPermissions = new List<PermissionViewModel>()
        {
            
            new PermissionViewModel("مشاهده لیست کاربران" , 1),
            new PermissionViewModel("اضافه کردن کاربر جدید",2),
            new PermissionViewModel("ویرایش کاربر", 3),
            new PermissionViewModel("حذف کاربر",4),
        };

        public static List<PermissionViewModel> RoleManagementPermissions = new List<PermissionViewModel>()
        {
            new PermissionViewModel("مشاهده لیست سطوح دسترسی" , 11),
            new PermissionViewModel("اضافه کردن سطح دسترسی جدید",12),
            new PermissionViewModel("ویرایش سطح دسترسی",13),
            new PermissionViewModel("حذف سطح دسترسی" , 14)
        };


        public static List<PermissionGroupViewModel> AllSystemPermissions = new List<PermissionGroupViewModel>()
        {
            new PermissionGroupViewModel("مدیریت کاربران", UserManagementPermissions),
            new PermissionGroupViewModel("مدیریت سطوح دسترسی" , RoleManagementPermissions)
        };
    }

}
