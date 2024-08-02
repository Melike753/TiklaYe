using MediatR;

namespace TiklaYe_CQRS.Commands
{
    public class DownloadInvoiceCommand : IRequest<byte[]>
    {
        public int PurchaseId { get; set; }
    }
}