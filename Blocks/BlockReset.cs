namespace CheckpointBlock.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    public class BlockReset : BoxBlock, IBlockDebugColor
    {
        public static readonly Color BLOCKCODE_RESET = new Color(2, 238, 124);

        public BlockReset(Rectangle collider) : base(collider) { }

        public Color DebugColor => BLOCKCODE_RESET;

        protected override bool canBlockPlayer => false;
    }
}

