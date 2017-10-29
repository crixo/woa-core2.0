using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Woa.Data;
using Woa.Models;

using Microsoft.EntityFrameworkCore;

namespace Woa.ViewComponents
{
    [ViewComponent(Name = "Valutazioni")]
    public class ValutazioniComponent : ViewComponent
    {
        public WoaContext _context;

        public ValutazioniComponent(WoaContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int consultoId)
        {
            ViewBag.ConsultoId = consultoId;
            var list = await _context.Valutazioni
                                       .Where(x => x.ConsultoId == consultoId)
                                        .AsNoTracking()
                                       .ToListAsync();
            return View(list);
        }
    }
}
