using CsvHelper;
using System.Globalization;
using Newtonsoft.Json;

namespace ExportTool
{
    public class ExportService
    {
        private string _pathToDirectory { get; set; }
        private string _fileName { get; set; }
        public ExportService(string pathToDirectory, string fileName)
        {
            _pathToDirectory = pathToDirectory;
            _fileName = fileName;
        }

        public void ImportDBToCSV<T>(IEnumerable<T> persons)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(_pathToDirectory);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            string fullPath = GetFullPathToFile(_pathToDirectory, _fileName);
            using (FileStream fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    using (var writer = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                    {
                        writer.WriteRecords(persons);
                    }
                }
            }
        }
        //if it is necessary to change the csv file
        public IEnumerable<T> ImportCSVToDB<T>(string pathToDirectory, string csvFileName)
        {
            _pathToDirectory = pathToDirectory;
            _fileName = csvFileName;
            return ImportCSVToDB<T>();
        }
        public List<T> ImportCSVToDB<T>()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(_pathToDirectory);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            string fullPath = GetFullPathToFile(_pathToDirectory, _fileName);
            using (FileStream fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    using (var reader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                    {
                        return  reader.GetRecords<T>().ToList();
                    }
                }
            }
        }
        private string GetFullPathToFile(string pathToFile, string fileName)
        {
            return Path.Combine(pathToFile, fileName);
        }
        //Json serialized
        public async Task ExportDBToJson<T>(IEnumerable<T> persons)
        {
            string json = JsonConvert.SerializeObject(persons, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            string fullPath = GetFullPathToFile(_pathToDirectory, _fileName);

            using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter streamWriter = new(fileStream))
                {
                    await streamWriter.WriteAsync(json);
                }
            }
        }
        public async Task<List<T>> ImportJsonToDB<T>(string pathDirectory, string fileName)
        {
            _pathToDirectory = pathDirectory;
            _fileName = fileName;
            return await ImportJsonToDB<T>();
        }

        public async Task<List<T>> ImportJsonToDB<T>()
        {
            string fullPath = GetFullPathToFile(_pathToDirectory, _fileName);

            using (FileStream fileStream = new FileStream(fullPath, FileMode.Open))
            {
                using (StreamReader streamReader = new(fileStream))
                {
                    string json = await streamReader.ReadToEndAsync();
                    List<T> persons = JsonConvert.DeserializeObject<List<T>>(json);
                    return persons;
                }
            }

        }
    }
}