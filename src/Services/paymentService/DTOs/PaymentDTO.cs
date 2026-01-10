namespace paymentService.DTOs
{
    public class PaymentHistoryDTO
    {
        public Guid InvoiceId { get; set; }
        public Guid EnrollmentId { get; set; }
        public Guid ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public Guid CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string LanguageName { get; set; } = string.Empty;
        public string LevelName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string InvoiceStatus { get; set; } = string.Empty;
        public string EnrollmentStatus { get; set; } = string.Empty;
    }

    public class GetPaymentHistoryResponse
    {
        public Guid StudentId { get; set; }
        public List<PaymentHistoryDTO> PaymentHistory { get; set; } = new List<PaymentHistoryDTO>();
        public decimal TotalPaid { get; set; }
        public decimal TotalUnpaid { get; set; }
        public int TotalInvoices { get; set; }
    }

    public class VerifyPaymentRequest
    {
        public Guid InvoiceId { get; set; }
        public string? PaymentMethod { get; set; }
        public string? TransactionId { get; set; }
    }

    public class VerifyPaymentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid InvoiceId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string InvoiceStatus { get; set; } = string.Empty;
    }
}
