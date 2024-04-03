using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal interface IBolygo
    {
        double X { get; set; }
        double Y { get; set; }
        double Z { get; set; }

        double Tavolsag(IBolygo egyik, IBolygo masik);

        bool Szabad { get; set; }




    }
}
