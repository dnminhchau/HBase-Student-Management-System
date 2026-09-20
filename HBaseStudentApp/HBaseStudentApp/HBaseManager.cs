using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Linq; 
using System.Threading.Tasks;

namespace HBaseStudentApp
{
    public class HBaseManager
    {
        private readonly HttpClient client;
        private readonly string baseUrl = "http://YOUR_HBASE_SERVER_IP:8090/students";

        public HBaseManager()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private static string ToBase64(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
        }

        private static string FromBase64(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            return Encoding.UTF8.GetString(Convert.FromBase64String(str));
        }

        public async Task AddStudent(Student s)
        {
            var rowKeyBase64 = ToBase64(s.StudentID);

            var cells = new List<CellModel>
            {
                new CellModel { Column = ToBase64("info:name"), Value = ToBase64(s.Name) },
                new CellModel { Column = ToBase64("info:age"), Value = ToBase64(s.Age) },
                new CellModel { Column = ToBase64("info:email"), Value = ToBase64(s.Email) }
            };


            var cellSet = new CellSetModel
            {
                Rows = new List<RowModel>
                {
                    new RowModel
                    {
                        Key = rowKeyBase64,
                        Cells = cells
                    }
                }
            };

            var json = JsonSerializer.Serialize(cellSet);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{baseUrl}/{s.StudentID}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateStudent(Student s)
        {
            await AddStudent(s);
        }

        public async Task DeleteStudent(string id)
        {
            var response = await client.DeleteAsync($"{baseUrl}/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Student>> GetAllStudents()
        {
            var studentMap = new Dictionary<string, Student>();

            var scannerJson = new { batch = 100 }; 
            var scannerContent = new StringContent(JsonSerializer.Serialize(scannerJson), Encoding.UTF8, "application/json");

            var createScannerResp = await client.PostAsync(baseUrl + "/scanner", scannerContent);
            createScannerResp.EnsureSuccessStatusCode();

            if (createScannerResp.Headers.Location == null)
                throw new Exception("Scanner URL not returned by HBase.");

            string scannerUrl = createScannerResp.Headers.Location.ToString();

            try
            {
                while (true)
                {
                    var getResp = await client.GetAsync(scannerUrl);

                    if (getResp.StatusCode == System.Net.HttpStatusCode.NoContent)
                        break;

                    getResp.EnsureSuccessStatusCode();
                    var content = await getResp.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(content))
                        break;

                    using (JsonDocument doc = JsonDocument.Parse(content))
                    {
                        if (!doc.RootElement.TryGetProperty("Row", out JsonElement rows))
                            break;

                        foreach (var row in rows.EnumerateArray())
                        {
                            string keyBase64 = row.GetProperty("key").GetString();
                            string id = FromBase64(keyBase64);

                            if (!studentMap.ContainsKey(id))
                            {
                                studentMap[id] = new Student { StudentID = id };
                            }
                            var student = studentMap[id];

                            foreach (var cell in row.GetProperty("Cell").EnumerateArray())
                            {
                                string col = FromBase64(cell.GetProperty("column").GetString());
                                string val = FromBase64(cell.GetProperty("$").GetString());

                                switch (col)
                                {
                                    case "info:name":
                                        student.Name = val;
                                        break;
                                    case "info:age":
                                        student.Age = val;
                                        break;
                                    case "info:email":
                                        student.Email = val;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                await client.DeleteAsync(scannerUrl);
            }

            return studentMap.Values.ToList();
        }
    }
}