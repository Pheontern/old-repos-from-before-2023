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
    class Player
    {
        //Absolute position in center of screen.
        public Vector2 absPos = Game1.window / 2;

        public Vector2 startPos = new Vector2(3300, 1200);
        public double startRotation = Math.PI / 2;

        public Vector2 origin = new Vector2(0, 0);


        //Just copies value from car <
        //Position relative to background. Controlled in car object.
        public Vector2 relPos { get => car.relPos; set => car.relPos = value; }
        
        //Rotation controlled by car object.
        public double rotation { get => car.rotation; set => car.rotation = value; }

        //Speed controlled by car object.
        public Vector2 speed { get => car.speed; set => car.speed = value; }
        //Just copies value from car >


        public Car car;
        
        //Constructor to let starting position and rotation be defined by car constructor.
        public Player()
        {
            car = new Car(startPos, startRotation);
        }

        public void LoadContent(ContentManager Content)
        {
            car.car2D = Content.Load<Texture2D>("Sprites/car");

            //Sets correct rotational origin, between the two back wheels.
            origin = new Vector2(car.car2D.Width / 4, car.car2D.Height / 2);
        }

        public void Update(Keys[] keys, Board board)
        {

            //If statement removes controls while counting down.
            if (GameControl.inCountDown == 0) Move(keys, board);

        }

        //Controls all movement and keyboard controls.
        void Move(Keys[] keys, Board board)
        {
            //Updates car with correct parameters.
            car.Update(absPos, board);

            //Controls drifting and traction.
            bool traction = !Array.Exists(keys, key => key == Keys.LeftShift);
            if (traction) car.Traction();
            else car.timeSinceDrifting = 0;

            //Gas, acceleration with W.
            if (Array.Exists(keys, key => key == Keys.W)) car.Acc();



            //Reverse with S
            if (Array.Exists(keys, key => key == Keys.S)) car.Reverse();

            //Steering with A and D.
            if (Array.Exists(keys, key => key == Keys.A)) car.Steer(false, traction);
            if (Array.Exists(keys, key => key == Keys.D)) car.Steer(true, traction);

            //Resets to start with Enter. (only for testing purposes)
            if (Array.Exists(keys, key => key == Keys.Enter))
            {
                relPos = startPos;
                speed = Vector2.Zero;
                rotation = startRotation;
            }


            if (Array.Exists(keys, key => key == Keys.F12)) car.speed = car.wheelDir * 32;

        }

        public void Reset()
        {

            speed = Vector2.Zero;
            relPos = startPos;
            rotation = startRotation;

        }

    }
}
