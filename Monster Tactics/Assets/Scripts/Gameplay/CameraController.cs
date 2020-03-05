using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve SwitchTargetCurve= default;

        [SerializeField]
        private AnimationCurve PivotCurve= default;

        [SerializeField]
        private float horizontalIncrement = 45;

        [SerializeField]
        private float verticalSteps = 3;

        [SerializeField, MinMaxSlider(0, 90, true)]
        private Vector2 verticalLimits = new Vector2();

        private float verticalIncrement => (verticalLimits.y - verticalLimits.x) / verticalSteps;

        [SerializeField, MinMaxSlider(1, 10, true)]
        private Vector2Int zoomLimits = new Vector2Int();

        private Transform pivot;
        private Camera gameCamera;
        private bool pivoting;

        private void Awake()
        {
            pivot = transform.GetChild(0);
            gameCamera = GetComponentInChildren<Camera>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) CameraMove(CameraDirection.Up);
            if (Input.GetKeyDown(KeyCode.DownArrow)) CameraMove(CameraDirection.Down);
            if (Input.GetKeyDown(KeyCode.LeftArrow)) CameraMove(CameraDirection.Left);
            if (Input.GetKeyDown(KeyCode.RightArrow)) CameraMove(CameraDirection.Right);
            if (Input.GetAxis("Mouse ScrollWheel") < 0) CameraMove(CameraDirection.ZoomIn);
            if (Input.GetAxis("Mouse ScrollWheel") > 0) CameraMove(CameraDirection.ZoomOut);
        }

        private void CameraMove(CameraDirection direction)
        {
            Vector3 rotation = pivot.rotation.eulerAngles;
            int targetZoom;
            switch (direction)
            {
                case CameraDirection.Up:
                    rotation.x = Mathf.Clamp(rotation.x + verticalIncrement, verticalLimits.x, verticalLimits.y);
                    break;
                case CameraDirection.Down:
                    rotation.x = Mathf.Clamp(rotation.x - verticalIncrement, verticalLimits.x, verticalLimits.y);
                    break;
                case CameraDirection.Left:
                    rotation.y += horizontalIncrement;
                    break;
                case CameraDirection.Right:
                    rotation.y -= horizontalIncrement;
                    break;
                case CameraDirection.ZoomIn:
                    targetZoom = (int) Mathf.Clamp(gameCamera.orthographicSize + 1, zoomLimits.x, zoomLimits.y);
                    StartCoroutine(ZoomSmooth(targetZoom));
                    break;
                case CameraDirection.ZoomOut:
                    targetZoom = (int) Mathf.Clamp(gameCamera.orthographicSize - 1, zoomLimits.x, zoomLimits.y);
                    StartCoroutine(ZoomSmooth(targetZoom));
                    break;
            }

            if (!pivoting) StartCoroutine(PivotCamera(Quaternion.Euler(rotation)));
        }

        enum CameraDirection
        {
            Up,
            Down,
            Left,
            Right,
            ZoomIn,
            ZoomOut
        }

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

        public IEnumerator PivotCamera(Quaternion target)
        {
            pivoting = true;
            Quaternion start = pivot.rotation;
            float duration = PivotCurve.keys.Last().time;
            for (float elapsed = 0; elapsed < duration; elapsed += Time.deltaTime)
            {
                pivot.rotation =
                    Quaternion.Lerp(start, target, PivotCurve.Evaluate(elapsed / duration));
                yield return null;
            }

            pivot.rotation = target;
            pivoting = false;
        }

        public IEnumerator ZoomSmooth(int target)
        {
            float start = gameCamera.orthographicSize;
            float duration = PivotCurve.keys.Last().time;
            for (float elapsed = 0; elapsed < duration; elapsed += Time.deltaTime)
            {
                gameCamera.orthographicSize =
                    Mathf.Lerp(start, target, PivotCurve.Evaluate(elapsed / duration));
                yield return null;
            }

            gameCamera.orthographicSize = target;
            pivoting = false;
        }
    }
}