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
    class Ghost
    {
        //Ghost car values, same meanings as normal car (explained in car class).
        public Vector2 absPos = new Vector2(0, 0);
        public double rotation;
        public Vector2 origin;

        //Recorded positions and rotations that are played back.
        public List<CarStatus> playback = new List<CarStatus>();

        //Fills up with data during recording, is moved to playback when restarted. 
        public List<CarStatus> recording = new List<CarStatus>();

        //Controls if currently recording (saving positional and rotational values of player)
        public bool recStatus = false;

        //Controls if recording is being played back repeatedly.
        public bool pbStatus = false;
        
        //Counts on what stage, what data number the playback is on.
        public int playbackCounter = 0;

        public void LoadContent(Player player)
        {
            //Sets ghosts origin to the same as player origin so that it rotates and positions correctly.
            origin = player.origin;
        }

        public void Update(Player player, Board board, Keys[] keys, Keys[] oldKeys)
        {

            if (recStatus)
            { 
                //Stores current position and rotation in recording list.
                recording.Add(new CarStatus(player.relPos + player.absPos, player.rotation));
            }

            //Controls playback, changes ghosts position and rotation according to recorded data in lists. 
            if (pbStatus)
            {

                if (playbackCounter < playback.Count)
                {
                    absPos = board.absPos + playback[playbackCounter].position;
                    rotation = playback[playbackCounter].rotation;
                }
                else playbackCounter = 0;

                playbackCounter++;
            }
            else absPos = new Vector2(-50, -50);


        }

        public void Reset()
        {

            recording.Clear();
            playback.Clear();
            recStatus = false;
            pbStatus = false;
            playbackCounter = 0;

        }
    }
}
