namespace CheckpointBlock.Behaviours
{
    using CheckpointBlock.Blocks;
    using CheckpointBlock.Entities;
    using JumpKing;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    public class BehaviourReset : IBlockBehaviour
    {
        public float BlockPriority => 2.0f;

        public bool IsPlayerOnBlock { get; set; }

        private EntityFlag EntityFlag { get; set; }

        public BehaviourReset(EntityFlag entityFlag) => this.EntityFlag = entityFlag;

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

            if (!this.IsPlayerOnBlock)
            {
                return true;
            }

            var bodyComp = behaviourContext.BodyComp;
            bodyComp.Position.X = this.EntityFlag.CurrentPosition.X - (bodyComp.GetHitbox().Width / 2.0f);
            bodyComp.Position.Y = this.EntityFlag.CurrentPosition.Y - bodyComp.GetHitbox().Height;
            bodyComp.Velocity = Vector2.Zero;
            Camera.UpdateCamera(bodyComp.Position.ToPoint());

            return true;
        }
    }
}
