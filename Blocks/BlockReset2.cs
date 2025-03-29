namespace CheckpointBlock.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    public class BlockReset2 : IBlock, IBlockDebugColor
    {
        public static readonly Color BLOCKCODE_RESET_2 = new Color(4, 238, 124);

        private readonly Rectangle collider;

        public BlockReset2(Rectangle collider) => this.collider = collider;

        public Color DebugColor => BLOCKCODE_RESET_2;

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

