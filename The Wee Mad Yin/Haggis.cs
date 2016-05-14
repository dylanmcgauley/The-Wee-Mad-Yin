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
        //public Texture2D haggis_sprite;
        public sprite2 haggis_sprite;
        public Vector2 haggis_position;
        public Rectangle haggis_box;
        public float haggis_velo = 0.15f;


        public Haggis(ContentManager content)
       {
            // haggis_sprite = content.Load<Texture2D>("haggis");
            haggis_sprite = new sprite2(content, "haggis_move", 100, 100, 4, 4, 0.7f);
        }

        public virtual void Draw_Haggis(SpriteBatch batch)
        {
            if (haggis_sprite != null)
            {
                //Vector2 position = haggis_position;
                haggis_sprite.DrawNormal(batch);
                // batch.Draw(haggis_sprite, position, Color.White);

            }
        }
    }
}
