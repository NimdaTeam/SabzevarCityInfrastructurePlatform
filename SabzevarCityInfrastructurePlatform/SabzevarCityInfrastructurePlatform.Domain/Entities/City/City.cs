using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.EntityBase;
using NetTopologySuite.Geometries;

namespace Domain.Entities.City
{
    public class City:EntityBase
    {
        public string Name { get; set; }

        public Polygon CityArea { get; set; }
    }
}
