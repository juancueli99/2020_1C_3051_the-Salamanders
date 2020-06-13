using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    internal class Maleta : IInteractuable
    {
        private TgcMesh mesh;

        public Maleta(TgcMesh mesh)
        {
            this.mesh = mesh;
        }

        public TGCVector3 getPosition()
        {
            return mesh.BoundingBox.PMin;
        }

        public void Interactuar(Personaje personaje)
        {
            this.Usar(personaje);
        }

        public void Usar(Personaje personaje)
        {
            personaje.YouWin();
        }
    }
}