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

        public void instanciarVelita()
        {
            drawer2D = new Drawer2D();
            sprite = new CustomSprite();
            sprite.Bitmap = new CustomBitmap(MediaDir + "Velita.png", D3DDevice.Instance.Device);

            var textureSize = sprite.Bitmap.Size;
            sprite.Position = new TGCVector2(FastMath.Max(D3DDevice.Instance.Width / 1.135f - textureSize.Width / 1.135f, 0), 
                FastMath.Max(D3DDevice.Instance.Height / 1.02f - textureSize.Height / 1.02f, 0));
            
            sprite.Scaling = new TGCVector2(0.45f, 0.45f);
        }

        public void instanciarLinternas(int porcentajeVela)
        {
            drawer2D = new Drawer2D();
            sprite = new CustomSprite();

            sprite.Bitmap = new CustomBitmap(MediaDir + "vidaUtilLinterna\\vidaUtilLinterna" + porcentajeVela + ".png", D3DDevice.Instance.Device);

            var textureSize = sprite.Bitmap.Size;
            sprite.Position = new TGCVector2(FastMath.Max(D3DDevice.Instance.Width / 1.001f - textureSize.Width / 1.001f, 0),
                FastMath.Max(D3DDevice.Instance.Height / 0.847f - textureSize.Height / 0.847f, 0));

            sprite.Scaling = new TGCVector2(0.55f, 0.55f);
        }

        public void instanciarLinternita()
        {
            drawer2D = new Drawer2D();
            sprite = new CustomSprite();
            sprite.Bitmap = new CustomBitmap(MediaDir + "Linternita.png", D3DDevice.Instance.Device);

            var textureSize = sprite.Bitmap.Size;
            sprite.Position = new TGCVector2(FastMath.Max(D3DDevice.Instance.Width / 1.135f - textureSize.Width / 1.135f, 0),
                FastMath.Max(D3DDevice.Instance.Height / 0.99f - textureSize.Height / 0.99f, 0));

            sprite.Scaling = new TGCVector2(0.45f, 0.45f);
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
