namespace FifthModJam
{
    public class TowerCollapseManager : AbstractManager<TowerCollapseManager>
    {
        // Neat way to share about the tower collapse.
        public delegate void TowerCollapseEvent();
        public TowerCollapseEvent OnTowerCollapse;

        private void Start()
        {
            if (!FifthModJam.Instance.IsInJamFiveSystem()) return;

            FifthModJam.WriteLineReady("TowerCollapseManager");
        }

        public void CollapseTower()
        {
            Locator.GetShipLogManager().RevealFact("COSMICCURATORS_INHABITANT_TOWER_COLLAPSE_R");
            OnTowerCollapse?.Invoke(); // Event to let the towers update their status
        }
    }
}
