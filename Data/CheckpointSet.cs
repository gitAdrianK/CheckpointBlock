namespace CheckpointBlock.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Relevant information for a checkpoint set.
    /// </summary>
    public class CheckpointSet
    {
        public static CheckpointSet FromXElement(XElement element)
        {
            // Because they are generated automatically it should never be null
            // but just to be sure.
            if (element == null)
            {
                return new CheckpointSet();
            }

            var used = new HashSet<Point>();
            var positions = element.Element("Used")?.Elements("Position");
            if (positions != null)
            {
                foreach (var position in positions)
                {
                    var x = int.Parse(position.Element("X").Value);
                    var y = int.Parse(position.Element("Y").Value);
                    _ = used.Add(new Point(x, y));
                }
            }

            return new CheckpointSet
            {
                Current = new Point(
                    int.Parse(element.Element("Current").Element("X").Value),
                    int.Parse(element.Element("Current").Element("Y").Value)
                ),
                Used = used
            };
        }

        public XElement ToXElement(string rootName) => new XElement(rootName,
            new XElement("Current",
                new XElement("X", this.Current.X),
                new XElement("Y", this.Current.Y)),
            new XElement("Used",
            this.Used.Count() != 0
            ? this.Used.Select(entry =>
                new XElement("Position",
                    new XElement("X", entry.X),
                    new XElement("Y", entry.Y)))
            : null));

        public CheckpointSet() => this.Used = new HashSet<Point>();

        public CheckpointSet(Point start)
        {
            this.Current = start;
            this.Used = new HashSet<Point>();
        }

        /// <summary>
        /// The current checkpoint position.
        /// </summary>
        public Point Current { get; set; }

        /// <summary>
        /// The positions of checkpoints that have been used/ activated once.
        /// </summary>
        public HashSet<Point> Used { get; set; }
    }
}
