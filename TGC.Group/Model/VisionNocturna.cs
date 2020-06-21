using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    class VisionNocturna : IInteractuable
    {
        private TgcMesh mesh;
        private GameModel gameModel;
        private bool nvActivada = false;

        public VisionNocturna(TgcMesh mesh, GameModel gameModel)
        {
            this.mesh = mesh;
            this.gameModel = gameModel;
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
            if (nvActivada)
            {
                //muestro el post procesado
                nvActivada = false;
            }
            else
            {
                //saco el post procesado
                nvActivada = true;
            }
        }

        private void eliminarMesh()
        {
            TGCVector3 posicionDeBorrado = new TGCVector3(0, -4000, 0);
            mesh.Move(posicionDeBorrado);
            mesh.updateBoundingBox();
            mesh.UpdateMeshTransform();
        }
    }
}
