namespace CheckpointBlock.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    public class BlockCheckpointSingleUse : BoxBlock, IBlockDebugColor
    {
        public static readonly Color BLOCKCODE_CHECKPOINT_SINGLE_USE = new Color(5, 238, 124);

        public BlockCheckpointSingleUse(Rectangle collider) : base(collider) { }

        public Color DebugColor => BLOCKCODE_CHECKPOINT_SINGLE_USE;

        protected override bool canBlockPlayer => false;
    }
}

