using Lix.NightGame.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lix.NightGame.Enemy
{
    /// <summary>
    /// Spawn manager - used to get an unoccupied spawn point
    /// </summary>
    public class SpawnManager : Singleton<SpawnManager>
    {
        public GameObject prefab1;
        public GameObject prefab2;
        public GameObject prefab3;
        private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
        protected  void Start()
        {            
            LazyLoadSpawnPoints();
        }

        /// <summary>Lazy load the spawn points - this assumes that all spawn points are children of the SpawnManager
        /// </summary>
        private void LazyLoadSpawnPoints()
        {
            if (spawnPoints != null && spawnPoints.Count > 0)
            {
                return;
            }
            SpawnPoint[] foundSpawnPoints = GetComponentsInChildren<SpawnPoint>();
            spawnPoints.AddRange(foundSpawnPoints);
        }
        /// <summary>
		/// Gets index of a random empty spawn point
		/// </summary>
		/// <returns>The random empty spawn point index.</returns>
		public int GetRandomEmptySpawnPointIndex()
        {
            LazyLoadSpawnPoints();
            //Check for empty zones
            List<SpawnPoint> emptySpawnPoints = spawnPoints.Where(sp => sp.IsEmptyZone).ToList();

            //If no zones are empty, which is impossible if the setup is correct, then return the first spawnpoint in the list
            if (emptySpawnPoints.Count == 0)
            {
                return 0;
            }

            //Get random empty spawn point
            SpawnPoint emptySpawnPoint = emptySpawnPoints[UnityEngine.Random.Range(0, emptySpawnPoints.Count)];

            //Mark it as dirty
            emptySpawnPoint.SetDirty();

            //return the index of this spawn point
            return spawnPoints.IndexOf(emptySpawnPoint);
        }
        /// <summary>
        /// Get a random empty spawn point
        /// </summary>
        /// <returns>The random empty spawn point.</returns>
        public SpawnPoint GetRandomEmptySpawnPoint()
        {
            return GetSpawnPointByIndex(GetRandomEmptySpawnPointIndex());
        }
        /// <summary>
        /// Get a spawn point by his index
        /// </summary>
        /// <returns>The spawn point.</returns>
        public SpawnPoint GetSpawnPointByIndex(int i)
        {
            LazyLoadSpawnPoints();
            spawnPoints[i].SetDirty();
            return spawnPoints[i];
        }

        public  void SpawnEnemy(SpawnPoint spawnPoint, string enemyType)
        {
            switch (enemyType)
            {
                case Consts.ENEMY_1_TYPE:
                    ObjectPool.Spawn(prefab1, spawnPoint.transform);
                    break;
                case Consts.ENEMY_2_TYPE:
                    ObjectPool.Spawn(prefab2, spawnPoint.transform);
                    break;
                case Consts.ENEMY_3_TYPE:
                    ObjectPool.Spawn(prefab3, spawnPoint.transform);
                    break;
            }
        }
    }
}