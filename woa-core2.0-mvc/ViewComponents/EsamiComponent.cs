using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Woa.Data;

using Microsoft.EntityFrameworkCore;

namespace Woa.ViewComponents
{
    [ViewComponent(Name = "Esami")]
    public class EsamiComponent : ViewComponent
    {
        public WoaContext _context;

        public EsamiComponent(WoaContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int consultoId)
        {
            ViewBag.ConsultoId = consultoId;
            var list = await _context.Esami
                                        .Include(x => x.Tipo)
                                       .Where(x => x.ConsultoId == consultoId)
                                        .AsNoTracking()
                                       .ToListAsync();
            return View(list);
        }
    }
}
