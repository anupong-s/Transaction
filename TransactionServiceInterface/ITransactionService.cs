using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace TransactionServiceInterface
{
    [DataContract]
    public abstract class AbstractRequest
    {

    }

    [DataContract]
    public abstract class AbstractResponse
    {
        public static string DEFAULT_SUCCESS_MSG = "Operation runs successfully.";
        public static string DEFAULT_FAILURE_MSG = "The operation has failed for some reason.";

        /// <summary>
        /// Operation begin timestamp
        /// </summary>        
        [DataMember]
        public DateTime BeginTimestamp { get; protected internal set; }

        /// <summary>
        /// Operation end timestamp
        /// </summary>        
        [DataMember]
        public DateTime EndTimestamp { get; protected internal set; }

        /// <summary>
        /// Whether the operation succeeds or not.
        /// </summary>        
        [DataMember]
        public bool IsSuccessful { get; protected internal set; }

        /// <summary>
        /// Operational message
        /// </summary>
        [DataMember]
        public string Message { get; protected internal set; }

        protected AbstractResponse()
        {
            BeginTimestamp = DateTime.Now;
            EndTimestamp = DateTime.MaxValue;

            IsSuccessful = true;
            Message = DEFAULT_SUCCESS_MSG;
        }

        public void Fail()
        {
            Fail(null);
        }

        public void Fail(Exception x)
        {
            EndTimestamp = DateTime.Now;
            IsSuccessful = false;
            Message = DEFAULT_FAILURE_MSG;

            if (x != null)
            {
                Message = x.StackTrace;
            }
        }

        public void Succeed()
        {
            EndTimestamp = DateTime.Now;
            IsSuccessful = true;
            Message = DEFAULT_SUCCESS_MSG;
        }
    }

    [ServiceContract]
    public partial interface ITransactionService
    {
        // Details are on each service partial files...
    }
}
