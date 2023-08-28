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
    class Board
    {

        public Texture2D track2D;

        //absolute position, where is the board drawn.
        public Vector2 absPos = new Vector2(0, 0);

        //Position relative to player car.
        public Vector2 relPos = new Vector2(0, 0);

        //List of all the walls on the track.
        public List<Wall> walls = new List<Wall>();

        //Checkpoints keeping track of correct play and counting laps.
        public Checkpoint checkpoint1 = new Checkpoint(new Vector2(4110, 1635), new Vector2(533, 58));
        public Checkpoint checkpoint2 = new Checkpoint(new Vector2(1869, 1659), new Vector2(510, 55));

        public void LoadContent(ContentManager Content)
        {
            
            track2D = Content.Load<Texture2D>("Sprites/track");


            //The coordinates and sizes of the walls should be defined in a text file instead to easily switch between levels and store/create them.
            //Didn't have time for more than one though.
            walls.Add(new Wall(new Vector2(876, 876), new Vector2(5, 245), "right"));
            walls.Add(new Wall(new Vector2(4114, 876), new Vector2(5, 1123), "left"));
            walls.Add(new Wall(new Vector2(881, 876), new Vector2(3219, 5), "down"));
            walls.Add(new Wall(new Vector2(881, 1112), new Vector2(3219, 5), "up"));

            walls.Add(new Wall(new Vector2(2373, 1119), new Vector2(5, 870), "right"));
            walls.Add(new Wall(new Vector2(2620, 1119), new Vector2(5, 870), "left"));
            walls.Add(new Wall(new Vector2(2378, 1999), new Vector2(256, 5), "up"));

            walls.Add(new Wall(new Vector2(3128, 1637), new Vector2(241, 5), "down"));
            walls.Add(new Wall(new Vector2(3128, 1627), new Vector2(5, 876), "right"));
            walls.Add(new Wall(new Vector2(3367, 1627), new Vector2(5, 876), "left"));

            walls.Add(new Wall(new Vector2(3879, 1121), new Vector2(5, 876), "right"));
            walls.Add(new Wall(new Vector2(3879, 1988), new Vector2(245, 5), "up"));

            walls.Add(new Wall(new Vector2(276, 281), new Vector2(100, 1460), "left"));
            walls.Add(new Wall(new Vector2(272, 1628), new Vector2(1603, 10), "down"));
            walls.Add(new Wall(new Vector2(1866, 1628), new Vector2(10, 945), "left"));
            walls.Add(new Wall(new Vector2(1810, 2502), new Vector2(2915, 65), "down"));
            walls.Add(new Wall(new Vector2(4626, 240), new Vector2(69, 2290), "right"));
            walls.Add(new Wall(new Vector2(327, 289), new Vector2(4391, 83), "up"));

        }

        public void Update(Player player)
        {
            //Corrects the boards absolute position with the negative player relative position.
            absPos = relPos - player.relPos;

            //Updates all walls, moving their rectangles with the board.
            foreach (Wall wall in walls) wall.Update(absPos);


            //Updates checkpoint positions. (I could've put these in a list but since there are only two...)
            checkpoint1.Update(absPos);
            checkpoint2.Update(absPos);

        }

        public void Reset()
        {

            walls.Clear();

        }

    }
}
