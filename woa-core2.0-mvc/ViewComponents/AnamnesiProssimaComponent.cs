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
    [ViewComponent(Name = "AnamnesiProssima")]
    public class AnamnesiProssimaComponent : ViewComponent
    {
        public WoaContext _context;

        public AnamnesiProssimaComponent(WoaContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int pazienteId, int consultoId)
        {
            ViewBag.PazienteId = pazienteId;
            ViewBag.ConsultoId = consultoId;
            var entity = await _context.AnamnesiProssime
                                        .AsNoTracking()
                                       .SingleOrDefaultAsync(x => x.ConsultoId == consultoId);
                               ;
            return View(entity);
        }
    }
}
