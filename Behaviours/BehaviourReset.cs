namespace CheckpointBlock.Behaviours
{
    using CheckpointBlock.Blocks;
    using JumpKing;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    public class BehaviourReset : IBlockBehaviour
    {
        public float BlockPriority => 2.0f;

        public bool IsPlayerOnBlock { get; set; }

        private Point Start { get; }

        public BehaviourReset(Point start) => this.Start = start;

        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext) => inputXVelocity;

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext) => inputYVelocity;

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext) => inputGravity;

        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            if (behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo == null)
            {
                return true;
            }

            var advCollisionInfo = behaviourContext.CollisionInfo.PreResolutionCollisionInfo;
            this.IsPlayerOnBlock = advCollisionInfo.IsCollidingWith<BlockReset>();

            if (!this.IsPlayerOnBlock
                || (ModEntry.IgnoreStart && this.Start == ModEntry.CurrentPosition))
            {
                return true;
            }

            var bodyComp = behaviourContext.BodyComp;
            bodyComp.Position.X = ModEntry.CurrentPosition.X - (bodyComp.GetHitbox().Width / 2.0f);
            bodyComp.Position.Y = ModEntry.CurrentPosition.Y - bodyComp.GetHitbox().Height;
            bodyComp.Velocity = Vector2.Zero;
            Camera.UpdateCamera(bodyComp.Position.ToPoint());

            return true;
        }
    }
}
