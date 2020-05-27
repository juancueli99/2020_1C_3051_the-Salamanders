
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using System.Windows.Forms;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using TGC.Core.Camara;
using TGC.Core.Example;
using TGC.Core.Utils;
using TGC.Core.PortalRendering;

    namespace TGC.Group.Model
{
        public class SpotLight
        {
        /*private TGCVertex3fModifier lightDirModifier;
        private TGCColorModifier lightColorModifier;
        private TGCFloatModifier lightIntensityModifier;
        private TGCFloatModifier lightAttenuationModifier;
        private TGCFloatModifier specularExModifier;
        private TGCFloatModifier spotAngleModifier;
        private TGCFloatModifier spotExponentModifier;
        private TGCColorModifier mEmissiveModifier;
        private TGCColorModifier mAmbientModifier;
        private TGCColorModifier mDiffuseModifier;
        private TGCColorModifier mSpecularModifier;*/

        private bool lightEnableModifier;
        private TGCVector3[] lightPosModifier;
        private TGCBox lightMesh;
        private TgcScene scene;
        private Personaje Camara;

        public void instanciarSpotLight(Personaje personaje, Escenario escenario)
        {
            //Cargar escenario
            var loader = new TgcSceneLoader();
            //Configurar MeshFactory customizado
            scene = escenario.tgcScene;

            //Camara en 1ra persona
            Camara = personaje;

            //Mesh para la luz
            lightMesh = TGCBox.fromSize(new TGCVector3(10, 10, 10), Color.Red);

            //Modifiers de la luz
            lightEnableModifier = true;
            lightPosModifier = new TGCVector3 [3] { new TGCVector3 (-200, -100, -200), new TGCVector3 (200, 200, 300), new TGCVector3 (-60, 90, 175)};
           
            /*
            lightDirModifier = AddVertex3f("lightDir", new TGCVector3(-1, -1, -1), TGCVector3.One, new TGCVector3(-0.05f, 0, 0));
            lightColorModifier = AddColor("lightColor", Color.White);
            lightIntensityModifier = AddFloat("lightIntensity", 0, 150, 35);
            lightAttenuationModifier = AddFloat("lightAttenuation", 0.1f, 2, 0.3f);
            specularExModifier = AddFloat("specularEx", 0, 20, 9f);
            spotAngleModifier = AddFloat("spotAngle", 0, 180, 39f);
            spotExponentModifier = AddFloat("spotExponent", 0, 20, 7f);

            //Modifiers de material
            mEmissiveModifier = AddColor("mEmissive", Color.Black);
            mAmbientModifier = AddColor("mAmbient", Color.White);
            mDiffuseModifier = AddColor("mDiffuse", Color.White);
            mSpecularModifier = AddColor("mSpecular", Color.White);*/
        }

        public void UpdateSpotLight()
            {
                //PreUpdate();
                //PostUpdate();
            }

            public void RenderUpdateSpotLight()
            {
                //PreRender();

                //Habilitar luz
                var lightEnable = lightEnableModifier;
                Effect currentShader;
                if (lightEnable)
                {
                    //Con luz: Cambiar el shader actual por el shader default que trae el framework para iluminacion dinamica con SpotLight
                    currentShader = TGCShaders.Instance.TgcMeshSpotLightShader;
                }
                else
                {
                    //Sin luz: Restaurar shader default
                    currentShader = TGCShaders.Instance.TgcMeshShader;
                }

                //Aplicar a cada mesh el shader actual
                foreach (var mesh in scene.Meshes)
                {
                    mesh.Effect = currentShader;
                    //El Technique depende del tipo RenderType del mesh
                    mesh.Technique = TGCShaders.Instance.GetTGCMeshTechnique(mesh.RenderType);
                }

                //Actualzar posicion de la luz
                var lightPos = lightPosModifier;
                lightMesh.Position = Camara.Position; //ESTO ES UNA NEGRADA

                //Normalizar direccion de la luz
                var lightDir = Camara.LookAt;
                lightDir.Normalize();

                //Renderizar meshes
                foreach (var mesh in scene.Meshes)
                {
                    if (lightEnable)
                    {
                        //Cargar variables shader de la luz
                        mesh.Effect.SetValue("lightColor", ColorValue.FromColor(Camara.itemEnMano.getLuzColor()));
                        mesh.Effect.SetValue("lightPosition", TGCVector3.TGCVector3ToFloat4Array(Camara.Position));
                        mesh.Effect.SetValue("eyePosition", TGCVector3.TGCVector3ToFloat4Array(Camara.Position));
                        mesh.Effect.SetValue("spotLightDir", TGCVector3.TGCVector3ToFloat4Array(lightDir));
                        mesh.Effect.SetValue("lightIntensity", Camara.itemEnMano.getValorLuminico());
                        mesh.Effect.SetValue("lightAttenuation", Camara.itemEnMano.getValorAtenuacion());
                        mesh.Effect.SetValue("spotLightAngleCos", FastMath.ToRad(Camara.getAngulo()));
                        mesh.Effect.SetValue("spotLightExponent", Camara.getExponente());

                        //Cargar variables de shader de Material. El Material en realidad deberia ser propio de cada mesh. Pero en este ejemplo se simplifica con uno comun para todos
                        
                        /*mesh.Effect.SetValue("materialEmissiveColor", ColorValue.FromColor(mEmissiveModifier.Value));
                        mesh.Effect.SetValue("materialAmbientColor", ColorValue.FromColor(mAmbientModifier.Value));
                        mesh.Effect.SetValue("materialDiffuseColor", ColorValue.FromColor(mDiffuseModifier.Value));
                        mesh.Effect.SetValue("materialSpecularColor", ColorValue.FromColor(mSpecularModifier.Value));
                        mesh.Effect.SetValue("materialSpecularExp", specularExModifier.Value);*/
                    }

                    //Renderizar modelo
                    mesh.Render();
                }

                //Renderizar mesh de luz
                lightMesh.Render();

                //PostRender();
            }

            public void DisposeUpdateSpotLight()
            {
                lightMesh.Dispose();
            }
        }
    }