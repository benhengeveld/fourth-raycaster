using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace fourthRaycaster.Models
{
    public class Line
    {
        private Vector2 positionOne;
        private Vector2 positionTwo;
        private int side;

        public Vector2 PositionOne { get => positionOne; set => positionOne = value; }
        public Vector2 PositionTwo { get => positionTwo; set => positionTwo = value; }
        public int Side { get => side; set => side = value; }

        public Line(Vector2 positionOne, Vector2 positionTwo, int side = 0)
        {
            this.positionOne = positionOne;
            this.positionTwo = positionTwo;
            this.side = side;
        }
        public Line(float positionOneX, float positionOneY, float positionTwoX, float positionTwoY, int side)
        {
            this.positionOne = new Vector2(positionOneX, positionOneY);
            this.positionTwo = new Vector2(positionTwoX, positionTwoY);
            this.side = side;
        }
    }
}
