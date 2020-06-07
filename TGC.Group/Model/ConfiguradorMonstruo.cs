using BulletSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        internal static List<Sonido> ConfigurarSonidosRandoms()
        {
            
            switch (GameModel.monstruoActual)
            {
                case monstruos.GHOST:
                    return configurarSonidosFantasma();

                case monstruos.CLOWN:
                    return configurarSonidoPayaso();

                case monstruos.SECTARIAN:
                    return configurarSonidoSectario();
                 
                case monstruos.ALIEN:
                    return configurarSonidoAlien();
                 
            }
            return new List<Sonido>();
        }

        internal static Sonido ObtenerSonidoDeGameOver()
        {
            switch (GameModel.monstruoActual)
            {
                case monstruos.GHOST:
                    return configurarSonidoFantasmaGameOver();

                case monstruos.CLOWN:
                    return configurarSonidoPayasoGameOver();

                case monstruos.SECTARIAN:
                    return configurarSonidoSectarioGameOver();

                case monstruos.ALIEN:
                    return configurarSonidoAlienGameOver();

            }
            return null;

        }

        private static Sonido configurarSonidoSectarioGameOver()
        {
            return new Sonido("", -2000, false);
        }

        private static Sonido configurarSonidoAlienGameOver()
        {
            return new Sonido("", -2000, false);
        }

        private static Sonido configurarSonidoFantasmaGameOver()
        {
            return new Sonido("",-2000,false);
        }

        private static List<Sonido> configurarSonidoAlien()
        {
            var lista= new List<Sonido>();

            lista.Add(new Sonido("lobo mutante.wav", false));
            return lista;
        }

        private static List<Sonido> configurarSonidoSectario()
        {
            var lista = new List<Sonido>();
            lista.Add(new Sonido("ramita, partir.wav",false));
            lista.Add(new Sonido("risa infantil.wav", false));
            return lista;
        }

        private static List<Sonido> configurarSonidoPayaso()
        {

            var lista = new List<Sonido>();
            lista.Add(new Sonido("risa infantil.wav", false));
            return lista;
        }

        private static List<Sonido> configurarSonidosFantasma()
        {
            var lista = new List<Sonido>();
            lista.Add(new Sonido("risa infantil.wav", false)); 
            lista.Add(new Sonido("gemido fantasmal.wav", false));
            lista.Add(new Sonido("cadena agitar.wav", false));
            lista.Add(new Sonido("cadena arrastrada.wav", false));
            lista.Add(new Sonido("monstruo ventoso.wav", false));
            return lista;
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
            var scene2 = loader.loadSceneFromFile(MediaDir + "Modelame\\AlienV8-TgcScene.xml");

            alien = scene2.Meshes[0];

            alien.Position = new TGCVector3(200, 0, 100);

            alien.Transform = TGCMatrix.Translation(0, 0, 0) * TGCMatrix.Scaling(100, 100, 100);

            return alien;
        }

        public static TgcMesh ConfigurarMonstruo(monstruos monstruo)
        {
            TgcMesh monster=null;
            var ran = new Random();
            if (ran.Next() % 1000 == 57) {
                GameModel.monstruoActual = monstruos.GHOST;
                return configurarFantasma();
            }

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
