using Trainer_calender_version_2._0._0.Models;

namespace Trainer_calender_version_2._0._0.Services
{
    public interface IAdminService
    {
        public List<Trainer> GetAllTrainer();
        public Trainer GetTrainerById(int id);
        public List<Trainer> GetTrainersBySkill(int id);
        public List<Skill> GetAllSkill();
        public List<Session> GetAllSession();
    }
}
