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
    public class Sprite
    {
        String MediaDir = "..\\..\\..\\Media\\";
        private Drawer2D drawer2D;
        private CustomSprite sprite;       
        
        public void instanciarMenu()
        {
            drawer2D = new Drawer2D();
            sprite = new CustomSprite();


            sprite.Bitmap = new CustomBitmap(MediaDir + "pressSpacebarToContinue.jpg", D3DDevice.Instance.Device);

            var textureSize = sprite.Bitmap.Size;
            sprite.Position = new TGCVector2(FastMath.Max(D3DDevice.Instance.Width / 2 - textureSize.Width / 2, 0), FastMath.Max(D3DDevice.Instance.Height / 2 - textureSize.Height / 2, 0));

            Console.WriteLine("Width --> " + D3DDevice.Instance.Width);
            Console.WriteLine("Height --> " + D3DDevice.Instance.Height);

            sprite.Scaling = new TGCVector2(942.54f/D3DDevice.Instance.Width, 486.45f/D3DDevice.Instance.Height);
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
