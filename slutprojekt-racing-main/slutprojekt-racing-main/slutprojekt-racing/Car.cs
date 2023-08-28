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
    class Car
    {
        public Texture2D car2D;

        //Position relative to background.
        public Vector2 relPos = new Vector2(0, 0);
        
        public Vector2 speed = new Vector2(0, 0);

        public Rectangle hitbox = new Rectangle();
        public int hitboxSize = 50;

        //Rotation, angle in radians, used to calculate wheelDir.
        public double rotation = 0;

        //Defines the direction the car is pointing,
        //and the direction the car is moving as normalized vectors calculated in Move().
        public Vector2 wheelDir = new Vector2(0, 0);
        public Vector2 speedDir = new Vector2(0, 0);

        //Car properties
        public float acc = 0.3f; /*acceleration*/
        public float friction = 0.2f;
        public float maxSpeed = 32;
        public float maxRevSpeed = 10; /*Max reversing speed*/
        public float traction = 5; /*How well the car keeps to it's path without drifting.*/
        
        //Used to control drifting induced by crashing.
        public bool tractionControl = true;

        //Defines if driving forward or reversing 1 : forwards, -3: backwards.
        int direction = 1;

        //It took a looong time to tune all the physics for optimal fun, especially the traction function.

        //Constructor to instantiate a starting position and rotation for the car.
        public Car(Vector2 startPos, double startRotation)
        {
            this.relPos = startPos;
            this.rotation = startRotation;
        }

        public void Update(Vector2 absPos, Board board)
        {
            //Moves and updates hitbox.
            hitbox = new Rectangle((int)absPos.X - hitboxSize / 2, (int)absPos.Y - hitboxSize / 2, hitboxSize, hitboxSize);

            Move(board);

            //These check through all wall objects to see if anyone has been crashed into lately. If so, the car should drift, setting tractionControl to false.
            tractionControl = true;
            foreach (Wall wall in board.walls) if (wall.tractionControl == false) tractionControl = false;

        }

        //All movement code for the car.
        public void Move(Board board)
        {
            //Simplifies the rotation angle so that it doesn't just increase or decrease indefinitely. 
            if (rotation > 2 * Math.PI) rotation -= 2 * Math.PI;
            else if (rotation < -2 * Math.PI) rotation += 2 * Math.PI;

            //Direction the car is pointing and moving as two normalized vectors.
            wheelDir = new Vector2((float)Math.Cos(rotation), (float)-Math.Sin(rotation));
            speedDir = speed.Length() != 0 ? Vector2.Normalize(speed) : new Vector2(0, 0);

            //Friction calculations, works against acceleration.
            if (speed.Length() <= friction) speed = Vector2.Zero;
            else speed -= friction * speedDir;

            Crash(board);

            //Movement
            relPos += speed;
        }

        //Checks if colliding with wall and executing crash code.
        public void Crash(Board board)
        {
            foreach (Wall wall in board.walls)
            {
                //When in contact with wall.
                if (hitbox.Intersects(wall.hitbox))
                {
                    wall.TCCount = 0; /*counts time since latest crash into this wall*/

                    if (wall.crashChecker)
                    {
                        speed *= 0.5f;
                        
                        //Wall type represents what direction the wall is facing.
                        switch (wall.wallType)
                        {

                            case "up":
                                if (speedDir.Y < 0) relPos.Y += Math.Abs(speed.Y) + 1f;

                                speed.Y = Math.Abs(speed.Y);
                                break;

                            case "down":
                                if (speedDir.Y > 0) relPos.Y -= Math.Abs(speed.Y) + 1f;

                                speed.Y = -Math.Abs(speed.Y);
                                break;

                            case "left":
                                if (speedDir.X < 0) relPos.X += Math.Abs(speed.X) + 1f;

                                speed.X = Math.Abs(speed.X);
                                break;

                            case "right":
                                if (speedDir.X > 0) relPos.X -= Math.Abs(speed.X) + 1f;

                                speed.X = -Math.Abs(speed.X);
                                break;

                        }
                        wall.tractionControl = false; /*if crashing, car should start to drift*/
                        wall.crashChecker = false; /*crash code should only execute once, until the car isn't in contact with wall anymore*/

                    }
                }
                
                //If not in contact with wall, variables should reset.
                if (!hitbox.Intersects(wall.hitbox))
                {
                    wall.crashChecker = true;
                    wall.TCCount++;
                    if (wall.TCCount > 30) /*If car has been away from wall for 30 cycles it should stop drifting*/
                    {
                        wall.tractionControl = true;
                        wall.TCCount = 0;
                    }
                }

            }

        }

        public void Acc()
        {
            direction = 1;
            if (speed.Length() < maxSpeed) speed += acc * wheelDir;
        }

        //Extra code here is to allow braking at higher speeds than max reverse speed.
        public void Reverse()
        {
            direction = -1;
            bool braking = (wheelDir + speedDir).Length() > 1;
            if (braking) speed -= 0.9f * acc * wheelDir;
            else if (speed.Length() < maxRevSpeed) speed -= 0.9f * acc * wheelDir;
        }

        //Controls steering in both directions.
        public void Steer(bool direction, bool traction)
        {
            //Controls slowing down while turning with speeds over 15.
            if (speed.Length() > 15 && traction) speed -= 0.7f * friction * speedDir;

            //Reverses rotation when reversing, not perfect but works alright.
            if ((wheelDir + speedDir).Length() < 1 && timeSinceDrifting > 0) direction = !direction;

            //Rotates car in correct direction.
            if (direction) rotation -= TurningFunction(traction);
            else rotation += TurningFunction(traction);
        }
        
        //Function that controls how turning rate should change with speed and drifting.
        public double TurningFunction(bool traction)
        {
            double looser = -0.1 / (speed.Length() + 2) + 0.05;
            double tighter = -0.4 / (speed.Length() + 4) + 0.1;

            double function = traction ? looser : tighter;

            return function;
        }

        public int timeSinceDrifting = 0;
        //Keeps the car from drifting. Corrects speed direction to the cars rotation.
        public void Traction()
        {

            if (tractionControl)
            {

                timeSinceDrifting++;

                //If you just drifted, you could have rotated your car to the opposite direction.
                //In that case, traction should be in your last acceleration direction.
                //Otherwise, traction should be in the direction you are moving.
                //A bit hard to understand but means that drifting and traction works correctly both when reversing and moving forward.
                //Can be abused a bit but it doesn't matter.
                int tempDirection = (wheelDir + speedDir).Length() > 1 ? 1 : -1;
                int calcDirection = timeSinceDrifting < 10 ? direction : tempDirection;

                //speed isn't perfectly corrected to rotation but is affected by the speed a little so that the higher the speed, the worse the traction.
                speed = speed.Length() * Vector2.Normalize(calcDirection * wheelDir + speed / traction);

            }
        }
    }
}
