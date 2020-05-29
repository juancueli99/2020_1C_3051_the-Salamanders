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
using System.Linq;
using System;
using Microsoft.DirectX.Direct3D;
using TGC.Core.Collision;
using System.Collections.Generic;
using System.Security;
using TGC.Core.Shaders;
using TGC.Core.Utils;
using TGC.Core.BoundingVolumes;
using System.IO;
using System.Windows.Forms;

namespace TGC.Group.Model
{
    /// <summary>
    ///     Ejemplo para implementar el TP.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar el modelo que instancia GameForm <see cref="Form.GameForm.InitGraphics()" />
    ///     line 97.
    /// </summary>
    public class GameModel : TGCExample
    {
        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        /// <param name="mediaDir">Ruta donde esta la carpeta con los assets</param>
        /// <param name="shadersDir">Ruta donde esta la carpeta con los shaders</param>
        public GameModel(string mediaDir, string shadersDir) : base(mediaDir, shadersDir)
        {
            Category = Game.Default.Category;
            Name = Game.Default.Name;
            Description = Game.Default.Description;
        }
        Escenario escenario = new Escenario();
        Personaje personaje = new Personaje();
        Monster monster = new Monster();
        Escalera escalera = new Escalera();

        public List<IInteractuable> objetosInteractuables = new List<IInteractuable>();
 
        //Caja que se muestra en el ejemplo.
        private TGCBox Box { get; set; }

        //TgcRotationalCamera CamaraRotacional { get; set; }
        //Mesh de TgcLogo.
        private TgcMesh Mesh { get; set; }
        private TgcMesh fondo { get; set; }
        private TgcScene tgcScene { get; set; }
        //Boleano para ver si dibujamos el boundingbox
        private bool BoundingBox { get; set; }
        private List<LightData> lights;
        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aquí todo el código de inicialización: cargar modelos, texturas, estructuras de optimización, todo
        ///     procesamiento que podemos pre calcular para nuestro juego.
        ///     Borrar el codigo ejemplo no utilizado.
        /// </summary>
        public override void Init()
        {
            //Device de DirectX para crear primitivas.
            var d3dDevice = D3DDevice.Instance.Device;
            this.FixedTickEnable = false;

            escenario.InstanciarEstructuras();
            //escenario.InstanciarHeightmap(); No los usamos mas
            //escenario.InstanciarSkyBox(); Queda feo
            monster.InstanciarMonster();
            CrearObjetosEnEscenario();


            /*
            var cameraPosition = new TGCVector3(-2500, 0, -15000);
            var lookAt = new TGCVector3(0, 0, 0);
            Camara.SetCamera(cameraPosition, lookAt);
            */

            //ESTA ORIGINALMENTE FUNCIONA
            // MiCamara camaraInterna = new MiCamara(personaje.PosicionMesh(), 220, 300);
            //Camara = camaraInterna;

            //ESTE VA QUERIENDO
            Camera = personaje;
            //Camara.SetCamera(personaje.PosicionMesh(), new TGCVector3(0, 0, 0));

            personaje.LockMouse = true;

            //Internamente el framework construye la matriz de view con estos dos vectores.
            //Luego en nuestro juego tendremos que crear una cámara que cambie la matriz de view con variables como movimientos o animaciones de escenas

        }


        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la lógica de computo del modelo, así como también verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        /// 

        private void CrearObjetosEnEscenario()
        {
            escenario.tgcScene.Meshes.ForEach(mesh => CrearInteractuableAsociado(mesh));
        }

        private void CrearInteractuableAsociado(TgcMesh mesh)
        {
            Console.WriteLine(mesh.Name);
            IInteractuable interactuable;
            if (mesh.Name.Equals("notas"))
            {
                interactuable = new Nota(mesh);
                objetosInteractuables.Add(interactuable);
            }
            if (mesh.Name.Equals("vela"))
            {
                interactuable = new Vela(mesh);
                objetosInteractuables.Add(interactuable);
            }
            if (mesh.Name.Equals("pilas"))
            {
                interactuable = new Pila(mesh);
                objetosInteractuables.Add(interactuable);
            }
            if (mesh.Name.Contains("puerta"))
            {
                //tengo que crear una puerta exterior o interior
            }

            if (mesh.Name.Contains("posteLuz"))
            {
                escenario.listaDePostes.Add(mesh);       
            }
            if (mesh.Name.Contains("BarrilPolvora"))
            {
                interactuable = new Escondite(mesh);
                objetosInteractuables.Add(interactuable);
            }
 
           

           // objetosInteractuables.Add((IInteractuable)escenario.GetEscalera());
        }



