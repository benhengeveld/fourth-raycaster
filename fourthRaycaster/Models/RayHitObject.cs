using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace fourthRaycaster.Models
{
    public class RayHitObject
    {
        private bool hit;
        private CollidableObject objectHit;
        private Vector2 hitPosition;
        private int hitSide;
        private float distance;

        public bool Hit { get => hit; set => hit = value; }
        public CollidableObject ObjectHit { get => objectHit; set => objectHit = value; }
        public Vector2 HitPosition { get => hitPosition; set => hitPosition = value; }
        public int HitSide { get => hitSide; set => hitSide = value; }
        public float Distance { get => distance; set => distance = value; }

        public RayHitObject(bool hit, CollidableObject objectHit, Vector2 hitPosition, int hitSide, float distance)
        {
            this.hit = hit;
            this.objectHit = objectHit;
            this.hitPosition = hitPosition;
            this.hitSide = hitSide;
            this.distance = distance;
        }
    }
}
