using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    class Linterna : IEquipable, IInteractuable

    {
        TgcMesh mesh1;
        private GameModel gameModel;
        TgcMesh mesh2;
        //12000 = 120 segundos
        public float duracionMax = 12000;
        public float duracion = 12000;
        public bool estaEncendida = false;
        Sonido sonidoInterruptor;


        public Linterna(TgcMesh nuevoMesh, TgcMesh nuevoMesh2, GameModel gameModel)
        {
            this.mesh2 = nuevoMesh2;
            this.mesh1 = nuevoMesh;
            this.gameModel = gameModel;
            sonidoInterruptor = new Sonido("Click2-Sebastian-759472264.wav", -3000,false);
        }
        public TGCVector3 getPosition()
        {
            return mesh1.BoundingBox.PMin;
        }

        public void Interactuar(Personaje personaje)
        {
            if (!Inventario.inventario.Any(objeto => objeto is Linterna))
            {
                Inventario.inventario.Add(this);
            }
            eliminarMesh();
        }

        private void eliminarMesh()
        {
            TGCVector3 posicionDeBorrado = new TGCVector3(0, -4000, 0);
            mesh1.Move(posicionDeBorrado);
            mesh1.updateBoundingBox();
            mesh1.UpdateMeshTransform();
            mesh2.Move(posicionDeBorrado);
            mesh2.updateBoundingBox();
            mesh2.UpdateMeshTransform();
        }

        public void Usar(Personaje personaje)
        {
            if (personaje.tieneLuz)
            {
                this.Apagar(personaje);
                
            }
            else
            {
                this.Encender(personaje);
                
            }
            this.sonidoInterruptor.escucharSonidoActual(false);
        }

        public void Equipar(Personaje personaje)
        {
            personaje.setItemEnMano(this);
        }

        public void FinDuracion(Personaje personaje)
        {
            this.Apagar(personaje);
        }

        public void Apagar(Personaje personaje)
        {
            this.estaEncendida = false;
            personaje.tieneLuz = false;
            //personaje.itemEnMano = (IEquipable)personaje.objetosInteractuables.Find(itemDefault => itemDefault is ItemVacioDefault);
        }

        public void Recargar()
        {
            this.duracion = duracionMax;
            this.ActualizarHUD();
        }

        public void DisminuirDuracion(Personaje personaje)
        {
            if(this.duracion > 0)
            {
                this.duracion -= 1;
                Console.WriteLine("Estoy disminuyendo");
                this.ActualizarHUD();
            }
            else
            {
                this.FinDuracion(personaje);
                Console.WriteLine("ME APAGO");
            }
        }

        public void ActualizarHUD()
        {
            int aux = (int)((duracion / duracionMax) * 75);
            gameModel.vidaUtilLinterna.instanciarLinternas(aux);
        }

        public float getDuracion()
        {
            return this.duracion;
        }

        public float getValorLuminico()
        {
            return 600f;
        }

        public float getValorAtenuacion()
        {
            return 0.5f;
        }

        public Color getLuzColor()
        {
            return Color.LightYellow;
        }

        public void Encender(Personaje personaje)
        {
            gameModel.effectPosProcesado.Technique = "PostProcessDefault";
            this.estaEncendida = true;
            personaje.tieneLuz = true;
        }
    }
}
