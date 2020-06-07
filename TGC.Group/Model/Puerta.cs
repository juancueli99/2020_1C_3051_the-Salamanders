using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    class Puerta : IInteractuable
    {

        TgcMesh meshAsociado;
        public Puerta otroExtremo;// por si quieren cambiar ese if asqueroso
        TGCVector3 posicionEntrada = new TGCVector3(-1222, 15, -6857);
        TGCVector3 posicionSalida = new TGCVector3(-1200, 15, -7500);
        Sonido sonidoApertura;
        Sonido sonidoCierre;


        public Puerta(TgcMesh mesh)
        {
            sonidoApertura = new Sonido("puerta ruidosa, abrir.wav", -3900,false);
            sonidoCierre = sonidoApertura; //as tincho asked
            meshAsociado = mesh;
        }

        public TGCVector3 getPosition()
        {
            return meshAsociado.BoundingBox.PMin;
        }

        public void Interactuar(Personaje personaje)
        {
            this.Usar(personaje);

        }

        public void Usar(Personaje personaje)
        {
            if (personaje.estoyAdentro)
            {
                personaje.TeletrasportarmeA(posicionSalida);
                personaje.setearSonidosOutdoor();
                sonidoApertura.escucharSonidoActual(false);
            }
            else
            {
                personaje.TeletrasportarmeA(posicionEntrada);
                personaje.setearSonidosIndoor();
                sonidoCierre.escucharSonidoActual(false);
            }

            personaje.estoyAdentro = !personaje.estoyAdentro;

        }
    }
}
