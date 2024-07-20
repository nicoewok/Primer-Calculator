using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimerCalculator
{
    internal class Primer
    {
        public int position { get; }
        public string sequence { get; }
        public float melting_temp { get; }
        public float delta_g { get; }
        public float gc_count { get; }

        public Primer(int position, string sequence, float melting_temp, float delta_g, float gc_count)
        {
            this.position = position;
            this.sequence = sequence;
            this.melting_temp = melting_temp;
            this.delta_g = delta_g;
            this.gc_count = gc_count;
        }
    }
}
