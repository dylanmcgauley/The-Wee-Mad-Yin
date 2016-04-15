using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The_Wee_Mad_Yin
{
     public class sprite
    {
        public Texture2D graphic;
        public Vector2 position;
        Rectangle rectangle;
        float size;

        public sprite(ContentManager content, string filename, int x, int y, float sizefactor)
        {
            graphic = content.Load<Texture2D>(filename);
            position = new Vector2(x, y);
            size = sizefactor;
        }

        public void DrawNormal(SpriteBatch batch)
        {
            batch.Draw(graphic, position, Color.White);
        }

        public void DrawScaled(SpriteBatch batch)
        {
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;
            rectangle.Width = (int)(graphic.Width * size);
            rectangle.Height = (int)(graphic.Height * size);
            batch.Draw(graphic, rectangle, Color.White);
        }


        public void DrawRectangle(SpriteBatch batch, int width, int height)
        {
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;
            rectangle.Width = width;
            rectangle.Height = height;
            batch.Draw(graphic, rectangle, Color.White);
        }

    }
}
