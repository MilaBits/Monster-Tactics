using DefaultNamespace;
using Sirenix.OdinInspector;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField, InlineEditor]
    private CharacterData characterData;

    public CharacterData GetCharacterData() => characterData;
}