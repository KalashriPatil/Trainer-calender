using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using Trainer_calender_version_2._0._0.Models;

namespace Trainer_calender_version_2._0._0.Services
{
    public class AdminService : IAdminService
    {
        private readonly IConfiguration _configuration;

        public AdminService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<Trainer> GetAllTrainer()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("select * from trainer", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Trainer> TrainerList = new List<Trainer>();
            foreach (DataRow dr in dt.Rows)
            {
                Trainer u = new Trainer();
                u.TrainerId = int.Parse(dr["trainerId"].ToString());
                u.Name = dr["name"].ToString();
                u.AdminId = int.Parse(dr["adminId"].ToString());
                u.Designation = dr["designation"].ToString();

                TrainerList.Add(u);
            }
            con.Close();
            return TrainerList;
        }

        public Trainer GetTrainerById(int id)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("select * from trainer where trainerId = '" + id + "' ", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            //Trainer u = new Trainer();
            foreach (DataRow dr in dt.Rows)
            {
                Trainer u = new Trainer();
                u.TrainerId = int.Parse(dr["trainerId"].ToString());
                u.Name = dr["name"].ToString();
                u.AdminId = int.Parse(dr["adminId"].ToString());
                u.Designation = dr["designation"].ToString();
                return u;
            }
            con.Close();
            return null;
        }

        public List<Trainer> GetTrainersBySkill(int id)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand(String.Format("select * from trainer where trainerId in (select trainerId from Trainer_skill where skillId = {0}); ",id), con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Trainer> TrainerList = new List<Trainer>();
            foreach (DataRow dr in dt.Rows)
            {
                Trainer u = new Trainer();
                u.TrainerId = int.Parse(dr["trainerId"].ToString());
                u.Name = dr["name"].ToString();
                u.AdminId = int.Parse(dr["adminId"].ToString());
                u.Designation = dr["designation"].ToString();

                TrainerList.Add(u);
            }
            con.Close();
            return TrainerList;
        }
        public List<Skill> GetAllSkill()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("select * from skill;", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Skill> skillList = new List<Skill>();
            foreach (DataRow dr in dt.Rows)
            {
                Skill u = new Skill();
                u.Id = int.Parse(dr["skillId"].ToString());
                u.SkillName = dr["skillName"].ToString();
                
                skillList.Add(u);
            }
            con.Close();
            return skillList;
        }
        public List<Session> GetAllSession()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("select * from session;", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Session> SessionList = new List<Session>();
            foreach (DataRow dr in dt.Rows)
            {
                Session u = new Session();
                u.SessionId = int.Parse(dr["SessionId"].ToString());
                u.SessionContent = dr["SessionContent"].ToString();
                u.TrackId = int.Parse(dr["TrackId"].ToString());
                u.TrainerId = int.Parse(dr["TrainerId"].ToString());
                u.LocationId = int.Parse(dr["locationId"].ToString());
                u.BatchId = int.Parse(dr["BatchId"].ToString());
                u.SessionStartTime = Convert.ToDateTime(dr["SessionStartTime"].ToString());
                u.SessionEndTime = Convert.ToDateTime(dr["SessionendTime"].ToString());

                SessionList.Add(u);
            }
            con.Close();
            return SessionList;
        }
    }
}
