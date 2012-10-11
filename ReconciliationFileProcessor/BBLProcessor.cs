using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransactionModel;
using TransactionModel.Utils;
using System.IO;
using System.Data;

namespace ReconciliationFileProcessor
{
    public class BBLProcessor : IBankProcessor
    {
        public void Reader(ReconciliationFile rec)
        {
            if (rec.FileType == FileType.C.ToString())
            {
                CreditCard(rec);
            }
            else if (rec.FileType == FileType.P.ToString())
            {
                PayInSlip(rec);
            }
        }

        private void CreditCard(ReconciliationFile rec)
        {
            var content = new StreamReader(new MemoryStream(rec.Contents));
            DataSet ds = TextToDataSet.Convert(content);
            var recons = MapperReconciliation.CreateCreditCardToBBL(ds.Tables[0],
                                            rec.FileType,
                                            rec.Id);

            SaveReconcilation(recons);
            ReconciliationFile.UpdateSuccessReconciliationFile(rec.Id);
        }

        private void PayInSlip(ReconciliationFile rec)
        {
            ReadContents(rec);
        }

        private void ReadContents(ReconciliationFile rec)
        {
            try
            {
                List<Reconciliation> reconciliations = new List<Reconciliation>();
                string sourceFileName = rec.BackupPath;
                using (var stream = new StreamReader(new MemoryStream(rec.Contents), Encoding.Default))
                {
                    string str = stream.ReadLine();
                    while (str != null)
                    {
                        str = str.Substring(19);
                        string flag = str.Substring(0, 1).ToUpper();

                        if (flag == "D")
                        {
                            //Line Item
                            var value = MapperReconciliation.CreatePayInSlip(str, rec.Id);
                            reconciliations.Add(value);
                        }
                        str = stream.ReadLine();
                    }

                    if (reconciliations.Count() <= 0)
                    {
                        string msg = string.Format("Invalid PayInSlip Pattern - Id:{0} FullPath:{1}", rec.Id, rec.BackupPath);
                        throw new Exception(msg);
                    }

                    SaveReconcilation(reconciliations.ToArray());
                    ReconciliationFile.UpdateSuccessReconciliationFile(rec.Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public void SaveReconcilation(Reconciliation[] recons)
        {
            foreach (Reconciliation recon in recons)
                Reconciliation.SaveReconciliation(recon);
        }
    }
}
