using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace slutprojekt_racing
{
    class Checkpoint
    {

        public Vector2 relPos = new Vector2();
        public Vector2 dimensions = new Vector2();

        public Rectangle hitbox = new Rectangle();

        public Checkpoint(Vector2 relPos, Vector2 dimensions)
        {

            this.relPos = relPos;
            this.dimensions = dimensions;

        }

        public void Update(Vector2 absPos)
        {

            //Updates position relative to board.
            hitbox = new Rectangle((int)(absPos.X + relPos.X), (int)(absPos.Y + relPos.Y), (int)dimensions.X, (int)dimensions.Y);

        }

    }
}
