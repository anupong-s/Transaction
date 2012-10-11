using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransactionModel.Utils;
using TransactionCommon;

namespace TransactionModel
{
    public partial class ReconciliationFile
    {
        #region Static Methods

        public ReconciliationFile()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="contents"></param>
        /// <param name="createDate"></param>
        /// <param name="createBy"></param>
        /// <param name="backupPath">Nullable</param>
        /// <param name="isValid">true or false</param>
        /// <param name="source">Nullable</param>
        public ReconciliationFile(string filename, Byte[] contents, DateTime createDate, string createBy, 
                                    string backupPath, bool isValid, string source, bool isRead)
        {
            FileName = filename;
            Contents = contents;
            CreateDate = createDate;
            CreateBy = createBy;
            BackupPath = backupPath;
            IsValid = isValid;
            Source = source;
            IsRead = isRead;
        }
        /// <summary>
        /// IsRead = false
        /// FileType = 'C','P'
        /// </summary>
        /// <returns></returns>
        public static List<ReconciliationFile> GetReconciliationFile()
        {
            using (var container = new TransactionModelContainer())
            {
                string[] fileType = new string[] { "C", "P" };
                return container.ReconciliationFiles.Where(x => x.IsRead == false && fileType.Contains(x.FileType)).ToList();
            }
        }

        public static ReconciliationFile GetReconciliationFile(long Id)
        {
            using (var container = new TransactionModelContainer())
            {
                return container.ReconciliationFiles.FirstOrDefault(x => x.Id == Id);
            }
        }

        public static ReconciliationFile GetReconciliationFile(long Id,bool IsRead)
        {
            using (var container = new TransactionModelContainer())
            {
                return container.ReconciliationFiles.FirstOrDefault(x => x.Id == Id && x.IsRead == IsRead);
            }
        }

        public static void UpdateSuccessReconciliationFile(long Id)
        {
            using (var container = new TransactionModelContainer())
            {
                var val = container.ReconciliationFiles.FirstOrDefault(x => x.Id == Id);
                val.IsRead = true;

                container.SaveChanges();
            }
        }

        public static void UpdateFailReconciliationFile(ReconciliationFile recon)
        {
            using (var container = new TransactionModelContainer())
            {
                var val = container.ReconciliationFiles.FirstOrDefault(x => x.Id == recon.Id);
                val.IsValid = false;
                val.IsRead = true;
                val.FileType = TransactionModel.Utils.FileType.E.ToString();
                val.BackupPath = recon.BackupPath;

                container.SaveChanges();
            }
        }

        #endregion


        #region Partial Methods

        partial void OnFileNameChanging(String value)
        {
            if(string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("NULL_OR_EMPTY_FILENAME");
            }

            if (value.Length > 100)
            {
                throw new ArgumentException("FILENAME_TOO_LONG");
            }
        }

        partial void OnFileNameChanged()
        {
            if (!string.IsNullOrEmpty(_FileName))
            {
                _FileName = _FileName.TextEncode();
            }
        }

        partial void OnContentsChanging(Byte[] value)
        {
            if (value == null || value.Length == 0)
            {
                throw new ArgumentException("NULL_OR_EMPTY_CONTENTS");
            }
        }

        partial void OnCreateDateChanging(DateTime value)
        {
            if (value == null)
            {
                throw new ArgumentException("NULL_OR_EMPTY_CREATEDATE");
            }

            if (!value.IsPersistable())
            {
                throw new ArgumentException("CREATEDATE_NOT_PERSISTABLE");
            }
        }

        partial void OnCreateByChanging(String value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("NULL_OR_EMPTY_CREATEBY");
            }

            if (value.Length > 50)
            {
                throw new ArgumentException("CREATEBY_TOO_LONG");
            }
        }

        partial void OnCreateByChanged()
        {
            if (!string.IsNullOrEmpty(_CreateBy))
            {
                _CreateBy = CreateBy.TextEncode();
            }
        }

        #endregion
    }
}
