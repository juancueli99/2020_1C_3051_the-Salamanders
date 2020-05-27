using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    class Nota : IInteractuable
    {
        private TgcMesh mesh;

        public Nota(TgcMesh mesh)
        {
            this.mesh = mesh;
        }

        public void Interactuar(Personaje personaje)
        {
            personaje.cantidadNotas++;
            eliminarMesh();
        }
        public TGCVector3 getPosition()
        {
            return mesh.BoundingBox.PMin;
        }

        public void Usar(Personaje personaje) { }

        private void eliminarMesh()
        {
            TGCVector3 posicionDeBorrado = new TGCVector3(0, -4000, 0);
            mesh.Move(posicionDeBorrado);
            mesh.updateBoundingBox();
            mesh.UpdateMeshTransform();
        }

    }
}
