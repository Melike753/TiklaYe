using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;

public class RegisterUserCommandHandler
{
    private readonly ApplicationDbContext _context;

    // Constructor
    public RegisterUserCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(RegisterUserCommand command)
    {
        if (await _context.Users.AnyAsync(u => u.Username == command.Username || u.Email == command.Email))
        {
            throw new InvalidOperationException("Kullanıcı adı veya e-posta zaten kullanılıyor.");
        }

        var user = new User
        {
            Name = command.Name,
            Username = command.Username,
            Email = command.Email,
            Mobile = command.Mobile,
            Address = command.Address,
            PostCode = command.PostCode,
            Password = HashPassword(command.Password),
            CreatedDate = DateTime.Now
        };

        // Yeni kullanıcıyı veritabanına ekler ve değişiklikleri kaydeder.
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    // Bu metod, şifreyi SHA-256 algoritması ile hashler.
    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                builder.Append(hashBytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}