namespace CheckpointBlock.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    public class BlockCheckpoint2 : BoxBlock, IBlockDebugColor
    {
        public static readonly Color BLOCKCODE_CHECKPOINT_2 = new Color(3, 238, 124);

        public BlockCheckpoint2(Rectangle collider) : base(collider) { }

        public Color DebugColor => BLOCKCODE_CHECKPOINT_2;

        protected override bool canBlockPlayer => false;
    }
}

