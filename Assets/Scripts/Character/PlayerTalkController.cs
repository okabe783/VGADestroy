using Talk;
using UnityEngine;

namespace VGADestroy.Character
{
    public class PlayerTalkController : MonoBehaviour
    {
        [SerializeField] private float _talkRadius = 2.5f;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private LayerMask _npcLayer;

        private readonly Collider[] _hits = new Collider[8];

        private void OnEnable()
        {
            if (_playerController == null) return;
            
            _playerController.OnInteract += TryTalk;
        }

        private void OnDisable()
        {
            if (_playerController == null) return;

            _playerController.OnInteract -= TryTalk;
        }

        private void TryTalk()
        {
            if (!TalkSystem.I.CanStartTalk)
            {
                Debug.Log("Can't talk");
                return;
            }

            if (TalkSystem.I.IsTalking)
                return;

            int count = Physics.OverlapSphereNonAlloc(
                transform.position,
                _talkRadius,
                _hits,
                _npcLayer
            );

            ITalkable target = null;
            float minSqrDistance = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                Collider col = _hits[i];
                if (col == null) continue;

                if (!col.TryGetComponent(out ITalkable talkable)) continue;

                float sqrDist =
                    (col.transform.position - transform.position).sqrMagnitude;

                if (sqrDist >= minSqrDistance) continue;

                minSqrDistance = sqrDist;
                target = talkable;
            }

            if (target == null)
            {
                Debug.Log("[Input] Talk target not found");
                return;
            }
            target.Talk();
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _talkRadius);
        }
    }
}