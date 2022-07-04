using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;

namespace Tadrebat.API.Model.Response
{
    public class ResponseImportTrainee
    {
        public ResponseImportTrainee()
        {
            IsValid = true;
        }
        public bool IsValid { get; set; }
        public string Error { get; set; }
        public string FileURL { get; set; }

        public void TraineeError(string strError, string strFileURL)
        {
            this.Error = strError;
            this.FileURL = strFileURL;
            this.IsValid = false;
        }
    }
}
