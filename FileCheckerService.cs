using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//FilesWatcher
namespace FilesWatcher
{
    internal class FileCheckerService
    {
        private string sourcePath;
        private string destPath;
        private string logsPath;
        private FileSystemWatcher watcher;
        private bool isMonitoring;

        public event EventHandler FilesDetected;

        public FileCheckerService(string sourcePath, string destPath, string logsPath)
        {
            this.sourcePath = sourcePath;
            this.destPath = destPath;
            this.logsPath = logsPath;
            isMonitoring = false;

            // Utwórz nowy obiekt FileSystemWatcher
            watcher = new FileSystemWatcher(sourcePath);

            // Ustaw opcje monitorowania
            watcher.NotifyFilter = NotifyFilters.FileName;

            // Dodaj obsługę zdarzenia dla dodania nowego pliku
            watcher.Created += OnCreated;

            // Uruchom monitorowanie
            StartMonitoring();
        }

     

        private void OnCreated(object sender, FileSystemEventArgs obj)
        {
            // Sprawdź, czy dodano plik
            if (obj.ChangeType == WatcherChangeTypes.Created)
            {
                Console.WriteLine($"New file detected: {obj.FullPath}");
                onFilesDetected();
            }
        }
        public virtual void onFilesDetected()
        {
            // Wywołaj zdarzenie FilesDetected
            FilesDetected?.Invoke(this, EventArgs.Empty);
        }
        public void StartMonitoring()
        {
            // Rozpocznij monitorowanie, jeśli nie jest już włączone
            if (!isMonitoring)
            {
                watcher.EnableRaisingEvents = true;
                isMonitoring = true;
                Console.WriteLine($"Folder monitoring has started: {sourcePath}");
            }
        }

        public void StopMonitoring()
        {
            // Zatrzymaj monitorowanie, jeśli jest włączone
            if (isMonitoring)
            {
                watcher.EnableRaisingEvents = false;
                isMonitoring = false;
                Console.WriteLine($"Folder monitoring has stopped: {sourcePath}");
            }
        }
    }
}
