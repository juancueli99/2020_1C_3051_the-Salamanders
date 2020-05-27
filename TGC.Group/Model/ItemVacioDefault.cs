using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace TGC.Group.Model
{
    class ItemVacioDefault : IEquipable
    {
        public void DisminuirDuracion()
        {
        }

        public void Equipar(Personaje personaje)
        {
        }

        public void FinDuracion(Personaje personaje)
        {
        }

        public float getDuracion()
        {
            return 0;
        }

        public Color getLuzColor()
        {
            return Color.White;
        }

        public TGCVector3 getPosition()
        {
            return new TGCVector3(0,0,0) ;
        }

        public float getValorAtenuacion()
        {
            return 0.3f;
        }

        public float getValorLuminico()
        {
            return 0;
        }

        public void Interactuar(Personaje personaje)
        {
        }

        public void Usar(Personaje personaje)
        {
        }
    }
}
