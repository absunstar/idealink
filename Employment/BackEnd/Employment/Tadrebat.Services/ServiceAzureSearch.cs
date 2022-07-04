using Employment.Interface;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Employment.Entity.Mongo;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;
using Employment.MongoDB.Interface;

namespace Employment.Services
{
    public class ServiceAzureSearch 
    {
        protected readonly IDBJob _dBJobs;
        protected readonly ICacheConfig _BLCacheConfig;


        public ServiceAzureSearch(IDBJob dBJobs, ICacheConfig cacheConfig)
        {
            _dBJobs = dBJobs;
            _BLCacheConfig = cacheConfig;
        }


        /// <summary>
        /// 
        /// </summary>
        string ApiKey { get; set; }
        string ServiceName { get; set; }
        string IndexName { get; set; }

        string ErrorText = "";

        StringBuilder strError = new StringBuilder();
        public int errorcount;
        public int successcount;

        protected static IMongoClient _client;
        protected static IMongoDatabase _database;
        static string logFilePath = string.Empty;
        public static string ProcesslogPath = string.Empty;

        //SS -Added for Insert Update data in Azure Index 

        public async Task<bool> UploadDataOnIndex(string Id)
        {
            bool IsComplted = true;
            try
            {

                var result = "";
                List<pusIndexJob> pusIndexJob = new List<pusIndexJob>();

                ServiceName = _BLCacheConfig.ServiceName;
                IndexName = _BLCacheConfig.IndexName;
                ApiKey = _BLCacheConfig.AdminApiKey;
                string IndexPath = "AzureSearch";
                logFilePath = Convert.ToString(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)) + "AzureIndex/Errorlog" + "/" + "Errorlog-" + DateTime.Now.ToString("yyyyMMddhhmm") + "." + "txt";
                string ProcesslogPath = Convert.ToString(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)) + "/" + "AzureIndex/ProcessLog" + "/" + "ProcessErrorlog-" + DateTime.Now.ToString("MMddyyyyhhmm") + "." + "txt";

                //_client = new MongoClient("mongodb+srv://mongodbuser:Idea%402021@tadrebat.nayx3.mongodb.net/Tadrebat?authSource=admin&replicaSet=atlas-tn2eb0-shard-0&w=majority&readPreference=primary&appname=MongoDB%20Compass&retryWrites=true&ssl=true");
                //_database = _client.GetDatabase("EmployementTestingDB");
                //var mongoCollection = _database.GetCollection<Job>("Job");
                //var documents = mongoCollection.AsQueryable();
                //var obj = documents.ToList();
                //use exsiting db connection

                var filter = Builders<Job>.Filter.Where(x => x._id == Id);
                var obj = await _dBJobs.GetPaged(filter, null, 1, int.MaxValue);

                for (int i = 0; i < obj.lstResult.Count; i++)
                {
                    var jsonData = new pusIndexJob();
                    jsonData.id = obj.lstResult[i]._id;
                    jsonData.Name = obj.lstResult[i].Name.Trim();
                    jsonData.Company = obj.lstResult[i].Company.Name.Trim();
                    jsonData.JobField = obj.lstResult[i].JobField.Name;
                    jsonData.JobSubField = obj.lstResult[i].JobSubField.Name;
                    jsonData.Experience = obj.lstResult[i].Experience.Name;
                    jsonData.Industry = obj.lstResult[i].Industry.Name;
                    jsonData.Qualification = obj.lstResult[i].Qualification.Name;
                    jsonData.Country = obj.lstResult[i].Country.Name;
                    jsonData.City = obj.lstResult[i].City.Name;
                    jsonData.search_action = "mergeOrUpload";
                    pusIndexJob.Add(jsonData);
                }
                string json = JsonConvert.SerializeObject(pusIndexJob);
                //AzureIndexer index = new AzureIndexer(ServiceName, IndexName, AdminApiKey);
                //var ApiIndexDef = index.GetIndex();
                var ApiIndexDef = GetIndex();
                if (ApiIndexDef == "The remote server returned an error: (404) Not Found.")
                {
                    result = CreateIndex(System.IO.File.ReadAllText(System.IO.Path.Combine(Directory.GetCurrentDirectory() + "/" + IndexPath, "CreateIndex.json")), ProcesslogPath);

                }
                else
                {
                    result = CreateIndex(ApiIndexDef, ProcesslogPath);

                }

                if (result == "success")
                {
                    UploadSingle(json, logFilePath);
                }
            }
            catch (Exception ex)
            {
                IsComplted = false;
                string error = "Program/" + "Main/n | Error Msg=" + ex.Message + "| DateTime: " + DateTime.Now.ToString("MM-dd-yyyy hh mm");
                //Logging.WriteSingleLog(error, ProcesslogPath);
            }

