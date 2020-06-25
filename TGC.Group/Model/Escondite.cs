using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.PrivateImplementationDetails;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using TGC.Group.Model;

namespace TGC.Group
{
    class Escondite : IInteractuable
    {
        public TgcMesh unEscondite;
        TGCVector3 posicionDeEntrada;
        private GameModel gameModel;
        public Escondite(TgcMesh unBarril, GameModel gameModel)
        {
            this.unEscondite = unBarril;
            this.gameModel = gameModel;
        }
        
        public TGCVector3 getPosition()
        {
            return this.unEscondite.BoundingBox.PMax;
        }

        public void Interactuar(Personaje personaje)
        {
            this.Usar(personaje);
        }

        public void Usar(Personaje personaje)
        {
            if (personaje.estoyEscondido)
            {
                this.Salir(personaje);
                personaje.estoyEscondido = false;
            }
            else
            {
                this.Entrar(personaje);
                personaje.estoyEscondido = true;
            }
        }

        public void Salir(Personaje personaje)
        {
            //Resetear tiempo sin luz
            personaje.tiempoSinLuz = 0;

            //Sacar al personaje del barril
            personaje.TeletrasportarmeA(posicionDeEntrada);
        }

        public void Entrar(Personaje personaje)
        {
            //Meter al personaje en el barril
            gameModel.effectPosProcesado.Technique = "PostProcessDefault";
            posicionDeEntrada = new TGCVector3(personaje.getPosition());
            TGCVector3 posicion = new TGCVector3(getPosition().X - 150, personaje.Position.Y, getPosition().Z - 150);
            personaje.TeletrasportarmeA(posicion);
            gameModel.estatica.DetenerSonido();
            gameModel.humanHeartbeat.escucharSonidoActual(false);
            gameModel.respiracion.escucharSonidoActual(false);
        }

        public void updatearMiPropiaLuz(Escenario escenario, Personaje personaje)
        {
            Microsoft.DirectX.Direct3D.Effect currentShader;
            currentShader = TGCShaders.Instance.TgcMeshPointLightShader;

            TGCVector3 radioDeLuz = new TGCVector3(50, 0, 50);

            radioDeLuz += unEscondite.BoundingBox.PMin;

            var listMeshesCercanos = escenario.tgcScene.Meshes.FindAll(unMesh => unMesh.BoundingBox.PMin.X < radioDeLuz.X && unMesh.BoundingBox.PMin.Z < radioDeLuz.Z);

            foreach (TgcMesh mesh in listMeshesCercanos)
            {
                mesh.Effect = currentShader;
                mesh.Technique = TGCShaders.Instance.GetTGCMeshTechnique(mesh.RenderType);

                // Estos son paramentros del current shader, si cambias el shader chequear los parametros o rompe
                mesh.Effect.SetValue("materialEmissiveColor", ColorValue.FromColor(Color.Black));
                mesh.Effect.SetValue("materialAmbientColor", ColorValue.FromColor(Color.White));
                mesh.Effect.SetValue("materialDiffuseColor", ColorValue.FromColor(Color.White));
                mesh.Effect.SetValue("materialSpecularColor", ColorValue.FromColor(Color.White));
                mesh.Effect.SetValue("materialSpecularExp", 9f);
                mesh.Effect.SetValue("eyePosition", TGCVector3.TGCVector3ToFloat4Array(personaje.eye));
                mesh.Effect.SetValue("lightAttenuation", 0.3f);
                mesh.Effect.SetValue("lightColor", ColorValue.FromColor(Color.Blue));
                mesh.Effect.SetValue("lightIntensity", 50f);
                mesh.Effect.SetValue("lightPosition", TGCVector3.TGCVector3ToFloat4Array(radioDeLuz));
            }
        }
    }
}
