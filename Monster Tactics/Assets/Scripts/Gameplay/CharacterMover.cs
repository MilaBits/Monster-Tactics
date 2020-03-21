using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Characters;
using Gameplay;
using Level;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Utilities;

public class CharacterMover : MonoBehaviour
{
    [SerializeField, InlineEditor]
    private Character target = default;

    private QuadTile oldTarget;

    private QuadTileMap tileMap;

    [Space]
    [SerializeField]
    private LineSegment ArrowPrefab = default;

    [SerializeField]
    private LineSegment LinePrefab = default;

    private Queue<LineSegment> LinePool = new Queue<LineSegment>();
    private List<LineSegment> visibleLines = new List<LineSegment>();
    private LineSegment Arrow;

    private List<QuadTile> oldPath;
    public bool StopUpdatingPath = true;

    private LineSegment GetLineSegmentFromPool()
    {
        if (LinePool.Count < 1) LinePool.Enqueue(Instantiate(LinePrefab));
        LineSegment lineSegment = LinePool.Dequeue();
        lineSegment.gameObject.SetActive(true);
        visibleLines.Add(lineSegment);
        return lineSegment;
    }

    private void StoreLineSegmentInPool(LineSegment lineSegment)
    {
        lineSegment.name = "pooledSegment";
        lineSegment.gameObject.SetActive(false);
        LinePool.Enqueue(lineSegment);
    }

    void Start()
    {
        Arrow = Instantiate(ArrowPrefab);
        Arrow.gameObject.SetActive(false);
        tileMap = FindObjectOfType<QuadTileMap>();
    }

    [Button]
    private void MarkPossible()
    {
        foreach (QuadTile tile in tileMap.GetTilesInRange(
            tileMap.GetTile(target.transform.position.ToVector2IntXZ()), target.Data.move,
            target.Data.stepLayerLimit,
            target.Data.useRoughness))
        {
            tile.ToggleViableMarker(true);
        }
    }

    private void Update()
    {
        if (StopUpdatingPath) return;

        QuadTile target = QuadTileMap.GetTarget(LayerMask.GetMask("Viable Marker"));
        if (target)
        {
            DrawPath(target);
        }
    }

    private void DrawPath(QuadTile target)
    {
        if (target == oldTarget) return;
        oldTarget = target;

        List<QuadTile> path = target.Path();

        Clear(PathfindingClear.Path);

        Arrow.gameObject.SetActive(true);
        Arrow.transform.position = path[path.Count - 1].PositionWithHeight;
        Arrow.transform.SetParent(path[path.Count - 1].transform);
        Arrow.UpdateSegment(path[path.Count - 1].pathFindingData.cameFrom.PositionWithHeight, Vector3.zero);

        for (int i = 0; i < path.Count - 1; i++)
        {
            LineSegment segment = GetLineSegmentFromPool();

            segment.transform.position = path[i].PositionWithHeight;
            segment.transform.SetParent(path[i].transform);
            segment.name = "Segment " + i;
            segment.UpdateSegment(path[i].pathFindingData.cameFrom.PositionWithHeight, path[i + 1].PositionWithHeight);
        }
    }

    public void Clear(PathfindingClear clear)
    {
        switch (clear)
        {
            case PathfindingClear.Path:
                ClearPath();
                break;
            case PathfindingClear.Possible:
                tileMap.ResetPathfindingData();
                break;
            case PathfindingClear.Both:
                ClearPath();
                tileMap.ResetPathfindingData();
                break;
        }
    }

    private void ClearPath()
    {
        Arrow.gameObject.SetActive(false);
        for (int i = visibleLines.Count - 1; i >= 0; i--)
        {
            LineSegment visibleLine = visibleLines[i];
            visibleLines.Remove(visibleLine);
            StoreLineSegmentInPool(visibleLine);
        }
    }

    public IEnumerator Move(List<QuadTile> path, MoveParams moveParams, MoveParams jumpParams)
    {
        Clear(PathfindingClear.Possible);

        target.ChangeAnimation(moveParams.animatorTrigger);
        for (int i = 0; i < path.Count; i++)
        {
            yield return StartCoroutine(
                TakePathStep(path[i].PositionWithHeight, moveParams, jumpParams));
        }

        target.ChangeAnimation("Idle");
        target.ResetFlip();
        StopUpdatingPath = true;
    }

    private IEnumerator TakePathStep(Vector3 target, MoveParams moveParams, MoveParams jumpParams)
    {
        Vector3 start = this.target.transform.position;

        // flip character the right way
        Vector3 localDirection =
            Camera.main.transform.InverseTransformDirection(this.target.transform.position - target).normalized;
        this.target.FlipCharacter(localDirection.x < 0);

        this.target.ChangeAnimation(moveParams.animatorTrigger);

        bool jump = start.y != target.y;
        MoveParams usedParams = jump ? jumpParams : moveParams;
        if (jump) this.target.ChangeAnimation(jumpParams.animatorTrigger);

        // Make stepped on tile bounce
        tileMap.GetTile(target.ToVector2IntXZ()).PushDown(usedParams.floorBounce);

        for (float elapsed = 0; elapsed < usedParams.duration; elapsed += Time.deltaTime)
        {
            float progress = elapsed / usedParams.duration;
            float yOffset = usedParams.verticalMovement.Evaluate(progress);
            float horizontalProgress = usedParams.horizontalMovement.Evaluate(progress);

            this.target.transform.position = Vector3.Lerp(start, target, horizontalProgress) + Vector3.up * yOffset;
            yield return null;
        }

        this.target.transform.position = target;
    }

    public void ShowPossible(Character character)
    {
        StopUpdatingPath = false;
        target = character;
        MarkPossible();
    }
}