using System.Collections.Generic;

namespace Uplift.Models.ViewModels
{
    public class OrderViewModel
    {
        public OrderHeader OrderHeader { get; set; }

        public IEnumerable<OrderDetails> OrderDetails { get; set; }
    }
}
