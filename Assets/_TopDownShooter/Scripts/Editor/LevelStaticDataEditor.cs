using System.Linq;
using _current.Core.Logic;
using _current.Core.Logic.EnemySpawners;
using _current.Core.Logic.MissionPointSpawners;
using _current.StaticData;
using _current.StaticData.ScriptableObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        private const string PlayerInitialPoint = "PlayerInitialPoint";
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var levelData = (LevelStaticData)target;

            if (GUILayout.Button("Collect"))
            {
                levelData.ObjectiveSpawners = FindObjectsOfType<MissionPointMarker>()
                    .Select(x=> new ObjectiveSpawnerLevelData(x.GetComponent<UniqueId>().Id, x.missionPointType, x.transform.position))
                    .ToList();
                
                levelData.EnemySpawners = FindObjectsOfType<EnemySpawnMarker>()
                    .Select(x=> new EnemySpawnerLevelData(x.EnemyType, x.transform.position))
                    .ToList();

                levelData.LevelKey = SceneManager.GetActiveScene().name;
                
                levelData.PlayerSpawnPoint = GameObject.FindWithTag(PlayerInitialPoint).transform.position;
            }
            
            EditorUtility.SetDirty(target);
        }
    }
}