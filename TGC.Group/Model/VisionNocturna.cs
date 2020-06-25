using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;

namespace TGC.Group.Model
{
    class VisionNocturna : IInteractuable, IEquipable
    {
        private TgcMesh mesh;
        private GameModel gameModel;
        private bool nvActivada = false;

        public VisionNocturna(TgcMesh mesh, GameModel gameModel)
        {
            this.mesh = mesh;
            this.gameModel = gameModel;
        }

        public void DisminuirDuracion(Personaje personaje)
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

        public float getValorLuminico()
        {
            return 600f;
        }

        public float getValorAtenuacion()
        {
            return 0.5f;
        }

        public Color getLuzColor()
        {
            return Color.LightYellow;
        }

        public void Interactuar(Personaje personaje)
        {
            Inventario.inventario.Add(this);
            eliminarMesh();
        }

        public void Usar(Personaje personaje)
        {
            var d3dDevice = D3DDevice.Instance.Device;

            if (nvActivada)
            {
                //muestro el post procesado
                gameModel.effectPosProcesado.Technique = "PostProcessNightVision";
                nvActivada = false;
            }
            else
            {
                //saco el post procesado
                gameModel.effectPosProcesado.Technique = "PostProcessMonster";

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

        public TGCVector3 getPosition()
        {
            return mesh.BoundingBox.PMin;
        }

        public void Apagar(Personaje personaje)
        {
            gameModel.effectPosProcesado.Technique = "PostProcessMonster";

            nvActivada = true;
        }

        public void Encender(Personaje personaje)
        {
            gameModel.effectPosProcesado.Technique = "PostProcessNightVision";
            nvActivada = false;
        }
    }
}
