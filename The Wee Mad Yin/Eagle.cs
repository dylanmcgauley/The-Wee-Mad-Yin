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


        public Eagle(ContentManager content)
       {
           //eagle_sprite = content.Load<Texture2D>("Eagle");
       }

        public virtual void Draw_Eagle(SpriteBatch batch)
        {
            if (eagle_sprite != null)
            {
                Vector2 position = eagle_position;
                position.X -= eagle_sprite.Width / 2;
                position.Y -= eagle_sprite.Height / 2;
                batch.Draw(eagle_sprite, position, Color.White);
            }
        }

    }
}
