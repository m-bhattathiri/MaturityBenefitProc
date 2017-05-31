using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.CustomerCommunication
{
    public class CustomerCommunicationService : ICustomerCommunicationService
    {
        public CustomerCommunicationResult SendMaturityIntimation(string policyNumber, string cifNumber, int daysBeforeMaturity)
        {
            return new CustomerCommunicationResult { Success = false, Message = "Not implemented" };
        }

        public CustomerCommunicationResult ValidateCommunication(string communicationId)
        {
            return new CustomerCommunicationResult { Success = false, Message = "Not implemented" };
        }

        public CustomerCommunicationResult SendPaymentConfirmation(string claimNumber, string cifNumber, decimal amount)
        {
            return new CustomerCommunicationResult { Success = false, Message = "Not implemented" };
        }

        public CustomerCommunicationResult SendTdsCertificate(string policyNumber, string cifNumber, string certificateNumber)
        {
            return new CustomerCommunicationResult { Success = false, Message = "Not implemented" };
        }

        public CustomerCommunicationResult SendClaimStatusUpdate(string claimNumber, string cifNumber, string status)
        {
            return new CustomerCommunicationResult { Success = false, Message = "Not implemented" };
        }

        public bool IsContactInfoValid(string cifNumber)
        {
            return false;
        }

        public CustomerCommunicationResult GenerateMaturityLetter(string policyNumber, string cifNumber)
        {
            return new CustomerCommunicationResult { Success = false, Message = "Not implemented" };
        }

        public CustomerCommunicationResult SendSmsNotification(string phoneNumber, string templateCode, string message)
        {
            return new CustomerCommunicationResult { Success = false, Message = "Not implemented" };
        }

        public CustomerCommunicationResult SendEmailNotification(string emailAddress, string subject, string body)
        {
            return new CustomerCommunicationResult { Success = false, Message = "Not implemented" };
        }

        public string GetCommunicationTemplate(string templateCode, string language)
        {
            return string.Empty;
        }

        public int GetCommunicationCount(string cifNumber, DateTime fromDate, DateTime toDate)
        {
            return 0;
        }

        public CustomerCommunicationResult GetCommunicationDetails(string communicationId)
        {
            return new CustomerCommunicationResult { Success = false, Message = "Not implemented" };
        }

        public List<CustomerCommunicationResult> GetCommunicationHistory(string cifNumber, DateTime fromDate, DateTime toDate)
        {
            return new List<CustomerCommunicationResult>();
        }

        public bool ValidatePhoneNumber(string phoneNumber)
        {
            return false;
        }

        public bool ValidateEmailAddress(string emailAddress)
        {
            return false;
        }
    }
}
