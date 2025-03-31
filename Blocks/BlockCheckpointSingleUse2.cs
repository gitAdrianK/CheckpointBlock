namespace CheckpointBlock.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    public class BlockCheckpointSingleUse2 : IBlock, IBlockDebugColor
    {
        public static readonly Color BLOCKCODE_CHECKPOINT_SINGLE_USE_2 = new Color(6, 238, 124);

        private readonly Rectangle collider;

        public BlockCheckpointSingleUse2(Rectangle collider) => this.collider = collider;

        public Color DebugColor => BLOCKCODE_CHECKPOINT_SINGLE_USE_2;

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

