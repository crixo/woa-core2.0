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
    [ViewComponent(Name = "PazienteAnagrafica")]
    public class PazienteAnagraficaComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(int pazienteId)
        {
            var model = new Paziente{ID = pazienteId};
            return Task.FromResult<IViewComponentResult>(View(model));
        }
    }
}
