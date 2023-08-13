    using CsvHelper;
using System.Globalization;

namespace ExportTool
{
    public class ExportService
    {
        private string _pathToDirectory { get; set; }
        private string _csvFileName { get; set; }
        public ExportService(string pathToDirectory, string csvFileName)
        {
            _pathToDirectory = pathToDirectory;
            _csvFileName = csvFileName;
        }

        public void ImportDBToCSV<T>(IEnumerable<T> persons)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(_pathToDirectory);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            string fullPath = GetFullPathToFile(_pathToDirectory, _csvFileName);
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
            _csvFileName = csvFileName;
            return ImportCSVToDB<T>();
        }
        public List<T> ImportCSVToDB<T>()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(_pathToDirectory);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            string fullPath = GetFullPathToFile(_pathToDirectory, _csvFileName);
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
    }
}