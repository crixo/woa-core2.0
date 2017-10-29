using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Woa.Data;
using Woa.Models;

using Microsoft.EntityFrameworkCore;

namespace Woa.ViewComponents
{
    [ViewComponent(Name = "Paziente")]
    public class PazienteComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int pazienteId)
        {
            var model = new Paziente{ID = pazienteId};
            return View(model);
        }
    }
}
