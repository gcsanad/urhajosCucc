using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class Gyongy
    {
        int x, y, z, ertek;

        public Gyongy(string sor)
        {
            string[] gyongyok = sor.Split(';');
            this.x = Convert.ToInt32(gyongyok[0]);
            this.y = Convert.ToInt32(gyongyok[1]);
            this.z = Convert.ToInt32(gyongyok[2]);
            this.ertek = Convert.ToInt32(gyongyok[3]);
        }

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Z { get => z; set => z = value; }
        public int Ertek { get => ertek; set => ertek = value; }
    }
}
