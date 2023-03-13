using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public delegate double FRaw(double x);
    
   public enum FRawEnum
    {
        linear,
        cube,
        random
    }
}
