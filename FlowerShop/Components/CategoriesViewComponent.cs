using FlowerShop.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlowerShop.Components
{
    public class CategoriesViewComponent: ViewComponent
    {
        private readonly FlowerContext _context;
        
        public CategoriesViewComponent(FlowerContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync() => View(await _context.Categories.ToListAsync());
    }
}
