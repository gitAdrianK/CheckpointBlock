namespace CheckpointBlock.Data
{
    using System.IO;
    using System.Xml.Linq;
    using JumpKing;
    using Microsoft.Xna.Framework;

    public class DataCheckpoint
    {
        public static DataCheckpoint TryDeserialize(Point start)
        {
            var file = Path.Combine(
                    Game1.instance.contentManager.root,
                    "zebrasSaves",
                    "checkpointBlock.sav");
            if (!File.Exists(file))
            {
                return new DataCheckpoint(start);
            }

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var doc = XDocument.Load(fs);
                var root = doc.Root;

                return new DataCheckpoint(start)
                {
                    Set1 = CheckpointSet.FromXElement(root.Element("Set1")),
                    Set2 = CheckpointSet.FromXElement(root.Element("Set2")),
                };
            };
        }

        /// <summary>
        /// Saves the data to file.
        /// </summary>
        public void SaveToFile()
        {
            var path = Path.Combine(
                Game1.instance.contentManager.root,
                "zebrasSaves");
            if (!Directory.Exists(path))
            {
                _ = Directory.CreateDirectory(path);
            }

            var doc = new XDocument(
                new XElement("CheckpointData",
                    this.Set1.ToXElement("Set1"),
                    this.Set2.ToXElement("Set2")));

            using (var fs = new FileStream(
                Path.Combine(path, "checkpointBlock.sav"),
                FileMode.Create,
                FileAccess.Write,
                FileShare.None))
            {
                doc.Save(fs);
            }
        }

        public DataCheckpoint(Point start)
        {
            this.Set1 = new CheckpointSet(start);
            this.Set2 = new CheckpointSet(start);
        }

        /// <summary>
        /// The first checkpoint set.
        /// </summary>
        public CheckpointSet Set1 { get; set; }

        /// <summary>
        /// The second checkpoint set.
        /// </summary>
        public CheckpointSet Set2 { get; set; }
    }
}
