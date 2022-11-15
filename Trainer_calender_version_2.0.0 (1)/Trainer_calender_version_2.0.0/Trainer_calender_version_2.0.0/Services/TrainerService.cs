using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Trainer_calender_version_2._0._0.Models;

namespace Trainer_calender_version_2._0._0.Services
{
    public class TrainerService : ITrainerService
    {
        private readonly IConfiguration _configuration;
        public TrainerService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string AddSession(Session session)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            SqlCommand cmd = new SqlCommand("Insert into session(sessionContent,trackId,trainerId,locationId,batchId,sessionStartTime,sessionEndTime) VALUES('" + session.SessionContent + "','" + session.TrackId + "','" + session.TrainerId + "','" + session.LocationId + "','" + session.BatchId + "','" + session.SessionStartTime + "','" + session.SessionEndTime + "')", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                return "Added new Calender";
            }
            else
            {
                return "Error";
            }
        }
        public string DeleteSession(int id)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("Delete From session Where sessionId='" + id + "' ", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                return "Deleted Calender";
            }
            else
            {
                return "Error";
            }
        }

        public string UpdateSession(int id, [FromBody] Session session)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("Update session set sessionContent='" + session.SessionContent + "',trackId='" + session.TrackId + "',trainerId='" + session.TrainerId + "',locationId='" + session.LocationId + "',batchId='" + session.BatchId + "',sessionStartTime='" + session.SessionStartTime + "',sessionEndTime='" + session.SessionEndTime + "' Where sessionId='" + id + "' ", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                return "Updated Calender";
            }
            else
            {
                return "Error";
            }
        }
        /*public string UpdateSkill(int id, string new_skill)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("Update session set sessionContent='" + session.SessionContent + "',trackId='" + session.TrackId + "',trainerId='" + session.TrainerId + "',locationId='" + session.LocationId + "',batchId='" + session.BatchId + "',sessionStartTime='" + session.SessionStartTime + "',sessionEndTime='" + session.SessionEndTime + "' Where sessionId='" + id + "' ", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                return "Updated Calender";
            }
            else
            {
                return "Error";
            }
        }*/
    }
}
