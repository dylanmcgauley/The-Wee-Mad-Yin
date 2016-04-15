using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Wee_Mad_Yin
{
    class Camera
    {
        private readonly Viewport player_viewport;

        public Camera(Viewport viewport)
        {
            player_viewport = viewport;

            Origin = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
            Position = Vector2.Zero;
        }

        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }

        public Matrix GetPlayerView()
        {
            return 
                Matrix.CreateTranslation(new Vector3(-Position, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(Origin, 0.0f)); 
        }
    }
}
