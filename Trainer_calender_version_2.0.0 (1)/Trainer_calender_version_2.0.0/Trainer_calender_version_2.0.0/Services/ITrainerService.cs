using Microsoft.AspNetCore.Mvc;
using Trainer_calender_version_2._0._0.Models;

namespace Trainer_calender_version_2._0._0.Services
{
    public interface ITrainerService
    {
        public string AddSession(Session session);
        public string UpdateSession(int id, [FromBody] Session session);
        public string DeleteSession(int id);
    }
}
