namespace CheckpointBlock.Entities
{
    using System.Diagnostics.CodeAnalysis;
    using EntityComponent;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class EntityFlag : Entity
    {
        private Texture2D Texture { get; set; }
        private Point Start { get; set; }

        public Point CurrentPosition
        {
            get => this._currentPosition;
            set
            {
                this._currentPosition = value;
                // Assuming the position is only set by touching the block,
                // and as such the current screen in the active screen.
                this.CurrentScreen = Camera.CurrentScreen;
            }
        }
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Field attributes are experimental.")]
        private Point _currentPosition;

        private int CurrentScreen { get; set; }

        public EntityFlag(Texture2D texture, Point start)
        {
            this.Texture = texture;
            this.Start = start;
            this.CurrentPosition = start;
        }

        public override void Draw()
        {
            if (this.CurrentScreen != Camera.CurrentScreen
                || this.Start == this.CurrentPosition)
            {
                return;
            }

            var currentPosition = this.CurrentPosition;
            currentPosition.Y += Camera.CurrentScreen * 360;
            currentPosition += Camera.Offset.ToPoint();

            Game1.spriteBatch.Draw(
                texture: this.Texture,
                position: new Vector2(
                    currentPosition.X - (this.Texture.Width / 2.0f),
                    currentPosition.Y - this.Texture.Height),
                sourceRectangle: new Rectangle(
                    0,
                    0,
                    this.Texture.Width,
                    this.Texture.Height),
                color: Color.White);
        }
    }
}
