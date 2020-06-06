using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.Text;

namespace TGC.Group.Model
{
    public class HUD
    {
        String MediaDir = "..\\..\\..\\Media\\";
        private Drawer2D drawer2D;
        private CustomSprite sprite;

        public void instanciarNotas(int numeroDeNota)
        {
            drawer2D = new Drawer2D();

            sprite = new CustomSprite();

            sprite.Bitmap = new CustomBitmap(MediaDir + "Notas\\Notas" + numeroDeNota+".png", D3DDevice.Instance.Device);

            var textureSize = sprite.Bitmap.Size;
            sprite.Position = new TGCVector2(FastMath.Max(D3DDevice.Instance.Width /1 - textureSize.Width /1, 0), 
                FastMath.Max(D3DDevice.Instance.Height /0.8f - textureSize.Height /0.8f, 0));

            sprite.Scaling = new TGCVector2(0.4f, 0.4f);

        }

        public void instanciarVelas(int porcentajeVela)
        {
            drawer2D = new Drawer2D();

            sprite = new CustomSprite();

            sprite.Bitmap = new CustomBitmap(MediaDir + "vidaUtilVela\\vidaUtilVela" + porcentajeVela + ".png", D3DDevice.Instance.Device);

            var textureSize = sprite.Bitmap.Size;
            sprite.Position = new TGCVector2(FastMath.Max(D3DDevice.Instance.Width / 1 - textureSize.Width / 1, 0),
                FastMath.Max(D3DDevice.Instance.Height / 0.83f - textureSize.Height / 0.83f, 0));

            sprite.Scaling = new TGCVector2(0.5f, 0.5f);

        }

        public void updateSprite()
        {
        }

        public void renderSprite()
        {
            //Iniciar dibujado de todos los Sprites de la escena (en este caso es solo uno)
            drawer2D.BeginDrawSprite();

            //Dibujar sprite (si hubiese mas, deberian ir todos aquí)
            drawer2D.DrawSprite(sprite);

            //Finalizar el dibujado de Sprites
            drawer2D.EndDrawSprite();
        }

        public void disposeSprite()
        {
            sprite.Dispose();
        }
    }
}
