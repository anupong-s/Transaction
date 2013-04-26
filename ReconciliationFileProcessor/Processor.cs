using System;
using System.IO;
using TransactionModel;
using TransactionModel.Utils;

namespace ReconciliationFileProcessor
{
    public class Processor
    {
        public void ReadReconciliation()
        {
            var reconciliations = ReconciliationFile.GetReconciliationFile();

            foreach (var rec in reconciliations)
            {
                try
                {
                    BankType(rec);
                }
                catch (Exception ex)
                {
                    CreateLog(ex);
                }
            }
        }

        private void BankType(ReconciliationFile rec)
        {
            var bankName = GetBank(rec.BackupPath);

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
            var sourceFile = rec.BackupPath;
            var destinationFile = sourceFile.Replace("Success", "Err");

            var parentPath = new DirectoryInfo(destinationFile).Parent.FullName;

            CreateDirectory(parentPath);

            File.Move(sourceFile, destinationFile);

            rec.BackupPath = destinationFile;

            ReconciliationFile.UpdateFailReconciliationFile(rec);
        }

        private void CreateLog(Exception ex)
        {
            ErrorLog.Log("System", ex, SystemError.ServiceProcessor);
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
