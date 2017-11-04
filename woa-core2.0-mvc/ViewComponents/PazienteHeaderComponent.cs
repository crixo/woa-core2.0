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
    [ViewComponent(Name = "PazienteHeader")]
    public class PazienteHeaderComponent : ViewComponent
    {
        public WoaContext _context;

        public PazienteHeaderComponent(WoaContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int pazienteId)
        {
            var model = await _context.Pazienti.SingleOrDefaultAsync(m => m.ID == pazienteId);
            return View(model);
        }
    }
}
