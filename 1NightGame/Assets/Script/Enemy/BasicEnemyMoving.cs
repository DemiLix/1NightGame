using Lix.NightGame.Game;
using UnityEngine;
namespace Lix.NightGame.Enemy
{
    [DisallowMultipleComponent]
    public class BasicEnemyMoving : MonoBehaviour
    {
        private BasicEnemy obj;
        public float speed;
        public void Init(BasicEnemy enemy)
        {
            this.obj = enemy;       
        }
        //We need select direction of travel after spawned from pool if needed 
        public void OnEnable()
        {
            if (obj!=null&&obj.NeedRotateForDirection) 
            {
                Vector3 moveDirection = transform.position - GameManager.S_Instance.wizardTransform.position;
                if (moveDirection != Vector3.zero)
                {
                    float angle =  Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg-90;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); 
                }
            }
        }
        /// <summary> Basic moving </summary>
        public virtual void Move()
        {
            transform.position = Vector3.MoveTowards(transform.position, GameManager.S_Instance.wizardTransform.position, speed * Time.deltaTime);
        }
        
        // Update is called once per frame
        void Update()
        {
            Move();
        }
    }
}
