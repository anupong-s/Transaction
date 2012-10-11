using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransactionModel;
using TransactionServiceAdaptor.IDMService;

namespace TransactionFrontEnd.Common
{
    public class Main
    {
        public virtual string[] SupportedCultures
        {
            get { return new string[] { "en-US", "th-TH" }; }
        }

        private string _currentCulture = null;
        public virtual string CurrentCulture
        {
            get
            {
                string cult = SupportedCultures[0];
                if (!string.IsNullOrEmpty(_currentCulture))
                {
                    cult = _currentCulture;
                }
                else
                {
                    _currentCulture = cult;
                }
                return cult;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && SupportedCultures.Contains(value))
                {
                    _currentCulture = value;
                }
                else
                {
                    throw new ApplicationException("INVALID_CULTURE");
                }
            }
        }

        public virtual long OttTicket { get; private set; }
        public virtual long? UserAccountId { get; private set; }
        public virtual long? PartyId { get; private set; }
        public virtual string UserName { get; private set; }
        public virtual string FirstName { get; private set; }
        public virtual string LastName { get; private set; }
        public virtual string LastLoginDate { get; private set; }

        public Main()
        {
            CurrentCulture = SupportedCultures[0];
            SetUpGuest();
        }

        public virtual long LogsError(Exception x)
        {
            long id = -1;
            using (TransactionModelContainer container = new TransactionModelContainer())
            {
                string message = x.Message;
                if (x.InnerException != null)
                    message = x.InnerException.Message;
                
                ErrorLog log = container.LogsError(UserName + "(" + UserAccountId + ")",
                    message, SeverityEnum.HIGH);

                container.SaveChanges();
                id = log.Id;
            }
            return id;
        }

        public virtual string GetErrorLog(long id)
        {
            string msg = null;
            using (TransactionModelContainer container = new TransactionModelContainer())
            {
                ErrorLog log = container.ErrorLogs.Where(l => l.Id == id).FirstOrDefault();
                msg = log.IssuedMessage;
            }
            return msg;
        }

        public virtual void SetUpGuest()
        {
            CurrentCulture = "en-US";
            UserAccountId = -1;
            LastLoginDate = DateTime.Now.ToString(ConfigurationManager.Format.DateTime_Format);
            FirstName = "John";
            LastName = "Doe";
            UserName = "guest1";
        }

        public virtual void SetUpUser(long ottTicket)
        {
            IDMOtt ott = null;
            IDMUserAccount user = null;
            IDMPerson person = null;

            using (IDMServiceClient iClient = new IDMServiceClient())
            {
                GetOttByTicketResponse getOttByTicketResponse = iClient.GetOttByTicket(new GetOttByTicketRequest { Ticket = ottTicket });
                if (!getOttByTicketResponse.IsSuccessful) 
                    throw new ApplicationException(getOttByTicketResponse.Message);
                ott = getOttByTicketResponse.Result;
                GetUserByUserIdResponse getUserByUserIdResponse = iClient.GetUserByUserId(new GetUserByUserIdRequest { UserId = ott.UserId });
                if (!getOttByTicketResponse.IsSuccessful) 
                    throw new ApplicationException(getUserByUserIdResponse.Message);
                user = getUserByUserIdResponse.Result;
                GetPersonByPartyIdResponse getPersonByPartyIdResponse = iClient.GetPersonByPartyId(new GetPersonByPartyIdRequest { PartyId = user.PartyId });
                if (!getPersonByPartyIdResponse.IsSuccessful) 
                    throw new ApplicationException(getPersonByPartyIdResponse.Message);
                person = getPersonByPartyIdResponse.Result;
            }

            OttTicket = ottTicket;
            CurrentCulture = ott.Culture;
            UserAccountId = ott.UserId;
            PartyId = user.PartyId;
            LastLoginDate = ott.LastLoggedInTimestamp.ToString(ConfigurationManager.Format.DateTime_Format);
            FirstName = person.FirstName;
            LastName = person.LastName;
            UserName = user.Username;
        }
    }
}
