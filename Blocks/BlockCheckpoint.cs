namespace CheckpointBlock.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    public class BlockCheckpoint : BoxBlock, IBlockDebugColor
    {
        public static readonly Color BLOCKCODE_CHECKPOINT = new Color(1, 238, 124);

        public BlockCheckpoint(Rectangle collider) : base(collider) { }

        public Color DebugColor => BLOCKCODE_CHECKPOINT;

        protected override bool canBlockPlayer => false;
    }
}

