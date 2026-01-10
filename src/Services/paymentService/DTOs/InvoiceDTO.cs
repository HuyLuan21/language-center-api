namespace paymentService.DTOs
{
    public class InvoiceDTO
    {
        public Guid InvoiceId { get; set; }
        public Guid EnrollmentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string InvoiceStatus { get; set; } = string.Empty;
    }

    public class GetStudentInvoicesResponse
    {
        public Guid StudentId { get; set; }
        public List<InvoiceDTO> Invoices { get; set; } = new List<InvoiceDTO>();
    }
}
