using System.Data;
using paymentService.DTOs;
using paymentService.Repositories;

namespace paymentService.Services
{
    public class InvoiceService
    {
        private readonly InvoiceRepository _invoiceRepository;

        public InvoiceService(InvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public GetStudentInvoicesResponse GetInvoicesByStudentId(Guid studentId)
        {
            DataTable invoiceData = _invoiceRepository.GetInvoicesByStudentId(studentId);
            
            var invoices = new List<InvoiceDTO>();
            foreach (DataRow row in invoiceData.Rows)
            {
                var invoice = new InvoiceDTO
                {
                    InvoiceId = Guid.Parse(row["invoice_id"].ToString()!),
                    EnrollmentId = Guid.Parse(row["enrollment_id"].ToString()!),
                    Amount = decimal.Parse(row["amount"].ToString()!),
                    IssueDate = DateTime.Parse(row["issue_date"].ToString()!),
                    DueDate = DateTime.Parse(row["due_date"].ToString()!),
                    PaymentDate = row["payment_date"] != DBNull.Value 
                        ? DateTime.Parse(row["payment_date"].ToString()!) 
                        : null,
                    InvoiceStatus = row["invoice_status"].ToString()!
                };
                invoices.Add(invoice);
            }

            return new GetStudentInvoicesResponse
            {
                StudentId = studentId,
                Invoices = invoices
            };
        }

        public GetPaymentHistoryResponse GetPaymentHistory(Guid studentId)
        {
            DataTable paymentData = _invoiceRepository.GetPaymentHistoryByStudentId(studentId);
            
            var paymentHistory = new List<PaymentHistoryDTO>();
            decimal totalPaid = 0;
            decimal totalUnpaid = 0;

            foreach (DataRow row in paymentData.Rows)
            {
                var payment = new PaymentHistoryDTO
                {
                    InvoiceId = Guid.Parse(row["invoice_id"].ToString()!),
                    EnrollmentId = Guid.Parse(row["enrollment_id"].ToString()!),
                    ClassId = Guid.Parse(row["class_id"].ToString()!),
                    ClassName = row["class_name"].ToString()!,
                    CourseId = Guid.Parse(row["course_id"].ToString()!),
                    CourseName = row["course_name"].ToString()!,
                    LanguageName = row["language_name"].ToString()!,
                    LevelName = row["level_name"].ToString()!,
                    Amount = decimal.Parse(row["amount"].ToString()!),
                    IssueDate = DateTime.Parse(row["issue_date"].ToString()!),
                    DueDate = DateTime.Parse(row["due_date"].ToString()!),
                    PaymentDate = row["payment_date"] != DBNull.Value 
                        ? DateTime.Parse(row["payment_date"].ToString()!) 
                        : null,
                    InvoiceStatus = row["invoice_status"].ToString()!,
                    EnrollmentStatus = row["enrollment_status"].ToString()!
                };

                // Calculate totals
                if (payment.InvoiceStatus == "paid")
                {
                    totalPaid += payment.Amount;
                }
                else
                {
                    totalUnpaid += payment.Amount;
                }

                paymentHistory.Add(payment);
            }

            return new GetPaymentHistoryResponse
            {
                StudentId = studentId,
                PaymentHistory = paymentHistory,
                TotalPaid = totalPaid,
                TotalUnpaid = totalUnpaid,
                TotalInvoices = paymentHistory.Count
            };
        }

        public VerifyPaymentResponse VerifyPayment(VerifyPaymentRequest request)
        {
            // Get invoice details first
            DataTable invoiceData = _invoiceRepository.GetInvoiceById(request.InvoiceId);
            
            if (invoiceData.Rows.Count == 0)
            {
                return new VerifyPaymentResponse
                {
                    Success = false,
                    Message = "Invoice not found",
                    InvoiceId = request.InvoiceId
                };
            }

            DataRow invoice = invoiceData.Rows[0];
            string currentStatus = invoice["invoice_status"].ToString()!;

            if (currentStatus == "paid")
            {
                return new VerifyPaymentResponse
                {
                    Success = false,
                    Message = "Invoice has already been paid",
                    InvoiceId = request.InvoiceId,
                    InvoiceStatus = currentStatus
                };
            }

            // Verify payment
            DateTime paymentDate = DateTime.Now;
            bool isVerified = _invoiceRepository.VerifyPayment(request.InvoiceId, paymentDate);

            if (!isVerified)
            {
                return new VerifyPaymentResponse
                {
                    Success = false,
                    Message = "Failed to verify payment",
                    InvoiceId = request.InvoiceId
                };
            }

            // Update enrollment status to active
            Guid enrollmentId = Guid.Parse(invoice["enrollment_id"].ToString()!);
            _invoiceRepository.UpdateEnrollmentStatus(enrollmentId, "active");

            return new VerifyPaymentResponse
            {
                Success = true,
                Message = "Payment verified successfully",
                InvoiceId = request.InvoiceId,
                PaymentDate = paymentDate,
                Amount = decimal.Parse(invoice["amount"].ToString()!),
                InvoiceStatus = "paid"
            };
        }
    }
}
