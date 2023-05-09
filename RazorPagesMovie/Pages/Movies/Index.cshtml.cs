using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;

namespace RazorPagesMovie.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesMovie.Data.RazorPagesMovieContext _context;

        public IndexModel(RazorPagesMovie.Data.RazorPagesMovieContext context)
        {
            _context = context;
        }        

            public IList<Movie> Movie { get; set; }

            public async Task OnGetAsync()
            {
                //Customers = await _context.Customers.ToListAsync();
            }

            [BindProperty]
            public DataTables.DataTablesRequest DataTablesRequest { get; set; }

            public async Task<JsonResult> OnPostAsync()
            {
                var recordsTotal = _context.Movie.Count();

                var customersQuery = _context.Movie.AsQueryable();

                var searchText = DataTablesRequest.Search.Value?.ToUpper();
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    customersQuery = customersQuery.Where(s =>
                        s.Title.ToUpper().Contains(searchText) ||
                        s.Genre.ToUpper().Contains(searchText) ||
                        s.Rating.ToUpper().Contains(searchText));
                }

                var recordsFiltered = customersQuery.Count();

                var sortColumnName = DataTablesRequest.Columns.ElementAt(DataTablesRequest.Order.ElementAt(0).Column).Name;
                var sortDirection = DataTablesRequest.Order.ElementAt(0).Dir.ToLower();

                customersQuery = customersQuery;

            var skip = DataTablesRequest.Start;
                var take = DataTablesRequest.Length;
                var data = await customersQuery
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                return new JsonResult(new
                {
                    Draw = DataTablesRequest.Draw,
                    RecordsTotal = recordsTotal,
                    RecordsFiltered = recordsFiltered,
                    Data = data
                });
            }
        }          
}
