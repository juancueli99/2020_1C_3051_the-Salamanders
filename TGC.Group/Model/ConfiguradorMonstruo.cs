using BulletSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    static class ConfiguradorMonstruo
    {
        static String MediaDir = "..\\..\\..\\Media\\";

        public static TgcMesh configurarFantasma()
        {
            //easter egg, aparece randomly
            TgcMesh ghost;

            var loader = new TgcSceneLoader();
            var scene2 = loader.loadSceneFromFile(MediaDir + "Modelame\\GhostGrande-TgcScene.xml");

            ghost = scene2.Meshes[0];

            ghost.Position = new TGCVector3(200, -350, 100);

            ghost.Transform = TGCMatrix.Translation(0, -350, 0);

            return ghost;
        }

        public static TgcMesh configurarPayaso()
        {
            TgcMesh payaso;

            var loader = new TgcSceneLoader();
            var scene2 = loader.loadSceneFromFile(MediaDir + "Modelame\\Clown-TgcScene.xml");

            payaso = scene2.Meshes[0];

            payaso.Position = new TGCVector3(200, -300, 100);

            payaso.Transform = TGCMatrix.Translation(0, -300, 0) * TGCMatrix.Scaling(5, 5, 5);

            return payaso;
        }

        public static TgcMesh configurarSectario()
        {
            TgcMesh sectario;

            var loader = new TgcSceneLoader();
            var scene2 = loader.loadSceneFromFile(MediaDir + "Modelame\\Sectarian-TgcScene.xml");

            sectario = scene2.Meshes[0];

            sectario.Position = new TGCVector3(200, -350, 100);

            sectario.Transform = TGCMatrix.Translation(0, -350, 0);

            return sectario;
        }

        public static TgcMesh configurarAlien()
        {
            TgcMesh alien;

            var loader = new TgcSceneLoader();
            var scene2 = loader.loadSceneFromFile(MediaDir + "Modelame\\Alien-TgcScene.xml");

            alien = scene2.Meshes[0];

            alien.Position = new TGCVector3(200, -100, 100);

            alien.Transform = TGCMatrix.Translation(0, -100, 0) * TGCMatrix.Scaling(5, 5, 5);

            return alien;
        }

        public static TgcMesh ConfigurarMonstruo(monstruos monstruo)
        {
            TgcMesh monster=null;

            switch (monstruo)
            {
                case monstruos.GHOST:
                    monster = configurarFantasma();
                    break;

                case monstruos.CLOWN:
                    monster = configurarPayaso();
                    break;

                case monstruos.SECTARIAN:
                    monster = configurarSectario();
                    break;

                case monstruos.ALIEN:
                    monster = configurarAlien();
                    break;
            }

            return monster;
        }
    }
}
