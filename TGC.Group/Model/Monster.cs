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
        public void InstanciarMonster()
        {
            var loader = new TgcSceneLoader();
            var scene2 = loader.loadSceneFromFile(MediaDir + "Modelame\\GhostGrande-TgcScene.xml"); //Con demon no funca, aca rompe

            //Solo nos interesa el primer modelo de esta escena (tiene solo uno)
            ghost = scene2.Meshes[0];

            ghost.Position = new TGCVector3(200, -350, 100);
            //ghost.Transform = TGCMatrix.Translation(0, 5, 0);
        }

        public void RenderMonster()
        {
            ghost.Render();
        }

        public void DisposeMonster()
        {
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
            ghost.Position = new TGCVector3(0, -2000, 0); //Lo mando abajo del mapa 
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
                else if (diferenciaEnX > 0 && diferenciaEnZ > 0)
                {
                    //1er Cuadrante
                }
                else if (diferenciaEnX > 0 && diferenciaEnZ < 0)
                {
                    //4to Cuadrante
                    anguloRotacion = 2 * (float)Math.PI - anguloRotacion;
                }
                else
                {
                    //2do Cuadrante
                    anguloRotacion = (float)Math.PI - anguloRotacion;
                }

                ghost.RotateY(anguloRotacion);

                this.lookAt = personaje.getPosition();
            }
        }
    }
}
