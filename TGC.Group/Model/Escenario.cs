using System;
using Microsoft.DirectX.DirectInput;
using System.Drawing;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Geometry;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;
using TGC.Core.Camara;
using TGC.Core.Terrain;
using System.Collections.Generic;

namespace TGC.Group.Model
{

    public class Escenario
    {
        String MediaDir = "..\\..\\..\\Media\\";
        public TgcScene tgcScene { get; set; }
        public TgcSimpleTerrain heightmap = new TgcSimpleTerrain();
        private TgcSkyBox skyBox = new TgcSkyBox();
        string currentHeightmap;
        string currentTexture;
        float currentScaleXZ;
        float currentScaleY;
        public Escalera escalera = null;
        public List<TgcMesh> listaDePostes = new List<TgcMesh>();

     
        public void InstanciarEstructuras()
        {
            TgcSceneLoader loader = new TgcSceneLoader();
            tgcScene = loader.loadSceneFromFile(MediaDir + "NuestrosModelos\\MapaFullReleaseV2-TgcScene.xml");
        }

        public void InstanciarSkyBox()
        {
            //Configurar tamaño SkyBox
            skyBox.Center = new TGCVector3(-250, 1000, -1500);
            skyBox.Size = new TGCVector3(30000, 5000, 30000);
            

            //Configurar las texturas para cada una de las 6 caras
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Up, MediaDir + "Sky.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Down, MediaDir + "Sky.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Left, MediaDir + "Sky 3.png");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Right, MediaDir + "Sky 4.png");

            //Hay veces es necesario invertir las texturas Front y Back si se pasa de un sistema RightHanded a uno LeftHanded
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Front, MediaDir + "Sky 1.png");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Back, MediaDir + "Sky 2.png");

            //Inicializa las configuraciones del skybox
            skyBox.Init();
        }

        public void InstanciarHeightmap() // Deprecated - Ya no usamos heightmap
        {
            currentScaleXZ = 600f;
            currentScaleY = 100f;

            currentHeightmap = MediaDir + "Heighmaps\\alfajorGrandote.jpg";
            currentTexture = MediaDir + "Heighmaps\\Grass.jpg";

            heightmap.loadHeightmap(currentHeightmap, currentScaleXZ, currentScaleY, new TGCVector3(0, -4, 0));
            heightmap.loadTexture(currentTexture);
        }

        public void RenderEscenario()
        {
            tgcScene.Meshes.ForEach(mesh => mesh.UpdateMeshTransform());
            tgcScene.Meshes.ForEach(mesh => mesh.Render());

            //heightmap.Render();

            //skyBox.Render();
        }

        public void DisposeEscenario()
        {
            //skyBox.Dispose();
            //heightmap.Dispose();
            tgcScene.Meshes.ForEach(mesh => mesh.Dispose());
        }
    }
}

