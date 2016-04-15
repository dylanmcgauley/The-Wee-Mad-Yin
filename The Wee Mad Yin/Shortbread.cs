using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The_Wee_Mad_Yin
{
    class Shortbread
    {
        public Texture2D shortbread_sprite;
        public Vector2 shortbread_position;


        public Shortbread(ContentManager content)
       {
           //shortbread_sprite = content.Load<Texture2D>("Shortbread");
       }

        public virtual void Draw_Shortbread(SpriteBatch batch)
        {
            if (shortbread_sprite != null)
            {
                Vector2 position = shortbread_position;
                position.X -= shortbread_sprite.Width / 2;
                position.Y -= shortbread_sprite.Height / 2;
                batch.Draw(shortbread_sprite, position, Color.White);
            }
        }
    }
}
