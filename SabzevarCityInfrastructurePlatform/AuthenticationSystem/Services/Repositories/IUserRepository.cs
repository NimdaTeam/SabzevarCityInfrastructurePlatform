using _0_Framework.DTO;
using _0_Framework.GenericRepositoy.Interface;
using AuthenticationSystem.Domain.User;

namespace AuthenticationSystem.Services.Repositories
{
	public interface IUserRepository:IRepository<long,Users>
	{

        //Users LoginUser(LoginViewModel model);

        //List<AllUsersViewModel> GetAllUsersForGrid();

        Users GetUserByPhoneNumber(string phoneNumber);

    }
}
