using System.Data;
using System.Data.SqlClient;
using Trainer_calender_version_2._0._0.Models;
namespace Trainer_calender_version_2._0._0.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;

        public UserService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public User Get(UserLogin userlogin)
        {
            //User user = UserRepository.Users.FirstOrDefault(o => o.UserName.Equals(userlogin.UserName, StringComparison.OrdinalIgnoreCase) && o.Password.Equals(userlogin.Password));
            //return user;
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            string query = "select * from trainer where username = '" + userlogin.UserName + "'and password = '" + userlogin.Password + "';";

            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int size = dt.Rows.Count;
            if(size>0)
            {
                User user = new User();
                user.UserName = userlogin.UserName;
                user.Password = userlogin.Password;
                user.Role = "Trainer";
                return user;
            }
            con.Close();
            query = "select * from admin where username = '" + userlogin.UserName + "'and password = '" + userlogin.Password + "';";

            cmd = new SqlCommand(query, con);
            con.Open();
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            size = dt.Rows.Count;
            if (size > 0)
            {
                User user = new User();
                user.UserName = userlogin.UserName;
                user.Password = userlogin.Password;
                user.Role = "Admin";
                return user;
            }
            return null;
        }
    }
}
