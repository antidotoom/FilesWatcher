using System;
using System.IO;
using Newtonsoft.Json; // Wymaga instalacji biblioteki Newtonsoft.Json z NuGet

//FilesWatcher
namespace FilesWatcher
{
    internal class FilesWatcher
    {
    
        //Pobranie pliku konfiguracyjnego
        private static string configFilePath = "config.json"; // Ścieżka względna do pliku konfiguracyjnego

        // Wczytaj zawartość pliku konfiguracyjnego
        private static string configJson = File.ReadAllText(configFilePath);

        // Deserializuj zawartość pliku JSON do obiektu klasy Config
        private static Config_FilesWatcher config = JsonConvert.DeserializeObject<Config_FilesWatcher>(configJson);

        // Użyj wczytanych wartości stałych 
        private static string sourcePath = config.SOURCE_PATH;
        private static string destPath = config.DEST_PATH;
        private static string logsPath = config.LOGS_PATH;
        public static void Main(string[] args)
        {
            Console.WriteLine("Hi! This is FilesWatcher application to monitor the source path. When new files are created in the source directory, FilesWatcher will move them to the target directory");
            Console.WriteLine("Please place the config.json configuration file in the directory with the executable file");
            Console.WriteLine("");
            Console.WriteLine("The paths have been configured : ");
            Console.WriteLine($"Source: {sourcePath}");
            Console.WriteLine($"Destination: {destPath}");
            Console.WriteLine($"Logs: {logsPath}");

            Console.WriteLine("");
            Console.WriteLine("Let's start monitoring....");


            // Utwórz instancję klasy FileCheckerService i przekaż ścieżkę folderu do monitorowania
            FileCheckerService fileCheckerService = new FileCheckerService(sourcePath, destPath, logsPath);
            
            // Zasubskrybuj zdarzenie FilesDetected
            fileCheckerService.FilesDetected += MyOrderWhenFilesDetected;

            // Dodanie klasy  FileManagerService
            FileManagerService fileManagerService = new FileManagerService(sourcePath, destPath, logsPath);
            fileManagerService.ListFiles();
            
            // Główna pętla programu
            Console.WriteLine("Press any key to finish.");
            Console.ReadKey();
            
        }
        
        private static void MyOrderWhenFilesDetected(object sender, EventArgs obj)
        {
            // Utworzenie instancji klasy FileManager
            FileManagerService fileManagerService = new FileManagerService(sourcePath, destPath, logsPath);

            // Wywołanie metody ListFiles() do listowania plików
            fileManagerService.ListFiles();

            // Przeniesienie plików do katalogu docelowego
            fileManagerService.MoveFilesToDestination();

            // Log
            //fileManagerService.WriteLogFile();

            //Kontynuacja monitorowania folderdu źródłowego
            Console.WriteLine("Let's continue monitoring... [or press any key to finish]");
            Console.WriteLine("");
        }
        

    }
}
