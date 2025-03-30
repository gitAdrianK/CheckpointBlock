namespace CheckpointBlock.Setups
{
    using System.IO;
    using System.Reflection;
    using CheckpointBlock.Behaviours;
    using CheckpointBlock.Blocks;
    using CheckpointBlock.Data;
    using CheckpointBlock.Entities;
    using CheckpointBlock.Factories;
    using JumpKing;
    using JumpKing.Player;
    using JumpKing.Workshop;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public static class SetupSet1
    {
        private static Texture2D CheckpointTexture { get; set; }

        private static EntityFlag EntityFlag { get; set; }

        public static void LoadTexture(JKContentManager contentManager)
            => CheckpointTexture = contentManager.Load<Texture2D>(
                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "checkpoint"));

        public static void Setup(
            JKContentManager contentManager,
            Level level,
            PlayerEntity player,
            DataCheckpoint data,
            Point start)
        {
            if (level.ID == FactoryCheckpoint.LastUsedMapIdSet1)
            {
                return;
            }

            var customPath = Path.Combine(level.Root, "checkpoint");
            Texture2D checkpointTexture;
            if (File.Exists(customPath + ".xnb"))
            {
                checkpointTexture = contentManager.Load<Texture2D>(customPath);
            }
            else
            {
                checkpointTexture = CheckpointTexture;
            }

            EntityFlag = new EntityFlag(checkpointTexture, start);
            _ = player.m_body.RegisterBlockBehaviour(
                typeof(BlockReset), new BehaviourReset(data.Set1, start));
            _ = player.m_body.RegisterBlockBehaviour(
                typeof(BlockCheckpoint), new BehaviourCheckpoint(data.Set1, EntityFlag));
        }
    }
}

