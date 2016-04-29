using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The_Wee_Mad_Yin
{
    class Eagle
    {
        public Texture2D eagle_sprite;
        public Vector2 eagle_position;
        public Rectangle eagle_box;
        public float eagle_velo = 0.15f;


        public Eagle(ContentManager content)
       {
            eagle_sprite = content.Load<Texture2D>("haggis");
        }

        public virtual void Draw_Eagle(SpriteBatch batch)
        {
            if (eagle_sprite != null)
            {
                Vector2 position = eagle_position;
                batch.Draw(eagle_sprite, position, Color.White);
            }
        }

    }
}
