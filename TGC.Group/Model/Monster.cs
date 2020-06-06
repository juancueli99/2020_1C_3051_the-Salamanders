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
    public class Monster
    {
        public TgcMesh ghost;
        String MediaDir = "..\\..\\..\\Media\\";
        TGCVector3 lookAt = new TGCVector3();
        private float velocidad=2;

        public void InstanciarMonster(monstruos tipo)
        {
            ghost = ConfiguradorMonstruo.ConfigurarMonstruo(tipo);

            this.lookAt = new TGCVector3(ghost.Position);
        }

        public void RenderMonster()
        {
            ghost.Render();
        }

        public void DisposeMonster()
        {
            this.Desaparecer();
            ghost.Dispose();
        }

        //Cuando el player no usa una fuente luminosa en X tiempo
        public bool Aparecer(Personaje personaje)
        {
            if (!personaje.tieneLuz && !personaje.estoyEscondido && personaje.tiempoSinLuz > 3500)
            {
                if (personaje.tiempoSinLuz == 4000)
                {
                    this.ModificarPosicion(personaje);
                }

                if (personaje.tiempoSinLuz == 5000)
                {
                    //El monster aparece detrás del personaje
                    this.AparecerAlLadoDelPersonaje(personaje);
                    personaje.AsustarAlPersonaje(ghost.Position);
                    return true;
                }
            }
            return false;
        }

        //Cuando el player usa una fuente luminosa o llega a un refugio
        public void Desaparecer()
        {
            //ghost.Position = new TGCVector3(0, -2000, 0);
            ghost.Transform = TGCMatrix.Translation(new TGCVector3(0, -2000, 0));
             //Lo mando abajo del mapa 
        }

        public void ModificarPosicion(Personaje personaje)
        {
            var posicionLejana = new TGCVector3(1500, -350, 1500);

            //El monster se acerca al personaje
            ghost.Position = personaje.getPosition() + posicionLejana;

            //Roto el mesh del monster
            this.RotarMesh(personaje);
        }

        public void AparecerAlLadoDelPersonaje(Personaje personaje)
        {
            var posicionLejana = new TGCVector3(500, -350, 500);

            //El monster se acerca al personaje
            ghost.Position = personaje.getPosition() + posicionLejana;

            //Roto el mesh del monster
            this.RotarMesh(personaje);
        }

        public void RotarMesh(Personaje personaje)
        {

            if (!this.lookAt.Equals(personaje.getPosition()))
            {
                float diferenciaEnX = personaje.getPosition().X - ghost.Position.X;
                float diferenciaEnZ = personaje.getPosition().Z - ghost.Position.Z;

                float anguloRotacion = (float)Math.Atan(diferenciaEnX / diferenciaEnZ);

                if (diferenciaEnX < 0 && diferenciaEnZ < 0)
                {
                    //3er Cuadrante
                    anguloRotacion = (float)Math.PI + anguloRotacion;
                }
                else
                {
                    if (diferenciaEnX < 0 && diferenciaEnZ > 0)
                    {
                        //2do Cuadrante
                        anguloRotacion = 2*(float)Math.PI + anguloRotacion;
                    }
                    else
                    {
                        if (diferenciaEnX > 0 && diferenciaEnZ < 0)
                        {
                            //4to Cuadrante
                            anguloRotacion = (float)Math.PI + anguloRotacion;
                        }
                    }
                }

               

                ghost.Transform = TGCMatrix.Scaling(ghost.Scale) *
                 TGCMatrix.RotationYawPitchRoll(anguloRotacion, ghost.Rotation.X, ghost.Rotation.Z)
                 * TGCMatrix.Translation(ghost.Position);


                this.lookAt = personaje.getPosition();
            }
        }

        internal void InstanciarMonster(Personaje personaje, TGCVector3 posicionDeAlejamiento, monstruos tipo)
        {
            //Solo nos interesa el primer modelo de esta escena (tiene solo uno)
            ghost = ConfiguradorMonstruo.ConfigurarMonstruo(tipo);
            ghost.Position = new TGCVector3(posicionDeAlejamiento);
            this.lookAt= new TGCVector3(ghost.Rotation);
            RotarMesh(personaje);
            
        }

        internal void HuirDe(Personaje personaje, float elapsedTime)
        {
            var posicionDeBorrado = new TGCVector3(ghost.Position.X+2000, ghost.Position.Y, ghost.Position.Z - 15000);
            ghost.Move(posicionDeBorrado);
            ghost.updateBoundingBox();
            ghost.UpdateMeshTransform();

        }

        internal void MirarA(Personaje personaje, float elapsedTime)
        {
           // var vectorRotacion = new TGCVector3(personaje.Position.X * elapsedTime,ghost.Position.Y, personaje.Position.Z * elapsedTime);
            this.RotarMesh(personaje);
        }
    }
}
