using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX.DirectInput;
using System.Drawing;
using System.Windows.Forms;
using TGC.Core.Camara;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.BoundingVolumes;
using Device = Microsoft.DirectX.Direct3D.Device;
using BulletSharp;
using System.Collections;
using TGC.Core.SceneLoader;
using Microsoft.DirectX.Direct3D;
using TGC.Core.Collision;
using TGC.Core.SkeletalAnimation;
using System.Runtime.InteropServices.WindowsRuntime;

namespace TGC.Group.Model
{
    public class Personaje : TgcCamera
    {
        public List<IInteractuable> objetosInteractuables = new List<IInteractuable>();
        public int cantidadNotas;
        public int notasRequeridas = 0;
        public bool tieneLuz = false;
        public float tiempoSinLuz = 0;
        public TgcMesh meshPersonaje;
        public IEquipable itemEnMano;

        public IEquipable getItemEnMano()
        {
            return itemEnMano;
        }

        public void setItemEnMano(IEquipable nuevoItemEnMano)
        {
            itemEnMano = nuevoItemEnMano;
        }

        public bool Enable { get; set; }

        /// <summary>
        /// Hacia donde es X, desde la perspectiva de la camara.
        /// </summary>
        TGCVector3 xAxis;

        /// <summary>
        /// Hacia donde es Y, desde la perspectiva de la camara.
        /// </summary>
        TGCVector3 yAxis;

        /// <summary>
        /// Hacia donde es Z, desde la perspectiva de la camara.
        /// </summary>
        TGCVector3 zAxis;

        /// <summary>
        /// Hacia adonde es "adelante" (Z+ desde la perspectiva del que camina).
        /// </summary>
        public TGCVector3 forward;

        /// <summary>
        /// Hacia donde mira la camara, desde los ejes del mundo.
        /// </summary>
        TGCVector3 target;

        /// <summary>
        /// Donde esta la camara, desde los ejes del mundo.
        /// </summary>
        public TGCVector3 eye;

        /// <summary>
        /// Hacia donde es arriba, desde los ejes del mundo.
        /// </summary>
        readonly TGCVector3 up = new TGCVector3(0.0f, 1.0f, 0.0f);

        /// <summary>
        /// Matriz de vista
        /// </summary>
        TGCMatrix vM;

        /// <summary>
        /// true si la posicion cambio desde el ultimo render.
        /// </summary>
        bool positionChanged;

        /// <summary>
        /// true si la direccion de vista cambio desde el ultimo render.
        /// </summary>
        bool rotationChanged;

        /// <summary>
        /// La rotacion total sobre el eje x.
        /// </summary>
        public float absoluteRotationX;

        public char key_left;
        public char key_back;
        public char key_right;
        public char key_forward;
        public float MovementSpeed { get; set; }

        /// <summary>
        /// Cuánto mas rapido puede ir cuando camina hacia adelante.
        /// </summary>
        public float ForwardFactor { get; set; }

        /// <summary>
        /// La velocidad actual de la camara.
        /// </summary>
        public float CurrentSpeed { get; set; }

        float rotationSpeed;

        /// <summary>
        /// Velocidad de rotacion, en grados / ( segundo * pix ).
        /// </summary>
        public float RotationSpeed
        {
            set { rotationSpeed = FastMath.ToRad(value); }
            get { return FastMath.ToDeg(rotationSpeed); }
        }

        float maxTopAngle;
        float maxBottomAngle;

        /// <summary>
        /// Establece el maximo angulo de rotacion X (pitch) hacia arriba, en grados.
        /// </summary>
        public float MaxTopAngle
        {
            set { maxTopAngle = FastMath.ToRad(value); }
            get { return FastMath.ToDeg(maxTopAngle); }
        }

        /// <summary>
        /// Establece el maximo angulo de rotacion X (pitch) hacia abajo, en grados.
        /// </summary>
        public float MaxBottomAngle
        {
            set { maxBottomAngle = FastMath.ToRad(value); }
            get { return FastMath.ToDeg(maxBottomAngle); }
        }

