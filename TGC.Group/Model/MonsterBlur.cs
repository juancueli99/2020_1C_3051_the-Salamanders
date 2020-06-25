using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX.Direct3D;
using System;
using System.Drawing;
using System.Windows.Forms;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using TGC.Core.Textures;
using TGC.Core.Collision;

namespace TGC.Group.Model
{
    public class MonsterBlur : ITecnicaRender
    {
        private TGCMatrix antMatWorldView;
        private Effect effect;
        private Surface g_pDepthStencil; // Depth-stencil buffer
        private Texture g_pRenderTarget;
        private GameModel gameModel;
        private VertexBuffer g_pVBV3D;
        private Texture g_pVel1, g_pVel2; // velocidad
        private TgcMesh mesh;
        private string MyShaderDir;
        private float time;
        private Escenario escenario;
        private List<TgcMesh> meshes = new List<TgcMesh>();

        public MonsterBlur(Escenario nuestra_scene,GameModel modelo)
        {
            escenario = nuestra_scene;
            gameModel = modelo;
        }
        public void instanciarMonsterBlur(string ShadersDir, Monster monster, string MediaDir)
        {
            var d3dDevice = D3DDevice.Instance.Device;

            MyShaderDir = ShadersDir;

            //Cargar mesh
            /*TgcMesh sectario;

            var loader = new TgcSceneLoader();
            var scene2 = loader.loadSceneFromFile(MediaDir + "Modelame\\Sectarian-TgcScene.xml");

            sectario = scene2.Meshes[0];*/

            //sectario.Position = new TGCVector3(9500f, -350f, -15000f);

            //sectario.Transform = TGCMatrix.Translation(0, -350, 0);


            /*mesh = monster.ghost;
            effect = TGCShaders.Instance.LoadEffect(MyShaderDir + "MonsterMotionBlur.fx");
            effect.Technique = "DefaultTechnique";
            mesh.Effect = effect;*/

            effect = TGCShaders.Instance.LoadEffect(MyShaderDir + "MonsterMotionBlur.fx");
            //Configurar Technique dentro del shader

            // stencil
            g_pDepthStencil = d3dDevice.CreateDepthStencilSurface(d3dDevice.PresentationParameters.BackBufferWidth,
                d3dDevice.PresentationParameters.BackBufferHeight,
                DepthFormat.D24S8, MultiSampleType.None, 0, true);

            // inicializo el render target
            g_pRenderTarget = new Texture(d3dDevice, d3dDevice.PresentationParameters.BackBufferWidth
                , d3dDevice.PresentationParameters.BackBufferHeight, 1, Usage.RenderTarget,
                Format.X8R8G8B8, Pool.Default);

            // velocidad del pixel
            g_pVel1 = new Texture(d3dDevice, d3dDevice.PresentationParameters.BackBufferWidth
                , d3dDevice.PresentationParameters.BackBufferHeight, 1, Usage.RenderTarget,
                Format.A16B16G16R16F, Pool.Default);

            g_pVel2 = new Texture(d3dDevice, d3dDevice.PresentationParameters.BackBufferWidth
                , d3dDevice.PresentationParameters.BackBufferHeight, 1, Usage.RenderTarget,
                Format.A16B16G16R16F, Pool.Default);

            // Resolucion de pantalla
            effect.SetValue("screen_dx", d3dDevice.PresentationParameters.BackBufferWidth);
            effect.SetValue("screen_dy", d3dDevice.PresentationParameters.BackBufferHeight);

            CustomVertex.PositionTextured[] vertices =
            {
                new CustomVertex.PositionTextured(-1, 1, 1, 0, 0),
                new CustomVertex.PositionTextured(1, 1, 1, 1, 0),
                new CustomVertex.PositionTextured(-1, -1, 1, 0, 1),
                new CustomVertex.PositionTextured(1, -1, 1, 1, 1)
            };
            //vertex buffer de los triangulos
            g_pVBV3D = new VertexBuffer(typeof(CustomVertex.PositionTextured),
                4, d3dDevice, Usage.Dynamic | Usage.WriteOnly,
                CustomVertex.PositionTextured.Format, Pool.Default);
            g_pVBV3D.SetData(vertices, 0, LockFlags.None);

            antMatWorldView = TGCMatrix.Identity;
        }

