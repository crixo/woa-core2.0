using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Woa.Data;
using Woa.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Woa.Controllers
{
    public class ConsultiController : Controller
    {
        private readonly WoaContext _context;
        private readonly ILogger<ConsultiController> _logger;

        public ConsultiController(WoaContext context, ILogger<ConsultiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Create(int pazienteId)
        {
            var model = new Consulto { PazienteId = pazienteId};
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Consulto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Pazienti", new {id=model.PazienteId});
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Consulto creation failed");
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _context.Consulti.SingleOrDefaultAsync(m => m.ID == id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _context.Consulti.SingleOrDefaultAsync(m => m.ID == id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var modelToUpdate = await _context.Consulti.SingleOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Consulto>(
                    modelToUpdate, 
                    "", 
                    s=>s.Data, s=>s.ProblemaIniziale))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Pazienti", new { id = modelToUpdate.PazienteId });
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Consulto update failed");
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(modelToUpdate);
        }



    }
}
