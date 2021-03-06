﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Woa.Data;

using Microsoft.EntityFrameworkCore;

namespace Woa.ViewComponents
{
    [ViewComponent(Name = "AnamnesiRemote")]
    public class AnamnesiRemoteComponent : ViewComponent
    {
        public WoaContext _context;

        public AnamnesiRemoteComponent(WoaContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int pazienteId)
        {
            ViewBag.PazienteId = pazienteId;
            var list = await _context.AnamnesiRemote
                               .Include(x=>x.Tipo)
                               .Where(x => x.PazienteId == pazienteId)
                               .OrderByDescending(x=>x.Data)
                               .AsNoTracking()
                               .ToListAsync();
            return View(list);
        }
    }
}
