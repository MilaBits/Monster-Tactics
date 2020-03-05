using Boo.Lang;
using Characters;
using UnityEngine;

namespace Gameplay
{
    public class GameUIManager : MonoBehaviour
    {
        private Character target;

        [SerializeField]
        private ActionWindow actionWindow = default;

        public ActionWindow ActionWindow => actionWindow;

        [SerializeField]
        private StatsBox statsBox = default;

        public StatsBox StatsBox => statsBox;

        [SerializeField]
        private TurnList turnList = default;

        public TurnList TurnList => turnList;

        public void ChangeTarget(Character newTarget, List<Character> turnOrder)
        {
            target = newTarget;
        }
    }
}