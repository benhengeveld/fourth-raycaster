using fourthRaycaster.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace fourthRaycaster.Models
{
    public class CollidableObject : GameComponent
    {
        private bool canCollide;
        private bool isVisable;
        private bool hasTransparentPixels;

        public bool CanCollide { get => canCollide; set => canCollide = value; }
        public bool HasTransparentPixels { get => hasTransparentPixels; set => hasTransparentPixels = value; }
        public bool IsVisable { get => isVisable; set => isVisable = value; }

        public CollidableObject(Game game, bool canCollide, bool isVisable = true, bool hasTransparentPixels = false) : base(game)
        {
            this.canCollide = canCollide;
            this.isVisable = isVisable;
            this.hasTransparentPixels = hasTransparentPixels;
        }

        public virtual RayRectangle GetRenderRectangle(RayHitObject rayObject, Player player, double rayAngle, int xScreenPos)
        {
            return null;
        }

        public virtual List<Line> GetLines()
        {
            return null;
        }
    }
}
