using Microsoft.AspNetCore.Mvc;
using TiklaYe.Models;
using Microsoft.EntityFrameworkCore;
using iTextSharp.text;
using iTextSharp.text.pdf;
using TiklaYe.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace TiklaYe.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Satın Alma Geçmişi Sayfası
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Login");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var purchases = await _context.Purchases.Where(p => p.UserId == user.UserId).ToListAsync();
            return View(purchases);
        }

        // PDF İndirme İşlemi
        public async Task<IActionResult> DownloadInvoice(int id)
        {
            var purchase = await _context.Purchases.FirstOrDefaultAsync(p => p.PurchaseId == id);
            if (purchase == null)
                return NotFound();

            using (var stream = new MemoryStream())
            {
                var document = new Document();
                PdfWriter.GetInstance(document, stream).CloseStream = false;
                document.Open();

                // Türkçe karakter desteği için font ayarı
                string arialFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                var bfArialUniCode = BaseFont.CreateFont(arialFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
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

                cell = new PdfPCell(new Phrase(purchase.PurchaseDate.ToString(), font))
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
                    SpacingBefore = 450
                };
                document.Add(footer);

                document.Close();
                byte[] file = stream.ToArray();
                stream.Close();
                return File(file, "application/pdf", "TıklaYeFatura.pdf");
            }
        }
    }
}