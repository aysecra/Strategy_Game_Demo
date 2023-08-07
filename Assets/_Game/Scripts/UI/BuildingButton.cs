using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PanteonDemo
{
    /// <summary>
    /// Event is triggered when building is spawned and placement is not completed 
    /// </summary>
    public struct BuildingSpawnEvent
    {
        public BuildingController SpawnedBuilding { get; private set; }
        public List<GridsCell> FirstSpawnAreaList { get; private set; }
        public Vector3 FirstSpawnPosition { get; private set; }

        public BuildingSpawnEvent(BuildingController buildingController, Vector3 spawnPosition,
            List<GridsCell> cellList)
        {
            SpawnedBuilding = buildingController;
            FirstSpawnAreaList = cellList;
            FirstSpawnPosition = spawnPosition;
        }
    }

    public class BuildingButton : PoolableObject
        , EventListener<BuildingSpawnEvent>
        , EventListener<BuildingPlaceEvent>
    {
        [Header("Core Elements")] [SerializeField]
        private Image _imgElement;

        [SerializeField] private TMPro.TextMeshProUGUI _textTitle;
        [SerializeField] private TMPro.TextMeshProUGUI _textHealth;
        [SerializeField] private Button _button;

        private Building _currentBuildingData;
        private bool _isActive = true;

        public void SetElementValue(Building buildingData)
        {
            _currentBuildingData = buildingData;
            _imgElement.sprite = buildingData.Image;
            _textTitle.text = buildingData.Name;
            _textHealth.text = buildingData.Health.ToString();
        }

        public void OnButtonClicked()
        {
            uint rowCount = _currentBuildingData.Row;
            uint columnCount = _currentBuildingData.Column;

            List<GridsCell> cellList =
                GridSystem.Instance.GetEmptyArea(rowCount, columnCount);

            if (_isActive && cellList != null && cellList.Count > 0)
            {
                float cellSize = GridSystem.Instance.CellSize.x;
                Vector3 changingDist =
                    new Vector3(cellSize * (int) columnCount * .5f, cellSize * (int) rowCount * .5f, 0);
                Vector3 downLeftCorner = cellList[0].transform.position - new Vector3(cellSize, cellSize, 0) * .5f;
                Vector3 position = downLeftCorner + changingDist;
                BuildingController building =
                    SharedLevelManager.Instance.SpawnElement<BuildingController>(_currentBuildingData.Name, position);

                building.SetFirstPositionWithDownLeftCell((int) cellList[0].Row, (int) cellList[0].Row);
                EventManager.TriggerEvent(new BuildingSpawnEvent(building, position, cellList));
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            EventManager.EventStartListening<BuildingSpawnEvent>(this);
            EventManager.EventStartListening<BuildingPlaceEvent>(this);
        }

        private void OnDisable()
        {
            EventManager.EventStopListening<BuildingSpawnEvent>(this);
            EventManager.EventStopListening<BuildingPlaceEvent>(this);
        }

        public void OnEventTrigger(BuildingSpawnEvent currentEvent)
        {
            _isActive = false;
        }

        public void OnEventTrigger(BuildingPlaceEvent currentEvent)
        {
            _isActive = true;
        }
    }
}