using authentication_project.Extensions;
using System.ComponentModel;

namespace authentication_project.Enums
{
    public enum UserRoleEnum
    {
        [Description("Standart Kullanıcı")]
        user = 1,
        [Description("Takım Lideri")]
        teamleader = 2
    }
}
