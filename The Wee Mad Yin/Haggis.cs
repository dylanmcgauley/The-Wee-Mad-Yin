using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The_Wee_Mad_Yin
{
    class Haggis
    {
        public Texture2D haggis_sprite;
        public Vector2 haggis_position;


        public Haggis(ContentManager content)
       {
           //haggis_sprite = content.Load<Texture2D>("Haggis");
       }

        public virtual void Draw_Haggis(SpriteBatch batch)
        {
            if (haggis_sprite != null)
            {
                Vector2 position = haggis_position;
                position.X -= haggis_sprite.Width / 2;
                position.Y -= haggis_sprite.Height / 2;
                batch.Draw(haggis_sprite, position, Color.White);
            }
        }
    }
}
