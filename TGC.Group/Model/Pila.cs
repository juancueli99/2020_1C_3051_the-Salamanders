using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    class Pila : IInteractuable
    {
        private TgcMesh mesh;
        public Pila(TgcMesh meshAsociado)
        {
            this.mesh = meshAsociado;
        }

        public TGCVector3 getPosition()
        {
            return mesh.BoundingBox.PMin;
        }

        public void Interactuar(Personaje personaje)
        {
            personaje.objetosInteractuables.Add(this);
            eliminarMesh();
        }

        public void Usar(Personaje personaje)
        {
            if (personaje.objetosInteractuables.Any(objeto => objeto is Linterna))
            {
                var linterna = (Linterna)personaje.objetosInteractuables.Find(objeto => objeto is Linterna);
                linterna.Recargar();

                personaje.objetosInteractuables.Remove(this);
            }

        }
        private void eliminarMesh()
        {
            TGCVector3 posicionDeBorrado = new TGCVector3(0, -4000, 0);
            mesh.Move(posicionDeBorrado);
            mesh.updateBoundingBox();
            mesh.UpdateMeshTransform();
            //no hago el dispose aca porque sino lo tendria que sacar del TGCScene y no se si quiero hacer eso

        }
    }
}
