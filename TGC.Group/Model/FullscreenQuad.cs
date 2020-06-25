using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Shaders;

namespace TGC.Group.Model
{
    public class FullscreenQuad
    {
        public Effect unEfecto;
        public TgcScreenQuad unQuad;
        public bool loEstoyUsando = false;

        public FullscreenQuad(Effect effect)
        {
            this.unEfecto = effect;
            this.unQuad = new TgcScreenQuad();

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
            this.unQuad.ScreenQuadVB = new VertexBuffer(typeof(CustomVertex.PositionTextured), 4, d3dDevice, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionTextured.Format, Pool.Default);
            this.unQuad.ScreenQuadVB.SetData(vertices, 0, LockFlags.None);
        }

       
    }
}
