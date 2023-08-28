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
    class Wall
    {
        //Position relative to background.
        public Vector2 relPos = new Vector2();

        //The size of the wall.
        public Vector2 dimensions = new Vector2();

        public Rectangle hitbox = new Rectangle();

        //up down left or right
        public string wallType;

        //Explained more where they are used, in car.Crash();
        public bool crashChecker = true;
        public int TCCount = 0;
        public bool tractionControl = true;

        public Wall(Vector2 relPos, Vector2 dimensions, string wallType)
        {

            this.relPos = relPos;
            this.dimensions = dimensions;
            this.wallType = wallType;

        }

        public void Update(Vector2 absPos)
        {

            //Updates hitbox position relative to background.
            hitbox = new Rectangle((int)(absPos.X + relPos.X), (int)(absPos.Y + relPos.Y), (int)dimensions.X, (int)dimensions.Y);

        }

    }
}
