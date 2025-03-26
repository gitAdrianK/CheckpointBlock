namespace CheckpointBlock
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Linq;
    using CheckpointBlock.Behaviours;
    using CheckpointBlock.Blocks;
    using CheckpointBlock.Entities;
    using CheckpointBlock.Factories;
    using EntityComponent;
    using JumpKing;
    using JumpKing.Level;
    using JumpKing.Mods;
    using JumpKing.Player;
    using JumpKing.SaveThread;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    [JumpKingMod("Zebra.CheckpointBlock")]
    public static class ModEntry
    {
        private static string XmlFile { get; set; }
        private static Dictionary<ulong, Point> Checkpoints { get; set; }
        private static Texture2D CheckpointTexture { get; set; }
        private static EntityFlag EntityFlag { get; set; }
        public static Point CurrentPosition { get; set; }
        public static bool IgnoreStart { get; set; }

        /// <summary>
        /// Called by Jump King before the level loads
        /// </summary>
        [BeforeLevelLoad]
        public static void BeforeLevelLoad()
        {
            //_ = Debugger.Launch();

            _ = LevelManager.RegisterBlockFactory(new FactoryCheckpoint());

            XmlFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "checkpoints.sav");
            Checkpoints = new Dictionary<ulong, Point>();
            if (File.Exists(XmlFile))
            {
                XmlReader reader = null;
                try
                {
                    reader = XmlReader.Create(XmlFile);
                    var root = XElement.Load(reader);
                    foreach (var element in root.Elements())
                    {
                        var split = element.Value.Split(' ');
                        Checkpoints.Add(
                            ulong.Parse(new string(element.Name.LocalName.Skip(2).ToArray())),
                            new Point(int.Parse(split[0]), int.Parse(split[1])));
                    }
                }
                finally
                {
                    reader?.Close();
                    reader?.Dispose();
                }
            }

            var contentManager = Game1.instance.contentManager;
            // The path doesn't exist only when you removed the file. Won't check.
            CheckpointTexture = contentManager.Load<Texture2D>(
                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "checkpoint"));
        }

        /// <summary>
        /// Called by Jump King when the Level Starts
        /// </summary>
        [OnLevelStart]
        public static void OnLevelStart()
        {
            var contentManager = Game1.instance.contentManager;
            var level = contentManager.level;
            if (level == null
                || level.ID != FactoryCheckpoint.LastUsedMapId)
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

            var customPath = Path.Combine(contentManager.level.Root, "checkpoint");
            Texture2D checkpointTexture;
            if (File.Exists(customPath + ".xnb"))
            {
                checkpointTexture = contentManager.Load<Texture2D>(customPath);
            }
            else
            {
                checkpointTexture = CheckpointTexture;
            }

            var startPosition = new Point(231, 302);
            var startData = contentManager.level.Info.About.StartData;
            if (startData.HasValue && startData.Value.Position.HasValue)
            {
                startPosition = startData.Value.Position.Value.ToPoint();
            }

            EntityFlag = new EntityFlag(checkpointTexture, startPosition);
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockReset), new BehaviourReset(startPosition));
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockCheckpoint), new BehaviourCheckpoint(EntityFlag));

            CurrentPosition = startPosition;
            if (!SaveManager.instance.IsNewGame && Checkpoints.TryGetValue(level.ID, out var value))
            {
                CurrentPosition = value;
                EntityFlag.FlagPosition = value;
            }

            var entities = entityManager.Entities
                .SkipWhile(e => e != player)
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
            var contentManager = Game1.instance.contentManager;
            if (contentManager.level == null
                || contentManager.level.ID != FactoryCheckpoint.LastUsedMapId)
            {
                return;
            }

            Checkpoints[contentManager.level.ID] = EntityFlag.FlagPosition;

            XmlWriter writer = null;
            try
            {
                writer = XmlWriter.Create(XmlFile, new XmlWriterSettings() { Indent = true });
                var element = new XElement("levels",
                        Checkpoints.Select(kv => new XElement($"id{kv.Key}", $"{kv.Value.X} {kv.Value.Y}")));
                element.Save(writer);
            }
            finally
            {
                writer?.Flush();
                writer?.Close();
                writer?.Dispose();
            }
        }
    }
}
