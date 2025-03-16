namespace CheckpointBlock.Factories
{
    using CheckpointBlock.Blocks;
    using JumpKing.API;
    using JumpKing.Level;
    using JumpKing.Level.Sampler;
    using JumpKing.Workshop;
    using Microsoft.Xna.Framework;
    using System;
    using System.Collections.Generic;

    public class FactoryCheckpoint : IBlockFactory
    {
        public static ulong LastUsedMapId { get; private set; } = ulong.MaxValue;

        private static readonly HashSet<Color> SupportedBlockCodes = new HashSet<Color> {
            BlockCheckpoint.BLOCKCODE_CHECKPOINT,
            BlockReset.BLOCKCODE_RESET,
        };

        public bool CanMakeBlock(Color blockCode, Level level) => SupportedBlockCodes.Contains(blockCode);

        public bool IsSolidBlock(Color blockCode) => false;

        public IBlock GetBlock(Color blockCode, Rectangle blockRect, Level level, LevelTexture textureSrc, int currentScreen, int x, int y)
        {
            if (LastUsedMapId != level.ID && SupportedBlockCodes.Contains(blockCode))
            {
                LastUsedMapId = level.ID;
            }
            switch (blockCode)
            {
                case var _ when blockCode == BlockCheckpoint.BLOCKCODE_CHECKPOINT:
                    return new BlockCheckpoint(blockRect);
                case var _ when blockCode == BlockReset.BLOCKCODE_RESET:
                    return new BlockReset(blockRect);
                default:
                    throw new InvalidOperationException($"{nameof(FactoryCheckpoint)} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
