namespace CheckpointBlock.Behaviours
{
    using System.Linq;
    using CheckpointBlock.Blocks;
    using CheckpointBlock.Data;
    using CheckpointBlock.Entities;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    public class BehaviourCheckpointSingleUse2 : IBlockBehaviour
    {
        public float BlockPriority => 2.0f;

        public bool IsPlayerOnBlock { get; set; }
        private bool HasSet { get; set; }

        private CheckpointSet Set { get; set; }
        private EntityFlag EntityFlag { get; set; }

        public BehaviourCheckpointSingleUse2(CheckpointSet set, EntityFlag entityFlag)
        {
            this.Set = set;
            this.EntityFlag = entityFlag;
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

            var advCollisionInfo = behaviourContext.CollisionInfo.PreResolutionCollisionInfo;
            this.IsPlayerOnBlock = advCollisionInfo.IsCollidingWith<BlockCheckpointSingleUse2>();

            if (!this.IsPlayerOnBlock)
            {
                this.HasSet = false;
                return true;
            }

            if (this.HasSet)
            {
                return true;
            }
            this.HasSet = true;

            var rect = advCollisionInfo.GetCollidedBlocks<BlockCheckpointSingleUse2>().First().GetRect();
            var point = new Point(rect.Left + (rect.Width / 2), rect.Bottom);

            if (this.Set.Used.Contains(point))
            {
                return true;
            }

            _ = this.Set.Used.Add(point);
            this.Set.Current = point;
            this.EntityFlag.FlagPosition = point;

            return true;
        }
    }
}
