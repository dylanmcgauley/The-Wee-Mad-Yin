using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The_Wee_Mad_Yin
{
    class Thistle
    {
        public sprite2 thistle_sprite;
        public Rectangle thistle_box;


        public Thistle(ContentManager content)
       {
           thistle_sprite = new sprite2(content, "thistle_move", 100, 100, 3, 7, 0.2f);
        }

        public virtual void Draw_Thistle(SpriteBatch batch)
        {
            if (thistle_sprite != null)
            {
                thistle_sprite.DrawNormal(batch);
            }
        }
    }
}