        public void ObtenerMeshesDelFrustum()
        {
            meshes = gameModel.escenario.tgcScene.Meshes.FindAll
               (mesh => TgcCollisionUtils.classifyFrustumAABB(gameModel.Frustum, mesh.BoundingBox) !=
               TgcCollisionUtils.FrustumResult.OUTSIDE);

            foreach (TgcMesh mesh in meshes)
            {
                effect.Technique = "DefaultTechnique";
                mesh.Effect = effect;
            }

        }

        public void UpdateMonsterBlur(float ElapsedTime, Monster monster)
        {
            time += ElapsedTime;
            float r = 40;
            foreach (TgcMesh mesh in meshes)
            {
                mesh.Transform = TGCMatrix.Translation(new TGCVector3(r * (float)Math.Cos(time * 4), 0, 0 * (float)Math.Sin(time * 4)));
            }
            
            //monster.ghost.Transform = TGCMatrix.Translation(new TGCVector3(r * (float)Math.Cos(time * 5), -350, 0 * (float)Math.Sin(time * 5)));
        }

        public void renderScene(string technique)
        {
            //mesh.Technique = technique;
            //mesh.Render();
            foreach (TgcMesh mesh in meshes)
            {
                mesh.Technique = technique;
                mesh.Render();
            }
        }

        public void RenderMonsterBlur()
        {
            TexturesManager.Instance.clearAll();
            var device = D3DDevice.Instance.Device;

            // guardo el Render target anterior y seteo la textura como render target
            var pOldRT = device.GetRenderTarget(0);
            var pSurf = g_pVel1.GetSurfaceLevel(0);
            device.SetRenderTarget(0, pSurf);
            // hago lo mismo con el depthbuffer, necesito el que no tiene multisampling
            var pOldDS = device.DepthStencilSurface;
            device.DepthStencilSurface = g_pDepthStencil;

            // 1 - Genero un mapa de velocidad
            effect.Technique = "VelocityMap";
            // necesito mandarle la matrix de view proj anterior
            effect.SetValue("matWorldViewProjAnt", antMatWorldView.ToMatrix() * device.Transform.Projection);
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Gray, 1.0f, 0);
            device.BeginScene();

            renderScene("VelocityMap");
            device.EndScene();
            device.Present();

            pSurf.Dispose();

            // 2- Genero la imagen pp dicha
            effect.Technique = "DefaultTechnique";
            pSurf = g_pRenderTarget.GetSurfaceLevel(0);
            device.SetRenderTarget(0, pSurf);
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Gray, 1.0f, 0);
            device.BeginScene();
            
            renderScene("DefaultTechnique");
            
            device.EndScene();
            device.Present();
            pSurf.Dispose();

            // Ultima pasada vertical va sobre la pantalla pp dicha
            device.SetRenderTarget(0, pOldRT);
            device.DepthStencilSurface = pOldDS;

            device.BeginScene();
            effect.Technique = "PostProcessMotionBlur";
            device.VertexFormat = CustomVertex.PositionTextured.Format;
            device.SetStreamSource(0, g_pVBV3D, 0);
            effect.SetValue("g_RenderTarget", g_pRenderTarget);
            effect.SetValue("texVelocityMap", g_pVel1);
            effect.SetValue("texVelocityMapAnt", g_pVel2);
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Gray, 1.0f, 0);
            effect.Begin(FX.None);
            effect.BeginPass(0);
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);

            effect.EndPass();

            effect.End();
            gameModel.RenderFps();

            gameModel.nota.renderSprite();
            gameModel.vidaUtilVela.renderSprite();
            gameModel.velita.renderSprite();
            gameModel.vidaUtilLinterna.renderSprite();
            gameModel.linternita.renderSprite();

            device.EndScene();
            device.Present();

            // actualizo los valores para el proximo frame
            foreach (TgcMesh mesh in meshes)
            {
                antMatWorldView = mesh.Transform * TGCMatrix.FromMatrix(device.Transform.View);
                var aux = g_pVel2;
                g_pVel2 = g_pVel1;
                g_pVel1 = aux;
            }
        }

        public void DisposeMonsterBlur()
        {
            mesh.Dispose();
            g_pRenderTarget.Dispose();
            g_pDepthStencil.Dispose();
            g_pVBV3D.Dispose();
            g_pVel1.Dispose();
            g_pVel2.Dispose();
        }

        public void SetearTecnica()
        {
            meshes.Add(gameModel.monster.ghost);
            this.ObtenerMeshesDelFrustum();
        }

        public void AplicarRender()
        {
            this.RenderMonsterBlur();
        }
    }
}