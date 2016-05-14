using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The_Wee_Mad_Yin
{
    public class sprite2
    {
        public animation sprite_animation;
        public Vector2 position;
        public Vector2 velocity;
        Rectangle rectangle;
        float fpsfactor = 10f;
        float size;

        public sprite2(ContentManager content, string filename, int x, int y, int rows, int colms, float sizefactor)
        {
            position = new Vector2(x, y);
            size = sizefactor;
            sprite_animation = new animation(content, filename, x, y, size, Color.White, true, 24, rows, colms, true, false, false);
            sprite_animation.start(new Vector3(x,y,0),0,0);
        }



        public void update(float gtime)
        {
            sprite_animation.framespersecond = (int)Math.Round(Math.Abs(velocity.X * fpsfactor), 0);

            sprite_animation.position.X = position.X;
            sprite_animation.position.Y = position.Y;
            sprite_animation.update(gtime);
        }
        

        public void DrawNormal(SpriteBatch batch)
        {
            sprite_animation.drawme(batch);
        }
    }


     public class sprite
    {
        public Texture2D graphic;
        public Vector2 position;
        public Vector2 velocity;
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
