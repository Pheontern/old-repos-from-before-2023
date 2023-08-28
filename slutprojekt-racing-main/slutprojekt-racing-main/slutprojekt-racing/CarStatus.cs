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
    class CarStatus
    {

        public Vector2 position;
        public double rotation;

        public CarStatus(Vector2 position, double rotation)
        {

            this.position = position;
            this.rotation = rotation;

        }

    }
}
