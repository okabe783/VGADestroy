using System.Collections.Generic;

namespace Talk
{
    public class TalkData
    {
        public string TalkID;
        public List<TalkNode> TalkNodes = new();

        public TalkNode GetNode(int nodeID)
        {
            return TalkNodes[nodeID];
        }
    }

    public class TalkNode
    {
        public int NodeID;
        public string Speaker;
        public string Text;
        public List<TalkChoice> Choices = new();
        public List<TalkReward> Rewards = new();
    }

    public class TalkChoice
    {
        public string Text;
        public int JumpNodeID;
    }

    public class TalkReward
    {
        public RewardType Type;
        public string RewardID;
        public int Amount;
    }

    public enum RewardType
    {
        Item,
        Coin,
        Flag,
    }
}