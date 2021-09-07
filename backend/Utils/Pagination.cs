using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo
{
    public class Pagination<T> where T : class
    {
        public static async Task<Page<T>> Create(IQueryable<T> query, int page, int pageSize)
        {
            if (page == 0)
            {
                page = 1;
            }

            if (pageSize == 0)
            {
                pageSize = 15;
            }
                
            var results = await query
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize)
                 .ToListAsync();

            var total = await query.CountAsync();

            return new Page<T>
            {
                Results = results,
                Total = total,
                CurrentPage = page,
                PageSize = pageSize
            };
        }
    }

    public class Page<T> where T : class
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public List<T> Results { get; set; }
    }
}
