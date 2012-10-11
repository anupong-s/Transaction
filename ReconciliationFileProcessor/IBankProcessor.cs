using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransactionModel;

namespace ReconciliationFileProcessor
{
    public interface IBankProcessor
    {
        void Reader(ReconciliationFile rec);
        void SaveReconcilation(Reconciliation[] recons);
    }
}
