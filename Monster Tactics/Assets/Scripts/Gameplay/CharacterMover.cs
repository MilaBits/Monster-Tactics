using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Characters;
using Level;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Utilities;

public class CharacterMover : MonoBehaviour
{
    [SerializeField, InlineEditor]
    private Character target = default;

    private QuadTileMap tileMap;

    [Space]
    [SerializeField]
    private MoveParams moveParams = default;

    [SerializeField]
    private MoveParams jumpParams = default;

    [Space]
    [SerializeField]
    private GameObject ArrowMarkerPrefab = default;

    [SerializeField]
    private GameObject LineMarkerPrefab = default;

    private Queue<GameObject> LinePool = new Queue<GameObject>();
    private List<GameObject> visibleLines = new List<GameObject>();
    private GameObject Arrow;

    private bool moving = false;

    private List<QuadTile> oldPath;

    private Action<bool> returnAction;

    private GameObject GetLineSegmentFromPool()
    {
        if (LinePool.Count < 1) LinePool.Enqueue(Instantiate(LineMarkerPrefab));
        GameObject lineSegment = LinePool.Dequeue();
        lineSegment.SetActive(true);
        visibleLines.Add(lineSegment);
        return lineSegment;
    }

    private void StoreLineSegmentInPool(GameObject lineSegment)
    {
        lineSegment.SetActive(false);
        LinePool.Enqueue(lineSegment);
    }

    void Start()
    {
        Arrow = Instantiate(ArrowMarkerPrefab);
        Arrow.SetActive(false);

        tileMap = FindObjectOfType<QuadTileMap>();
    }

    [Button]
    private void MarkPossible()
    {
        foreach (QuadTile tile in tileMap.GetTilesInRange(
            tileMap.GetTile(target.transform.position.ToVector2IntXZ()), target.Data().move,
            target.Data().stepLayerLimit,
            target.Data().useRoughness))
        {
            tile.ToggleViableMarker(true);
        }
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Viable Marker")))
        {
            if (!moving)
            {
                List<QuadTile> path = hit.transform.GetComponentInParent<QuadTile>().ChainToList();

                if (oldPath == path)
                {
                    return;
                }


                path.Reverse();
                DrawPath(hit.transform.GetComponentInParent<QuadTile>());

                if (Input.GetButtonDown("Fire1"))
                {
                    ClearPossible();
                    Move(path);
                }

                oldPath = path;
            }
        }
    }

    private void ClearPossible() => tileMap.GetTiles().ForEach(x => x.Value.ToggleViableMarker(false));

    private void DrawPath(QuadTile target)
    {
        List<QuadTile> path = target.ChainToList();
        path.Reverse();

        ClearLines();

        Arrow.SetActive(true);
        Arrow.transform.position = target.PositionWithHeight();
        RotateMarkerBasedOnDirection(Arrow.transform,
            target.transform.position.ToVector2IntXZ(),
            target.pathFindingData.cameFrom.transform.position.ToVector2IntXZ());
        for (int i = 0; i < path.Count - 1; i++)
        {
            GameObject segment = GetLineSegmentFromPool();
            segment.transform.position = path[i].PositionWithHeight();
            segment.name = $"segment {i}.{1}";
            RotateMarkerBasedOnDirection(segment.transform,
                path[i].transform.position.ToVector2IntXZ(),
                path[i].pathFindingData.cameFrom.transform.position.ToVector2IntXZ());

            GameObject segment2 = GetLineSegmentFromPool();
            segment2.transform.position = path[i].PositionWithHeight();
            segment2.name = $"segment {i}.{2}";
            RotateMarkerBasedOnDirection(segment2.transform,
                path[i].transform.position.ToVector2IntXZ(),
                path[i + 1].transform.position.ToVector2IntXZ());
        }
    }

    private void ClearLines()
    {
        Arrow.SetActive(false);
        foreach (GameObject visibleLine in visibleLines)
        {
            StoreLineSegmentInPool(visibleLine);
        }
    }

    private void RotateMarkerBasedOnDirection(Transform marker, Vector2Int curr, Vector2Int prev)
    {
        Vector2Int dir = curr - prev;
        if (dir.Equals(Vector2Int.up)) marker.rotation = Quaternion.Euler(0, 180, 0);
        else if (dir.Equals(Vector2Int.right)) marker.rotation = Quaternion.Euler(0, 270, 0);
        else if (dir.Equals(Vector2Int.down)) marker.rotation = Quaternion.Euler(0, 0, 0);
        else if (dir.Equals(Vector2Int.left)) marker.rotation = Quaternion.Euler(0, 90, 0);
    }

    private void Move(List<QuadTile> path) => StartCoroutine(MoveAlongPath(path, moveParams.duration));

    private IEnumerator MoveAlongPath(List<QuadTile> path, float stepTime)
    {
        moving = true;
        target.ChangeAnimation("Walk");
        for (int i = 0; i < path.Count; i++)
        {
            yield return StartCoroutine(
                TakePathStep(path[i].PositionWithHeight(), stepTime));
        }

        ClearLines();
        target.ChangeAnimation("Idle");
        target.FlipCharacter(false);
        moving = false;
        returnAction.Invoke(true);
    }

    private IEnumerator TakePathStep(Vector3 target, float stepDuration)
    {
        Vector3 start = this.target.transform.position;
        FlipCharacterBasedOnDirection(target, start);

        bool jump = start.y != target.y;

        float progress;

        AnimationCurve curve = moveParams.floorBounce;

        if (jump)
        {
            stepDuration = jumpParams.duration;
            curve = jumpParams.floorBounce;
            this.target.ChangeAnimation("Jump");
        }


        tileMap.GetTile(target.ToVector2IntXZ()).PushDown(curve);

        for (float elapsed = 0; elapsed < stepDuration; elapsed += Time.deltaTime)
        {
            progress = elapsed / stepDuration;
            float yOffset =
                jump
                    ? jumpParams.verticalMovement.Evaluate(progress)
                    : 0;
            float horizontalProgress =
                jump
                    ? jumpParams.horizontalMovement.Evaluate(progress)
                    : moveParams.horizontalMovement.Evaluate(progress);

            this.target.transform.position =
                Vector3.Lerp(start, target, horizontalProgress) + Vector3.up * yOffset;
            yield return null;
        }

        this.target.transform.position = target;
    }

    [Serializable]
    private struct MoveParams
    {
        public float duration;
        public AnimationCurve horizontalMovement;
        public AnimationCurve verticalMovement;
        public AnimationCurve floorBounce;

        public MoveParams(float duration, AnimationCurve horizontalMovement,
            AnimationCurve verticalMovement, AnimationCurve floorBounce)
        {
            this.duration = duration;
            this.horizontalMovement = horizontalMovement;
            this.verticalMovement = verticalMovement;
            this.floorBounce = floorBounce;
        }
    }

    private void FlipCharacterBasedOnDirection(Vector3 target, Vector3 start)
    {
        Vector2Int direction = target.ToVector2IntXZ() - start.ToVector2IntXZ();
        if (direction.Equals(Vector2Int.up)) this.target.FlipCharacter(true);
        else if (direction.Equals(Vector2Int.right)) this.target.FlipCharacter(true);
        else if (direction.Equals(Vector2Int.down)) this.target.FlipCharacter(false);
        else if (direction.Equals(Vector2Int.left)) this.target.FlipCharacter(false);
    }

    public void StartMove(Action<bool> toggleWindow, Character character)
    {
        returnAction = toggleWindow;
        target = character;
        MarkPossible();
    }
}