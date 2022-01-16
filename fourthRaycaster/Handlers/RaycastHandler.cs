using fourthRaycaster.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace fourthRaycaster.Handlers
{
    public class RaycastHandler
    {
        private Game1 game1;
        private int rayDistance = 500;

        public RaycastHandler(Game1 game1)
        {
            this.game1 = game1;
        }

        /// <summary>
        /// Casts a ray line to a list of objects at a given angle
        /// </summary>
        /// <param name="rayStart">The start position of the ray</param>
        /// <param name="rayAngle">The angle of the ray</param>
        /// <param name="collidableObjects">The list of objects</param>
        /// <returns>A list of objects that where hit</returns>
        public List<RayHitObject> CastRayAtAngle (Vector2 rayStart, double rayAngle, List<CollidableObject> collidableObjects)
        {
            //Make a list to hold the objects that where hit
            List<RayHitObject> hitObjects = new List<RayHitObject>();

            //Get the end position of the ray using the start position and the angle
            Vector2 rayEnd = Vector2.Zero;
            try
            {
                Vector2 delta = new Vector2((float)(rayDistance * Math.Cos(rayAngle)), (float)(rayDistance * Math.Sin(rayAngle)));
                rayEnd = rayStart + delta;
            }
            catch (Exception)
            {
                rayEnd = Vector2.Zero;
            }

            if (rayEnd != Vector2.Zero)
            {
                //Make the ray line and cast the ray to the list of objects
                Line rayLine = new Line(rayStart, rayEnd);
                hitObjects = CastRayToObjectList(rayLine, collidableObjects);
            }

            return hitObjects;
        }

        /// <summary>
        /// Casts a ray line to a list of objects
        /// </summary>
        /// <param name="rayLine">The rays line</param>
        /// <param name="collidableObjects">The list of objects</param>
        /// <returns>A list of objects that where hit</returns>
        public List<RayHitObject> CastRayToObjectList(Line rayLine, List<CollidableObject> collidableObjects)
        {
            //Make a list to hold the objects that where hit
            List<RayHitObject> hitObjects = new List<RayHitObject>();

            //Ge through every object given
            foreach (CollidableObject collidableObject in collidableObjects)
            {
                //If the object is visable cast a ray to the object
                if (collidableObject.IsVisable)
                {
                    RayHitObject hitObject = CastRayToObject(rayLine, collidableObject);
                    //If the ray hit add it to the hit object list
                    if (hitObject != null && hitObject.Hit)
                        hitObjects.Add(hitObject);
                }
            }

            return hitObjects;
        }

        /// <summary>
        /// Casts a ray line to a objects list of lines
        /// </summary>
        /// <param name="rayLine">The rays line</param>
        /// <param name="collidableObject">The object that is being checked</param>
        /// <returns>The closest line that was hit</returns>
        public RayHitObject CastRayToObject(Line rayLine, CollidableObject collidableObject)
        {
            RayHitObject hitObject = null;

            //If the object if not visable return null
            if (!collidableObject.IsVisable)
                return null;

            //Go through every line in the object
            List<Line> objectsLines = collidableObject.GetLines();
            foreach (Line objectLine in objectsLines)
            {
                //Cast the ray to the line
                ReturnHitPos rHitPos = CastRay(objectLine, rayLine);
                if (rHitPos != null)
                {
                    float distance = GetDistance(rayLine.PositionOne, rHitPos.HitPos);
                    if (rHitPos.Hit && (hitObject == null || !hitObject.Hit || distance < hitObject.Distance))
                    {
                        hitObject = new RayHitObject(true, collidableObject, rHitPos.HitPos, objectLine.Side, distance);
                    }
                }
            }

            return hitObject;
        }

        /// <summary>
        /// Check if two lines ever intersect and where they would intersect
        /// </summary>
        /// <param name="line">The line to check if the ray hits</param>
        /// <param name="rayLine">The ray</param>
        /// <returns>If they hit and the position if they did</returns>
        public ReturnHitPos CastRay(Line line, Line rayLine)
        {
            //The line
            float x1 = line.PositionOne.X;
            float y1 = line.PositionOne.Y;
            float x2 = line.PositionTwo.X;
            float y2 = line.PositionTwo.Y;

            //The ray
            float x3 = rayLine.PositionOne.X;
            float y3 = rayLine.PositionOne.Y;
            float x4 = rayLine.PositionTwo.X;
            float y4 = rayLine.PositionTwo.Y;

            float den = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
            if (den == 0)
                return null; ; // hit is false

            float t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / den;
            float u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / den;

            if (t > 0 && t < 1 && u > 0)
            {
                Vector2 hitPos = Vector2.Zero;
                hitPos.X = x1 + t * (x2 - x1);
                hitPos.Y = y1 + t * (y2 - y1);

                return new ReturnHitPos(true, hitPos); // hit is true
            }
            else
            {
                return null; // hit is false
            }
        }

        /// <summary>
        /// Gets the closest object from a list of hit objects
        /// </summary>
        /// <param name="hitObjects">The list of objects that where hit</param>
        /// <returns>The object that was the closest</returns>
        public RayHitObject GetClosestSolidObject(List<RayHitObject> hitObjects)
        {
            RayHitObject closestObject = null;

            foreach (RayHitObject hitObject in hitObjects)
            {
                if (!hitObject.ObjectHit.HasTransparentPixels)
                {
                    if (closestObject == null|| hitObject.Distance < closestObject.Distance || !closestObject.Hit)
                    {
                        closestObject = hitObject;
                    }
                }
            }

            return closestObject;
        }

        /// <summary>
        /// Gets and sorts a list of transparent objects
        /// </summary>
        /// <param name="hitObjects">The list of objects that where hit<</param>
        /// <param name="cutOffDistance">The cut off distance</param>
        /// <returns>A sorted list of transparent objects with a distance smaller then the cut off distance</returns>
        public List<RayHitObject> SortTransparentObjects(List<RayHitObject> hitObjects, float cutOffDistance)
        {
            List<RayHitObject> transparentObjects = new List<RayHitObject>();

            foreach (RayHitObject hitObject in hitObjects)
            {
                if (hitObject.ObjectHit.HasTransparentPixels && hitObject.Distance < cutOffDistance)
                {
                    transparentObjects.Add(hitObject);
                }
            }
            transparentObjects = transparentObjects.OrderByDescending(o => o.Distance).ToList();

            return transparentObjects;
        }

        /// <summary>
        /// Gets the distance between two points
        /// </summary>
        /// <param name="posOne">The first position</param>
        /// <param name="posTwo">The secound position</param>
        /// <returns>The distance between the two points</returns>
        public float GetDistance(Vector2 posOne, Vector2 posTwo)
        {
            float xDis = Math.Abs(posOne.X - posTwo.X);
            float yDis = Math.Abs(posOne.Y - posTwo.Y);

            float dis = (float)Math.Sqrt(xDis * xDis + yDis * yDis);
            return dis;
        }
    }
}
