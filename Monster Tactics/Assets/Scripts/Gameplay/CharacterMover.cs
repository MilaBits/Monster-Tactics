using System.Collections;
using System.Collections.Generic;
using System.IO;
using Level;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities;

public class CharacterMover : MonoBehaviour
{
    [SerializeField, InlineEditor]
    private Character Character;

    private QuadTileMap tileMap;

    [SerializeField]
    private AnimationCurve jumpCurve;

    void Start() => tileMap = FindObjectOfType<QuadTileMap>();

    [Button]
    private void MarkPossible()
    {
        foreach (QuadTile tile in tileMap.GetTilesInRange(
            tileMap.GetTile(Character.transform.position.ToVector2IntXZ()), Character.Data().move,
            Character.Data().stepLayerLimit,
            Character.Data().useRoughness))
        {
            tile.ToggleViableMarker(true);
        }
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetButtonDown("Fire1"))
        {
            if (Physics.Raycast(ray, out hit, 100, LayerMask.NameToLayer("Viable Marker")))
            {
                List<QuadTile> path = hit.transform.GetComponentInParent<QuadTile>().ChainToList();
                path.Reverse();
                Move(path);
            }
        }
    }

    private void Move(List<QuadTile> path)
    {
        StartCoroutine(MoveAlongPath(path, .5f));
    }

    private IEnumerator MoveAlongPath(List<QuadTile> path, float stepTime)
    {
        for (int i = 0; i < path.Count; i++)
        {
            yield return StartCoroutine(
                TakePathStep(path[i].transform.position + Vector3.up * path[i].height, stepTime));
        }

        MarkPossible();
    }

    private IEnumerator TakePathStep(Vector3 target, float stepTime)
    {
        Vector3 start = Character.transform.position;
        bool jump = start.y != target.y;

        float progress;

        if (jump)
        {
            yield return new WaitForSeconds(stepTime * .33f);
            stepTime *= .66f;
        }

        for (float elapsed = 0; elapsed < stepTime; elapsed += Time.deltaTime)
        {
            progress = elapsed / stepTime;
            float yOffset = jump ? jumpCurve.Evaluate(progress) : 0;

            Character.transform.position = Vector3.Lerp(start, target, progress) + Vector3.up * yOffset;
            yield return null;
        }

        Character.transform.position = target;
    }
}