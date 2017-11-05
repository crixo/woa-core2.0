using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Woa.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Woa.Controllers
{
    public class UtilityController : Controller
    {
        private readonly WoaContext _context;

        public UtilityController(WoaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: /<controller>/
        public IActionResult ReplaceHtmlEncode()
        {
            foreach(var p in _context.Pazienti)
            {
                p.Nome = Replace(p.Nome);
                p.Cognome = Replace(p.Cognome);
                p.Indirizzo = Replace(p.Indirizzo);
                p.Citta = Replace(p.Citta);
                p.Professione = Replace(p.Professione);
            }
            _context.SaveChanges();

            foreach (var e in _context.AnamnesiRemote)
            {
                e.Descrizione = Replace(e.Descrizione);
            }
            _context.SaveChanges();

            foreach (var e in _context.AnamnesiProssime)
            {
                e.PrimaVolta = Replace(e.PrimaVolta);
                e.AltreTerapie = Replace(e.AltreTerapie);
                e.Durata = Replace(e.Durata);
                e.Familiarita = Replace(e.Familiarita);
                e.Irradiazione = Replace(e.Irradiazione);
                e.Localizzazione = Replace(e.Localizzazione);
                e.PeriodoInsorgenza = Replace(e.PeriodoInsorgenza);
                e.PrimaVolta = Replace(e.PrimaVolta);
                e.Tipologia = Replace(e.Tipologia);
                e.Varie = Replace(e.Varie);
            }
            _context.SaveChanges();

            foreach (var e in _context.Esami)
            {
                e.Descrizione = Replace(e.Descrizione);
            }
            _context.SaveChanges();

            foreach (var e in _context.Trattamenti)
            {
                e.Descrizione = Replace(e.Descrizione);
            }
            _context.SaveChanges();

            foreach (var e in _context.Valutazioni)
            {
                e.AkOrtodontica = Replace(e.AkOrtodontica);
                e.CranioSacrale = Replace(e.CranioSacrale);
                e.Strutturale = Replace(e.Strutturale);
            }
            _context.SaveChanges();

            return View();
        }

        private static string Replace(string input)
        {
            if (String.IsNullOrEmpty(input)) return input;

            var output = input;
            foreach(KeyValuePair<string,string> kv in HtmlEscapeToReplace)
            {
                output = output.Replace(kv.Key, kv.Value);
            }
            return output;
        }

        private static Dictionary<string, string> HtmlEscapeToReplace = new Dictionary<string, string>()
        {
            {"&#192;", "À"},
            {"&#193;", "Á"},
            {"&#200;", "È"},
            {"&#201;", "É"},
            {"&#204;", "Ì"},
            {"&#205;", "Í"},
            {"&#210;", "Ò"},
            {"&#211;", "Ó"},
            {"&#217;", "Ù"},
            {"&#218;", "Ú"},
            {"&#224;", "à"},
            {"&#225;", "á"},
            {"&#232;", "è"},
            {"&#233;", "é"},
            {"&#236;", "ì"},
            {"&#237;", "í"},
            {"&#243;", "ò"},
            {"&#244;", "ó"},
            {"&#249;", "ù"},
            {"&#250;", "ú"},

            {"&#38;", "&"},

            {"&#8364;", "€"},
        };
        /*

        */
    }
}
