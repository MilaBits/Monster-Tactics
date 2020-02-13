using System.Collections.Generic;
using System.Linq;
using Level;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities;

public class CharacterMover : MonoBehaviour
{
    [SerializeField]
    private Character Character;

    private QuadTileMap tileMap;

    void Start()
    {
        tileMap = FindObjectOfType<QuadTileMap>();
    }

    [Button]
    private void MarkPossible()
    {
        foreach (QuadTile tile in tileMap.GetTilesInRange(
            tileMap.GetTile(Character.transform.position.ToVector2IntXZ()), Character.GetCharacterData().move, false))
        {
            tile.ToggleViableMarker(true);
        }
    }

}