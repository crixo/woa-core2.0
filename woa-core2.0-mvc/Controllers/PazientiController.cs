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

    }
}
