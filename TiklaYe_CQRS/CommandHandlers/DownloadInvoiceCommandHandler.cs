using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;
using Document = iTextSharp.text.Document;

namespace TiklaYe_CQRS.CommandHandlers
{
    // Belirli bir satın alma işlemi için bir PDF fatura oluşturur ve bu faturayı bir dosya olarak kullanıcıya sunar. 
    public class DownloadInvoiceCommandHandler
    {
        private readonly ApplicationDbContext _context;

        public DownloadInvoiceCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Handle(DownloadInvoiceCommand command)
        {
            var purchase = await _context.Purchases.FirstOrDefaultAsync(p => p.PurchaseId == command.PurchaseId);
            if (purchase == null)
                return new NotFoundResult();

            using (var stream = new MemoryStream())
            {
                var document = new Document();
                PdfWriter.GetInstance(document, stream).CloseStream = false; // PDF dosyasına yazmak için
                document.Open();

                // Türkçe karakter desteği için font ayarı
                string fontPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fonts", "arial.ttf");
                var bfArialUniCode = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                var font = new Font(bfArialUniCode, 12, Font.NORMAL);
                var boldFont = new Font(bfArialUniCode, 12, Font.BOLD);

                // Başlık
                var title = new Paragraph("Tıkla Ye Fatura", boldFont) { Alignment = Element.ALIGN_CENTER };
                title.SpacingAfter = 30;
                document.Add(title);

                // Tablo oluşturma
                var table = new PdfPTable(2); // 2 sütunlu bir tablo
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 1, 2 });

                // Tablo başlık hücreleri
                PdfPCell cell;

                cell = new PdfPCell(new Phrase("Sipariş Numarası:", boldFont))
                {
                    Border = PdfPCell.NO_BORDER,
                    PaddingBottom = 10
                };
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(purchase.OrderNumber.ToString(), font))
                {
                    Border = PdfPCell.NO_BORDER,
                    PaddingBottom = 10
                };
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Sipariş Tarihi:", boldFont))
                {
                    Border = PdfPCell.NO_BORDER,
                    PaddingBottom = 10
                };
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(purchase.PurchaseDate.ToString("dd/MM/yyyy"), font))
                {
                    Border = PdfPCell.NO_BORDER,
                    PaddingBottom = 10
                };
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Ürün Adı:", boldFont))
                {
                    Border = PdfPCell.NO_BORDER,
                    PaddingBottom = 10
                };
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(purchase.ProductName, font))
                {
                    Border = PdfPCell.NO_BORDER,
                    PaddingBottom = 10
                };
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Birim Fiyatı:", boldFont))
                {
                    Border = PdfPCell.NO_BORDER,
                    PaddingBottom = 10
                };
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(purchase.UnitPrice.ToString("C"), font))
                {
                    Border = PdfPCell.NO_BORDER,
                    PaddingBottom = 10
                };
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Miktar:", boldFont))
                {
                    Border = PdfPCell.NO_BORDER,
                    PaddingBottom = 10
                };
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(purchase.Quantity.ToString(), font))
                {
                    Border = PdfPCell.NO_BORDER,
                    PaddingBottom = 10
                };
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Toplam Tutar:", boldFont))
                {
                    Border = PdfPCell.NO_BORDER,
                    PaddingBottom = 10
                };
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(purchase.TotalPrice.ToString("C"), font))
                {
                    Border = PdfPCell.NO_BORDER,
                    PaddingBottom = 10
                };
                table.AddCell(cell);

                document.Add(table);

                // Dipnot ekleme
                var footer = new Paragraph("Tıkla Ye'yi tercih ettiğiniz için teşekkür ederiz!", font)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingBefore = 30
                };
                document.Add(footer);

                document.Close();
                byte[] file = stream.ToArray(); // MemoryStream içindeki PDF verilerini byte dizisine çevirir.
                return new FileContentResult(file, "application/pdf") { FileDownloadName = "TıklaYeFatura.pdf" };
            }
        }
    }
}