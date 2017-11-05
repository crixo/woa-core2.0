using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Woa.Data;

using Microsoft.EntityFrameworkCore;

namespace Woa.ViewComponents
{
    [ViewComponent(Name = "ConsultoHeader")]
    public class ConsultoHeaderComponent : ViewComponent
    {
        public WoaContext _context;

        public ConsultoHeaderComponent(WoaContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int consultoId)
        {
            var entity = await _context.Consulti.SingleOrDefaultAsync(m => m.ID == consultoId);
            return View(entity);
        }
    }
}
