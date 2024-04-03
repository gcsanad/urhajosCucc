using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class Bolygo : IBolygo
    {


        double x, y, z, ertek;
        public bool Szabad { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Bolygo(string sor)
        {
            string[] gyongyok = sor.Split(';');
            this.x = Convert.ToInt32(gyongyok[0]);
            this.y = Convert.ToInt32(gyongyok[1]);
            this.z = Convert.ToInt32(gyongyok[2]);
            this.ertek = Convert.ToInt32(gyongyok[3]);
        }


        public double Tavolsag(IBolygo egyik, IBolygo masik)
        {
            return Math.Sqrt(Math.Pow(egyik.X - masik.X, 2) + Math.Pow(egyik.Y - masik.Y, 2) + Math.Pow(egyik.Z - masik.Z, 2));
            
        }


        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public double Z { get => z; set => z = value; }
        public double Ertek { get => ertek; set => ertek = value; }
    }
}
