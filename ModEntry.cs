namespace CheckpointBlock
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
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
        private static Dictionary<ulong, Point> Checkpoints2 { get; set; }
        private static Texture2D CheckpointTexture { get; set; }
        private static Texture2D CheckpointTexture2 { get; set; }
        private static EntityFlag EntityFlag { get; set; }
        private static EntityFlag EntityFlag2 { get; set; }
        public static Point CurrentPosition { get; set; }
        public static Point CurrentPosition2 { get; set; }
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
            Checkpoints2 = new Dictionary<ulong, Point>();
            if (File.Exists(XmlFile))
            {
                using (var fs = new FileStream(XmlFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var doc = XDocument.Load(fs);
                    var root = doc.Root;
                    foreach (var element in root.Elements())
                    {
                        if (element.HasElements)
                        {
                            var key = ulong.Parse(element.Name.LocalName.Skip(2).ToString());
                            XElement xel;
                            if ((xel = element.Element("_set1")) != null)
                            {
                                var split = xel.Value.Split(' ');
                                Checkpoints.Add(
                                    key,
                                    new Point(int.Parse(split[0]), int.Parse(split[1])));
                            }
                            if ((xel = element.Element("_set2")) != null)
                            {
                                var split = xel.Value.Split(' ');
                                Checkpoints2.Add(
                                    key,
                                    new Point(int.Parse(split[0]), int.Parse(split[1])));
                            }
                        }
                        else
                        {
                            var split = element.Value.Split(' ');
                            Checkpoints.Add(
                                ulong.Parse(new string(element.Name.LocalName.Skip(2).ToArray())),
                                new Point(int.Parse(split[0]), int.Parse(split[1])));
                        }
                    }
                }
            }

            var contentManager = Game1.instance.contentManager;
            // The path doesn't exist only when you removed the file. Won't check.
            CheckpointTexture = contentManager.Load<Texture2D>(
                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "checkpoint"));
            CheckpointTexture2 = contentManager.Load<Texture2D>(
                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "checkpoint2"));
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

            var customPath2 = Path.Combine(contentManager.level.Root, "checkpoint2");
            Texture2D checkpointTexture2;
            if (File.Exists(customPath2 + ".xnb"))
            {
                checkpointTexture2 = contentManager.Load<Texture2D>(customPath);
            }
            else
            {
                checkpointTexture2 = CheckpointTexture2;
            }

            var startPosition = new Point(231, 302);
            var startData = contentManager.level.Info.About.StartData;
            if (startData.HasValue && startData.Value.Position.HasValue)
            {
                startPosition = startData.Value.Position.Value.ToPoint();
            }

            EntityFlag = null;
            if (level.ID == FactoryCheckpoint.LastUsedMapIdSet1)
            {
                EntityFlag = new EntityFlag(checkpointTexture, startPosition);
                _ = player.m_body.RegisterBlockBehaviour(typeof(BlockReset), new BehaviourReset(startPosition));
                _ = player.m_body.RegisterBlockBehaviour(typeof(BlockCheckpoint), new BehaviourCheckpoint(EntityFlag));
            }

            EntityFlag2 = null;
            if (level.ID == FactoryCheckpoint.LastUsedMapIdSet2)
            {
                EntityFlag2 = new EntityFlag(checkpointTexture2, startPosition);
                _ = player.m_body.RegisterBlockBehaviour(typeof(BlockReset2), new BehaviourReset2(startPosition));
                _ = player.m_body.RegisterBlockBehaviour(typeof(BlockCheckpoint2), new BehaviourCheckpoint2(EntityFlag2));
            }

            CurrentPosition = startPosition;
            CurrentPosition2 = startPosition;
            if (!SaveManager.instance.IsNewGame && Checkpoints.TryGetValue(level.ID, out var value))
            {
                CurrentPosition = value;
                CurrentPosition2 = value;
                EntityFlag.FlagPosition = value;
                EntityFlag2.FlagPosition = value;
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

            if (EntityFlag != null)
            {
                Checkpoints[contentManager.level.ID] = EntityFlag.FlagPosition;
            }
            if (EntityFlag2 != null)
            {
                Checkpoints2[contentManager.level.ID] = EntityFlag2.FlagPosition;
            }

            var allKeys = new HashSet<ulong>(Checkpoints.Keys.Concat(Checkpoints2.Keys));
            var doc = new XElement("levels",
            allKeys.Select(key =>
                new XElement("id" + key,
                    Checkpoints.TryGetValue(key, out var value1) ? new XElement("_set1", value1.X + " " + value1.Y) : null,
                    Checkpoints2.TryGetValue(key, out var value2) ? new XElement("_set2", value2.X + " " + value2.Y) : null
                )
            )
        );

            using (var fs = new FileStream(XmlFile,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None))
            {
                doc.Save(fs);
            }
        }
    }
}
