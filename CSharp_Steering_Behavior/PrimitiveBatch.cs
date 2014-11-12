using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace CSharp_Steering_Behavior
{
    public class PrimitiveBatch
    {
        private GraphicsDevice _graphics;
        private BasicEffect _basicEffect;
        private VertexPositionColor[] _vertices;
        private int _positionInBuffer = 0;
        private PrimitiveType _primitiveType;
        private bool _hasBegun = false;

        public PrimitiveBatch(GraphicsDevice graphicsDevice)
        {
            _graphics = graphicsDevice;
            
            this.Init();
        }

        private void Init()
        {
            var center = new Vector2(_graphics.Viewport.Width * 0.5f, _graphics.Viewport.Height * 0.5f);

            _basicEffect = new BasicEffect(_graphics);
            _basicEffect.World = Matrix.Identity;
            _basicEffect.View = Matrix.CreateLookAt(new Vector3(center, 0), new Vector3(center, 1), Vector3.Down);
            _basicEffect.Projection = Matrix.CreateOrthographic(center.X * 2, center.Y * 2, -0.5f, 1);

            _basicEffect.VertexColorEnabled = true;
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;

            _graphics.RasterizerState = rasterizerState;

            _vertices = new VertexPositionColor[500];
        }

        public void Begin(PrimitiveType primitiveType)
        {
            if (_hasBegun)
            {
                throw new Exception("You should not call the begin method twice");
            }

            if (primitiveType != PrimitiveType.LineList && primitiveType != PrimitiveType.TriangleList)
            {
                throw new Exception("Not supported primitiveType");
            }

            _primitiveType = primitiveType;

            //tell our basic effect to begin.
            _basicEffect.CurrentTechnique.Passes[0].Apply();

            _hasBegun = true;
        }

        public void End()
        {
            if (!_hasBegun)
            {
                throw new Exception("Must call begin before you call end");
            }

            var primitivesCount = _positionInBuffer / this.NumVertsPerPrimitive();

            if (primitivesCount > 0)
            {
                _graphics.DrawUserPrimitives<VertexPositionColor>(_primitiveType, _vertices, 0, primitivesCount);    
            }

            _positionInBuffer = 0;
            _hasBegun = false;
        }

        public void AddVertices(VertexPositionColor[] vertices)
        {
            //if (vertices.Count() != this.NumVertsPerPrimitive())
            //{
            //    throw new Exception("Wrong number of vertices for the primitiveType");
            //}

            AddVerticesToBuffer(vertices);
        }

        private void AddVerticesToBuffer(VertexPositionColor[] vertices)
        {
            foreach (var vertexPositionColor in vertices)
            {
                _vertices[_positionInBuffer] = vertexPositionColor;
                _positionInBuffer++;
            }
        }

        private int NumVertsPerPrimitive()
        {
            switch (_primitiveType)
            {
                case PrimitiveType.TriangleList:
                    return 3;

                case PrimitiveType.LineList:
                    return 2;
            }

            return 0;
        }
    }
}
