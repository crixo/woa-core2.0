using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Woa.ViewComponents
{
    [ViewComponent(Name = "AnamnesiRemote")]
    public class AnamnesiRemoteComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int pazienteId)
        {
            return View();
        }
    }
}
