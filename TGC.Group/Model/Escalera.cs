using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;


namespace TGC.Group.Model
{
     public class Escalera
    {
        TgcMesh escalera;

        public TgcMesh devolverEscalera(Escenario escenario)
        {
           escalera = escenario.tgcScene.Meshes.Find(mesh => mesh.Name.Equals("EscaleraMetalMovil"));

            //Console.WriteLine(escalera.Name);
            return escalera;
        }


    }
}
