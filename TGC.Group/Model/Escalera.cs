using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;


namespace TGC.Group.Model
{
     public class Escalera: IInteractuable
    {
        TgcMesh escalera;
        public TGCVector3 posicionArriba;
        TGCVector3 posicionAbajo;


        public Escalera(TgcMesh meshNuevo) {
            this.escalera = meshNuevo;
            posicionAbajo = new TGCVector3(meshNuevo.BoundingBox.PMin.X, 15f, meshNuevo.BoundingBox.PMin.Z);
            posicionArriba = new TGCVector3(meshNuevo.BoundingBox.PMin.X, 515f, meshNuevo.BoundingBox.PMin.Z);
        }

        public TgcMesh devolverEscalera(Escenario escenario)
        {
           escalera = escenario.tgcScene.Meshes.Find(mesh => mesh.Name.Equals("EscaleraMetalMovil"));

            //Console.WriteLine(escalera.Name);
            return escalera;
        }

        public TGCVector3 getPosition()
        {
            return escalera.BoundingBox.PMin;
        }

        public void Interactuar(Personaje personaje)
        {
            this.Usar(personaje);
        }

        public void Usar(Personaje personaje)
        {

            TGCVector3 posicion;
            if (personaje.estoyArriba) 
            {
                posicion = posicionAbajo;
            }
            else
            {
                posicion = posicionArriba;
                posicion.Z -= 250;
            }
            personaje.estoyArriba = !personaje.estoyArriba;
            personaje.TeletrasportarmeA(posicion);
        }
    }
}
