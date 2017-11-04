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
    public class AnamnesiRemoteController : Controller
    {
        private readonly WoaContext _context;
        private readonly ILogger<AnamnesiRemoteController> _logger;

        public AnamnesiRemoteController(WoaContext context, ILogger<AnamnesiRemoteController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Create(int pazienteId)
        {
            var model = new AnamnesiRemota { PazienteId = pazienteId};
            ViewData["TipiAnamnesi"] = new SelectList(_context.TipoAnamnesi, "ID", "Descrizione");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnamnesiRemota anamnesi)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(anamnesi);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Pazienti", new {id=anamnesi.PazienteId});
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "AnamnesiRemota creation failed");
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(anamnesi);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _context.AnamnesiRemote
                                       .Include(x => x.Tipo)
                                       .SingleOrDefaultAsync(m => m.ID == id);
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

            var entity = await _context.AnamnesiRemote.SingleOrDefaultAsync(m => m.ID == id);
            if (entity == null)
            {
                return NotFound();
            }

            ViewData["TipiAnamnesi"] = new SelectList(_context.TipoAnamnesi, "ID", "Descrizione", entity.TipoId);

            return View(entity);
        }

        // POST: Students/Edit/5
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
            var modelToUpdate = await _context.AnamnesiRemote.SingleOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<AnamnesiRemota>(
                    modelToUpdate, 
                    "", 
                    s=>s.Data, s=>s.Descrizione, s=>s.TipoId))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Pazienti", new { id = modelToUpdate.PazienteId });
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "AnamnesiRemota update failed");
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(modelToUpdate);
        }

        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _context.AnamnesiRemote
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (entity == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(entity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _context.AnamnesiRemote
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (entity == null)
            {
                return NotFound();
            }

            try
            {
                _context.AnamnesiRemote.Remove(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Pazienti", new { id = entity.PazienteId });
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

    }
}
