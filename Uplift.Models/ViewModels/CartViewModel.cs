using System.Collections.Generic;

namespace Uplift.Models.ViewModels
{
    public class CartViewModel
    {
        public IList<Service> ServicesInCart { get; set; }

        public OrderHeader OrderHeader { get; set; }

        public double TotalPrice { get; set; }  
    }
}
