using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace TGC.Group.Model
{
    public interface IInteractuable
    {
        void Interactuar(Personaje personaje);

        void Usar(Personaje personaje);
        TGCVector3 getPosition();
    }
}