        /// <summary>
        /// true si el mouse esta actualmente capturado por la camara.
        /// </summary>
        private bool lockMouse;

        /// <summary>
        /// Centro de la ventana actual, en coordenadas de la pantalla.
        /// </summary>
        private Point windowCenter;

        /// <summary>
        /// El sonido usado para caminar.
        /// </summary>
        //public TgcStaticSound MovementSound { get; set; }

        private TgcBoundingAxisAlignBox boundingBox;

        /// <summary>
        /// Retorna la AABB de la posicion actual.
        /// </summary>
        public TgcBoundingAxisAlignBox BoundingBox
        {
            get { return boundingBox; }
        }

        /// <summary>
        /// Controla la captura del mouse.
        /// </summary>
        /// 

        private TgcSkeletalMesh personaje;

        public TGCVector3 posicionAnterior;
        public bool LockMouse
        {
            set
            {
                lockMouse = value;

                if (lockMouse)
                    Cursor.Hide();

                else
                    Cursor.Show();

            }

            get { return lockMouse; }
        }

        String MediaDir = "..\\..\\..\\Media\\";
        public bool estoyAdentro;
        public bool estoyEscondido;
        TGCVector3 posicionInicial;
        internal bool estoyUsandoEscaleras=false;

        public Personaje()
        {
            estoyAdentro = true;
            estoyEscondido = false;

            positionChanged = true;
            rotationChanged = true;

            posicionInicial = new TGCVector3(100f, 15f, 100f); //Estaria piola que arranque afuera del indoor

            target = new TGCVector3(100f, 15f, 150f);
            eye = new TGCVector3(100f, 15f, 100f);

            ItemVacioDefault itemDefault = new ItemVacioDefault();

            objetosInteractuables.Add(itemDefault);

            this.itemEnMano = itemDefault;

            var loader = new TgcSceneLoader();
            var scene2 = loader.loadSceneFromFile(MediaDir + "Modelame\\GhostGrande-TgcScene.xml"); //Con demon no funca, aca rompe

            //Solo nos interesa el primer modelo de esta escena (tiene solo uno)
            meshPersonaje = scene2.Meshes[0];

            const float cte = 15f;
            meshPersonaje.Position = new TGCVector3(100f, cte, 100f);
            meshPersonaje.Scale = new TGCVector3(0f, 0.5f, 0f);

            const int cteY = -570;
            meshPersonaje.BoundingBox = new TgcBoundingAxisAlignBox(new TGCVector3(-20, cteY, -20), new TGCVector3(20, 20, 20));


            // \todo: configurable
            float half_box = 4.0f;
            boundingBox = new TgcBoundingAxisAlignBox(
                new TGCVector3(eye.X - half_box, 0.0f, eye.Z - half_box),
                new TGCVector3(eye.X + half_box, eye.Y, eye.Z + half_box));

            vM = TGCMatrix.Identity;

            xAxis = new TGCVector3();
            yAxis = new TGCVector3();
            zAxis = new TGCVector3();
            forward = new TGCVector3();
             key_left='A';
             key_back = 'S';
             key_right = 'D';
             key_forward = 'W';

            absoluteRotationX = 0.0f;

            MovementSpeed = 50.0f;
            ForwardFactor = 1.5f;
            RotationSpeed = 1.5f;
            MaxTopAngle = 88.0f;
            MaxBottomAngle = -80.0f;
            CurrentSpeed = MovementSpeed;

            Control window = D3DDevice.Instance.Device.CreationParameters.FocusWindow;

            windowCenter = window.PointToScreen(
                new Point(window.Width / 2, window.Height / 2));

            lockMouse = false;

            Enable = true;

            setCamera(eye, target);
        }


        public TGCVector3 getPosition()
        {
            return eye;
        }

