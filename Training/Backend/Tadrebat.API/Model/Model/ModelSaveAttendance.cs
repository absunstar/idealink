using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Model
{
    public class ModelSaveAttendance
    {
        public string trainingId { get; set; }
        public ModelAttendance Attendances { get; set; }
    }
    public class ModelAttendance
    {
        public ModelAttendance()
        {
            Attendances = new List<ModelAttendanceTrainee>();
        }
        public string SessionId { get; set; }
        public List<ModelAttendanceTrainee> Attendances { get; set; }
    }
    public class ModelAttendanceTrainee
    {
        public string TraineeId { get; set; }
        public bool IsAttendant { get; set; }
    }
}
