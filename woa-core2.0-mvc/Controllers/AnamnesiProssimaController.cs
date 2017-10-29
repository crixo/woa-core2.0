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

            var model = new AnamnesiProssima {PazienteId=entity.PazienteId, ConsultoId=consultoId.Value};
            return View();
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

        public async Task<IActionResult> Edit(int? consultoId)
        {
            if (consultoId == null)
            {
                return NotFound();
            }

            var entity = await _context.AnamnesiProssime
                                       .SingleOrDefaultAsync(m=>m.ConsultoId==consultoId);
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
        public async Task<IActionResult> EditPost(int? consultoId)
        {
            if (consultoId == null)
            {
                return NotFound();
            }
            var modelToUpdate = await _context.AnamnesiProssime
                            .SingleOrDefaultAsync(
                                m => m.ConsultoId == consultoId);
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



    }
}