        /// <returns>
        /// Retorna el vector donde mira la camara (a una distancia
        /// de 1.0f del ojo), en relacion al mundo.
        /// </returns>
        public TGCVector3 getLookAt()
        {
            return target;
        }

        /// <summary>
        /// Actualizar el estado interno de la cámara en cada frame
        /// </summary>
        public void updateCamera(float elapsedTime, TgcD3dInput input)
        {
            if (!Enable)
                return;

            if (input.keyPressed(Key.L))
                LockMouse = !LockMouse;

            if (!LockMouse)
                return;

            // posicion
            //
            bool moved = false;
            TGCVector3 movement = new TGCVector3(0.0f, 0.0f, 0.0f);

            

            if (input.keyDown(Key.W))
            {
                movement += forward * (MovementSpeed * elapsedTime * ForwardFactor);
                moved = true;
            }

            if (input.keyDown(Key.A))
            {
                movement += xAxis * (-MovementSpeed * elapsedTime);
                moved = true;
            }

            if (input.keyDown(Key.S))
            {
                movement += forward * (-MovementSpeed * elapsedTime);
                moved = true;
            }

            if (input.keyDown(Key.D))
            {
                movement += xAxis * (MovementSpeed * elapsedTime);
                moved = true;
            }

           /* if (moved)
            {
                move(movement);

                MovementSound.play();
            }
            */
            // rotacion
            //

            // invertidos: moverse en x cambia el heading (rotacion sobre y) y viceversa.
            float rotY = input.XposRelative * rotationSpeed;
            float rotX = input.YposRelative * rotationSpeed;

            if (rotY != 0.0f || rotX != 0.0f)
                look(rotX, rotY);

            if (lockMouse)
                Cursor.Position = windowCenter;

        }

        /// <summary>
        /// Rota en los deltas indicados.
        /// </summary>
        /// <param name="rotX"></param>
        /// <param name="rotY"></param>
        private void look(float rotX, float rotY)
        {
            // Controlar los limites de rotacion sobre X (pitch)
           
            absoluteRotationX += rotX;

            if (absoluteRotationX > maxTopAngle)
            {
                rotX = maxTopAngle - (absoluteRotationX - rotX);
                absoluteRotationX = maxTopAngle;
            }
            else if (absoluteRotationX < maxBottomAngle)
            {
                rotX = maxBottomAngle - (absoluteRotationX - rotX);
                absoluteRotationX = maxBottomAngle;
            }

            // rotar la camara
            //

            // \todo optimize ?
            TGCMatrix deltaRM =
                TGCMatrix.RotationAxis(xAxis, rotX) *
                TGCMatrix.RotationAxis(up, rotY);

            TGCVector4 result;

            result = TGCVector3.Transform(xAxis, deltaRM);
            xAxis = new TGCVector3(result.X, result.Y, result.Z);

            result = TGCVector3.Transform(yAxis, deltaRM);
            yAxis = new TGCVector3(result.X, result.Y, result.Z);

            result = TGCVector3.Transform(zAxis, deltaRM);
            zAxis = new TGCVector3(result.X, result.Y, result.Z);

            // Recalcular las dependencias

            forward = TGCVector3.Cross(xAxis, up);
            
            forward.Normalize();
            if (Math.Abs(forward.X) > Math.Abs(forward.Y) + Math.Abs(forward.Z))
            {
                if (forward.X > 0) {

                    key_right='S';
                    key_back = 'A';
                    key_forward = 'D';
                    key_left = 'W';
                } else {
                    key_right = 'W';
                    key_back = 'D';
                    key_forward = 'A';
                    key_left = 'S';
                }
               

                //miro para adelante en x
            }
            else if (Math.Abs(forward.Y) > Math.Abs(forward.X) + Math.Abs(forward.Z))
            {
                //miro para adelante en Y

            }
            else {
                //Miro para adelante en z
                if (forward.Z > 0)
                {
                    key_right = 'A';
                    key_back = 'W';
                    key_forward = 'S';
                    key_left = 'D';

                }
                else
                {
                    key_right = 'D';
                    key_back = 'S';
                    key_forward = 'W';
                    key_left = 'A';
                    
                }
            }

            target = eye + zAxis;

            rotationChanged = true;
        }

