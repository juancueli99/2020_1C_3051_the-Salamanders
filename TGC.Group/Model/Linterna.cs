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
    class Linterna : IEquipable

    {
        TgcMesh mesh;
        //12000 = 120 segundos
        public float duracionMax = 12000;
        public float duracion = 12000;
        public bool estaEncendida = false;


        public Linterna(TgcMesh nuevoMesh)
        {

            this.mesh = nuevoMesh;
        }
        public TGCVector3 getPosition()
        {
            return mesh.Position;
        }

        public void Interactuar(Personaje personaje)
        {
            if (!personaje.objetosInteractuables.Any(objeto => objeto is Linterna))
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

        public void Usar(Personaje personaje)
        {
            if (this.estaEncendida)
            {
                this.ApagarLinterna(personaje);
            }
            else
            {
                if (personaje.tieneLuz)
                {
                    personaje.UsarItemEnMano();
                }

                this.EncenderLinterna();
            }
        }

        public void Equipar(Personaje personaje)
        {
            personaje.setItemEnMano(this);
        }

        public void FinDuracion(Personaje personaje)
        {
            this.ApagarLinterna(personaje);
        }

        public void EncenderLinterna()
        {
            this.estaEncendida = true;
        }

        public void ApagarLinterna(Personaje personaje)
        {
            this.estaEncendida = false;
            personaje.itemEnMano = (IEquipable)personaje.objetosInteractuables.Find(itemDefault => itemDefault is ItemVacioDefault);
        }

        public void Recargar()
        {
            this.duracion = duracionMax;
        }

        public void DisminuirDuracion()
        {
            if (this.estaEncendida)
            {
                this.duracion -= 1;
            }
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
            throw new NotImplementedException();
        }

        public Color getLuzColor()
        {
            return Color.LightYellow;
        }
    }
}
