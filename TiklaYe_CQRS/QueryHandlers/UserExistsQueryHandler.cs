using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Queries;

// Bir kullanıcının var olup olmadığını kontrol etmek için 
public class UserExistsQueryHandler
{
    private readonly ApplicationDbContext _context;

    // Constructor
    public UserExistsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    // Kullanıcı varlık sorgusunu gerçekleştirir.
    public async Task<bool> Handle(UserExistsQuery query)
    {
        return await _context.Users.AnyAsync(u => u.Username == query.Username || u.Email == query.Email);
    }
}