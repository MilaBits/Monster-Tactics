using Characters;
using Level;
using UnityEngine;
using Utilities;

namespace Gameplay
{
    public class CharacterAttacker : MonoBehaviour
    {
        private Character activeCharacter;
        private QuadTileMap tileMap;

        private void Start()
        {
            tileMap = FindObjectOfType<QuadTileMap>();
        }

        public void Clear() => tileMap.ResetPathfindingData();

        public void MarkPossible()
        {
            float heightLimit =
                activeCharacter.Data.AttackRange > 1
                    ? 20
                    : activeCharacter.Data.stepLayerLimit; // Should vertical range be unlimited or not
            foreach (QuadTile tile in tileMap.GetTilesInRange(
                tileMap.GetTile(activeCharacter.transform.position.ToVector2IntXZ()),
                activeCharacter.Data.AttackRange, heightLimit, false))
            {
                tile.ToggleViableMarker(true);
            }
        }

        public void Attack(QuadTile target)
        {
            Debug.Log($"BLAM!! {target.transform.position.ToVector2IntXZ()} hit");
        }

        public void ShowPossible(Character character)
        {
            activeCharacter = character;
            MarkPossible();
        }
    }
}