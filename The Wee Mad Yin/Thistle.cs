﻿using Microsoft.Xna.Framework;
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
        public Texture2D thistle_sprite;
        public Vector2 thistle_position;
        public Rectangle thistle_box;


        public Thistle(ContentManager content)
       {
           //thistle_sprite = content.Load<Texture2D>("Thistle");
       }

        public virtual void Draw_Thistle(SpriteBatch batch)
        {
            if (thistle_sprite != null)
            {
                Vector2 position = thistle_position;
                batch.Draw(thistle_sprite, position, Color.White);
            }
        }
    }
}
