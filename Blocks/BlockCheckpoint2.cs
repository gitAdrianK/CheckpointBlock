namespace CheckpointBlock.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    public class BlockCheckpoint2 : IBlock, IBlockDebugColor
    {
        public static readonly Color BLOCKCODE_CHECKPOINT_2 = new Color(3, 238, 124);

        private readonly Rectangle collider;

        public BlockCheckpoint2(Rectangle collider) => this.collider = collider;

        public Color DebugColor => BLOCKCODE_CHECKPOINT_2;

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

