using Lix.NightGame.Enemy;
using Lix.NightGame.Utilities;
using System;
using UnityEngine;

namespace Lix.NightGame.Game
{/// <summary>Game manager - handles game state and player state
    public class GameManager : Singleton<GameManager>
    {
        public event Action<int> UpdateHpWizardDelegate;
        public event Action<int> UpdateHpCalciferDelegate;
        public Transform wizardTransform;

        public enum GAME_STATE
        {
            INIT, PAUSE, GAMELOOP, WIN, LOSE
        }
        private int hpWizard = 5;
        public int HpWizard
        {
            get { return hpWizard; }
        }

        private int hpCalcifer = 5;
        public int HpCalcifer
        {
            get { return hpCalcifer; }
        }
        private GAME_STATE state;
        public GAME_STATE State
        {
            get { return state; }
        }

        private GameObject playerDamage_Blood;
        private GameObject deathEnemy;

        // Use this for initialization
        void Start()
        {
            state = GAME_STATE.INIT;
            InitPlayerDamagePool();
            InitDeathEnemyPool();
        }
        /// <summary>Create pool objects with animations of wizard's damage   </summary>
        private void InitPlayerDamagePool()
        {
            playerDamage_Blood = Resources.Load<GameObject>(Consts.ANIMATION_WIZARD_BLOOD_PREFAB);
            ObjectPool.CreatePool(playerDamage_Blood, 5);
        }
        /// <summary>Create pool objects with animations of enemy's death</summary>
        private void InitDeathEnemyPool()
        {
            deathEnemy = Resources.Load<GameObject>(Consts.ANIMATION_ENEMY_DEATH_PREFAB);
            ObjectPool.CreatePool(deathEnemy, 10);
        }

        public void ShowDeathAnimation(DeathEnemyType type, Vector3 position)
        {
            switch (type)
            {
                case DeathEnemyType.FIRE:
                    ObjectPool.Spawn(deathEnemy, position).GetComponent<Animator>().Play(Consts.ANIMATION_ENEMY_DEATH_FIRE);
                    break;
                case DeathEnemyType.SIMPLE:
                    ObjectPool.Spawn(deathEnemy, position).GetComponent<Animator>().Play(Consts.ANIMATION_ENEMY_DEATH_SIMPLE);
                    break;
            }
        }


        private void Update()
        {
            switch (state)
            {
                case GAME_STATE.INIT:
                    WaveManager.S_Instance.StartWave();
                    state = GAME_STATE.GAMELOOP;
                    break;
            }
        }

        public void DamageWizard(Transform posHit, int value)
        {
            hpWizard -= value;
            if (UpdateHpWizardDelegate != null)
            {
                UpdateHpWizardDelegate(hpWizard);
            }
            if (hpWizard < 0)
            {
                state = GAME_STATE.LOSE;
                Time.timeScale = 0;
                Debug.Log("LOOOOOOSE");
            }
            //Showing animation of wizard's damage
            Vector3 moveDirection = wizardTransform.position - posHit.position;
            if (moveDirection != Vector3.zero)
            {
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg + 135;
                ObjectPool.Spawn(playerDamage_Blood, null, posHit.position, Quaternion.AngleAxis(angle, Vector3.forward));
            }

        }

        public void DamageCalcifer(int value)
        {
            hpCalcifer -= value;
            if (UpdateHpCalciferDelegate != null)
            {
                UpdateHpCalciferDelegate(hpCalcifer);
            }
            if (hpCalcifer < 0)
            {
                state = GAME_STATE.LOSE;
                Time.timeScale = 0;
                Debug.Log("LOOOOOOSE! Calci is dead :(");
            }
        }
    }
}