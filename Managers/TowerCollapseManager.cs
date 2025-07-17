namespace FifthModJam
{
    public class TowerCollapseManager : AbstractManager<TowerCollapseManager>
    {
        // Neat way to share about the tower collapse.
        public delegate void TowerCollapseEvent();
        public TowerCollapseEvent OnTowerCollapse;

        private void Start()
        {
            FifthModJam.WriteLineReady("TowerCollapseManager");
        }

        public void CollapseTower()
        {
            OnTowerCollapse?.Invoke(); // Event to let the towerSmall update its status
        }
    }
}
