namespace CheckpointBlock
{
    using System.Linq;
    using CheckpointBlock.Data;
    using CheckpointBlock.Entities;
    using CheckpointBlock.Factories;
    using CheckpointBlock.Setups;
    using EntityComponent;
    using JumpKing;
    using JumpKing.Level;
    using JumpKing.Mods;
    using JumpKing.Player;
    using JumpKing.SaveThread;
    using Microsoft.Xna.Framework;

    [JumpKingMod("Zebra.CheckpointBlock")]
    public static class ModEntry
    {
        public static DataCheckpoint Data { get; private set; }
        public static bool IgnoreStart { get; set; }

        /// <summary>
        /// Called by Jump King before the level loads
        /// </summary>
        [BeforeLevelLoad]
        public static void BeforeLevelLoad()
        {
            //_ = Debugger.Launch();

            _ = LevelManager.RegisterBlockFactory(new FactoryCheckpoint());

            var contentManager = Game1.instance.contentManager;
            SetupSet1.LoadTexture(contentManager);
            SetupSet2.LoadTexture(contentManager);
        }

        /// <summary>
        /// Called by Jump King when the Level Starts
        /// </summary>
        [OnLevelStart]
        public static void OnLevelStart()
        {
            var contentManager = Game1.instance.contentManager;
            var level = contentManager.level;
            if (level == null || level.ID != FactoryCheckpoint.LastUsedMapId)
            {
                return;
            }

            var entityManager = EntityManager.instance;
            var player = entityManager.Find<PlayerEntity>();
            if (player == null)
            {
                return;
            }

            IgnoreStart = false;
            foreach (var tag in level.Info.Tags)
            {
                if (tag == "CheckpointsIgnoreStart")
                {
                    IgnoreStart = true;
                    break;
                }
            }

            // This is the default start position.
            var start = new Point(231, 302);
            var startData = level.Info.About.StartData?.Position;
            if (startData.HasValue)
            {
                start = startData.Value.ToPoint();
            }

            if (SaveManager.instance.IsNewGame)
            {
                Data = new DataCheckpoint(start);
            }
            else
            {
                Data = DataCheckpoint.TryDeserialize(start);
            }

            SetupSet1.Setup(contentManager, level, player, Data, start);
            SetupSet2.Setup(contentManager, level, player, Data, start);

            var entities = entityManager.Entities
                .SkipWhile(entity => entity != player)
                .ToList();
            entities.ForEach(entity =>
            {
                if (!(entity is EntityFlag))
                {
                    entity.GoToFront();
                }
            });
        }

        [OnLevelEnd]
        public static void OnLevelEnd()
        {
            var level = Game1.instance.contentManager.level;
            if (level == null || level.ID != FactoryCheckpoint.LastUsedMapId)
            {
                return;
            }
            Data.SaveToFile();
        }
    }
}
