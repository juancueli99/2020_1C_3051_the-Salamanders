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
using TGC.Core.Textures;
using TGC.Core.Camara;
using TGC.Core.Example;
using TGC.Core.Utils;
using TGC.Core.SceneLoader;
using TGC.Core.Text;

/*
namespace TGC.Group.Model
{
    class Menu
    {
        private TextBox drawer2D;
        private CustomSprite sprite;
        public void instanciarMenu()
        {
            drawer2D = new TextBox();

            //Crear Sprite
            sprite = new CustomSprite();
            sprite.Bitmap = new CustomBitmap(MediaDir + "\\Texturas\\LogoTGC.png", D3DDevice.Instance.Device);

            //Ubicarlo centrado en la pantalla
            var textureSize = sprite.Bitmap.Size;
            sprite.Position = new TGCVector2(FastMath.Max(D3DDevice.Instance.Width / 2 - textureSize.Width / 2, 0), FastMath.Max(D3DDevice.Instance.Height / 2 - textureSize.Height / 2, 0));

            //Modifiers para variar parametros del sprite
            positionModifier = AddVertex2f("position", TGCVector2.Zero, new TGCVector2(D3DDevice.Instance.Width, D3DDevice.Instance.Height), sprite.Position);
            scalingModifier = AddVertex2f("scaling", TGCVector2.Zero, new TGCVector2(4, 4), sprite.Scaling);
            rotationModifier = AddFloat("rotation", 0, 360, 0);

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
*/