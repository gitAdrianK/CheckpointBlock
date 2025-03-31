namespace CheckpointBlock.Factories
{
    using System;
    using System.Collections.Generic;
    using CheckpointBlock.Blocks;
    using JumpKing.API;
    using JumpKing.Level;
    using JumpKing.Level.Sampler;
    using JumpKing.Workshop;
    using Microsoft.Xna.Framework;

    public class FactoryCheckpoint : IBlockFactory
    {
        public static ulong LastUsedMapId { get; private set; } = ulong.MaxValue;
        public static ulong LastUsedMapIdSet1 { get; private set; } = ulong.MaxValue;
        public static ulong LastUsedMapIdSet2 { get; private set; } = ulong.MaxValue;

        private static readonly HashSet<Color> SupportedBlockCodes = new HashSet<Color> {
            BlockCheckpoint.BLOCKCODE_CHECKPOINT,
            BlockCheckpoint2.BLOCKCODE_CHECKPOINT_2,
            BlockReset.BLOCKCODE_RESET,
            BlockReset2.BLOCKCODE_RESET_2,
            BlockCheckpointSingleUse.BLOCKCODE_CHECKPOINT_SINGLE_USE,
            BlockCheckpointSingleUse2.BLOCKCODE_CHECKPOINT_SINGLE_USE_2,
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
                    LastUsedMapIdSet1 = level.ID;
                    return new BlockCheckpoint(blockRect);
                case var _ when blockCode == BlockCheckpoint2.BLOCKCODE_CHECKPOINT_2:
                    LastUsedMapIdSet2 = level.ID;
                    return new BlockCheckpoint2(blockRect);
                case var _ when blockCode == BlockReset.BLOCKCODE_RESET:
                    LastUsedMapIdSet1 = level.ID;
                    return new BlockReset(blockRect);
                case var _ when blockCode == BlockReset2.BLOCKCODE_RESET_2:
                    LastUsedMapIdSet2 = level.ID;
                    return new BlockReset2(blockRect);
                case var _ when blockCode == BlockCheckpointSingleUse.BLOCKCODE_CHECKPOINT_SINGLE_USE:
                    LastUsedMapIdSet1 = level.ID;
                    return new BlockCheckpointSingleUse(blockRect);
                case var _ when blockCode == BlockCheckpointSingleUse2.BLOCKCODE_CHECKPOINT_SINGLE_USE_2:
                    LastUsedMapIdSet2 = level.ID;
                    return new BlockCheckpointSingleUse2(blockRect);
                default:
                    throw new InvalidOperationException($"{nameof(FactoryCheckpoint)} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
