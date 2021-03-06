﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CSharp_Steering_Behavior.Primitives
{
    public class CircleObstacle : IDrawableObstacle
    {
        public Vector3 Center { get; set; }
        
        private VertexPositionColor[] _vertices;
        private float _radius;

        public CircleObstacle(Vector3 center, float radius)
        {
            this._radius = radius;
            this.Center = center;

            this.Init();
        }

        private void Init()
        {
            this._vertices = new VertexPositionColor[100];

            for (int i = 0; i < 99; i++)
            {
                float angle = (float)(i / 100.0 * Math.PI * 2);
                this._vertices[i] = new VertexPositionColor();
                this._vertices[i].Position = new Vector3(this.Center.X + (float)Math.Cos(angle) * this._radius, this.Center.Y + (float)Math.Sin(angle) * this._radius, 0);
                this._vertices[i].Color = Color.Black;
            }
            this._vertices[99] = this._vertices[0];
        }

        public void Draw(PrimitiveBatch primitiveBatch)
        {
            primitiveBatch.AddVertices(this._vertices);
        }

        public float GetRadius()
        {
            return this._radius;
        }
    }
}
