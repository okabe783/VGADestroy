using UnityEngine;

namespace Talk
{
    public class TalkableNPC : MonoBehaviour, ITalkable
    {
        [SerializeField] private string _talkID;

        public Transform Transform => transform;

        public void Talk()
        {
            LookAtPlayer(); 
            if (!TalkSystem.I.TryGetTalkData(_talkID, out TalkData talkData))
            {
                Debug.LogError($"TalkID not found: {_talkID}");
                return;
            }
            
            TalkSystem.I.TalkManager.StartTalk(talkData);
        }

        // Playerの向きに回転する
        private void LookAtPlayer()
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            Vector3 direction = player.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}