        public override void Update()
        {
            PreUpdate();
            bool caminar = false;
            //Capturar Input teclado


            if (Input.keyDown(Key.L))
            {
                personaje.LockMouse = !personaje.LockMouse;
            }



            if (personaje.LockMouse)
            {
            
                    if (Input.keyDown(Key.W))
                    {
                        //Le digo al wachin que vaya para adelante
                        //if (!(personaje.key_left == 'W' || DistanciaA2(escenario.GetEscalera().escalonActual) < 100) && !(personaje.key_back == 'W' || DistanciaA2(escenario.GetEscalera().escalonActual) < 100))
                        //{
                            personaje.MoverPersonaje('W', ElapsedTime, Input, escenario, monster, escalera);
                            caminar = true;
                        //}
                    }

                    if (Input.keyDown(Key.A))
                    {
                       // if (!(personaje.key_left == 'A' || DistanciaA2(escenario.GetEscalera().escalonActual) < 100) && !(personaje.key_back == 'A' || DistanciaA2(escenario.GetEscalera().escalonActual) < 100))
                        //{
                            //Le digo al wachin que vaya para la izquierda
                            personaje.MoverPersonaje('A', ElapsedTime, Input, escenario, monster, escalera);
                            caminar = true;
                        //}
                    }

                    if (Input.keyDown(Key.S))
                    {
                        //if (!(personaje.key_left == 'S' || DistanciaA2(escenario.GetEscalera().escalonActual) < 100) && !(personaje.key_back == 'S' || DistanciaA2(escenario.GetEscalera().escalonActual) < 100))
                        //{
                            //Le digo al wachin que vaya para la izquierda
                            personaje.MoverPersonaje('S', ElapsedTime, Input, escenario, monster, escalera);
                            caminar = true;
                       // }
                    }

                    if (Input.keyDown(Key.D))
                    {
                        // if (!(personaje.key_left == 'D' || DistanciaA2(escenario.GetEscalera().escalonActual) < 100) && !(personaje.key_back == 'D' || DistanciaA2(escenario.GetEscalera().escalonActual) < 100))
                        //{
                        //Le digo al wachin que vaya para la izquierda
                        personaje.MoverPersonaje('D', ElapsedTime, Input, escenario, monster, escalera);
                            caminar = true;
                            //}
                     }



                personaje.MoverPersonaje('x', ElapsedTime, Input, escenario, monster, escalera);

                if (Input.keyDown(Key.E))
                {
                    //Interacuar con meshes
                    Console.WriteLine("x: {0} \ny: {1} \nz: {2}", personaje.getPosition().X, personaje.getPosition().Y, personaje.getPosition().Z);

                    var objetoInteractuable = this.objetosInteractuables.OrderBy(mesh => this.DistanciaA(mesh)).First();
                    if(objetoInteractuable is Escondite && this.DistanciaA(objetoInteractuable) < 300)
                    {
                        objetoInteractuable.Interactuar(personaje);
                    }
                    else
                    {
                        if (this.DistanciaA(objetoInteractuable) < 300)
                        {
                            objetosInteractuables.Remove(objetoInteractuable);
                            objetoInteractuable.Interactuar(personaje);
                        }

                        if (personaje.Entre((int)personaje.getPosition().X, -1300, -800) &&
                              personaje.Entre((int)personaje.getPosition().Z, -8100, -6800))
                        {
                            Puerta unaPuerta = new Puerta(escenario.tgcScene.Meshes[0]);// esto es para que sea polimorfico nomas
                            unaPuerta.Interactuar(personaje);
                        }
                    }
                   
                }

                if (Input.keyDown(Key.F))
                {
                    //Prende/apaga la luz de la linterna
                    if(personaje.getItemEnMano() is Linterna)
                    {
                        personaje.getItemEnMano().Usar(personaje);
                    }
                }

                if (Input.keyDown(Key.R))
                {
                    //Recargar las pilas de la linterna
                    var pila = (Pila)personaje.objetosInteractuables.Find(objeto => objeto is Pila);
                    pila.Usar(personaje);
                }

                if (Input.keyDown(Key.Q))
                {
                    //Cambiar entre vela y linterna (si hubiere)
                    if ((personaje.getItemEnMano() is Linterna || personaje.getItemEnMano() is ItemVacioDefault) && personaje.objetosInteractuables.Any(objeto => objeto is Vela))
                    {
                        var vela = (Vela)personaje.objetosInteractuables.Find(objeto => objeto is Vela);
                        personaje.setItemEnMano(vela);
                    }
                    else
                    { 
                        if ((personaje.getItemEnMano() is Vela || personaje.getItemEnMano() is ItemVacioDefault) && personaje.objetosInteractuables.Any(objeto => objeto is Linterna))
                        {
                            var linterna = (Linterna)personaje.objetosInteractuables.Find(objeto => objeto is Linterna);
                            personaje.setItemEnMano(linterna);
                        }
                    }
                }

                if (Input.keyDown(Key.H))
                {
                    personaje.tieneLuz = !personaje.tieneLuz;
                }

                if(personaje.chocandoConEscalera && Input.keyDown(Key.Space))
                {
                    personaje.MoverPersonaje(' ', ElapsedTime, Input, escenario, monster, escalera);
                }

            }

            personaje.updateCamera(ElapsedTime, Input);
            
            personaje.aumentarTiempoSinLuz();
            
            if (personaje.tieneLuz)
            {
                monster.Desaparecer();
            }

            if (personaje.TieneItemEnMano())
            {
                personaje.getItemEnMano().DisminuirDuracion();

                if(personaje.getItemEnMano().getDuracion() <= 0)
                {
                    personaje.getItemEnMano().FinDuracion(personaje);
                }
            }

            bool loAtrapo = monster.Aparecer(personaje);

            if (loAtrapo)
            {
                personaje.GameOver();
            }
          

            personaje.YouWin();
            PostUpdate();
        }

