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
        public Rectangle haggis_box;
        public float haggis_velo = 0.15f;


        public Haggis(ContentManager content)
       {
           haggis_sprite = content.Load<Texture2D>("haggis");
       }

        public virtual void Draw_Haggis(SpriteBatch batch)
        {
            if (haggis_sprite != null)
            {
                Vector2 position = haggis_position;
                batch.Draw(haggis_sprite, position, Color.White);
            }
        }
    }
}
