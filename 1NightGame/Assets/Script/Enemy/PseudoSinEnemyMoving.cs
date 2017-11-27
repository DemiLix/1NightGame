using Lix.NightGame.Game;
using UnityEngine;
namespace Lix.NightGame.Enemy
{
    public class PseudoSinEnemyMoving : BasicEnemyMoving
    {
        public override void Move()
        {
             transform.position = Vector3.MoveTowards(transform.position, GameManager.S_Instance.wizardTransform.position, (Mathf.Sin(Time.time) + 1.1f) * speed * Time.deltaTime);
        }
    } 
}