﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The_Wee_Mad_Yin
{
    class Block
    {
        public Texture2D block_sprite;
        public Vector2 block_position;
        public Rectangle block_box;


        public Block(ContentManager content)
       {
           block_sprite = content.Load<Texture2D>("block");
       }

        public virtual void Draw_Block(SpriteBatch batch)
        {
            if (block_sprite != null)
            {
                Vector2 position = block_position;
                position.X -= block_sprite.Width / 2;
                position.Y -= block_sprite.Height / 2;
                batch.Draw(block_sprite, position, Color.White);
            }
        }
    }
}
