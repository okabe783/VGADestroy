using UnityEngine;

namespace Talk
{
    public class TalkableNPC : MonoBehaviour, ITalkable
    {
        [SerializeField] private int _talkID;
        [SerializeField] private TalkData _talkData;
        
        public void Talk()
        {
            TalkSystem.I.TalkManager.StartTalk(_talkData,_talkID);
        }
    }
}