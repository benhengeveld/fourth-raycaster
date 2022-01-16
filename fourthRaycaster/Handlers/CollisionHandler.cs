using fourthRaycaster.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace fourthRaycaster.Handlers
{
    public class CollisionHandler
    {
        private Game1 game1;

        public CollisionHandler(Game1 game1)
        {
            this.game1 = game1;
        }

        /// <summary>
        /// Check if a list of lines collides with one of the collidable objects in the games list
        /// </summary>
        /// <param name="lines">List of lines to check if they are colliding</param>
        /// <returns>If the list of lines collides with something</returns>
        public bool IsCollidingWithObject(List<Line> lines)
        {
            //If there is no lines then it cant collide
            if (lines == null)
                return false;

            //Go throgh every collidable object in the list
            foreach (CollidableObject collidableObject in game1.collidableObjects)
            {
                //Get the list of lines from the collidable object
                List<Line> objectsLines = collidableObject.GetLines();
                //If it has lines and can collide
                if (objectsLines != null && collidableObject.CanCollide)
                {
                    //Go through every line in the collidable object
                    foreach (Line objectLine in objectsLines)
                    {
                        //Go through every line from the lines given
                        foreach (Line line in lines)
                        {
                            //Check if they intersect, and if they do return true
                            if (LinesIntersect(objectLine, line))
                                return true;
                        }
                    }
                }
            }

            //Return false, no lines intersected
            return false;
        }

        /// <summary>
        /// Checks if two lines intersect
        /// </summary>
        /// <param name="lineOne">The first line to check</param>
        /// <param name="lineTwo">The second line to check</param>
        /// <returns>If the lines intersect</returns>
        public bool LinesIntersect(Line lineOne, Line lineTwo)
        {
            Vector2 a = lineOne.PositionOne;
            Vector2 b = lineOne.PositionTwo;
            Vector2 c = lineTwo.PositionOne;
            Vector2 d = lineTwo.PositionTwo;

            bool result = CCW(a, c, d) != CCW(b, c, d) && CCW(a, b, c) != CCW(a, b, d);
            return result;
        }

        public bool CCW(Vector2 a, Vector2 b, Vector2 c)
        {
            bool result = (c.Y - a.Y) * (b.X - a.X) > (b.Y - a.Y) * (c.X - a.X);
            return result;
        }
    }
}
