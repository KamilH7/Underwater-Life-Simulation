using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI
{
    public class GameObjectCountGraph : MonoBehaviour
    {
        #region Public Properties

        public int MaxValue { get; private set; }
        public int MinValue { get; private set; }

        #endregion

        [field: SerializeField]
        private RectTransform GraphContainer { get; set; }
        [field: SerializeField]
        private DataPoint DataPointPrefab { get; set; }
        [field: SerializeField]
        private List<GameObjectCountGraph> OverlayedGraphs { get; set; }

        [field: SerializeField]
        private DataLabel MaxValueLabel { get; set; }
        [field: SerializeField]
        private DataLabel MinValueLabel { get; set; }
        [field: SerializeField]
        private DataLabel CurrentValueLabel { get; set; }
        
        [field: Header("Data"), SerializeField, ReadOnly]
        private List<DataPoint> CurrentData { get; set; } = new();

        private Vector2 GraphSize => GraphContainer.sizeDelta;

        #region Public Methods

        private bool IsFirstData { get; set; } = true;


        public void AddData(int value)
        {
            UpdateCurrentValue(value);
            
            if (value > MaxValue || IsFirstData)
            {
                UpdateMaxValue(value);
            }

            if (value < MinValue || IsFirstData )
            {
                UpdateMinValue(value);
            }

            AddNewDataPoint(value);
            PositionCurrentDataOnGraph();

            IsFirstData = false;
        }

        #endregion

        #region Private Methods

        private void AddNewDataPoint(int value)
        {
            DataPoint newDataPoint = Instantiate(DataPointPrefab);
            newDataPoint.Initialize(value, GraphContainer);
            CurrentData.Add(newDataPoint);
        }

        private void PositionCurrentDataOnGraph()
        {
            for (int i = 0; i < CurrentData.Count; i++)
            {
                float xCellSize = GraphSize.x / CurrentData.Count;
                float xPosition = xCellSize * (i + 1) - xCellSize / 2;

                float yCellSize = GraphSize.y / GetOverlayedMaxValue();
                float yPosition = yCellSize * CurrentData[i].Value - yCellSize / 2;

                CurrentData[i].SetPosition(new Vector2(xPosition, yPosition));
            }
        }

        private int GetOverlayedMaxValue()
        {
            int maxValue = MaxValue;

            foreach (GameObjectCountGraph graph in OverlayedGraphs)
            {
                if (graph.MaxValue > maxValue)
                {
                    maxValue = graph.MaxValue;
                }
            }

            return maxValue;
        }

        private void UpdateMaxValue(int value)
        {
            MaxValue = value;
            MaxValueLabel.UpdateData(value);
        }

        private void UpdateMinValue(int value)
        {
            MinValue = value;
            MinValueLabel.UpdateData(value);
        }

        private void UpdateCurrentValue(int value)
        {
            CurrentValueLabel.UpdateData(value);
        }
        
        #endregion
    }
}