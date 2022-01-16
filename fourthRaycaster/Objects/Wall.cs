using fourthRaycaster.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace fourthRaycaster.Objects
{
    public class Wall : CollidableObject
    {
        private Game1 game1;
        private Texture2D texture;
        private Vector2 posOne;
        private Vector2 posTwo;
        private Color shadingColor;

        public Wall(Game game, bool canCollide, Texture2D texture, Color shadingColor, Vector2 posOne, Vector2 posTwo) : base(game, canCollide)
        {
            this.game1 = (Game1)game;
            this.texture = texture;
            this.shadingColor = shadingColor;
            this.posOne = posOne;
            this.posTwo = posTwo;
        }

        /// <summary>
        /// Renders the rectangle that is used to display on the screen
        /// </summary>
        /// <param name="rayObject">The object hit with other data</param>
        /// <param name="player">The player</param>
        /// <param name="rayAngle">The angle that ray came out as</param>
        /// <param name="xScreenPos">The x position that the ray is for</param>
        /// <returns>Returns a ray rectangle used for rendering</returns>
        public override RayRectangle GetRenderRectangle(RayHitObject rayObject, Player player, double rayAngle, int xScreenPos)
        {
            RayRectangle returnRectangle = null;
            float cubeSize = game1.cubeSize;

            //Fix fisheye
            float fixedDistance = rayObject.Distance;
            double ca = player.Angle - rayAngle;
            if (ca < 0)
                ca += 2 * Math.PI;
            if (ca > 2 * Math.PI)
                ca -= 2 * Math.PI;
            fixedDistance = (float)(fixedDistance * Math.Cos(ca));

            //Set the distance in the z buffer
            game1.zBuffer[xScreenPos] = fixedDistance;

            //Get height of line from the distance
            double lineHeight = (cubeSize * game1.bounds.Y) / fixedDistance;
            lineHeight = Math.Clamp(lineHeight, 0, double.MaxValue);

            //Get the strip of the texture
            int texturePos = (int)game1.raycastHandler.GetDistance(posOne, rayObject.HitPosition);
            while (texturePos > texture.Width)
            {
                texturePos -= texture.Width;
            }
            Rectangle textureRectangle = new Rectangle(Math.Clamp(texturePos, 0, texture.Width), 0, 1, texture.Height);

            //Set up the ray rectangle
            int offset = (int)(game1.bounds.Y / 2 - lineHeight / 2);
            Rectangle rectangle = new Rectangle(xScreenPos, offset, 1, (int)lineHeight);
            bool rectangleVisable = base.IsVisable;
            returnRectangle = new RayRectangle(texture, rectangle, shadingColor, textureRectangle, rectangleVisable);

            return returnRectangle;
        }

        /// <summary>
        /// Gets a list of lines of the wall
        /// </summary>
        /// <returns>Returns the list</returns>
        public override List<Line> GetLines()
        {
            List<Line> lines = new List<Line>();

            lines.Add(new Line(posOne, posTwo));

            return lines;
        }
    }
}
