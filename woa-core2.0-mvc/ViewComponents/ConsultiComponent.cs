using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Woa.Data;

using Microsoft.EntityFrameworkCore;

namespace Woa.ViewComponents
{
    [ViewComponent(Name = "Consulti")]
    public class ConsultiComponent : ViewComponent
    {
        public WoaContext _context;

        public ConsultiComponent(WoaContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int pazienteId)
        {
            ViewBag.PazienteId = pazienteId;
            var list = await _context.Consulti
                               .Where(x => x.PazienteId == pazienteId)
                               .OrderByDescending(x=>x.Data)
                               .AsNoTracking()
                               .ToListAsync();
            return View(list);
        }
    }
}
