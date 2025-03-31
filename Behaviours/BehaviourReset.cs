namespace CheckpointBlock.Behaviours
{
    using CheckpointBlock.Blocks;
    using CheckpointBlock.Data;
    using JumpKing;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    public class BehaviourReset : IBlockBehaviour
    {
        public float BlockPriority => 2.0f;

        public bool IsPlayerOnBlock { get; set; }

        private ICollisionQuery CollisionQuery { get; set; }
        private CheckpointSet Set { get; set; }
        private Point Start { get; }

        public BehaviourReset(ICollisionQuery collisionQuery, CheckpointSet set, Point start)
        {
            this.CollisionQuery = collisionQuery;
            this.Set = set;
            this.Start = start;
        }

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

            var bodyComp = behaviourContext.BodyComp;
            var hitbox = bodyComp.GetHitbox();
            _ = this.CollisionQuery.CheckCollision(hitbox, out var _, out AdvCollisionInfo info);
            this.IsPlayerOnBlock = info.IsCollidingWith<BlockReset>();

            if (!this.IsPlayerOnBlock
                || (ModEntry.IgnoreStart && this.Start == this.Set.Current))
            {
                return true;
            }

            bodyComp.Position.X = this.Set.Current.X - (bodyComp.GetHitbox().Width / 2.0f);
            bodyComp.Position.Y = this.Set.Current.Y - bodyComp.GetHitbox().Height;
            bodyComp.Velocity = Vector2.Zero;
            Camera.UpdateCamera(bodyComp.Position.ToPoint());

            return true;
        }
    }
}
