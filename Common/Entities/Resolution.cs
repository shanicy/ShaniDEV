using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public struct Resolution
    {
        public int Height { get; set; }
        public int Width { get; set; }

        public Resolution(int w, int h)
        {
            Width = w;
            Height = h;
        }

        public override string ToString()
        {
            return string.Format("{0}X{1}", Height, Width);
        }
    }
}