        public void setCamera(TGCVector3 eye, TGCVector3 target)
        {
            this.eye = eye;
            this.target = target;

            zAxis = target - eye;
            zAxis.Normalize();

            xAxis = TGCVector3.Cross(up, zAxis);
            xAxis.Normalize();

            yAxis = TGCVector3.Cross(zAxis, xAxis);
            yAxis.Normalize();

            forward = TGCVector3.Cross(xAxis, up);
            forward.Normalize();

            rotationChanged = true;
            positionChanged = true;
        }

        /// <summary>
        /// Entrega la matriz de vista a D3D.
        /// </summary>
        public void updateViewMatrix(Microsoft.DirectX.Direct3D.Device d3dDevice)
        {
            if (!Enable)
                return;

            rebuildViewMatrix();

            d3dDevice.Transform.View = vM;
        }

        public void move(TGCVector3 delta)
        {
            eye += delta;
            target += delta;

            boundingBox.move(delta);

            positionChanged = true;
        }

        /// <summary>
        /// Actualiza la matriz de vista, solo lo que sea necesario.
        /// </summary>
        void rebuildViewMatrix()
        {
            if (rotationChanged)
                goto Rotation;
            else if (positionChanged)
                goto Position;
            else
                return;

            Rotation:
            vM.M11 = xAxis.X; vM.M12 = yAxis.X; vM.M13 = zAxis.X; // (1,4) = 0
            vM.M21 = xAxis.Y; vM.M22 = yAxis.Y; vM.M23 = zAxis.Y; // (2,4) = 0
            vM.M31 = xAxis.Z; vM.M32 = yAxis.Z; vM.M33 = zAxis.Z; // (3,4) = 0

            rotationChanged = false;

        Position:
            vM.M41 = -TGCVector3.Dot(xAxis, eye);
            vM.M42 = -TGCVector3.Dot(yAxis, eye);
            vM.M43 = -TGCVector3.Dot(zAxis, eye);
            // (4,4) = 1

            positionChanged = false;
        }

        public void Dispose()
        {
           // MovementSound.dispose();
        }

        public void MoverPersonaje(char key, float elapsedTime, TgcD3dInput input, Escenario escenario, Monster monster)
        {
            MovementSpeed = 800.0f;
            var movimiento = TGCVector3.Empty;
            var posicionOriginal = this.Position;

            var moving = false;
            var estoyFlotando = false;

            if (key == key_forward)
            {
                movimiento.Z = -1;
                moving = true;
            }

            if (key == key_left)
            {
                movimiento.X = 1;
                moving = true;
            }

            if (key == key_back)
            {
                movimiento.Z = 1;
                moving = true;
            }


            if (key == key_right)
            {
                movimiento.X = -1;
                moving = true;
            }

            if (key == ' ')
            {
                movimiento.Y = 1;
                moving = true;
            }
            

            if (moving)
            {
                this.posicionAnterior = this.Position;

                var lastPos = meshPersonaje.Position;

                movimiento *= MovementSpeed * elapsedTime;
                meshPersonaje.Position = meshPersonaje.Position + movimiento;
                meshPersonaje.updateBoundingBox();

                //COLISIONES

                bool chocaron = escenario.tgcScene.Meshes.Any(mesh => TgcCollisionUtils.testAABBAABB(mesh.BoundingBox, meshPersonaje.BoundingBox));
                if (chocaron)
                {
                    meshPersonaje.Position = lastPos;
                }

                bool chocoConMonster = TgcCollisionUtils.testAABBAABB(monster.ghost.BoundingBox, meshPersonaje.BoundingBox);
                if (chocoConMonster)
                {
                    meshPersonaje.Position = lastPos;
                }

                meshPersonaje.Transform = TGCMatrix.Scaling(meshPersonaje.Scale) *
                                    TGCMatrix.RotationYawPitchRoll(meshPersonaje.Rotation.Y, meshPersonaje.Rotation.X, meshPersonaje.Rotation.Z) *
                                    TGCMatrix.Translation(meshPersonaje.Position);

                this.Position = meshPersonaje.Position;
                //Hacer que la camara siga al personaje en su nueva posicion
                //camaraInterna.Target = this.Position;
            }

            float rotY = input.XposRelative * rotationSpeed;
            float rotX = input.YposRelative * rotationSpeed;
            eye = this.Position;
            target += movimiento;
            if (lockMouse)
            {
                if (rotY != 0.0f || rotX != 0.0f)
                    look(rotX, rotY);

                Cursor.Position = windowCenter;

            }
            this.SetCamera(eye, target);

        }

