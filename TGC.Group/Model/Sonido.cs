using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Sound;
using TGC.Core.Direct3D;

namespace TGC.Group.Model
{
    class Sonido
    {
        TgcStaticSound player = new TgcStaticSound();
        String MediaDir = "..\\..\\..\\Media\\";
        String ReproduccionActual;
        private static TgcDirectSound DirectSound = new TgcDirectSound();
        public Sonido(String archivo, bool loop) {

            ReproduccionActual = archivo;
            var device= DirectSound.DsDevice;
            player.loadSound(MediaDir + archivo,device);
            player.play(loop);
            
        }
        public void DetenerSonido() {
            player.stop();
        }

        public void CambiarSonido(String archivo, bool loop) {
            this.DetenerSonido();
            this.CargarSonido(archivo);
            this.escucharSonidoActual(loop);
        
        }

        private void escucharSonidoActual(bool loop)
        {
            player.play(loop);
        }

        private void CargarSonido(string archivo)
        {
            var device = DirectSound.DsDevice;
            player.loadSound(MediaDir + archivo, device);
        }
    }
}
