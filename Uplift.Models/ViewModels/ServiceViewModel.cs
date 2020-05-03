using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Uplift.Models.ViewModels
{
    public class ServiceViewModel
    {
        public Service Service { get; set; }
        public IEnumerable<SelectListItem> FrequencySelectListItems { get; set; }
        public IEnumerable<SelectListItem> CategorySelectListItems { get; set; }
    }
}
