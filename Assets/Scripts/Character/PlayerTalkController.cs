using Talk;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VGADestroy.Character
{
    public class PlayerTalkController : MonoBehaviour
    {
        [SerializeField] private float _talkRadius = 2.5f;
        [SerializeField] private PlayerController _playerController;

        [SerializeField] LayerMask _npcLayer;
        private Collider[] _hits = new Collider[8];

        private void OnEnable()
        {
            Debug.Log($"PlayerController: {_playerController}");

            Debug.Log($"Action: {_playerController?.Action}");
            Debug.Log($"PlayerMap: {_playerController?.Action?.Player}");
            if (_playerController == null) return;
            if (_playerController.Action == null) return;
            Debug.Log($"Interact: {_playerController.Action.Player.Interact}");

            InputAction interact = _playerController.Action.Player.Interact;
            interact.performed += TryTalk;
            interact.Enable();
        }

        private void OnDisable()
        {
            _playerController.Action.Player.Interact.performed -= TryTalk;
        }

        // ToDo : これ動いてない
        private void TryTalk(InputAction.CallbackContext context)
        {
            Debug.Log($"Talking {context.ReadValueAsButton()}");
            int count = Physics.OverlapSphereNonAlloc(transform.position, _talkRadius, _hits, _npcLayer);

            ITalkable target = null;
            float minDistance = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                var col = _hits[i];
                if (col == null) continue;

                if (!col.TryGetComponent(out ITalkable t)) continue;

                float sqrDist = (col.transform.position - transform.position).sqrMagnitude;

                if (sqrDist < minDistance)
                {
                    minDistance = sqrDist;
                    target = t;
                }
            }

            if (target == null)
            {
                Debug.Log("Target is null");
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