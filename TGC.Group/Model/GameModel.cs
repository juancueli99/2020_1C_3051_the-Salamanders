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
using static TGC.Core.Collision.TgcCollisionUtils;


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
        public Escenario escenario = new Escenario();
        public Personaje personaje;
        public Monster monster;
        public Sprite menu = new Sprite();

        public HUD nota = new HUD();
        public HUD vidaUtilVela = new HUD();
        public HUD velita = new HUD();
        public HUD vidaUtilLinterna = new HUD();
        public HUD linternita = new HUD();

        public static bool estoyEnElMenu = true;

        public static bool estoyCorriendo = false;

        //PARED INVISIBLE
        public ParedInvisible paredInvisible = new ParedInvisible();

        //public List<Monster> bichos = new List<Monster>();

        public List<IInteractuable> objetosInteractuables = new List<IInteractuable>();
        public List<TgcMesh> iluminables= new List<TgcMesh>();
        public List<Sonido> sonidosRandoms = new List<Sonido>();
        public List<Sonido> sonidosOutDoorRandom = new List<Sonido>();
        public List<Sonido> sonidosInDoorRandom = new List<Sonido>();
        //Caja que se muestra en el ejemplo.
        private TGCBox Box { get; set; }

        //TgcRotationalCamera CamaraRotacional { get; set; }
        //Mesh de TgcLogo.
        private TgcMesh Mesh { get; set; }
        private TgcMesh fondo { get; set; }
        private TgcScene tgcScene { get; set; }
        //Boleano para ver si dibujamos el boundingbox
        private bool BoundingBox { get; set; }
        public FrustumResult INTERSECT { get; private set; }
        public static float TiempoDeGameOver = 30000;
        public static float TiempoDeAdvertencia = 4000;
        public static float TiempoSinAdvertencia = 3500;

        public static int notasParaGanar = 4;

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aquí todo el código de inicialización: cargar modelos, texturas, estructuras de optimización, todo
        ///     procesamiento que podemos pre calcular para nuestro juego.
        ///     Borrar el codigo ejemplo no utilizado.
        /// </summary>

        public bool estoyJugando = false;
        public Sonido musicaMenu ;
        public Sonido sonidoBarra;
        public Sonido musicaFondoOutdoor;


        public static monstruos monstruoActual= monstruos.SECTARIAN;
        public static Microsoft.DirectX.DirectSound.Device deviceMusica;

        public override void Init()
        {

            var d3dDevice = D3DDevice.Instance.Device;
            deviceMusica = DirectSound.DsDevice;
            this.FixedTickEnable = false;

            
            musicaMenu = new Sonido("SonidoPruebaTGC(Mono).wav", true);
            musicaFondoOutdoor = new Sonido("nocturno, continuo.wav", -4250, true);

            personaje = new Personaje();
            menu.instanciarMenu();
            nota.instanciarNotas(0);
            vidaUtilVela.instanciarVelas(0);
            velita.instanciarVelita();
            vidaUtilLinterna.instanciarLinternas(0);
            linternita.instanciarLinternita();
            InstanciarSonidosRandoms();
            InstanciarSondosOutDoorRandoms();
            InstanciasSonidosInDoorRandoms();

            escenario.InstanciarEstructuras();
            monster = new Monster();
            monster.InstanciarMonster(monstruoActual);
            CrearObjetosEnEscenario();
            iluminables.Add(monster.ghost);
            iluminables.AddRange(escenario.tgcScene.Meshes);

            TgcMesh mesh1 = escenario.tgcScene.Meshes.Find(mesh => mesh.Name.Equals("linterna_1"));
            TgcMesh mesh2 = escenario.tgcScene.Meshes.Find(mesh => mesh.Name.Equals("linterna_2"));
            var linterna = new Linterna(mesh1,mesh2,this);
            objetosInteractuables.Add(linterna);
            Camera = personaje;
            //Frustum.FarPlane;
            //Camara.SetCamera(personaje.PosicionMesh(), new TGCVector3(0, 0, 0));

            //personaje.LockMouse = true;

            //Internamente el framework construye la matriz de view con estos dos vectores.
            //Luego en nuestro juego tendremos que crear una cámara que cambie la matriz de view con variables como movimientos o animaciones de escenas

        }

        private void InstanciasSonidosInDoorRandoms()
        {
            sonidosInDoorRandom.Add(new Sonido("caja fuerte, golpe en.wav", -3500, false));
            sonidosInDoorRandom.Add(new Sonido("golpe metálico.wav", -3500, false));
        }

        private void InstanciarSondosOutDoorRandoms()
        {
            sonidosOutDoorRandom.Add(new Sonido("ráfaga ventosa.wav", -3500, false));
            sonidosOutDoorRandom.Add(new Sonido("viento en arbustos.wav", -3500, false));
            sonidosOutDoorRandom.Add(new Sonido("viento, golpe de.wav", -3500, false));
            sonidosOutDoorRandom.Add(new Sonido("viento, ráfaga larga.wav", -3500, false));
            sonidosOutDoorRandom.Add(new Sonido("árbol, caída de.wav", -3500, false));
            sonidosOutDoorRandom.Add(new Sonido("búho, grito.wav", -3500, false));
            sonidosOutDoorRandom.Add(new Sonido("cascabel.wav", -4250, false));

        }

        private void InstanciarSonidosRandoms()
        {
            sonidosRandoms.Add(new Sonido("campanadas horas.wav",-3500, false));
            sonidosRandoms.Add(new Sonido("gong reloj.wav", -3500,false));
            sonidosRandoms.Add(new Sonido("campanada reloj, (1).wav",-3500, false));
        }



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
                interactuable = new Nota(mesh,this);
                objetosInteractuables.Add(interactuable);
            }
            if (mesh.Name.Equals("vela"))
            {
                interactuable = new Vela(mesh,this);
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
            if (mesh.Name.Equals("EscaleraMetalMovil")|| mesh.Name.Equals("EscaleraMetalFija"))
            {
                interactuable = new Escalera(mesh);
                objetosInteractuables.Add(interactuable);
                var escalera = (Escalera)interactuable;
                paredInvisible.InstanciarPared(escalera);
            }
        }



        public override void Update()
        {
            PreUpdate();
           

            if (!estoyJugando)
            {
                estoyJugando = Input.keyDown(Key.Space) && !estoyEnElMenu;
                if (estoyJugando) 
                {
                    sonidoBarra = new Sonido("AllAroundYou.wav", false);
                    var sonidoStart = new Sonido("auto, abrir puerta.wav", -4000, false);
                    sonidoStart.escucharSonidoActual(false);

                }
            }
            else
            {
                this.musicaMenu.DetenerSonido();
                UpdateGame();
            }

            PostUpdate();
        }

        private void UpdateGame()
        {
            //Capturar Input teclado

            RevisarLockeoMouse();

            if (personaje.LockMouse)
            {
                UpdateAccionesDeMovimientoYCamara();

                if (Input.keyDown(Key.E))
                {
                    InteraccionPersonajeYMesh();

                }
                ReproducirSonidoRandomEscenario();

                RealizarAccionesDeInventario();

                /*if (Input.keyDown(Key.H))
                {
                    personaje.tieneLuz = !personaje.tieneLuz;
                }*/

                if (personaje.chocandoConEscalera && Input.keyDown(Key.Space))
                {
                    personaje.MoverPersonaje(' ', ElapsedTime, Input, this);//lo dejo asi porque no se que hace esto
                    // #TinchoHaceteCargo
                }
                personaje.updateCamera(ElapsedTime, Input);

                personaje.aumentarTiempoSinLuz();

                AccionesPersonajeMonstruo();

                personaje.YouWin();
            }
            

        }

        private void ReproducirSonidoRandomEscenario()
        {
            if (personaje.estoyAdentro)
            {

                reproducirSonidoRandomIndoor();
            }
            else 
            {
                reproducirSonidoRandomOutdoor();
                musicaFondoOutdoor.escucharSonidoActual(true);
            }

            reproducirSonidoRandomAmbiental();
        }

        private void reproducirSonidoRandomIndoor()
        {
            reproducirRandomDeLista(sonidosInDoorRandom);
        }

        private void reproducirSonidoRandomOutdoor()
        {
            reproducirRandomDeLista(sonidosOutDoorRandom);
        }

        private void reproducirSonidoRandomAmbiental()
        {
            reproducirRandomDeLista(sonidosRandoms);
        }

        private void reproducirRandomDeLista(List<Sonido> listaSonidos)
        {
            var ran = new Random();
            if (ran.Next() % 10000 == 7)
            {
                int indice = ran.Next() % (sonidosRandoms.Count());
                listaSonidos[Math.Max(indice-1,0)].escucharSonidoActual(false);
            }
        }

        private void AccionesPersonajeMonstruo()
        {
            if (DistanciaA2(monster.ghost) < 5000)
            {

                if (personaje.tieneLuz)
                {
                    monster.HuirDe(personaje, ElapsedTime);
                    //se aleja de la luz porque tiene cuiqui
                }
                else 
                {
                    monster.MirarA(personaje, ElapsedTime);
                    monster.avanzarHaciaPersonaje(ElapsedTime,personaje);
                }
                
            }

            if (personaje.TieneItemEnMano() && personaje.tieneLuz)
            {
                personaje.getItemEnMano().DisminuirDuracion();

                if (personaje.getItemEnMano().getDuracion() <= 0)
                {
                    personaje.getItemEnMano().FinDuracion(personaje);
                }
            }

            InteraccionMonster();
        }

        private void RealizarAccionesDeInventario()
        {
            if (Input.keyDown(Key.H))
            {
                personaje.getItemEnMano().Usar(personaje);
                var linterna = personaje.objetosInteractuables.Find(item=>item is Linterna);
                if(linterna != null)
                    personaje.setItemEnMano((IEquipable)linterna);
            }

            if (Input.keyDown(Key.F))
            {
                //Prende/apaga la luz de la linterna
                if (personaje.getItemEnMano() is Linterna || personaje.getItemEnMano() is Vela)
                {
                    personaje.getItemEnMano().Usar(personaje);
                    //personaje.tieneLuz = true;
                }
                else
                {
                    //personaje.tieneLuz = false;
                }
            }

            if (Input.keyDown(Key.R))
            {
                //Recargar las pilas de la linterna
                var pila = (Pila)personaje.objetosInteractuables.Find(objeto => objeto is Pila);
                //no puedo usar una pila null
                if (pila != null)
                    pila.Usar(personaje);
            }
            
            if (Input.keyDown(Key.Q))
            {
                //Cambiar entre vela y linterna (si hubiere)
                if ((personaje.getItemEnMano() is Linterna || personaje.getItemEnMano() is ItemVacioDefault) && personaje.objetosInteractuables.Any(objeto => objeto is Vela))
                {
                    personaje.getItemEnMano().Usar(personaje);
                    var vela = (Vela)personaje.objetosInteractuables.Find(objeto => objeto is Vela);
                    personaje.setItemEnMano(vela);
                }
                else
                {
                    if ((personaje.getItemEnMano() is Vela || personaje.getItemEnMano() is ItemVacioDefault) && personaje.objetosInteractuables.Any(objeto => objeto is Linterna))
                    {
                        personaje.getItemEnMano().Usar(personaje);
                        var linterna = (Linterna)personaje.objetosInteractuables.Find(objeto => objeto is Linterna);
                        personaje.setItemEnMano(linterna);
                    }
                }
            }
        }

        private void InteraccionPersonajeYMesh()
        {
            //Interacuar con meshes

            var objetoInteractuable = this.objetosInteractuables.OrderBy(mesh => this.DistanciaA(mesh)).First();
            if ((objetoInteractuable is Escondite || objetoInteractuable is Escalera) && this.DistanciaA(objetoInteractuable) < 300)
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

        private void UpdateAccionesDeMovimientoYCamara()
        {
            bool caminar = false;
            if (Input.keyDown(Key.W))
            {
                //Le digo al wachin que vaya para adelante

                personaje.MoverPersonaje('W', ElapsedTime, Input, this);
                caminar = true;
            }

            if (Input.keyDown(Key.A))
            {
                //Le digo al wachin que vaya para la izquierda
                personaje.MoverPersonaje('A', ElapsedTime, Input, this);
                caminar = true;
            }

            if (Input.keyDown(Key.S))
            {
                //Le digo al wachin que vaya para la izquierda
                personaje.MoverPersonaje('S', ElapsedTime, Input, this);
                caminar = true;
            }

            if (Input.keyDown(Key.D))
            {
                //Le digo al wachin que vaya para la izquierda
                personaje.MoverPersonaje('D', ElapsedTime, Input, this);
                caminar = true;
            }



            personaje.MoverPersonaje('x', ElapsedTime, Input, this);
        }

        private void RevisarLockeoMouse()
        {
            if (Input.keyDown(Key.Escape))
            {
                personaje.LockMouse = !personaje.LockMouse;
            }
            else
            {
                personaje.LockMouse = true;
            }
        }

        private void InteraccionMonster()
        {
            if (!personaje.tieneLuz && !personaje.estoyEscondido && personaje.tiempoSinLuz > GameModel.TiempoSinAdvertencia)
            {
                

                Monster unBicho;
                if (personaje.tiempoSinLuz == GameModel.TiempoDeAdvertencia)
                {
                    unBicho = new Monster();
                    var posicion = personaje.puntoDemira(personaje.anguloAbsolutoEnY, personaje.anguloAbsolutoEnX);
                    var nuevaPosicion = new TGCVector3(posicion.X, -350, posicion.Z);
                    var delta = new TGCVector3(1000,0,1000);

                    if (nuevaPosicion.X > 0 )
                    {
                        delta = new TGCVector3(-1000, 0, 1000);
                    }

                    if (nuevaPosicion.Z > 0)
                    {
                        delta = new TGCVector3(delta.X, 0, -1000);
                    }

                    nuevaPosicion += delta;

                    unBicho.InstanciarMonster(personaje, nuevaPosicion,monstruoActual);
                    //monster.DisposeMonster();
                    monster = unBicho;
                    iluminables.Add(unBicho.ghost);
                    monster.reproducirSonidoRandom();
                                   

                }

                

                if (personaje.tiempoSinLuz == GameModel.TiempoDeGameOver)
                {
                    monster.DisposeMonster();
                    //El monster aparece detrás del personaje
                    unBicho = new Monster();
                    var anguloDeRotacion = FastMath.PI;

                    var posicion = personaje.puntoDemira(personaje.anguloAbsolutoEnY + anguloDeRotacion, personaje.anguloAbsolutoEnX);
                    var nuevaPosicion = new TGCVector3(posicion.X + 300, -350, posicion.Z + 300);

                    unBicho.InstanciarMonster(personaje, nuevaPosicion, monstruoActual);
                    monster = unBicho;
                    iluminables.Add(unBicho.ghost);

                    personaje.LockMouse = false;

                    var newTarget = new TGCVector3(nuevaPosicion.X, nuevaPosicion.Y + 350, nuevaPosicion.Z);
                    personaje.SetCamera(personaje.eye, newTarget);
                    monster.ReproducirSonidoGameOver();
                    personaje.GameOver(this);
                }     
                
            }
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
            foreach (TgcMesh mesh in iluminables)
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
                if (!personaje.tieneLuz && DistanciaA2(unPoste) < 2000)
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
        

        public override void Render()
        {
            //Inicio el render de la escena, para ejemplos simples. Cuando tenemos postprocesado o shaders es mejor realizar las operaciones según nuestra conveniencia.
            PreRender();

            if (!estoyJugando)
            {
                menu.renderSprite();
                
            }
            else
            {
                this.updateLighting();
                

                //Pone el fondo negro en vez del azul feo ese
                D3DDevice.Instance.Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);

                //Frustum Culling -> OPCION 1
                var meshesQueChocanConFrustrum = escenario.tgcScene.Meshes.FindAll(mesh => TgcCollisionUtils.classifyFrustumAABB(this.Frustum, mesh.BoundingBox) != TgcCollisionUtils.FrustumResult.OUTSIDE);
                meshesQueChocanConFrustrum.ForEach(mesh => mesh.Render());

                if (DistanciaA2(monster.ghost) < 5000)
                {
                    monster.RenderMonster();
                }
                //Render de BoundingBox, muy útil para debug de colisiones.
                if (BoundingBox)
                {
                    Box.BoundingBox.Render();
                    tgcScene.Meshes.ForEach(mesh => mesh.BoundingBox.Render());
                    fondo.BoundingBox.Render();
                }

                nota.renderSprite();
                vidaUtilVela.renderSprite();
                velita.renderSprite();
                vidaUtilLinterna.renderSprite();
                linternita.renderSprite();
            }

            //Frustum Culling -> OPCION 2
            /*
            foreach (var mesh in escenario.tgcScene.Meshes)
            {
                //Nos ocupamos solo de las mallas habilitadas
                //if (mesh.Enabled)
                //{
                    //Solo mostrar la malla si colisiona contra el Frustum
                    var colisionConFrustum = TgcCollisionUtils.classifyFrustumAABB(Frustum, mesh.BoundingBox);
                    if (colisionConFrustum != TgcCollisionUtils.FrustumResult.OUTSIDE)
                    {
                        mesh.Render();
                    }
                //}
            }*/

            //personaje.RenderPersonaje(ElapsedTime);

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
            
            menu.disposeSprite();
            nota.disposeSprite();
            vidaUtilVela.disposeSprite();
            velita.disposeSprite();
            vidaUtilLinterna.disposeSprite();
            linternita.disposeSprite();

            paredInvisible.DisposePared();
        }

    }
}