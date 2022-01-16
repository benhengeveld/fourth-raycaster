using fourthRaycaster.Handlers;
using fourthRaycaster.Models;
using fourthRaycaster.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace fourthRaycaster.Drawers
{
    public class RaycastDrawer : DrawableGameComponent
    {
        private Game1 game1;
        private SpriteBatch spriteBatch;
        private RaycastHandler raycastHandler;
        private Player player;
        private Texture2D pixelTexture;

        private double maxAngle;
        private List<RayRectangle>[] rayRectangles;

        private bool updateRays = true;

        public RaycastDrawer(Game game, Player player, Texture2D pixelTexture) : base(game)
        {
            this.game1 = (Game1)game;
            this.spriteBatch = game1._spriteBatch;
            this.raycastHandler = game1.raycastHandler;
            this.player = player;
            this.pixelTexture = pixelTexture;

            rayRectangles = new List<RayRectangle>[(int)game1.bounds.X];
            maxAngle = DegreesToRadians(60);
        }

        /// <summary>
        /// Updates the rays
        /// </summary>
        public void UpdateRays()
        {
            //Make a array the size of the screen width
            rayRectangles = new List<RayRectangle>[(int)game1.bounds.X];

            //Get the position the ray starts at the center of the player
            Vector2 posOne = player.Position + player.Size / 2;
            //Get half the size of the max angle
            double halfAngle = maxAngle / 2;
            //Get the amount the angle increments for each x pos on the screen
            double angleIncrement = maxAngle / (rayRectangles.Length - 1);

            //Loop through the screen width
            for (int i = 0; i < rayRectangles.Length; i++)
            {
                //Make a new list for this screens x position
                rayRectangles[i] = new List<RayRectangle>();

                //Get the angle of this ray
                double rayAngle = angleIncrement * i + player.Angle - halfAngle;

                //Cast a ray at the angle
                List<RayHitObject> hitObjects = raycastHandler.CastRayAtAngle(posOne, rayAngle, game1.collidableObjects); //4ms
                
                //If the ray hit some objects
                if (hitObjects.Count > 0)
                {
                    //Get the closest solid object
                    RayHitObject closestSolidObject = raycastHandler.GetClosestSolidObject(hitObjects); //2ms but sometimes 1ms?
                    //Get the rendered rectangle of the solid object
                    RayRectangle rayRectangle = closestSolidObject.ObjectHit.GetRenderRectangle(closestSolidObject, player, rayAngle, i); //2ms
                    //Add the render rectangle to the list for this screens x positon
                    rayRectangles[i].Add(rayRectangle);

                    //Get a sorted list of transparent objects with a cut off of the solid object
                    List<RayHitObject> transparentObjects = raycastHandler.SortTransparentObjects(hitObjects, closestSolidObject.Distance);
                    //Loop throught the list of transparent objects from furthest to closest
                    foreach (RayHitObject hitObject in transparentObjects)
                    {
                        //Get the rendered rectangle of the transparent object
                        RayRectangle rayTransparentRectangle = hitObject.ObjectHit.GetRenderRectangle(hitObject, player, rayAngle, i);
                        //Add the render rectangle to the list for this screens x positon
                        rayRectangles[i].Add(rayTransparentRectangle);
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (updateRays)
            {
                UpdateRays();
                updateRays = false;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            updateRays = true;

            float frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (float.IsInfinity(frameRate))
                frameRate = 0;

            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null);

            //Draw the floor and sky
            Rectangle sky = new Rectangle(0, 0, (int)game1.bounds.X, (int)game1.bounds.Y / 2);
            Rectangle floor = new Rectangle(0, (int)game1.bounds.Y / 2, (int)game1.bounds.X, (int)game1.bounds.Y / 2);
            spriteBatch.Draw(pixelTexture, sky, Color.SkyBlue);
            spriteBatch.Draw(pixelTexture, floor, Color.Gray);

            //Draw walls
            for (int i = 0; i < rayRectangles.Length; i++)
            {
                //If the list is not empty
                if (rayRectangles[i] != null)
                {
                    //Loop through this rays rectangles
                    foreach (RayRectangle rayRectangle in rayRectangles[i])
                    {
                        //If the rectangle is visable
                        if (rayRectangle.Visable)
                        {
                            //Draw the rectangle
                            spriteBatch.Draw(rayRectangle.Texture, rayRectangle.Rectangle, rayRectangle.TextureRectangle, rayRectangle.Color);
                        }
                    }
                }
            }

            //Draw the fps
            spriteBatch.DrawString(game1.basicFont, $"FPS: {frameRate.ToString()}", new Vector2(10, 10), Color.Black);

            spriteBatch.End();
        }

        public double DegreesToRadians(double degrees)
        {
            double radians = Math.PI / 180 * degrees;
            return radians;
        }
    }
}
