using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TransactionModel;
using TransactionModel.Utils;
using System.Threading;

namespace ReconciliationFileReader
{
    public class Reader
    {
        private FileSystemWatcher watcher;
        private string inPath;
        private string successPath;
        private string errPath;
        private int maxSize;

        public Reader(FileSystemWatcher watcher, string basePath)
        {
            try
            {
                this.inPath = basePath + "In\\";
                this.successPath = basePath + "Success\\";
                this.errPath = basePath + "Err\\";
                this.maxSize = 1000000 * Configuration.GetConfiguration("RECONCILIATION", "RAW_FILE_MAXSIZE").Value1.ToInt();

                ValidateDirectory(basePath);

                this.watcher = watcher;
                this.watcher.Path = inPath;
                this.watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Attributes
                    | NotifyFilters.LastAccess | NotifyFilters.LastWrite |
                        NotifyFilters.Security | NotifyFilters.Size;

                this.watcher.Created += new FileSystemEventHandler(OnFileEvent);
                this.watcher.IncludeSubdirectories = true;
            }
            catch (Exception ex)
            {
                ErrorLog.Log("System", ex, SystemError.ServiceReader);
            }
        }

        private void ValidateDirectory(string basePath)
        {
            if (!Directory.Exists(basePath))
            {
                throw new Exception("Directory " + basePath + " not found");
            }
            if (!Directory.Exists(inPath))
            {
                throw new Exception("Directory " + inPath + " not found");
            }
            if (!Directory.Exists(successPath))
            {
                throw new Exception("Directory " + successPath + " not found");
            }
            if (!Directory.Exists(errPath))
            {
                throw new Exception("Directory " + errPath + " not found");
            }
        }

        public void StartReading()
        {
            watcher.EnableRaisingEvents = true;
        }

        public void StopReading()
        {
            watcher.EnableRaisingEvents = false;
        }

        public void CreateReconciliationFile(string sourcePath)
        {
            try
            {
                using (var container = new TransactionModelContainer())
                {
                    var fileName = GetFileName(sourcePath);
                    var fileinfo = new FileInfo(fileName);
                    var contents = ReadFile(sourcePath);

                    if (!IsValidFile(fileinfo))
                    {
                        var newFileName = MoveFileToFolder(container, errPath, sourcePath, fileinfo);
                        container.ReconciliationFiles.AddObject(new ReconciliationFile
                        {
                            FileName = fileinfo.Name,
                            Contents = contents,
                            CreateDate = DateTime.Now,
                            CreateBy = "System",
                            BackupPath = errPath + newFileName,
                            IsValid = false,
                            Source = null,
                            IsRead = false,
                            FileType = FileType.E.ToString(),
                        });
                    }
                    else
                    {
                        var parentFolder = new DirectoryInfo(successPath).Parent.Name;
                        var newFileName = MoveFileToFolder(container, successPath, sourcePath, fileinfo);
                        container.ReconciliationFiles.AddObject(new ReconciliationFile
                        {
                            FileName = fileinfo.Name,
                            Contents = contents,
                            CreateDate = DateTime.Now,
                            CreateBy = "System",
                            BackupPath = newFileName, //successPath + newFileName,
                            IsValid = true,
                            Source = null,
                            IsRead = false,
                            FileType = (parentFolder.ToLower().Contains("credit"))
                                            ? FileType.C.ToString()
                                            : FileType.P.ToString(),
                        });
                    }
                    container.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Log("System", ex, SystemError.ServiceReader);
            }
        }

        #region Private Method

        private string GetFileName(string sourcePath)
        {
            FileStream fileStream = null;
            do
            {
                try
                {
                    fileStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                }
                catch (IOException x)
                {
                    if (x.Message.ToLower().Contains("being used by another process"))
                    {
                        fileStream = null;
                        Thread.Sleep(1 * 1000);
                    }
                }
            } while (fileStream == null);

            using (fileStream)
            {
                return fileStream.Name;
            }
        }

        private byte[] ReadFile(string sourcePath)
        {
            byte[] buffer;
            using (var fileStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    var length = (int)fileStream.Length;  // get file length
                    buffer = new byte[length];            // create buffer
                    int count;                            // actual number of bytes read
                    var sum = 0;                          // total number of bytes read

                    // read until Read method returns 0 (end of the stream has been reached)
                    while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                        sum += count;  // sum is a buffer offset for next reading
                }
                finally
                {
                    fileStream.Close();
                }
            }
            return buffer;
        }

        private void OnFileEvent(object source, FileSystemEventArgs fsea)
        {
            lock (this)
            {
                CreateReconciliationFile(fsea.FullPath);
            }
        }

        private void CreateSubFolderDirectorySuccess(TransactionModelContainer container, string fullPath)
        {
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
        }

        private string MoveFileToFolder(TransactionModelContainer container, string moveToPath, string sourceFile, FileInfo fileInfo)
        {
            var folderName = DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00");
            var fullPath = moveToPath + folderName + "\\";
            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + fileInfo.Name;
            var destinationPath = fullPath + newFileName;

            CreateSubFolderDirectorySuccess(container, fullPath);

            File.Move(sourceFile, destinationPath);

            return destinationPath;
        }

        private bool IsValidFile(FileInfo fileInfo)
        {
            // File size is over than 10M
            if (fileInfo.Length > 10 * 1000000)
                throw new Exception("File " + fileInfo.Name + " is too big.");

            if (//fileInfo.Length > maxSize ||
                //fileInfo.Name.Length > 86 ||
                !fileInfo.Extension.Equals(".txt"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion
    }
}