            return IsComplted;
        }



        /// <summary>
        /// To upload Crawled document one at a time
        /// </summary>
        /// <param name="Jdata">Json data to be uploaded</param>
        /// <param name="FilePath">LogfilePath</param>
        public void UploadSingle(string Jdata, string FilePath)
        {
            logFilePath = FilePath;
            StringBuilder data = new StringBuilder();

            data.Append("{\"value\":");
            data.Append(Jdata);
            data.Append("}");


            PostData(data.ToString());

        }
        /// <summary>
        /// To Create Index in Azure
        /// </summary>
        /// <param name="json">Json Index with Indexfileds and scoringprofile</param>
        public string CreateIndex(string json, string ProcessLogPath)
        {
            var Indexresult = "";
            try
            {
                dynamic deserialized = JsonConvert.DeserializeObject<dynamic>(json);
                deserialized["name"].Value = IndexName;
                json = JsonConvert.SerializeObject(deserialized);
                byte[] dataStream = Encoding.UTF8.GetBytes(json.ToString());
                WebRequest request = WebRequest.Create($"https://{ServiceName}.search.windows.net/indexes/{IndexName}?api-version=2020-06-30");
                request.ContentType = "application/json";
                request.Headers["api-key"] = ApiKey;
                request.Method = "PUT";

                request.ContentLength = dataStream.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(dataStream, 0, dataStream.Length);
                newStream.Close();
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                string result = reader.ReadToEnd();
                Indexresult = "success";
            }
            catch (WebException ex)
            {
                Indexresult = "error";
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    string error = "AzureIndexer/ " + "CreateIndex| Error Msg=" + reader.ReadToEnd() + "| DateTime: " + DateTime.Now.ToString("MM-dd-yyyy hh mm");
                    //Logging.WriteSingleLog(error, ProcessLogPath);
                }
            }
            return Indexresult;
        }

        /// <summary>
        /// To Post the Url Data on Azure Index
        /// </summary>
        /// <param name="data">Data to be Uploaded</param>
        /// <returns></returns>
        string PostData(string data)
        {
            try
            {

                data = data.Replace("search_action", "@search.action");

                byte[] dataStream = Encoding.UTF8.GetBytes(data.ToString());
                WebRequest request = WebRequest.Create($"https://{ServiceName}.search.windows.net/indexes/{IndexName}/docs/index?api-version=2020-06-30");
                request.ContentType = "application/json";
                request.Headers["api-key"] = ApiKey;
                request.Method = "POST";

                request.ContentLength = dataStream.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(dataStream, 0, dataStream.Length);
                newStream.Close();
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                string result = reader.ReadToEnd();
                var json = JObject.Parse((result));
                var data1 = json["value"];

                for (int i = 0; i < data1.Count(); i++)
                {
                    string StatusCode = Convert.ToString(((JValue)((data1[i]["statusCode"]))).Value);
                    if (StatusCode != "200" && StatusCode != "201")
                    {
                        ErrorText = "PostData; Url :" + "Result : Error | Error Message: " + Convert.ToString(((JValue)((data1[i]["errorMessage"]))).Value) + " | StatusCode: " + Convert.ToString(((JValue)((data1[i]["statusCode"]))).Value) + " | DateTime: " + System.DateTime.Now.ToString("MM - dd - yyyy hh mm");
                        //Logging.WriteSingleLog(ErrorText, logFilePath);
                        errorcount = errorcount + 1;
                        Console.WriteLine("Data n  Push Unsuccessfully");
                    }
                    else
                    {
                        ErrorText = "PostData; Url :" + "Result : Success | Success Message: " + Convert.ToString(((JValue)((data1[i]["errorMessage"]))).Value) + " | StatusCode: " + Convert.ToString(((JValue)((data1[i]["statusCode"]))).Value) + " | DateTime: " + System.DateTime.Now.ToString("MM - dd - yyyy hh mm");                        //strError.AppendLine(ErrorText);
                        //Logging.WriteSingleLog(ErrorText, logFilePath);
                        successcount = successcount + 1;
                        Console.WriteLine("Data Push Successfully");
                    }
                }
                return result;
            }
            catch (WebException ex)
            {

                string error = "";
                if (ex.Response == null)
                {
                    ErrorText = "Error Posting Data for | Error Message: " + ex.Message + " | DateTime: " + System.DateTime.Now.ToString("MM-dd-yyyy hh mm");
                    error = ex.Message;
                    //Logging.WriteSingleLog(ErrorText, logFilePath);
                    errorcount = errorcount + 1;
                    return error;
                }
                else
                {
                    using (var stream = ex.Response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            error = reader.ReadToEnd();
                            ErrorText = "Error Posting Data | Error Message: " + error + "| DateTime: " + System.DateTime.Now.ToString("MM-dd-yyyy hh mm");
                            //Logging.WriteSingleLog(ErrorText, logFilePath);
                            errorcount = errorcount + 1;
                            return error;
                        }
                    }
                }


            }
        }
        /// <summary>
        /// To Update Synonyms for Index
        /// </summary>
        /// <param name="jsonSynonyms">json to add synonyms</param>
        public void AddUpdateSynonyms(string jsonSynonyms, string ProcesslogPath)
        {
            try
            {

                byte[] dataStream = Encoding.UTF8.GetBytes(jsonSynonyms.ToString());
                WebRequest request = WebRequest.Create($"https://{ServiceName}.search.windows.net/synonymmaps/cardsrule?api-version=2019-05-06");
                request.ContentType = "application/json";
                request.Headers["api-key"] = ApiKey;
                request.Method = "PUT";

                request.ContentLength = dataStream.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(dataStream, 0, dataStream.Length);
                newStream.Close();
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                string result = reader.ReadToEnd();
            }
            catch (WebException ex)
            {
                string error = "";
                if (ex.Response == null)
                {

                    error = ex.Message;
                }
                else
                {
                    using (var stream = ex.Response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            error = "AzureIndexer/" + "AddUpdateSynonyms/|Error Msg=" + reader.ReadToEnd() + "| DateTime: " + System.DateTime.Now.ToString("MM - dd - yyyy hh mm");
                            //Logging.WriteSingleLog(error, ProcesslogPath);

                        }
                    }
                }
            }
        }

        public string GetIndex()
        {

            string result = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create($"https://" + ServiceName + ".search.windows.net/indexes/" + IndexName + "?api-version=2020-06-30");
                request.ContentType = "application/json";
                request.Headers["api-key"] = ApiKey;
                request.Method = "GET";
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                result = reader.ReadToEnd();
            }
            catch (WebException ex)
            {
                string error = "";
                ErrorText = "Error in GetIndex Data " + "| Error Message: " + ex.Message + " | DateTime: " + System.DateTime.Now.ToString("MM-dd-yyyy hh mm");
                error = ex.Message;
                //Logging.WriteSingleLog(ErrorText, logFilePath);
                errorcount = errorcount + 1;
                return error;

            }
            return result;
        }

        public string GetSynonyms(string Synonymname)
        {

            string result = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create($"https://" + ServiceName + ".search.windows.net/synonymmaps/" + Synonymname + "?api-version=2019-05-06");
                request.ContentType = "application/json";
                request.Headers["api-key"] = ApiKey;
                request.Method = "GET";
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                result = reader.ReadToEnd();
            }
            catch (WebException ex)
            {
                string error = "";
                ErrorText = "Error In GetSynonyms Data " + "| Error Message: " + ex.Message + " | DateTime: " + System.DateTime.Now.ToString("MM-dd-yyyy hh mm");
                error = ex.Message;
                //Logging.WriteSingleLog(ErrorText, logFilePath);
                errorcount = errorcount + 1;
                return error;

            }
            return result;


        }


    }

    //Data Member

    //public class Jobs
    //{
    //    public ObjectId _id { get; set; }
    //    public DateTime CreatedAt { get; set; }
    //    public Boolean IsActive { get; set; }
    //    public string Name { get; set; }
    //    public string Name2 { get; set; }

    //    public Comapnay Company { get; set; }
    //    public string Description { get; set; }
    //    public string Skills { get; set; }
    //    public string Benefits { get; set; }

    //    public DateTime Deadline { get; set; }
    //    public JobField JobField { get; set; }
    //    public JobSubField JobSubField { get; set; }
    //    public int type { get; set; }
    //    public Experience Experience { get; set; }
    //    public Industry Industry { get; set; }
    //    public Qualification Qualification { get; set; }
    //    public Country Country { get; set; }
    //    public City City { get; set; }

    //    public int Gender { get; set; }
    //    public int Status { get; set; }
    //    public string Address { get; set; }
    //    public int ApplicantCount { get; set; }

    //    public string Remuneration { get; set; }

    //}
    public class RootObject
    {
        public pusIndexJob[] property { get; set; }
    }
    public class pusIndexJob
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string JobField { get; set; }
        public string JobSubField { get; set; }
        public string Experience { get; set; }
        public string Industry { get; set; }
        public string Qualification { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string search_action { get; set; }
    }
    //public class Comapnay
    //{
    //    public ObjectId _id { get; set; }
    //    public string Name { get; set; }
    //    public string URL { get; set; }
    //}
    //public class JobField
    //{
    //    public ObjectId _id { get; set; }
    //    public string Name { get; set; }
    //}
    //public class JobSubField
    //{
    //    public ObjectId _id { get; set; }
    //    public string Name { get; set; }
    //}
    //public class Experience
    //{
    //    public ObjectId _id { get; set; }
    //    public string Name { get; set; }
    //}
    //public class Industry
    //{
    //    public ObjectId _id { get; set; }
    //    public string Name { get; set; }
    //}
    //public class Qualification
    //{
    //    public ObjectId _id { get; set; }
    //    public string Name { get; set; }
    //}
    //public class Country
    //{
    //    public ObjectId _id { get; set; }
    //    public string Name { get; set; }
    //}
    //public class City
    //{
    //    public ObjectId _id { get; set; }
    //    public string Name { get; set; }
    //}
}
