using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Sound;
using TGC.Core.Direct3D;
using Microsoft.DirectX.DirectSound;
using Microsoft.DirectX.Direct3D;

namespace TGC.Group.Model
{
    public class Sonido
    {
        TgcStaticSound player = new TgcStaticSound();
        String media = "..\\..\\..\\Media\\Sonidos\\";
        String ReproduccionActual;
        private static TgcDirectSound DirectSound = new TgcDirectSound();
        public Sonido(String archivo, bool loop) {

            ReproduccionActual = archivo;
            var device= GameModel.deviceMusica;
            player.loadSound(this.media + archivo,device);
            player.play(loop);
            
        }
        public Sonido(String archivo,int volumen, bool loop)
        {// esto no esta andando cunado metes el volumen en el loadSound
            ReproduccionActual = archivo;
            
            var device = GameModel.deviceMusica;            
            player.loadSound(this.media + archivo, volumen, device);


        }
        public void DetenerSonido() {
            player.stop();
        }

        public void CambiarSonido(String archivo, bool loop) {
            this.DetenerSonido();
            this.CargarSonido(archivo);
            this.escucharSonidoActual(loop);
        
        }

        public void escucharSonidoActual(bool loop)
        {
            player.play(loop);
        }

        public void CargarSonido(string archivo)
        {
            ReproduccionActual = archivo;
            var device = GameModel.deviceMusica;
            player.loadSound(media + archivo, device);
        }

    }
}
