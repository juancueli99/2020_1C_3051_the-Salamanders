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

                case monstruos.SECTARIAN:
                    return configurarSonidoSectarioGameOver();

                case monstruos.ALIEN:
                    return configurarSonidoAlienGameOver();

            }
            return null;

        }

        private static Sonido configurarSonidoSectarioGameOver()
        {
            return new Sonido("Laughter-Mike_Koenig-360558723.wav", -2000, false);
        }

        private static Sonido configurarSonidoAlienGameOver()
        {
            return new Sonido("Eating-SoundBible.com-1470347575.wav", -2000, false);
        }

        private static Sonido configurarSonidoFantasmaGameOver()
        {
            return new Sonido("Demon_Your_Soul_is_mine-BlueMann-1903732045.wav", -2000,false);
        }

        private static List<Sonido> configurarSonidoAlien()
        {
            var lista= new List<Sonido>();

            lista.Add(new Sonido("lobo mutante.wav", -3000, false));
            lista.Add(new Sonido("Monster2Remastered.wav", -2000, false));
            lista.Add(new Sonido("Moster1Remastered.wav", -2000, false));

            return lista;
        }

        private static List<Sonido> configurarSonidoSectario()
        {
            var lista = new List<Sonido>();
            lista.Add(new Sonido("ramita, partir.wav",-3000,false));
            lista.Add(new Sonido("risa infantil.wav",-3000 ,false));
            lista.Add(new Sonido("iCanSeeYou.wav",-2000, false));
            lista.Add(new Sonido("AreYouScared.wav", -2000, false));
            lista.Add(new Sonido("WeMustKill.wav", -2000, false));
            lista.Add(new Sonido("GhostMaleVoiceWhisperingComeToMe.wav", -2000, false));
            lista.Add(new Sonido("embraceTheShadow.wav", -2000, false));
            lista.Add(new Sonido("theyRunInFear.wav", -2000, false));
            lista.Add(new Sonido("weBuryThem.wav", -2000, false));
            lista.Add(new Sonido("Ghost3.wav", -2000, false));

            return lista;
        }

       
        private static List<Sonido> configurarSonidosFantasma()
        {
            var lista = new List<Sonido>();
            lista.Add(new Sonido("risa infantil.wav", -3000, false)); 
            lista.Add(new Sonido("gemido fantasmal.wav", -2000, false));
            lista.Add(new Sonido("cadena, agitar.wav", -2000, false));
            lista.Add(new Sonido("cadena arrastrada.wav", -2000, false));
            lista.Add(new Sonido("monstruo ventoso.wav", -2000, false));
            lista.Add(new Sonido("weAreBound.wav", -2000, false));
            lista.Add(new Sonido("Ghost1.wav", -2000, false));
            lista.Add(new Sonido("Ghost2.wav", -2000, false));
            lista.Add(new Sonido("Ghost3.wav", -2000, false));
            lista.Add(new Sonido("AreYouScared.wav", -2000, false));
            lista.Add(new Sonido("SuccumbToYourNightmares.wav", -2000, false));
            lista.Add(new Sonido("ComeWithUs.wav", -2000, false));
            lista.Add(new Sonido("theyRunInFear.wav", -2000, false));

            return lista;
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

            switch (monstruo)
            {
                case monstruos.GHOST:
                    monster = configurarFantasma();
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
