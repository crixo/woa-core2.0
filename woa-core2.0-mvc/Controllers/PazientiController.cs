using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Woa.Models;
using Microsoft.EntityFrameworkCore;
using Woa.Data;      
using Microsoft.Extensions.Logging;
                    
namespace Woa.Controllers
{
    public class PazientiController : Controller
    {
        private readonly WoaContext _context;
        private readonly ILogger<PazientiController> _logger;

        public PazientiController(WoaContext context, ILogger<PazientiController> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogInformation("PazientiController");
        }

        // GET: Studentss
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CognomeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var items = from s in _context.Pazienti
                        select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Cognome.Contains(searchString)
                                       || s.Nome.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "cognome_desc":
                    items = items.OrderByDescending(s => s.Cognome);
                    break;
                default:
                    items = items.OrderBy(s => s.ID);
                    break;
            }

            int pageSize = 50;
            return View(await PaginatedList<Paziente>.CreateAsync(items, page ?? 1, pageSize));
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["Prov"] = new SelectList(_context.Province, "Sigla", "Descrizione");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Paziente paziente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(paziente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(paziente);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paziente = await _context.Pazienti.SingleOrDefaultAsync(m => m.ID == id);
            if (paziente == null)
            {
                return NotFound();
            }


            return View(paziente);
        }

        //[ChildActionOnly]
        public async Task<IActionResult> DetailsPartial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paziente = await _context.Pazienti.SingleOrDefaultAsync(m => m.ID == id);
            if (paziente == null)
            {
                return NotFound();
            }


            return PartialView(paziente);
        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paziente = await _context.Pazienti.SingleOrDefaultAsync(m => m.ID == id);
            if (paziente == null)
            {
                return NotFound();
            }

            ViewData["Prov"] = new SelectList(_context.Province, "Sigla", "Descrizione", paziente.Prov);

            return View(paziente);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Paziente paziente)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}
            //var pazienteToUpdate = await _context.Pazienti.SingleOrDefaultAsync(s => s.ID == paziente.ID);
            var pazienteToUpdate = paziente;
            //if (await TryUpdateModelAsync<Paziente>(pazienteToUpdate))
            {
                try
                {
                    _context.Entry(paziente).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(pazienteToUpdate);
        }
        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Pazienti
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

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Pazienti
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Pazienti.Remove(student);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

    }
}