        public void aumentarTiempoSinLuz()
        {
            if (!this.tieneLuz)
            {
                this.tiempoSinLuz++;
            }
        }
        
        public void InteractuarConObjeto(IInteractuable objeto)
        {
            objeto.Interactuar(this);
        }

        public void UsarItemEnMano()
        {
            itemEnMano.Usar(this);
        }

        public void EquiparVela()
        {
            var vela = (Vela)objetosInteractuables.Find(objeto => objeto is Vela);
            itemEnMano = vela;
        }

        public void EquiparLinterna()
        {
            var linterna = (Linterna)objetosInteractuables.Find(objeto => objeto is Linterna);
            itemEnMano = linterna;
        }

        public bool TieneItemEnMano()
        {
            return this.getItemEnMano() != null;
        }

        public void AsustarAlPersonaje(TGCVector3 posicionDelMonster)
        {   
            TGCVector3 vectorDesfasaje = new TGCVector3(0, 350, 0);

            vectorDesfasaje += posicionDelMonster;

            this.setCamera(eye, vectorDesfasaje);
        }

        public void GameOver()
        {
            //Por ahora lo dejamos asi hasta que tengamos una interfaz grafica
            Console.WriteLine("Game Over");
        }
        public void YouWin()
        {
            if (this.notasRequeridas == 4 && this.getPosition() == posicionInicial)
            {
                //Por ahora lo dejamos asi hasta que tengamos una interfaz grafica
                Console.WriteLine("Ganaste!!");
            }
        }

        public void TeletrasportarmeA(TGCVector3 posicionActual)
        {
            meshPersonaje.Position = posicionActual;
            meshPersonaje.updateBoundingBox();

            meshPersonaje.Transform = TGCMatrix.Scaling(meshPersonaje.Scale) *
                                   TGCMatrix.RotationYawPitchRoll(meshPersonaje.Rotation.Y, meshPersonaje.Rotation.X, meshPersonaje.Rotation.Z) *
                                   TGCMatrix.Translation(meshPersonaje.Position);

            this.Position = meshPersonaje.Position;
        }

        public bool Entre(int numero, int min, int max)
        {
            return Between(numero, min, max);
        }

        public bool Between(int num, int lower, int upper)
        {
            bool inclusive = false; 

            return inclusive
                ? lower <= num && num <= upper
                : lower < num && num < upper;
        }

        public float getAngulo()
        {
            return 0;
        }

        public float getExponente()
        {
            return 0;
        }
        internal double DistanciaHacia(TgcMesh mesh)
        {
            var distancia = mesh.BoundingBox.PMin - this.Position;
            var distanciaTotal = Math.Sqrt(Math.Pow(distancia.X, 2) + Math.Pow(distancia.Y, 2) + Math.Pow(distancia.Z, 2));
            return distanciaTotal;


        }
    }
}



