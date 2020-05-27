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

        public Vela(TgcMesh mesh)
        {
            this.mesh = mesh;
        }
        public TGCVector3 getPosition()
        {
            return mesh.BoundingBox.PMin;
        }
        public void Interactuar(Personaje personaje)
        {
            if (personaje.objetosInteractuables.Any(objeto => objeto is Vela))
            {
                var vela = (Vela)personaje.objetosInteractuables.Find(objeto => objeto is Vela);
                vela.AumentarDuracion();
            }
            else
            {
                personaje.objetosInteractuables.Add(this);
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
        public void Usar(Personaje personaje) { }

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
        }

        public void DisminuirDuracion()
        {
            this.duracion -= 1;
        }

        public void DesecharVela(Personaje personaje)
        {
            personaje.objetosInteractuables.Remove(this);
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
            return Color.DarkOrange;
        }
    }
}
