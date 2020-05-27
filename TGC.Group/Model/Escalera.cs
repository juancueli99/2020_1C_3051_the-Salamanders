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

        public List<TgcMesh> escalones= new List<TgcMesh> { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };
        public TgcMesh escalonActual;
        public TgcMesh escalonFinal;
        public TgcMesh escalonInicial;
        


        public void pasarPorEscalon(Personaje personaje)
        {
            
            if (estasFrenteAlEscalon(personaje))
            {

                //podes Seguir subiendo

                //mover el personaje arriba del escalon actual
                //TODO
                var posicionY = personaje.Position.Y + escalonActual.BoundingBox.PMax.Y;
                escalonActual = escalonSiguiente();
                TGCVector3 nuevaPosicion = new TGCVector3(personaje.Position.X, posicionY, personaje.Position.Z);
                //solo lo levanto para que no quede pegado a la escalera el x y el z se van a modificar solos con el update de la camara
                personaje.TeletrasportarmeA(nuevaPosicion);

            }

        }

        private bool estasFrenteAlEscalon(Personaje personaje)
        {
            var escalonMasCercano = escalones.OrderBy(mesh => personaje.DistanciaHacia(mesh)).First();
            return escalonMasCercano.Equals(escalonActual);
        }
        private TgcMesh escalonSiguiente()
        {
            escalones.OrderBy(mesh => mesh.Name);
            int index = escalones.FindIndex(mesh => mesh.Equals(escalonActual));
            if (index < escalones.Count())
            {
                index++;
            }
            return escalones.ElementAt(index);
        }

        public void Interactuar(Personaje personaje)
        {
            this.Usar(personaje);
        }

        public void Usar(Personaje personaje)
        {
            personaje.estoyUsandoEscaleras = true;
            personaje.setCamera(personaje.eye,new TGCVector3(escalonActual.BoundingBox.PMin.X,personaje.eye.Y, escalonActual.BoundingBox.PMin.Z));
            personaje.meshPersonaje.updateBoundingBox();
            
        }

        private void DejarDeUsar(Personaje personaje)
        {
            personaje.estoyUsandoEscaleras = false;
            var escalon=this.escalonFinal;
            escalonFinal = escalonActual;
            escalonActual = escalon;
        }

        public void Mover(Personaje personaje) {
            if (personaje.eye.Equals(escalonFinal.BoundingBox.PMin))
            {
                personaje.setCamera(personaje.eye, new TGCVector3(escalonActual.BoundingBox.PMin.X+100, personaje.eye.Y, escalonActual.BoundingBox.PMin.Z+100));
                personaje.meshPersonaje.updateBoundingBox();
                DejarDeUsar(personaje);
            }
            else {

                var escalonSiguiente=this.escalonSiguiente();
                personaje.setCamera(personaje.eye, new TGCVector3(escalonSiguiente.BoundingBox.PMin.X + 100, personaje.eye.Y+1, escalonSiguiente.BoundingBox.PMin.Z + 100));
                personaje.meshPersonaje.updateBoundingBox();

            }
        
        
        }


        public TGCVector3 getPosition()
        {
            return escalonActual.BoundingBox.PMin;
        }
    }
}
