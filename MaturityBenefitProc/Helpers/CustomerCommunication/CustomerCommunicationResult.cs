using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.CustomerCommunication
{
    public class CustomerCommunicationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ReferenceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ProcessedDate { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public string CommunicationId { get; set; }
        public string Channel { get; set; }
        public string Recipient { get; set; }
        public string TemplateName { get; set; }
        public string DeliveryStatus { get; set; }
        public DateTime SentDate { get; set; }

        public CustomerCommunicationResult()
        {
            Metadata = new Dictionary<string, string>();
            ProcessedDate = DateTime.UtcNow;
        }
    }
}
