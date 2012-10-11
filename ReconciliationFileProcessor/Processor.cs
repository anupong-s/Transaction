using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using TransactionModel;
using TransactionModel.Utils;
using System.Threading;

namespace ReconciliationFileProcessor
{
    public class Processor
    {
        public void ReadReconciliation()
        {
            //Exception exception = null;

            var reconciliations = ReconciliationFile.GetReconciliationFile();
            foreach (var rec in reconciliations)
            {
                try
                {
                    KindOfBank(rec);
                }
                catch (Exception ex)
                {
                    //exception = ex;
                    CreateLog(ex);
                    //UpdateFailReconciliationFile(rec);
                }

                //if (exception != null)
                //    UpdateFailReconciliationFile(rec);
            }
        }

        private void KindOfBank(ReconciliationFile rec)
        {
            string bankName = GetBank(rec.BackupPath);

            if (bankName == TransactionBanks.BBL.ToString())
            {
                var bbl = new BBLProcessor();
                bbl.Reader(rec);
            }
            else if (bankName == TransactionBanks.KBANK.ToString())
            {
                var kbank = new KBankProcessor();
                kbank.Reader(rec);
            }
            else if (bankName == TransactionBanks.SCB.ToString())
            {
                var scb = new SCBProcessor();
                scb.Reader(rec);
            }
        }

        private string GetBank(string path)
        {
            var values = path.Split('\\');
            
            return values[2].ToUpper();
        }

        private void UpdateFailReconciliationFile(ReconciliationFile rec)
        {
            string sourceFile = rec.BackupPath;
            string destinationFile = sourceFile.Replace("Success", "Err");

            string parentPath = new DirectoryInfo(destinationFile).Parent.FullName;

            CreateDirectory(parentPath);

            File.Move(sourceFile, destinationFile);

            rec.BackupPath = destinationFile;

            ReconciliationFile.UpdateFailReconciliationFile(rec);
        }

        private void CreateLog(Exception ex)
        {
            string message = ex.Message;
            if (ex.InnerException != null)
                message = ex.InnerException.Message;

            ErrorLog.CreateErrorLog("System", message, SeverityEnum.HIGH, SystemError.ServiceProcessor);
        }

        private void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

    }
}
