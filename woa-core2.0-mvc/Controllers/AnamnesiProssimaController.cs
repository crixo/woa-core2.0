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
    public class AnamnesiProssimaController : Controller
    {
        private readonly WoaContext _context;
        private readonly ILogger<AnamnesiRemoteController> _logger;

        public AnamnesiProssimaController(WoaContext context, ILogger<AnamnesiRemoteController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Create(int? idConsulto)
        {
            if (idConsulto == null)
            {
                return NotFound();
            }

            var entity = await _context.Consulti
                                       .SingleOrDefaultAsync(m => m.ID == idConsulto);
            if (entity == null)
            {
                return NotFound();
            }

            var model = new AnamnesiProssima {PazienteId=entity.PazienteId, ConsultoId=idConsulto.Value};
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnamnesiProssima anamnesi)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(anamnesi);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Consulti", new {id=anamnesi.ConsultoId});
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "AnamnesiProssima creation failed");
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(anamnesi);
        }

        public async Task<IActionResult> Details(int? idConsulto)
        {
            if (idConsulto == null)
            {
                return NotFound();
            }

            var entity = await _context.AnamnesiProssime
                                       .SingleOrDefaultAsync(m => m.ConsultoId == idConsulto);
            if (entity == null)
            {
                return NotFound();
            }


            return View(entity);
        }

        public async Task<IActionResult> Edit(int? idConsulto)
        {
            if (idConsulto == null)
            {
                return NotFound();
            }

            var entity = await _context.AnamnesiProssime
                                       .SingleOrDefaultAsync(m=>m.ConsultoId==idConsulto);
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
        public async Task<IActionResult> EditPost(int? idConsulto)
        {
            if (idConsulto == null)
            {
                return NotFound();
            }
            var modelToUpdate = await _context.AnamnesiProssime
                            .SingleOrDefaultAsync(
                                                  m => m.ConsultoId == idConsulto);
            if (await TryUpdateModelAsync<AnamnesiProssima>(
                    modelToUpdate,
                    "",
                    s => s.PrimaVolta, s => s.Tipologia, s => s.Localizzazione, s => s.Irradiazione,
                    s => s.PeriodoInsorgenza, s => s.Durata, s => s.Familiarita, s => s.AltreTerapie, s=>s.Varie))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Consulti", new { id = modelToUpdate.ConsultoId });
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "AnamnesiProssima update failed");
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(modelToUpdate);
        }

        public async Task<IActionResult> Delete(int? idConsulto, bool? saveChangesError = false)
        {
            if (idConsulto == null)
            {
                return NotFound();
            }

            var entity = await _context.AnamnesiProssime
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ConsultoId == idConsulto);
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
        public async Task<IActionResult> DeleteConfirmed(int idConsulto)
        {
            var entity = await _context.AnamnesiProssime
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ConsultoId == idConsulto);
            if (entity == null)
            {
                return NotFound();
            }

            try
            {
                _context.AnamnesiProssime.Remove(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Consulti", new { id = entity.ConsultoId });
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = idConsulto, saveChangesError = true });
            }
        }
    }
}
