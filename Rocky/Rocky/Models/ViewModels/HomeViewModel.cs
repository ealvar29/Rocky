using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Rocky.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable <Product> Products { get; set; }
        public IEnumerable <Category> Categories { get; set; }

        public IEnumerable <ApplicationType> Applications { get; set; }

    }
}