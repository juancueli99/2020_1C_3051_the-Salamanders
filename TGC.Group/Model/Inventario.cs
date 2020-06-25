using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model
{
    static class Inventario
    {
        public static List<IEquipable> inventario = new List<IEquipable>() { new ItemVacioDefault()};

        public static void objetoSiguiente(Personaje personaje)
        {
            IEquipable itemEnMano = inventario.First(); //Creo que First no lo popea de la lista
            itemEnMano.Apagar(personaje);
            inventario.RemoveAt(0); //Por eso hago esto
            inventario.Add(itemEnMano);
            IEquipable nuevoItemEnMano = inventario.First();
            personaje.setItemEnMano(nuevoItemEnMano);
            nuevoItemEnMano.Encender(personaje);
        }
    }
}
