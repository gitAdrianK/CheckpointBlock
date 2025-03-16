namespace CheckpointBlock.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    public class BlockCheckpoint : IBlock, IBlockDebugColor
    {
        public static readonly Color BLOCKCODE_CHECKPOINT = new Color(1, 238, 124);

        private readonly Rectangle collider;

        public BlockCheckpoint(Rectangle collider) => this.collider = collider;

        public Color DebugColor => BLOCKCODE_CHECKPOINT;

        public Rectangle GetRect() => this.collider;

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.collider);
                return BlockCollisionType.Collision_NonBlocking;
            }
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}

