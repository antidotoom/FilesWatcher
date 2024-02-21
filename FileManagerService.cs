using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesWatcher
{
    public class FileManagerService
    {
        //Utworzenie właściwości zawierające zmienne pobrane przez Program z configu 
        public string sourcePath { get; }
        public string destPath { get; }
        public string logsPath { get; }

        public FileManagerService(string SourcePath, string DestPath, string LogsPath)
        {
            sourcePath = SourcePath;
            destPath = DestPath;
            logsPath = LogsPath;

        }
        //public event EventHandler FilesDetected;

        //Utworenie listy
        List<string> listOfFiles = new List<string>();

        public void ListFiles()
        {
 
            // Sprawdzenie, czy ścieżka źródłowa istnieje
            if (Directory.Exists(sourcePath))
            {
                // Pobranie listy plików w katalogu źródłowym
                string[] files = Directory.GetFiles(sourcePath);

                // Wyświetlenie nazw wszystkich plików
                Console.WriteLine("Files in source directory: ");
                foreach (string file in files)
                {
                    Console.WriteLine(file);
                    listOfFiles.Add(file);

                }
            }
            else
            {
                Console.WriteLine("The specified path does not exist.");
            }



        }
        //public void Display()
        //{
        //    Console.WriteLine("Wyswietlenie listy ListOfFiles");
        //    // Sprawdzenie, czy ścieżka źródłowa istnieje
        //    if (Directory.Exists(sourcePath))
        //    {
        //        // Wyświetlenie nazw wszystkich plików
        //        Console.WriteLine("Files in listOfFiles: ");
        //        foreach (string file in listOfFiles)
        //        {
        //            Console.WriteLine($"Plik: {file}");
        //        }
        //    }
        //}

        public void MoveFilesToDestination()
        {
            // Sprawdzenie, czy ścieżka docelowa istnieje
            if (!Directory.Exists(destPath))
            {
                // Jeśli ścieżka docelowa nie istnieje, utwórz ją
                Directory.CreateDirectory(destPath);
            }

            // Pobranie listy plików w katalogu źródłowym
            string[] files = Directory.GetFiles(sourcePath);

            // Przeniesienie każdego pliku do katalogu docelowego
            int n = 0;
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string destinationFilePath = Path.Combine(destPath, fileName);

                try
                {
                    // Spróbuj przenieść plik
                    File.Move(file, destinationFilePath);
                    Console.WriteLine($"The file '{fileName}' has been moved to the destination directory.");
                    Thread.Sleep(1000); // Opóźnienie o 5 sekund (2000 milisekund)

                    // Utwórz log z przesunięcia pliku
                    string fname = (fileName.Split('.'))[0]; //file zawiera rozszerzenie wiec wycinam je
                    string logFileName = $"{DateTime.Now:yyyyMMdd_HHmmss}_{fname}.log";
                    string logFilePath = Path.Combine(logsPath, logFileName);
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(logFilePath))
                        {
                            writer.WriteLine($"Log creation date: {DateTime.Now}");
                            writer.WriteLine("Source path for moved file:");
                            writer.WriteLine(file);
                        }
                        Console.WriteLine($"The log file has been created: {logFilePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred while saving the log file: {ex.Message}");
                    }
                 }
                 catch (Exception ex)
                 {
                     // Obsłuż błąd przenoszenia pliku
                     Console.WriteLine($"An error occurred while moving the file '{fileName}': {ex.Message}");
                 }
            }


            //WriteLogFile();
        }

        
    }
}
