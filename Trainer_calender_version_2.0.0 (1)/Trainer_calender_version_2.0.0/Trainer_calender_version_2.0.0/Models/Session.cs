namespace Trainer_calender_version_2._0._0.Models
{
    public class Session
    {
        public int SessionId { get; set; }
        public string SessionContent { get; set; } = null!;
        public int TrackId { get; set; }
        public int TrainerId { get; set; }
        public int LocationId { get; set; }
        public int BatchId { get; set; }
        public DateTime SessionStartTime { get; set; }
        public DateTime SessionEndTime { get; set; }
    }
}
