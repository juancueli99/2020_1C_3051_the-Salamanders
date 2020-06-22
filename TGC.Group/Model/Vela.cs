using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    class Vela : IEquipable
    {
        //6000 = 60 segundos
        public float duracionMax = 6000;
        public float duracion = 6000;
        public bool estaEncendida = true;
        private TgcMesh mesh;
        private GameModel gameModel;

        public Vela(TgcMesh mesh, GameModel gameModel)
        {
            this.mesh = mesh;
            this.gameModel = gameModel;
        }
        public TGCVector3 getPosition()
        {
            return mesh.BoundingBox.PMin;
        }
        public void Interactuar(Personaje personaje)
        {
            if (Inventario.inventario.Any(objeto => objeto is Vela))
            {
                var vela = (Vela)Inventario.inventario.Find(objeto => objeto is Vela);
                vela.AumentarDuracion();
            }
            else
            {
                Inventario.inventario.Add(this);
            }
            eliminarMesh();
        }

        private void eliminarMesh()
        {
            TGCVector3 posicionDeBorrado = new TGCVector3(0, -4000, 0);
            mesh.Move(posicionDeBorrado);
            mesh.updateBoundingBox();
            mesh.UpdateMeshTransform();
        }
        public void Usar(Personaje personaje)
        {
            if (personaje.tieneLuz)
            {
                this.Apagar(personaje);
                personaje.tieneLuz = false;

            }
            else
            {
                this.Encender(personaje);
                personaje.tieneLuz = true;
            }
        }
    

        public void Equipar(Personaje personaje)
        {
            personaje.setItemEnMano(this);
        }

        public void FinDuracion(Personaje personaje)
        {
            this.DesecharVela(personaje);
        }

        public void AumentarDuracion()
        {
            this.duracion = duracionMax;
            this.ActualizarHUD();
        }

        public void DisminuirDuracion()
        {
            this.duracion -= 1;
            this.ActualizarHUD();
        }

        public void ActualizarHUD()
        {
            int aux = (int)((duracion / duracionMax) * 66);
            gameModel.vidaUtilVela.instanciarVelas(aux);
        }
        public void DesecharVela(Personaje personaje)
        {
            Inventario.inventario.Remove(this);
            personaje.itemEnMano = (IEquipable)personaje.objetosInteractuables.Find(itemDefault => itemDefault is ItemVacioDefault);
        }

        public float getDuracion()
        {
            return this.duracion;
        }

        public float getValorLuminico()
        {
            return 400f;
        }

        public float getValorAtenuacion()
        {
            return 0.3f;
        }

        public Color getLuzColor()
        {
            //Hay que ir buscando un buen color
            //return Color.FromArgb(194,91,41);
            return Color.FromArgb(32, 22, 13);
        }

        public void Apagar(Personaje personaje)
        {
                this.estaEncendida = false;

        }

        public void Encender(Personaje personaje)
        {
            this.estaEncendida = true;
        }
    }
}
