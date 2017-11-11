using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2017_07_28_objektu_rinkinys_su_sqlite
{
    public class Telefonas
    {
        public string Modelis { get; private set; }
        public double Istrizaine { get; private set; }
        public int Atmintis { get; private set; }
        public int BaterijosTalpa { get; private set; }

        public Telefonas(string modelis, double istrizaine, int atmintis, int baterijosTalpa)
        {
            Modelis = modelis;
            Istrizaine = istrizaine;
            Atmintis = atmintis;
            BaterijosTalpa = baterijosTalpa;
        }
    }
}
