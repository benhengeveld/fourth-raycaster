using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace fourthRaycaster.Models
{
    public class RaySpriteReturn
    {
        private Vector2 drawSize;
        private Vector2 drawPos;
        private float distance;

        public Vector2 DrawSize { get => drawSize; set => drawSize = value; }
        public Vector2 DrawPos { get => drawPos; set => drawPos = value; }
        public float Distance { get => distance; set => distance = value; }

        public RaySpriteReturn(Vector2 drawSize, Vector2 drawPos, float distance)
        {
            this.drawSize = drawSize;
            this.drawPos = drawPos;
            this.distance = distance;
        }
    }
}
