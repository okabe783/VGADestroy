using UnityEngine;

namespace Talk
{
    public class TalkableNPC : MonoBehaviour, ITalkable
    {
        [SerializeField] private string _talkID;
        
        public void Talk()
        {
            // ToDo : ここにTalkManagerにアクセスして会話をスタートさせる
        }
    }
}