namespace CheckpointBlock.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    public class BlockReset2 : BoxBlock, IBlockDebugColor
    {
        public static readonly Color BLOCKCODE_RESET_2 = new Color(4, 238, 124);

        public BlockReset2(Rectangle collider) : base(collider) { }

        public Color DebugColor => BLOCKCODE_RESET_2;

        protected override bool canBlockPlayer => false;
    }
}

