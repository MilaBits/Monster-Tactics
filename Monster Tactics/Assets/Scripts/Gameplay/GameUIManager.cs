using Boo.Lang;
using Characters;
using UnityEngine;

namespace Gameplay
{
    public class GameUIManager : MonoBehaviour
    {
        private Character target;

        [SerializeField]
        private ActionWindow actionWindow;

        public ActionWindow ActionWindow => actionWindow;

        [SerializeField]
        private StatsBox statsBox;

        public StatsBox StatsBox => statsBox;

        [SerializeField]
        private TurnList turnList;

        public TurnList TurnList => turnList;

        // private TurnManager turnManager;

        private void Start()
        {
            // turnManager = FindObjectOfType<TurnManager>();
            // if (!turnManager) Debug.LogWarning("No Turn Manager present in scene!");
        }

        public void ChangeTarget(Character newTarget, List<Character> turnOrder)
        {
            target = newTarget;
        }
    }
}