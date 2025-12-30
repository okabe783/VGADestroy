using System.Collections.Generic;
using UnityEngine;

namespace Talk
{
    public class TalkData
    {
        public string TalkID;
        public Dictionary<int, TalkNode> NodeMap = new();

        public TalkNode GetNode(int nodeID)
        {
            if (NodeMap.TryGetValue(nodeID, out var node))
                return node;

            Debug.LogError($"[TalkData] NodeID {nodeID} not found");
            return null;
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