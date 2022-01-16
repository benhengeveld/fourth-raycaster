using fourthRaycaster.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace fourthRaycaster.Handlers
{
    public class MapLoader
    {
        private Game1 game1;
        private int[,] map;
        private Texture2D wallTexture;
        private int wallSize;

        public MapLoader(Game1 game1, int[,] map, Texture2D wallTexture, int wallSize)
        {
            this.game1 = game1;
            this.map = map;
            this.wallTexture = wallTexture;
            this.wallSize = wallSize;
        }

        /// <summary>
        /// Loads the map
        /// </summary>
        public void LoadMap()
        {
            bool newLine = true;
            Vector2 posOne = Vector2.Zero;
            Vector2 posTwo = Vector2.Zero;

            //X side walls
            Color xSideColor = new Color(255, 255, 255, 255);
            //Loop through the map
            for (int y = 0; y <= map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    //Check if the spot needs a horizontal wall
                    if (CheckForHorizontalWall(x, y))
                    {
                        //If it is a new line
                        if (newLine)
                        {
                            //Set the first position to the current position
                            posOne = GetMapPos(x, y);
                            //Set the second position to one to the right
                            posTwo = GetMapPos(x + 1, y);
                            newLine = false;
                        }
                        else
                        {
                            //If its not a new line set the second position to one to the right
                            posTwo = GetMapPos(x + 1, y);
                        }
                    }
                    //If there is no wall in the current position and there is a line
                    else if (newLine == false)
                    {
                        //Make a new wall and add it to the collidable objects list
                        Wall newWall = new Wall(game1, true, wallTexture, xSideColor, posOne, posTwo);
                        game1.collidableObjects.Add(newWall);

                        //Reset the line
                        posOne = Vector2.Zero;
                        posTwo = Vector2.Zero;
                        newLine = true;
                    }
                }
            }

            //Reset the line
            newLine = true;
            posOne = Vector2.Zero;
            posTwo = Vector2.Zero;

            //Y side walls
            Color ySideColor = new Color(128, 128, 128, 255);
            //Loop through the map
            for (int x = 0; x <= map.GetLength(1); x++)
            {
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    //Check if the spot needs a vertical wall
                    if (CheckForVerticalWall(x, y))
                    {
                        //If it is a new line
                        if (newLine)
                        {
                            //Set the first position to the current position
                            posOne = GetMapPos(x, y);
                            //Set the second position to one to the right
                            posTwo = GetMapPos(x, y + 1);
                            newLine = false;
                        }
                        else
                        {
                            //If its not a new line set the second position to one to the right
                            posTwo = GetMapPos(x, y + 1);
                        }
                    }
                    //If there is no wall in the current position and there is a line
                    else if (newLine == false)
                    {
                        //Make a new wall and add it to the collidable objects list
                        Wall newWall = new Wall(game1, true, wallTexture, ySideColor, posOne, posTwo);
                        game1.collidableObjects.Add(newWall);

                        //Reset the line
                        posOne = Vector2.Zero;
                        posTwo = Vector2.Zero;
                        newLine = true;
                    }
                }
            }

            Debug.WriteLine(game1.collidableObjects.Count);
        }

        /// <summary>
        /// Check if the given spot needs a horizontal wall
        /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        /// <returns>Returns if the spot needs a horizontal wall or not</returns>
        private bool CheckForHorizontalWall(int x, int y)
        {
            //Gets the top wall of the given position if there is one
            int topWall = 0;
            if (y - 1 >= 0 && y - 1 < map.GetLength(0))
                topWall = map[y - 1, x];

            //Gets the bottom wall of the given position if there is one
            int bottomWall = 0;
            if (y >= 0 && y < map.GetLength(0))
                bottomWall = map[y, x];

            //Check if only one has a wall, and if so then you need to put a wall in that spot
            if (topWall == 1 && bottomWall == 0)
                return true;

            if (topWall == 0 && bottomWall == 1)
                return true;

            return false;
        }

        /// <summary>
        /// Check if the given spot needs a vertical wall
        /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        /// <returns>Returns if the spot needs a vertical wall or not</returns>
        private bool CheckForVerticalWall(int x, int y)
        {
            //Gets the right wall of the given position if there is one
            int rightWall = 0;
            if (x - 1 >= 0 && x - 1 < map.GetLength(1))
                rightWall = map[y, x - 1];

            //Gets the left wall of the given position if there is one
            int leftWall = 0;
            if (x >= 0 && x < map.GetLength(1))
                leftWall = map[y, x];

            //Check if only one has a wall, and if so then you need to put a wall in that spot
            if (rightWall == 1 && leftWall == 0)
                return true;

            if (rightWall == 0 && leftWall == 1)
                return true;

            return false;
        }

        /// <summary>
        /// Turns a array position into a position in the game
        /// </summary>
        /// <param name="x">The array x position</param>
        /// <param name="y">The array y position</param>
        /// <returns>Returns the position in the game</returns>
        private Vector2 GetMapPos(int x, int y)
        {
            return new Vector2(x * wallSize, y * wallSize);
        }
    }
}
