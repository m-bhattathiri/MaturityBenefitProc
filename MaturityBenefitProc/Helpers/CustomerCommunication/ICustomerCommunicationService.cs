using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.CustomerCommunication
{
    public interface ICustomerCommunicationService
    {
        CustomerCommunicationResult SendMaturityIntimation(string policyNumber, string cifNumber, int daysBeforeMaturity);

        CustomerCommunicationResult ValidateCommunication(string communicationId);

        CustomerCommunicationResult SendPaymentConfirmation(string claimNumber, string cifNumber, decimal amount);

        CustomerCommunicationResult SendTdsCertificate(string policyNumber, string cifNumber, string certificateNumber);

        CustomerCommunicationResult SendClaimStatusUpdate(string claimNumber, string cifNumber, string status);

        bool IsContactInfoValid(string cifNumber);

        CustomerCommunicationResult GenerateMaturityLetter(string policyNumber, string cifNumber);

        CustomerCommunicationResult SendSmsNotification(string phoneNumber, string templateCode, string message);

        CustomerCommunicationResult SendEmailNotification(string emailAddress, string subject, string body);

        string GetCommunicationTemplate(string templateCode, string language);

        int GetCommunicationCount(string cifNumber, DateTime fromDate, DateTime toDate);

        CustomerCommunicationResult GetCommunicationDetails(string communicationId);

        List<CustomerCommunicationResult> GetCommunicationHistory(string cifNumber, DateTime fromDate, DateTime toDate);

        bool ValidatePhoneNumber(string phoneNumber);

        bool ValidateEmailAddress(string emailAddress);
    }
}
