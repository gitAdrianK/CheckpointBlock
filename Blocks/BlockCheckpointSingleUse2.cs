namespace CheckpointBlock.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    public class BlockCheckpointSingleUse2 : BoxBlock, IBlockDebugColor
    {
        public static readonly Color BLOCKCODE_CHECKPOINT_SINGLE_USE_2 = new Color(6, 238, 124);

        public BlockCheckpointSingleUse2(Rectangle collider) : base(collider) { }

        public Color DebugColor => BLOCKCODE_CHECKPOINT_SINGLE_USE_2;

        protected override bool canBlockPlayer => false;
    }
}

