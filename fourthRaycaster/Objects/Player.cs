using fourthRaycaster.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace fourthRaycaster.Objects
{
    public class Player : GameComponent
    {
        private Game1 game1;
        private Vector2 position;
        private Vector2 size;
        private double angle;

        private float deltaX;
        private float deltaY;

        private const float DPI = 500;

        public Vector2 Position { get => position; set => position = value; }
        public Vector2 Size { get => size; set => size = value; }
        public double Angle { get => angle; set => angle = value; }

        public Player(Game game, Vector2 position, Vector2 size, double angle) : base(game)
        {
            this.game1 = (Game1)game;
            this.position = position;
            this.size = size;
            this.angle = angle;

            deltaX = (float)Math.Cos(angle) * 5;
            deltaY = (float)Math.Sin(angle) * 5;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            //Double the speed if q is pressed
            float speedMulti = 1;
            if (ks.IsKeyDown(Keys.Q))
            {
                speedMulti = 2;
            }

            //Get the x distance from the center of the screen
            float mouseDis = (Mouse.GetState().X - game1.bounds.X / 2) / DPI;
            //Set the mouse to the center of the screen
            Mouse.SetPosition((int)game1.bounds.X / 2, (int)game1.bounds.Y / 2);

            //Looking to the left
            if (mouseDis < 0)
            {
                angle += mouseDis;
                //angle -= 0.05f;
                if (angle < 0)
                    angle += 2 * Math.PI;
                deltaX = (float)Math.Cos(angle) * 5;
                deltaY = (float)Math.Sin(angle) * 5;
            }
            //Looking to the right
            else if (mouseDis > 0)
            {
                angle += mouseDis;
                //angle += 0.05f;
                if (angle > 2 * Math.PI)
                    angle -= 2 * Math.PI;
                deltaX = (float)Math.Cos(angle) * 5;
                deltaY = (float)Math.Sin(angle) * 5;
            }

            //Save the current position before moving
            Vector2 pastPosition = position;
            //Get the distance the player is move in the x and y
            Vector2 delta = new Vector2(deltaX, deltaY) * speedMulti;

            //Moving forward
            if (ks.IsKeyDown(Keys.W) || ks.IsKeyDown(Keys.Up))
            {
                //Save the current position before moving
                pastPosition = position;
                //Move the play on the x axis
                position = new Vector2(position.X + delta.X, position.Y);
                //If the player is colliding with something move back
                if (game1.collisionHandler.IsCollidingWithObject(GetLines()))
                {
                    position = pastPosition;
                }

                //Save the current position before moving
                pastPosition = position;
                //Move the play on the y axis
                position = new Vector2(position.X, position.Y + delta.Y);
                //If the player is colliding with something move back
                if (game1.collisionHandler.IsCollidingWithObject(GetLines()))
                {
                    position = pastPosition;
                }
            }

            //Moving backwards
            if (ks.IsKeyDown(Keys.S) || ks.IsKeyDown(Keys.Down))
            {
                //Save the current position before moving
                pastPosition = position;
                //Move the play on the x axis
                position = new Vector2(position.X - delta.X, position.Y);
                //If the player is colliding with something move back
                if (game1.collisionHandler.IsCollidingWithObject(GetLines()))
                {
                    position = pastPosition;
                }

                //Save the current position before moving
                pastPosition = position;
                //Move the play on the y axis
                position = new Vector2(position.X, position.Y - delta.Y);
                //If the player is colliding with something move back
                if (game1.collisionHandler.IsCollidingWithObject(GetLines()))
                {
                    position = pastPosition;
                }
            }

            delta /= 1.5f;

            //Moving to the left
            if (ks.IsKeyDown(Keys.A))
            {
                //Save the current position before moving
                pastPosition = position;
                //Move the play on the x axis
                position = new Vector2(position.X + delta.Y, position.Y);
                //If the player is colliding with something move back
                if (game1.collisionHandler.IsCollidingWithObject(GetLines()))
                {
                    position = pastPosition;
                }

                //Save the current position before moving
                pastPosition = position;
                //Move the play on the y axis
                position = new Vector2(position.X, position.Y - delta.X);
                //If the player is colliding with something move back
                if (game1.collisionHandler.IsCollidingWithObject(GetLines()))
                {
                    position = pastPosition;
                }
            }

            //Moving to the right
            if (ks.IsKeyDown(Keys.D))
            {
                //Save the current position before moving
                pastPosition = position;
                //Move the play on the x axis
                position = new Vector2(position.X - delta.Y, position.Y);
                //If the player is colliding with something move back
                if (game1.collisionHandler.IsCollidingWithObject(GetLines()))
                {
                    position = pastPosition;
                }

                //Save the current position before moving
                pastPosition = position;
                //Move the play on the y axis
                position = new Vector2(position.X, position.Y + delta.X);
                //If the player is colliding with something move back
                if (game1.collisionHandler.IsCollidingWithObject(GetLines()))
                {
                    position = pastPosition;
                }
            }
        }

        /// <summary>
        /// Gets the lines of the players bounds
        /// </summary>
        /// <returns>A list of the lines</returns>
        public List<Line> GetLines()
        {
            //Get the position of each corner
            Vector2 topLeftPoint = new Vector2(position.X, position.Y);
            Vector2 topRightPoint = new Vector2(position.X + size.X, position.Y);
            Vector2 bottomLeftPoint = new Vector2(position.X, position.Y + size.Y);
            Vector2 bottomRightPoint = new Vector2(position.X + size.X, position.Y + size.Y);

            //Make a list for the sides
            List<Line> lines = new List<Line>();

            //Add each side to the list of lines
            lines.Add(new Line(topLeftPoint, topRightPoint, 1)); //Top wall
            lines.Add(new Line(topRightPoint, bottomRightPoint, 2)); //Right wall
            lines.Add(new Line(bottomLeftPoint, bottomRightPoint, 3)); //Bottom wall
            lines.Add(new Line(topLeftPoint, bottomLeftPoint, 4)); //Left wall

            return lines;
        }
    }
}
