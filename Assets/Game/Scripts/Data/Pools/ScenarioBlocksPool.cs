using System.Collections.Generic;
using UnityEngine;

namespace SC
{
    [CreateAssetMenu(fileName = "ScenarioBlocksPool", menuName = "Data/Pools/ScenarioBlocksPool")]
    public class ScenarioBlocksPool : ScriptableObject, ILoadablePool
    {
        [SerializeField] private string _dataPath;

        private Dictionary<int, ScenarioPoolData> _scenarioBlocks;

        public ScenarioPoolData GetScenarioBlock(int id)
        {
            if (_scenarioBlocks.TryGetValue(id, out var dataValue))
            {
                return dataValue;
            }

            return null;
        }

        public void Load()
        {
            if (string.IsNullOrEmpty(_dataPath))
            {
                return;
            }

            _scenarioBlocks = new Dictionary<int, ScenarioPoolData>();
            var assets = Resources.LoadAll<ScenarioPoolData>(_dataPath);

            foreach (var a in assets)
            {
                _scenarioBlocks.Add(a.id, a);
            }

            Debug.Log($"[{name}] is loaded!");
        }
    }
}
