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
    public class EsamiController : Controller
    {
        private readonly WoaContext _context;
        private readonly ILogger<EsamiController> _logger;

        public EsamiController(WoaContext context, ILogger<EsamiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index(int pazienteId, int consultoId)
        {
            return ViewComponent("Esami", new { pazienteId = pazienteId, consultoId = consultoId });
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

            var model = new Esame {PazienteId=entity.PazienteId, ConsultoId=consultoId.Value};
            ViewData["TipiEsami"] = new SelectList(_context.TipoEsami, "ID", "Descrizione");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Esame model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Consulti", new {id=model.ConsultoId});
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Esame creation failed");
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

            var entity = await _context.Esami
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

            var entity = await _context.Esami
                                       .SingleOrDefaultAsync(m => m.ID == id);
            if (entity == null)
            {
                return NotFound();
            }


            ViewData["TipiEsami"] = new SelectList(_context.TipoEsami, "ID", "Descrizione", entity.TipoId);
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
                    s => s.Data, s => s.Descrizione, s=>s.TipoId))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Consulti", new { id = modelToUpdate.ConsultoId });
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Esame update failed");
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

            var student = await _context.Esami
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _context.Esami
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (entity == null)
            {
                return NotFound();
            }

            try
            {
                _context.Esami.Remove(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Consulti", new { id = entity.ConsultoId });
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }
    }
}
