using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FTP
{
    public class FTPConnector
    {
        public event Action<string, long> OnGetFileInfo = delegate {  };
        public event Action<long, long> OnFileSizeLoaded = delegate { };

        private readonly string _login;
        private readonly string _password;
        private readonly string _defaultFTPFolder;

        public FTPConnector(string login, string password, string defaultFtpFolder)
        {
            _login = login;
            _password = password;
            _defaultFTPFolder = defaultFtpFolder;
        }

        public string GetFile(string path)
        {
            var response = GetResponse(path, WebRequestMethods.Ftp.DownloadFile);
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }
        }

        public void RemoveFile(string path)
        {
            GetResponse(path, WebRequestMethods.Ftp.DeleteFile);
        }

        public void MakeDirectory(string directory)
        {
            try
            {
                GetResponse(directory, WebRequestMethods.Ftp.MakeDirectory);
            }
            catch
            {
            }
        }

        public void LoadFile(string localPath, string nameFile)
        {
            DateTime? dateFtp = null;
            try
            {
                dateFtp = GetDateFile(nameFile);
            }
            catch
            {

            }

            var fi = new FileInfo(localPath);

            if (dateFtp != null && dateFtp.Value > fi.LastWriteTime.AddMilliseconds(-fi.LastWriteTime.Millisecond))
            {
                return;
            }

            var ftpRequest = (FtpWebRequest)WebRequest.Create(Path.Combine(_defaultFTPFolder, nameFile));

            ftpRequest.Credentials = new NetworkCredential(_login, _password);
            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
            ftpRequest.UseBinary = true;
            ftpRequest.KeepAlive = true;
            ftpRequest.ContentLength = fi.Length;

            var buffer = new byte[4096];
            int bytes = 0;
            int totalBytes = (int)fi.Length;

            using (var fs = fi.OpenRead())
            {
                using (var rs = ftpRequest.GetRequestStream())
                {
                    while (totalBytes > 0)
                    {
                        bytes = fs.Read(buffer, 0, buffer.Length);
                        rs.Write(buffer, 0, bytes);
                        totalBytes = totalBytes - bytes;
                    }
                }
            }
        }

        public long GetFileSize(string nameFile)
        {
            var response = GetResponse(nameFile, WebRequestMethods.Ftp.GetFileSize);
            return response.ContentLength;
        }

        public void DownLoadFile(string nameFile, string localPath)
        {
            DateTime? dateFtp = null;
            try
            {
                dateFtp = GetDateFile(nameFile);
            }
            catch
            {

            }

            if (File.Exists(localPath))
            {
                var fi = new FileInfo(localPath);

                //if (dateFtp != null && dateFtp.Value <= fi.LastWriteTime.AddMilliseconds(-fi.LastWriteTime.Millisecond))
                //{
                //    return;
                //}
            }

            long fileSize = GetFileSize(nameFile);

            if (fileSize == 0)
                return;

            OnGetFileInfo(Path.GetFileName(nameFile), fileSize);

            int countRetries = 1;
            while (countRetries <= 5)
            {
                try
                {
                    var ftpRequest = (FtpWebRequest)WebRequest.Create(Path.Combine(_defaultFTPFolder, nameFile));

                    ftpRequest.Credentials = new NetworkCredential(_login, _password);
                    ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                    ftpRequest.UseBinary = true;
                    ftpRequest.KeepAlive = true;
                    ftpRequest.Timeout = Timeout.Infinite;

                    const int countBytes = 1024 * 32;
                    var buffer = new byte[countBytes];

                    using (var writer = new FileStream(localPath, FileMode.Create))
                    {
                        using (var rs = ftpRequest.GetResponse().GetResponseStream())
                        {
                            int bytes;
                            int loadedBytes = 0;
                            do
                            {
                                bytes = rs.Read(buffer, 0, buffer.Length);
                                writer.Write(buffer, 0, bytes);
                                loadedBytes += bytes;

                                OnFileSizeLoaded(loadedBytes, fileSize);

                                Thread.Sleep(50 + (countRetries - 1) * 25);
                            } while (bytes == countBytes);
                            writer.Flush();
                        }
                    }
                }
                catch
                {
                    Debug.WriteLine("1) " + nameFile);
                    countRetries++;
                    Thread.Sleep(100);
                    continue;
                }

                var fi = new FileInfo(localPath);
                if (fileSize != fi.Length)
                {
                    Debug.WriteLine("2) " + nameFile);
                    countRetries++;
                    Thread.Sleep(100);
                    continue;
                }

                break;
            }

            if (countRetries == 6)
            {
                throw new Exception();
            }
        }

        public DateTime GetDateFile(string path)
        {
            var response = GetResponse(path, WebRequestMethods.Ftp.GetDateTimestamp);
            return response.LastModified;
        }

        private void LoadDirectoryRecurcive(string fullPathDirectory, string nameDirectory, string nameBaseDirectory, int lengthPathBase)
        {
            MakeDirectory(nameDirectory);

            var files = Directory.GetFiles(fullPathDirectory);
            var directories = Directory.GetDirectories(fullPathDirectory);

            var filesFtp = GetFiles(nameDirectory);
            var exceptFiles = filesFtp.Except(files.Select(Path.GetFileName));
            foreach (var exceptFile in exceptFiles)
            {
                RemoveFile(Path.Combine(nameDirectory, exceptFile));
            }

            var directoriesFtp = GetDirectories(nameDirectory);
            var exceptDirectories = directoriesFtp.Except(directories.Select(Path.GetFileName));
            foreach (var exceptDirectory in exceptDirectories)
            {
                RemoveDirectory(Path.Combine(nameDirectory, exceptDirectory));
            }

            foreach (var file in files)
            {
                string fileName = Path.Combine(nameBaseDirectory, file.Substring(lengthPathBase + 1));
                LoadFile(file, fileName);
            }

            foreach (var directory in directories)
            {
                string name = Path.Combine(nameBaseDirectory, directory.Substring(lengthPathBase + 1));
                LoadDirectoryRecurcive(directory, name, nameBaseDirectory, lengthPathBase);
            }
        }

        public void LoadDirectory(string fullPathDirectory, string nameDirectory)
        {
            LoadDirectoryRecurcive(fullPathDirectory, nameDirectory, nameDirectory, fullPathDirectory.Length);
        }

        public void DownloadDirectory(string ftpPath, string localPath)
        {
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }

            var filesFtp = GetFiles(ftpPath);
            var directoriesFtp = GetDirectories(ftpPath);

            foreach (var file in filesFtp)
            {
                string fileLocal = Path.Combine(localPath, file);
                DownLoadFile(Path.Combine(ftpPath, file), fileLocal);
            }

            foreach (var directory in directoriesFtp)
            {
                string directoryName = Path.Combine(ftpPath, directory);
                string name = Path.Combine(localPath, directory);
                DownloadDirectory(directoryName, name);
            }
        }

        public void RemoveDirectory(string directory)
        {
            var response = GetResponse(directory, WebRequestMethods.Ftp.ListDirectory);

            var files = new List<string>();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                while (!streamReader.EndOfStream)
                {
                    string file = Path.GetFileName(streamReader.ReadLine());
                    if (file != "." && file != ".." && !file.EndsWith("/.") && !file.EndsWith("/.."))
                    {
                        files.Add(Path.Combine(directory, file));
                    }
                }
            }

            foreach (var file in files)
            {
                if (IsFile(file))
                {
                    RemoveFile(file);
                    continue;
                }

                RemoveDirectory(file);
            }

            RemoveEmptyDirectory(directory);
        }

        public bool IsFile(string file)
        {
            //var response = GetResponse(file, WebRequestMethods.Ftp.ListDirectory);

            //using (var streamReader = new StreamReader(response.GetResponseStream()))
            //{
            //    while (!streamReader.EndOfStream)
            //    {
            //        string path = streamReader.ReadLine();
            //        if (path == "." || path.EndsWith("/."))
            //        {
            //            return false;
            //        }
            //    }
            //}

            //return true;

            return Path.GetExtension(file) != "";
        }

        private void RemoveEmptyDirectory(string directory)
        {
            GetResponse(directory, WebRequestMethods.Ftp.RemoveDirectory);
        }

        private FtpWebResponse GetResponse(string path, string methodFtp)
        {
            var ftpRequest = (FtpWebRequest)WebRequest.Create(Path.Combine(_defaultFTPFolder, path));

            ftpRequest.Credentials = new NetworkCredential(_login, _password);
            ftpRequest.Method = methodFtp;
            return (FtpWebResponse)ftpRequest.GetResponse();
        }

        private List<string> GetFiles(string directory)
        {
            var files = new List<string>();

            var response = GetResponse(directory, WebRequestMethods.Ftp.ListDirectory);
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                while (!streamReader.EndOfStream)
                {
                    string path = Path.GetFileName(streamReader.ReadLine());
                    if (path == "." || path.EndsWith("/.") || path == ".." || path.EndsWith("/.."))
                    {
                        continue;
                    }

                    files.Add(path);
                }
            }

            files = files.Where(file => IsFile(Path.Combine(directory, file))).ToList();

            return files;
        }

        private List<string> GetDirectories(string directory)
        {
            var directories = new List<string>();

            var response = GetResponse(directory, WebRequestMethods.Ftp.ListDirectory);
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                while (!streamReader.EndOfStream)
                {
                    string path = Path.GetFileName(streamReader.ReadLine());
                    if (path == "." || path.EndsWith("/.") || path == ".." || path.EndsWith("/.."))
                    {
                        continue;
                    }

                    directories.Add(path);
                }
            }

            directories = directories.Where(dir => !IsFile(Path.Combine(directory, dir))).ToList();

            return directories;
        }
    }
}
