using Lix.NightGame.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Lix.NightGame.Enemy
{
    /// <summary>
    ///  WaveManager loads and issues waves data
    /// </summary>
    public class WaveManager : Singleton<WaveManager>
    {
        public enum LoadWavesFrom
        {
            JSON, CODE
        }
        private List<Wave> waves;
        private int currentWave = -1;
        /// <summary> Delay before starting first wave</summary>
        public float firstWaveDelay = 1f;
        public LoadWavesFrom from = LoadWavesFrom.JSON;
        private WaveData gameData;

        public void Start()
        {
            switch (from)
            {
                case LoadWavesFrom.CODE:
                    LoadWavesFromCode();
                    break;
                case LoadWavesFrom.JSON:
                    if (!LoadWavesFromJson())
                    {
                        LoadWavesFromCode();
                    }
                    break;
                default:
                    LoadWavesFromCode();
                    break;
            }
        }

        private bool LoadWavesFromJson()
        {
            string filePath = Application.dataPath + Consts.WAVES_JSON_PATHNAME;
            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                try
                {
                    gameData = JsonUtility.FromJson<WaveData>(dataAsJson);
                    waves = new List<Wave>(gameData.Waves);
                }
                catch (ArgumentException exc)
                {

                    Debug.LogException(exc, this);
                    //Or need break game?
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        private void LoadWavesFromCode()
        {
            waves = new List<Wave>
            {
                new Wave(){
                    steps = new List<WaveStep>()
                    {
                new WaveStep() {delay=1f,text="1 wave step 1",
                    spawners = new SpawnPair[]{
                        new SpawnPair(){index=Consts.INDEX_RANDOM_SPAWN_POINT,name=Consts.ENEMY_1_TYPE},
                        new SpawnPair(){index=Consts.INDEX_RANDOM_SPAWN_POINT,name=Consts.ENEMY_2_TYPE },
                        new SpawnPair(){index=Consts.INDEX_RANDOM_SPAWN_POINT,name=Consts.ENEMY_3_TYPE}
                    }
                }
                ,
                new WaveStep() {delay=2f,text="1 wave step 2" ,
                    spawners = new SpawnPair[]{
                        new SpawnPair(){index=0,name=Consts.ENEMY_3_TYPE }
                    }
                }
                }
                },
                new Wave(){
                    steps = new List<WaveStep>(){
                        new WaveStep() {delay=5f,text="2 wave step 1" },
                        new WaveStep() {delay=1f,text="2 wave step 2" },
                        new WaveStep() {delay=1f,text="2 wave step 3" }
                    }
                }
            };
        }

        public void StartWave()
        {
            if (WaveExist())
            {
                currentWave = 0;
                StartCoroutine(SpawnWaves());
            }
        }

        private IEnumerator SpawnWaves()
        {
            yield return new WaitForSeconds(firstWaveDelay);
            //while we have unfinished waves
            while (currentWave < waves.Count)
            {
                ///if wave is not finished
                if (!CheckFinishedCurrentWave())
                {
                    var step = waves[currentWave].GetCurrentWaveStep();
                    if (step != null)
                    {
                        yield return new WaitForSeconds(step.delay);
                        var spawners = step.spawners;
                        if (spawners != null)
                        {
                            //spawning all enemy for concrete step of wave
                            foreach (var spawn in spawners)
                            {
                                SpawnPoint spawnPoint;
                                if (spawn.index == Consts.INDEX_RANDOM_SPAWN_POINT)
                                {
                                    spawnPoint = SpawnManager.S_Instance.GetRandomEmptySpawnPoint();
                                }
                                else
                                {
                                    spawnPoint = SpawnManager.S_Instance.GetSpawnPointByIndex(spawn.index);
                                }
                                if (spawnPoint != null)
                                {
                                    SpawnManager.S_Instance.SpawnEnemy(spawnPoint, spawn.name);
                                }
                                else
                                {
                                    throw new Exception("Not empty spawn point");
                                }
                            }
                        }
                        Debug.Log(step.text);
                    }
                    //next step of wave
                    waves[currentWave].currentStep++;
                }
                else
                {
                    SelectNextWave();
                }
            }
        }

        private void SelectNextWave()
        {
            currentWave++;
        }

        private bool WaveExist()
        {
            return waves != null && waves.Count > 0;
        }

        private bool CheckFinishedCurrentWave()
        {
            var wave = waves[currentWave];
            return wave.IsFinished();
        }
    }
    [Serializable]
    public class SpawnPair
    {
        public int index;
        public string name;
    }
    [Serializable]
    public class WaveStep
    {
        public float delay;
        public string text;
        /// <summary> index=index spawnPoint(Consts.INDEX_RANDOM_SPAWN_POINT(-1) is Random point), name=spawned object name 
        /// </summary>
        [SerializeField]
        public SpawnPair[] spawners;
    }
    [Serializable]
    public class Wave
    {
        public List<WaveStep> steps;
        [HideInInspector]
        public int currentStep = 0;
        public WaveStep GetCurrentWaveStep()
        {
            if (steps != null && steps.Count > 0 && currentStep < steps.Count)
            {
                return steps[currentStep];
            }
            else
            {
                return null;
            }
        }
        public bool IsFinished()
        {
            if (steps != null && steps.Count > 0)
            {
                return currentStep > steps.Count;
            }
            else return true;
        }
    }
    [System.Serializable]
    public class WaveData
    {
        public string name;
        public Wave[] Waves;
    }
}