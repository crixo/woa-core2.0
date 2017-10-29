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
    public class TrattamentiController : Controller
    {
        private readonly WoaContext _context;
        private readonly ILogger<TrattamentiController> _logger;

        public TrattamentiController(WoaContext context, ILogger<TrattamentiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Create(int? consultoId)
        {
            if (consultoId == null)
            {
                return NotFound();
            }

            var entity = await _context.Consulti
                                       .SingleOrDefaultAsync(m => m.ID == consultoId);
            if (entity == null)
            {
                return NotFound();
            }

            var model = new Esame { PazienteId = entity.PazienteId, ConsultoId = consultoId.Value };
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Trattamento model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Consulti", new { id = model.ConsultoId });
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Trattamento creation failed");
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _context.Trattamenti
                                       .SingleOrDefaultAsync(m => m.ID == id);
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
            var modelToUpdate = await _context.Esami
                                       .SingleOrDefaultAsync(m => m.ID == id);
            if (await TryUpdateModelAsync<Esame>(
                    modelToUpdate,
                    "",
                    s => s.Data, s => s.Descrizione))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Consulti", new { id = modelToUpdate.ConsultoId });
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Trattamento update failed");
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(modelToUpdate);
        }



    }
}
