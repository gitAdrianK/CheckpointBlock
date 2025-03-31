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
    using JumpKing.Level;
    using JumpKing.Player;
    using JumpKing.Workshop;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    // Yeah it's a little copy-pasteish. I know, i know.

    public class SetupSet2
    {
        private static EntityFlag EntityFlag { get; set; }

        public static void Setup(
            JKContentManager contentManager,
            Level level,
            PlayerEntity player,
            DataCheckpoint data,
            Point start)
        {
            if (level.ID != FactoryCheckpoint.LastUsedMapIdSet2)
            {
                return;
            }

            var customPath = Path.Combine(level.Root, "checkpoint2");
            Texture2D checkpointTexture;
            if (File.Exists(customPath + ".xnb"))
            {
                checkpointTexture = contentManager.Load<Texture2D>(customPath);
            }
            else
            {
                checkpointTexture = contentManager.Load<Texture2D>(
                    Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "checkpoint2"));
            }

            EntityFlag = new EntityFlag(checkpointTexture, start)
            {
                FlagPosition = data.Set2.Current
            };
            _ = player.m_body.RegisterBlockBehaviour(
                typeof(BlockReset2), new BehaviourReset2(LevelManager.Instance, data.Set2, start));
            _ = player.m_body.RegisterBlockBehaviour(
                typeof(BlockCheckpoint2), new BehaviourCheckpoint2(data.Set2, EntityFlag));
            _ = player.m_body.RegisterBlockBehaviour(
                typeof(BlockCheckpointSingleUse2), new BehaviourCheckpointSingleUse2(data.Set2, EntityFlag));
        }
    }
}
