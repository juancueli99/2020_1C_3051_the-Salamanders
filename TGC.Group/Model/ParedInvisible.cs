using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SkeletalAnimation;
using TGC.Core.Camara;
using TGC.Core.Input;
using Microsoft.DirectX.Direct3D;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class ParedInvisible
    {
        public TgcMesh paredInvisible;
        String MediaDir = "..\\..\\..\\Media\\";

        public void InstanciarPared(Escalera escalera)
        {
            var loader = new TgcSceneLoader();
            var scene2 = loader.loadSceneFromFile(MediaDir + "Modelame\\ParedCastillo-TgcScene.xml");

            paredInvisible = scene2.Meshes[0];

            TGCVector3 traslacion = new TGCVector3((escalera.posicionArriba.X) - 850, (escalera.posicionArriba.Y) - 250, escalera.posicionArriba.Z);

            TGCVector3 escalado = new TGCVector3(paredInvisible.Scale.X * 27, paredInvisible.Scale.Y * 5, paredInvisible.Scale.Z +10);

            //paredInvisible.BoundingBox.scaleTranslate(traslacion, escalado);

            paredInvisible.Transform = TGCMatrix.Scaling(escalado) * 
                TGCMatrix.RotationYawPitchRoll(paredInvisible.Rotation.Y, paredInvisible.Rotation.X, paredInvisible.Rotation.Z) *  
                TGCMatrix.Translation(traslacion);

            paredInvisible.BoundingBox.transform(paredInvisible.Transform);

            // paredInvisible.updateBoundingBox();

        }

        public void RenderPared()
        {
            //No le hago el render porque justamente quiero que sea invisible.
            paredInvisible.Render();
        }

        public void DisposePared()
        {
            paredInvisible.Dispose();
        }
    }
}
