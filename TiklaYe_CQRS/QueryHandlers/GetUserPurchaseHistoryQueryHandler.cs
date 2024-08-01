﻿using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;

namespace TiklaYe_CQRS.QueryHandlers
{
    public class GetUserPurchaseHistoryQueryHandler
    {
        private readonly ApplicationDbContext _context;

        public GetUserPurchaseHistoryQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Purchase>> Handle(GetUserPurchaseHistoryQuery query)
        {
            return await _context.Purchases
                .Where(p => p.UserId == query.UserId)
                .ToListAsync();
        }
    }
}