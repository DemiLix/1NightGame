using Lix.NightGame.Game;
using UnityEngine;

namespace Lix.NightGame.Enemy
{
    public enum DeathEnemyType
    {
        SIMPLE, FIRE
    }
    [DisallowMultipleComponent]
    public class BasicEnemy : MonoBehaviour
    {
        [SerializeField]
        protected int damage;
        public float hp;
        [SerializeField]
        private BasicEnemyMoving moving;
        [SerializeField]
        private bool needRotateForDirection;
        public bool NeedRotateForDirection
        {
            get { return needRotateForDirection; }
        }
        protected bool IsDead;



        public virtual void Awake()
        {
            moving = GetComponent<BasicEnemyMoving>();
            InitMoving();
        }

        public void InitMoving()
        {
            if (moving != null)
            {
                moving.Init(this);
            }
        }
        public virtual void ContactWizard()
        {
            Debug.Log("ContactWizard");
            IsDead = true;
            GameManager.S_Instance.DamageWizard(transform, damage);
            ShowDeathAnimation(DeathEnemyType.SIMPLE);
            ObjectPool.Recycle(this);
        }

        public virtual void ContactCalcifer()
        {
            Debug.Log("Base ContactCalcifer");
            IsDead = true;
            ShowDeathAnimation(DeathEnemyType.FIRE);
            ObjectPool.Recycle(this);

        }

        public virtual void ShowDeathAnimation(DeathEnemyType type)
        {
            GameManager.S_Instance.ShowDeathAnimation(type, this.transform.position);
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (!IsDead)
            {
                switch (collision.tag)
                {
                    case Consts.TAG_PLAYER:
                        ContactWizard();
                        break;
                    case Consts.TAG_CALCIFER:
                        ContactCalcifer();
                        break;
                }
            }
        }
    }
}
