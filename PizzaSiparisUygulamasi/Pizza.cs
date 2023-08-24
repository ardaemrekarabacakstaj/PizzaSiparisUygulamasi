using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PizzaSiparisUygulamasi
{
    public class Pizza
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public List<string> Toppings { get; set; }

        public override string ToString()
        {
            return $"{Name} ({Size})\n{string.Join(", ", Toppings)}";
        }
    }
    public class PizzaOrder
    {
        public string CustomerName { get; set; }
        public string DeliveryAddress { get; set; }
        public string OrderDate { get; set; }
        public List<Pizza> Pizzas { get; set; }
        public double TotalAmount { get; set; }

        public override string ToString()
        {
            return $"{CustomerName} ({DeliveryAddress})\n{string.Join(", ", Pizzas)} - {TotalAmount} - {OrderDate} - ";
        }

    }
}
