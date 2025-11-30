using Microsoft.EntityFrameworkCore;
using PrintingOrderManager.Core.Entities;
using PrintingOrderManager.Core.Interfaces;
using PrintingOrderManager.Infrastructure.Data;
using PrintingOrderManager.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}