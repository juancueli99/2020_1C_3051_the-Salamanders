using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TGC.Group.Model
{
    public interface IEquipable : IInteractuable
    {
        void Equipar(Personaje personaje);

        void FinDuracion(Personaje personaje);

        void DisminuirDuracion();

        float getDuracion();

        float getValorLuminico();

        float getValorAtenuacion();
        Color getLuzColor();

    }
}