        private double DistanciaA(IInteractuable mesh)
        {
            TGCVector3 vector = personaje.getPosition() - mesh.getPosition();

            return Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Z, 2));
            
        }

        private double DistanciaA2(TgcMesh mesh)
        {
            TGCVector3 vector = personaje.getPosition() - mesh.BoundingBox.PMin;

            return Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Z, 2));

        }

        private void updateLighting()
        {
            Microsoft.DirectX.Direct3D.Effect currentShader;
            TgcMesh unPoste;

            //Con luz: Cambiar el shader actual por el shader default que trae el framework para iluminacion dinamica con PointLight
            if (personaje.tieneLuz)
            {

                currentShader = TGCShaders.Instance.TgcMeshSpotLightShader;
            }
            else
            {
                currentShader = TGCShaders.Instance.TgcMeshPointLightShader;
            }

            //Aplicar a cada mesh el shader actual
            foreach (TgcMesh mesh in escenario.tgcScene.Meshes)
            {
                mesh.Effect = currentShader;
                mesh.Technique = TGCShaders.Instance.GetTGCMeshTechnique(mesh.RenderType);

                // Estos son paramentros del current shader, si cambias el shader chequear los parametros o rompe
                mesh.Effect.SetValue("materialEmissiveColor", ColorValue.FromColor(Color.Black));
                mesh.Effect.SetValue("materialAmbientColor", ColorValue.FromColor(Color.White));
                mesh.Effect.SetValue("materialDiffuseColor", ColorValue.FromColor(Color.White));
                mesh.Effect.SetValue("materialSpecularColor", ColorValue.FromColor(Color.White));
                mesh.Effect.SetValue("materialSpecularExp", 9f);
                mesh.Effect.SetValue("eyePosition", TGCVector3.TGCVector3ToFloat4Array(personaje.eye));
                mesh.Effect.SetValue("lightAttenuation", personaje.itemEnMano.getValorAtenuacion());
                mesh.Effect.SetValue("lightColor", ColorValue.FromColor(Color.White));

                unPoste = escenario.listaDePostes.OrderBy(poste => this.DistanciaA2(poste)).First();
                if (DistanciaA2(unPoste) < 2000)
                {
                    //Se prende el farol mas cercano
                    mesh.Effect.SetValue("lightColor", ColorValue.FromColor(Color.White));
                    mesh.Effect.SetValue("lightIntensity", 50f);
                    mesh.Effect.SetValue("lightPosition", TGCVector3.TGCVector3ToFloat4Array(unPoste.BoundingBox.PMin));
                }
                else
                {
                    if (personaje.tieneLuz)
                    {
                        this.AplicarShaderSpotLight(mesh);
                    }
                    else
                    {
                        mesh.Effect.SetValue("lightIntensity", 50f);
                        mesh.Effect.SetValue("lightPosition", TGCVector3.TGCVector3ToFloat4Array(personaje.getPosition()));
                    }
                   
                }
            }
        }

        public void AplicarShaderSpotLight(TgcMesh mesh)
        {
            //Actualizar posición de la luz
            TGCVector3 lightPos = personaje.getPosition() + new TGCVector3(0, 100, 0) + new TGCVector3(FastMath.Sin(5.5f) * -150, 0, FastMath.Cos(5.5f) * -150);

            //Normalizar direccion de la luz
            TGCVector3 lightDir = personaje.forward;
            lightDir.Normalize();

            //Cargar variables shader de la luz
            mesh.Effect.SetValue("lightPosition", TGCVector3.TGCVector3ToFloat4Array(lightPos));
            mesh.Effect.SetValue("spotLightDir", TGCVector3.TGCVector3ToFloat4Array(lightDir));
            mesh.Effect.SetValue("lightIntensity", personaje.itemEnMano.getValorLuminico());
            mesh.Effect.SetValue("spotLightAngleCos", FastMath.ToRad(20));
            mesh.Effect.SetValue("spotLightExponent", 25);
            mesh.Effect.SetValue("lightColor", ColorValue.FromColor(personaje.itemEnMano.getLuzColor()));
        }
        public void AplicarShaderAlHeightmap()
        {
            Microsoft.DirectX.Direct3D.Effect currentShader;
            currentShader = TGCShaders.Instance.TgcMeshPointLightShader;
            escenario.heightmap.Effect = currentShader;
            escenario.heightmap.Technique = TGCShaders.Instance.GetTGCMeshTechnique(TgcMesh.MeshRenderType.DIFFUSE_MAP_AND_LIGHTMAP);

            // Estos son paramentros del current shader, si cambias el shader chequear los parametros o rompe
            escenario.heightmap.Effect.SetValue("materialEmissiveColor", ColorValue.FromColor(Color.Black));
            escenario.heightmap.Effect.SetValue("materialAmbientColor", ColorValue.FromColor(Color.White));
            escenario.heightmap.Effect.SetValue("materialDiffuseColor", ColorValue.FromColor(Color.White));
            escenario.heightmap.Effect.SetValue("materialSpecularColor", ColorValue.FromColor(Color.White));
            escenario.heightmap.Effect.SetValue("materialSpecularExp", 9f);
            escenario.heightmap.Effect.SetValue("eyePosition", TGCVector3.TGCVector3ToFloat4Array(personaje.eye));
            escenario.heightmap.Effect.SetValue("lightAttenuation", 0.3f);
            escenario.heightmap.Effect.SetValue("lightColor", ColorValue.FromColor(Color.White));
            escenario.heightmap.Effect.SetValue("lightIntensity", 50f);
            escenario.heightmap.Effect.SetValue("lightPosition", TGCVector3.TGCVector3ToFloat4Array(personaje.getPosition()));
        }


        /// ////////////////////////////////////////////////////////77
        /// /////////////////////////////////////////////////////////
        public class LightData
        {
            public TgcBoundingAxisAlignBox aabb;
            public Color color;
            public TGCVector3 pos;
        }

        private LightData getClosestLight(TGCVector3 pos)
        {
            var minDist = float.MaxValue;
            LightData minLight = null;

            foreach (var light in lights)
            {
                var distSq = TGCVector3.LengthSq(pos - light.pos);
                if (distSq < minDist)
                {
                    minDist = distSq;
                    minLight = light;
                }
            }

            return minLight;
        }
        public void RenderMultiplesLuces()
        {
            //Habilitar luz
            Microsoft.DirectX.Direct3D.Effect currentShader;
            if (personaje.tieneLuz)
            {
                //Con luz: Cambiar el shader actual por el shader default que trae el framework para iluminacion dinamica con PointLight
                currentShader = TGCShaders.Instance.TgcMeshPointLightShader;
            }
            else
            {
                //Sin luz: Restaurar shader default
                currentShader = TGCShaders.Instance.TgcMeshShader;
            }

            //Aplicar a cada mesh el shader actual
            foreach (TgcMesh mesh in escenario.tgcScene.Meshes)
            {
                mesh.Effect = currentShader;
                //El Technique depende del tipo RenderType del mesh
                mesh.Technique = TGCShaders.Instance.GetTGCMeshTechnique(mesh.RenderType);
            }

            //Renderizar meshes
            foreach (TgcMesh mesh in escenario.tgcScene.Meshes)
            {
                if (personaje.tieneLuz)
                {
                    var light = getClosestLight(mesh.BoundingBox.calculateBoxCenter());

                    //Cargar variables shader de la luz

                    mesh.Effect.SetValue("eyePosition", TGCVector3.TGCVector3ToFloat4Array(personaje.eye));
                    mesh.Effect.SetValue("lightAttenuation", personaje.itemEnMano.getValorAtenuacion());
                    mesh.Effect.SetValue("lightPosition", TGCVector3.TGCVector3ToFloat4Array(light.pos));
                    mesh.Effect.SetValue("lightIntensity", personaje.itemEnMano.getValorLuminico());
                    mesh.Effect.SetValue("lightColor", ColorValue.FromColor(personaje.itemEnMano.getLuzColor()));

                    //Cargar variables de shader de Material. El Material en realidad deberia ser propio de cada mesh. Pero en este ejemplo se simplifica con uno comun para todos
                    
                    mesh.Effect.SetValue("materialEmissiveColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialAmbientColor", ColorValue.FromColor(Color.White));
                    mesh.Effect.SetValue("materialDiffuseColor", ColorValue.FromColor(Color.White));
                    mesh.Effect.SetValue("materialSpecularColor", ColorValue.FromColor(Color.White));
                    mesh.Effect.SetValue("materialSpecularExp", 9f);
                }

                //Renderizar modelo
                mesh.Render();
            }
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aquí todo el código referido al renderizado.
        ///     Borrar todo lo que no haga falta.
        /// </summary>
        public override void Render()
        {
            //Inicio el render de la escena, para ejemplos simples. Cuando tenemos postprocesado o shaders es mejor realizar las operaciones según nuestra conveniencia.
            PreRender();

            this.updateLighting();

            //Pone el fondo negro en vez del azul feo ese
            D3DDevice.Instance.Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            
            escenario.RenderEscenario();
            //personaje.RenderPersonaje(ElapsedTime);
            monster.RenderMonster();

            //Render de BoundingBox, muy útil para debug de colisiones.
            if (BoundingBox)
            {
                Box.BoundingBox.Render();
                tgcScene.Meshes.ForEach(mesh => mesh.BoundingBox.Render());
                //fondo.BoundingBox.Render();
            }

            //Finaliza el render y presenta en pantalla, al igual que el preRender se debe para casos puntuales es mejor utilizar a mano las operaciones de EndScene y PresentScene
            PostRender();
        }

        /// <summary>
        ///     Se llama cuando termina la ejecución del ejemplo.
        ///     Hacer Dispose() de todos los objetos creados.
        ///     Es muy importante liberar los recursos, sobretodo los gráficos ya que quedan bloqueados en el device de video.
        /// </summary>
        public override void Dispose()
        {
            escenario.DisposeEscenario();
            //personaje.DisposePersonaje();
            monster.DisposeMonster();
        }

    }
}