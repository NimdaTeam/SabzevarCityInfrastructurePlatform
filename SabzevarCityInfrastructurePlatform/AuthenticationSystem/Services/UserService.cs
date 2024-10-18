using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.DTO;
using _0_Framework.GenericRepositoy.Service;
using AuthenticationSystem.Domain.User;
using AuthenticationSystem.Infrastructure;
using AuthenticationSystem.Services.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationSystem.Services
{
    public class UserService : RepositoryService<long, Users>, IUserRepository
    {
        private readonly AuthenticationSystemContext _context;

        public UserService(AuthenticationSystemContext context) : base(context)
        {
            _context = context;
        }


        //public List<AllUsersViewModel> GetAllUsersForGrid()
        //{
        //    var users = new List<AllUsersViewModel>();

        //    foreach (var user in _context.Users)
        //    {
        //        var stock = _context.Stocks.Find(user.StockId);

        //        users.Add(new AllUsersViewModel()
        //        {
        //            Id = user.Id,
        //            Username = user.Username,
        //            Stock = stock.Name,
        //            RoleId = user.RoleId
        //        });
        //    };

        //    return users;
        //}
        public Users GetUserByPhoneNumber(string phoneNumber)
        {
            return _context.Users.FirstOrDefault(x => x.PhoneNumber == phoneNumber);
        }
    }
}
