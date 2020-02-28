using System.Collections;
using System.Linq;
using UnityEngine;

namespace Gameplay
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve SwitchTargetCurve;

        public IEnumerator SwitchTarget(Transform target)
        {
            transform.SetParent(null);
            Vector3 start = transform.position;
            float duration = SwitchTargetCurve.keys.Last().time;
            for (float elapsed = 0; elapsed < duration; elapsed += Time.deltaTime)
            {
                transform.position =
                    Vector3.Lerp(start, target.position, SwitchTargetCurve.Evaluate(elapsed / duration));
                yield return null;
            }

            transform.position = target.position;
            transform.SetParent(target);
        }
    }
}