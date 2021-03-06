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

        public double tiempoDeRotacion = 0;

        public static bool estoyEnElMenu = true;
        public static bool perdi = false;
        public static bool estoyCorriendo = false;

        public float timer = 0f;

        //PARED INVISIBLE
        public ParedInvisible paredInvisible = new ParedInvisible();

        //public List<Monster> bichos = new List<Monster>();

        public List<IInteractuable> objetosInteractuables = new List<IInteractuable>();
        public List<TgcMesh> iluminables = new List<TgcMesh>();
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
        public static GameModel instancia;
        public static int notasParaGanar = 4;

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aquí todo el código de inicialización: cargar modelos, texturas, estructuras de optimización, todo
        ///     procesamiento que podemos pre calcular para nuestro juego.
        ///     Borrar el codigo ejemplo no utilizado.
        /// </summary>

        public bool estoyJugando = false;
        public Sonido musicaMenu;
        public Sonido sonidoBarra;
        public Sonido musicaFondoOutdoor;
        public Sonido estatica;
        public Sonido agarrarPagina;
        public Sonido humanHeartbeat;
        public Sonido respiracion;


        public static monstruos monstruoActual = monstruos.SECTARIAN;
        public static Microsoft.DirectX.DirectSound.Device deviceMusica;
        //private TgcScreenQuad fullScreenQuad;
        //private VertexBuffer vertexBuffer;
        public Surface depthStencil;
        private Texture renderTarget;
        public List<FullscreenQuad> quads;
        public MonsterBlur monsterBlur;

        ITecnicaRender renderizado;

        //string lala = "..\\..\\..\\shaders\\";
        public Microsoft.DirectX.Direct3D.Effect effectPosProcesado;
        //bool render = false;


        public Sombras sombras;

        public override void Init()
        {
            var d3dDevice = D3DDevice.Instance.Device;
            deviceMusica = DirectSound.DsDevice;
            this.FixedTickEnable = false;

            GameModel.instancia = this;
            musicaMenu = new Sonido("SonidoPruebaTGC(Mono).wav", true);
            musicaFondoOutdoor = new Sonido("nocturno, continuo.wav", -3000, true);
            estatica = new Sonido("Radio Static-SoundBible.com-629277574.wav", -2500, true);
            agarrarPagina = new Sonido("Page_Turn-Mark_DiAngelo-1304638748.wav", -300, false);
            humanHeartbeat = new Sonido("human-heartbeat-daniel_simon.wav", -1000, false);
            respiracion = new Sonido("Breathing Vent-SoundBible.com-18702822.wav", -600, false);

            //CreateFullScreenQuad();
            CreateRenderTarget();

            personaje = new Personaje();
            menu.instanciarMenu();
            nota.instanciarNotas(0);
            vidaUtilVela.instanciarVelas(0);
            velita.instanciarVelita();
            vidaUtilLinterna.instanciarLinternas(0);
            linternita.instanciarLinternita();
            InstanciarSonidosRandoms();
            InstanciarSonidosOutDoorRandoms();
            InstanciasSonidosInDoorRandoms();

            escenario.InstanciarEstructuras();
            monster = new Monster();
            monster.InstanciarMonster(monstruoActual);
            CrearObjetosEnEscenario();
            //iluminables.Add(monster.ghost);
            //iluminables.AddRange(escenario.tgcScene.Meshes);

            TgcMesh mesh1 = escenario.tgcScene.Meshes.Find(mesh => mesh.Name.Equals("linterna_1"));
            TgcMesh mesh2 = escenario.tgcScene.Meshes.Find(mesh => mesh.Name.Equals("linterna_2"));
            var linterna = new Linterna(mesh1, mesh2, this);
            objetosInteractuables.Add(linterna);

            Camera = personaje;

            quads = new List<FullscreenQuad>();

            //ShadersDir
            effectPosProcesado = TGCShaders.Instance.LoadEffect(ShadersDir + "PostProcesado.fx");
            effectPosProcesado.Technique = "PostProcessDefault";
            var unQuad = new FullscreenQuad(effectPosProcesado);
            unQuad.loEstoyUsando = true;
            this.quads.Add(unQuad);

            sombras = new Sombras(this);
            sombras.InstanciarSombras();

            this.renderizado = sombras;

            monsterBlur = new MonsterBlur(escenario,this);
            monsterBlur.instanciarMonsterBlur(ShadersDir, monster, MediaDir);
        }

        private void InstanciasSonidosInDoorRandoms()
        {
            sonidosInDoorRandom.Add(new Sonido("caja fuerte, golpe en.wav", -3500, false));
            sonidosInDoorRandom.Add(new Sonido("golpe metálico.wav", -3500, false));
        }

        private void InstanciarSonidosOutDoorRandoms()
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
            sonidosRandoms.Add(new Sonido("campanadas horas.wav", -3500, false));
            sonidosRandoms.Add(new Sonido("gong reloj.wav", -3500, false));
            sonidosRandoms.Add(new Sonido("campanada reloj, (1).wav", -3500, false));
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
                interactuable = new Nota(mesh, this);
                objetosInteractuables.Add(interactuable);
            }
            if (mesh.Name.Equals("vela"))
            {
                interactuable = new Vela(mesh, this);
                objetosInteractuables.Add(interactuable);
            }
            if (mesh.Name.Equals("pilas"))
            {
                interactuable = new Pila(mesh);
                objetosInteractuables.Add(interactuable);
            }
            if (mesh.Name.Equals("NVG"))
            {
                interactuable = new VisionNocturna(mesh, this);
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
                interactuable = new Escondite(mesh, this);
                objetosInteractuables.Add(interactuable);
            }
            if (mesh.Name.Equals("EscaleraMetalMovil") || mesh.Name.Equals("EscaleraMetalFija"))
            {
                interactuable = new Escalera(mesh);
                objetosInteractuables.Add(interactuable);
                var escalera = (Escalera)interactuable;
                paredInvisible.InstanciarPared(escalera);
            }
            if (mesh.Name.Equals("maleta")) {
                interactuable = new Maleta(mesh);
                objetosInteractuables.Add(interactuable);
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
                    var sonidoStart = new Sonido("auto, abrir puerta.wav", -3000, false);
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
                if (!perdi)
                    UpdateAccionesDeMovimientoYCamara();

                if (Input.keyDown(Key.E))
                {
                    InteraccionPersonajeYMesh();

                }

                ReproducirSonidoRandomEscenario();

                reproducirRandomDeLista(monster.getSoundList());

                RealizarAccionesDeInventario();

                personaje.updateCamera(ElapsedTime, Input);

                personaje.aumentarTiempoSinLuz();

                if (personaje.tieneLuz)
                    personaje.itemEnMano.DisminuirDuracion(personaje);

                AccionesPersonajeMonstruo();
            }

            timer += ElapsedTime;
            var d3dDevice = D3DDevice.Instance.Device;

            effectPosProcesado.SetValue("eyePosition", TGCVector3.TGCVector3ToFloat3Array(personaje.Position));

            effectPosProcesado.SetValue("screenWidth", d3dDevice.PresentationParameters.BackBufferWidth);
            effectPosProcesado.SetValue("screenHeight", d3dDevice.PresentationParameters.BackBufferHeight);

            effectPosProcesado.SetValue("timer", timer);

            if(renderizado is MonsterBlur)
                monsterBlur.UpdateMonsterBlur(ElapsedTime,monster);
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
            if (ran.Next() % 5000 == 7)
            {
                int indice = ran.Next() % (sonidosRandoms.Count());
                listaSonidos[Math.Max(indice - 1, 0)].escucharSonidoActual(false);
            }
        }

        private void AccionesPersonajeMonstruo()
        {
            if (DistanciaA2(monster.ghost) < 5000)
            {

                if (personaje.tieneLuz)
                {
                    //Se aleja de la luz porque tiene cuiqui
                    monster.HuirDe(personaje, ElapsedTime);
                }
                else
                {
                    monster.MirarA(personaje, ElapsedTime);
                }

            }
            InteraccionMonster();
        }

        private void RealizarAccionesDeInventario()
        {

            if (Input.keyDown(Key.H)) //deprecado
            {
                personaje.getItemEnMano().Usar(personaje);
                var linterna = personaje.objetosInteractuables.Find(item => item is Linterna);
                if (linterna != null)
                    personaje.setItemEnMano((IEquipable)linterna);
            }

            if (Input.keyDown(Key.F))
            {
                personaje.getItemEnMano().Usar(personaje);
            }

            if (Input.keyDown(Key.R))
            {
                //Recargar las pilas de la linterna
                var pila = (Pila)personaje.objetosInteractuables.Find(objeto => objeto is Pila);
                //No puedo usar una pila null
                if (pila != null)
                    pila.Usar(personaje);
            }

            if (Input.keyDown(Key.Q))
            {
                Inventario.objetoSiguiente(personaje);
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

                this.renderizado = monsterBlur;

                
                if (personaje.tiempoSinLuz == GameModel.TiempoDeAdvertencia)
                {
                    
                    var posicion = personaje.puntoDemira(personaje.anguloAbsolutoEnY, personaje.anguloAbsolutoEnX);
                    var nuevaPosicion = new TGCVector3(posicion.X, -350, posicion.Z);
                    var delta = new TGCVector3(1000, 0, 1000);

                    if (nuevaPosicion.X > 0)
                    {
                        delta = new TGCVector3(-1000, 0, 1000);
                    }

                    if (nuevaPosicion.Z > 0)
                    {
                        delta = new TGCVector3(delta.X, 0, -1000);
                    }

                    nuevaPosicion += delta;
                    
                    monster.InstanciarMonster(personaje, nuevaPosicion, monstruoActual);
                    //escenario.tgcScene.Meshes.Add(monster.ghost);
                    //monster.DisposeMonster();
                    monster.reproducirSonidoRandom();
                    estatica.escucharSonidoActual(true);
                    personaje.viAlMonster = true;

                }

                if (personaje.tiempoSinLuz == GameModel.TiempoDeGameOver)
                {
                    perdi = true;
                    tiempoDeRotacion = 0;

                    monster.DisposeMonster();
                    //El monster aparece detrás del personaje
                    var anguloDeRotacion = FastMath.PI;

                    var posicion = personaje.puntoDemira(personaje.anguloAbsolutoEnY + anguloDeRotacion, personaje.anguloAbsolutoEnX);
                    var nuevaPosicion = new TGCVector3(posicion.X + 300, -350, posicion.Z + 300);

                    monster.InstanciarMonster(personaje, nuevaPosicion, monstruoActual);
                    //escenario.tgcScene.Meshes.Add(unBicho.ghost);

                    personaje.LockMouse = false;

                    //escenario.tgcScene.Meshes.Add(monster.ghost);
                    var newTarget = new TGCVector3(nuevaPosicion.X, nuevaPosicion.Y + 350, nuevaPosicion.Z);
                    personaje.SetCamera(personaje.eye, newTarget);
                    monster.ReproducirSonidoGameOver();
                    //personaje.GameOver(this);
                }

                

                if (tiempoDeRotacion > 6 && personaje.tiempoSinLuz > GameModel.TiempoDeGameOver)
                {
                    personaje.GameOver(this);
                }

                tiempoDeRotacion += ElapsedTime;
            }
            else
            {
                renderizado = this.sombras;
            }
        }


        private double DistanciaA(IInteractuable mesh)
        {
            TGCVector3 vector = personaje.getPosition() - mesh.getPosition();

            return Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Z, 2));

        }

        public double DistanciaA2(TgcMesh mesh)
        {
            TGCVector3 vector = personaje.getPosition() - mesh.BoundingBox.PMin;

            return Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Z, 2));

        }

        private void updateLighting()
        {
            Microsoft.DirectX.Direct3D.Effect currentShader;

            //Con luz: Cambiar el shader actual por el shader default que trae el framework para iluminacion dinamica con PointLight
            if (personaje.tieneLuz)
            {

                currentShader = TGCShaders.Instance.TgcMeshSpotLightShader;
            }
            else
            {
                currentShader = TGCShaders.Instance.TgcMeshPointLightShader;
            }

            if (personaje.estoyAdentro)
            {
                currentShader = TGCShaders.Instance.LoadEffect(ShadersDir + "ShaderIndoor.fx");
            }

            var iluminablesFiltrado = iluminables.FindAll(mesh => TgcCollisionUtils.classifyFrustumAABB(this.Frustum, mesh.BoundingBox) != TgcCollisionUtils.FrustumResult.OUTSIDE);

            //Aplicar a cada mesh el shader actual
            foreach (TgcMesh mesh in iluminablesFiltrado)
            {
                this.AplicarShader(mesh, currentShader);
            }
        }

        private void AplicarShader(TgcMesh mesh, Microsoft.DirectX.Direct3D.Effect currentShader)
        {
            TgcMesh unPoste;
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
            renderizado.SetearTecnica();
            renderizado.AplicarRender();
        }

        public void RenderQuad(FullscreenQuad unQuad)
        {
            var device = D3DDevice.Instance.Device;

            device.VertexFormat = CustomVertex.PositionTextured.Format;
            device.SetStreamSource(0, unQuad.unQuad.ScreenQuadVB, 0);
            unQuad.unEfecto.SetValue("renderTarget", renderTarget1);

            // Dibujamos el full screen quad
            unQuad.unEfecto.Begin(FX.None);
            unQuad.unEfecto.BeginPass(0);
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            unQuad.unEfecto.EndPass();
            unQuad.unEfecto.End();
        }

        public void OurFullScreenQuad(Microsoft.DirectX.Direct3D.Device device, Microsoft.DirectX.Direct3D.Effect efecto){
            
            nota.renderSprite();
            vidaUtilVela.renderSprite();
            velita.renderSprite();
            vidaUtilLinterna.renderSprite();
            linternita.renderSprite();     
        }
        private void GameRender()
        {

            //RenderPantallaConMonsterCerca();
            //this.updateLighting();
            //sombras.renderSombras(personaje.getPosition(),personaje.getLookAt(), new TGCVector3(100, 10, -150));

            //var sombrasFaroles = new Sombras(this);

            
                TgcMesh unPoste = escenario.listaDePostes.OrderBy(poste => this.DistanciaA2(poste)).First();
                if (!personaje.tieneLuz && DistanciaA2(unPoste) < 1000)
                {
                    //Se prende el farol mas cercano
                    sombras.renderSombras(unPoste.BoundingBox.PMin, new TGCVector3(unPoste.BoundingBox.PMin.X, 15, unPoste.BoundingBox.PMin.Z), new TGCVector3(15, 380, 15));

                }
                else
                {
                    //01158354515 --> Numero de Marce
                    sombras.renderSombras(personaje.getPosition(), personaje.getLookAt(), new TGCVector3(100, 10, -150));
                    //monsterBlur.RenderMonsterBlur();
                }

            //monsterBlur.RenderMonsterBlur();



            /*if (personaje.tiempoSinLuz > GameModel.TiempoDeAdvertencia)
            {
                monsterBlur.RenderMonsterBlur();
            }*/


            //Frustum Culling -> OPCION 1
            //var meshesQueChocanConFrustrum = escenario.tgcScene.Meshes.FindAll(mesh => TgcCollisionUtils.classifyFrustumAABB(this.Frustum, mesh.BoundingBox) != TgcCollisionUtils.FrustumResult.OUTSIDE);
            //meshesQueChocanConFrustrum.ForEach(mesh => mesh.Render());

            /*
            if (DistanciaA2(monster.ghost) < 5000)
            {
                monster.RenderMonster();
            }*/


            //Render de BoundingBox, muy útil para debug de colisiones.
            /*if (BoundingBox)
            {
                Box.BoundingBox.Render();
                tgcScene.Meshes.ForEach(mesh => mesh.BoundingBox.Render());
                fondo.BoundingBox.Render();
            }*/
            /*
            nota.renderSprite();
            vidaUtilVela.renderSprite();
            velita.renderSprite();
            vidaUtilLinterna.renderSprite();
            linternita.renderSprite();
            */
        }
        

        private VertexBuffer fullScreenQuad1;
        private void CreateFullScreenQuad()
        {
            var d3dDevice = D3DDevice.Instance.Device;

            // Creamos un FullScreen Quad
            CustomVertex.PositionTextured[] vertices =
            {
                new CustomVertex.PositionTextured(-1, 1, 1, 0, 0),
                new CustomVertex.PositionTextured(1, 1, 1, 1, 0),
                new CustomVertex.PositionTextured(-1, -1, 1, 0, 1),
                new CustomVertex.PositionTextured(1, -1, 1, 1, 1)
            };

            // Vertex buffer de los triangulos
            fullScreenQuad1 = new VertexBuffer(typeof(CustomVertex.PositionTextured), 4, d3dDevice, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionTextured.Format, Pool.Default);
            fullScreenQuad1.SetData(vertices, 0, LockFlags.None);

        }

        public Texture renderTarget1;
        private void CreateRenderTarget()
        {
            var d3dDevice = D3DDevice.Instance.Device;

            depthStencil = d3dDevice.CreateDepthStencilSurface(d3dDevice.PresentationParameters.BackBufferWidth, d3dDevice.PresentationParameters.BackBufferHeight, DepthFormat.D24S8, MultiSampleType.None, 0, true);

            renderTarget1 = new Texture(d3dDevice, d3dDevice.PresentationParameters.BackBufferWidth, d3dDevice.PresentationParameters.BackBufferHeight, 1, Usage.RenderTarget, Format.X8R8G8B8, Pool.Default);
        }
        /*
        private void CreateFullScreenQuad()
        {
            var d3dDevice = D3DDevice.Instance.Device;

            // Creamos un FullScreen Quad
            CustomVertex.PositionTextured[] vertices =
            {
                new CustomVertex.PositionTextured(-1, 1, 1, 0, 0),
                new CustomVertex.PositionTextured(1, 1, 1, 1, 0),
                new CustomVertex.PositionTextured(-1, -1, 1, 0, 1),
                new CustomVertex.PositionTextured(1, -1, 1, 1, 1)
            };

            // Vertex buffer de los triangulos
            //fullScreenQuad = new VertexBuffer(typeof(CustomVertex.PositionTextured), 4, d3dDevice, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionTextured.Format, Pool.Default);
            //fullScreenQuad.SetData(vertices, 0, LockFlags.None);
        }

        private void CreateRenderTarget()
        {
            var d3dDevice = D3DDevice.Instance.Device;

            depthStencil = d3dDevice.CreateDepthStencilSurface(d3dDevice.PresentationParameters.BackBufferWidth, d3dDevice.PresentationParameters.BackBufferHeight, DepthFormat.D24S8, MultiSampleType.None, 0, true);

            renderTarget = new Texture(d3dDevice, d3dDevice.PresentationParameters.BackBufferWidth, d3dDevice.PresentationParameters.BackBufferHeight, 1, Usage.RenderTarget, Format.X8R8G8B8, Pool.Default);
        }*/

        
        public void RenderFps()
        {
            RenderFPS();
        }




        private void RenderPantallaConMonsterCerca()
        {   
            //esta condicion es para que pueda ver al monster cuando me atrapa y tambien para que se aplique cuando aparece antes
            if (GameModel.TiempoDeAdvertencia < personaje.tiempoSinLuz && GameModel.TiempoDeGameOver < personaje.tiempoSinLuz)
            {
                effectPosProcesado.Technique = "PostProcessMonster";
            }
        }

        /// <summary>
        ///     Se llama cuando termina la ejecución del ejemplo.
        ///     Hacer Dispose() de todos los objetos creados.
        ///     Es muy importante liberar los recursos, sobretodo los gráficos ya que quedan bloqueados en el device de video.
        /// </summary>
        public override void Dispose()
        {
            //monsterBlur.DisposeMonsterBlur();
            escenario.DisposeEscenario();
            //personaje.DisposePersonaje();
            //monster.DisposeMonster();     
            menu.disposeSprite();
            nota.disposeSprite();
            vidaUtilVela.disposeSprite();
            velita.disposeSprite();
            vidaUtilLinterna.disposeSprite();
            linternita.disposeSprite();
            paredInvisible.DisposePared();
            //fullScreenQuad.dispose();
            this.depthStencil.Dispose();

        }

    }
}