using Lix.NightGame.Game;
using UnityEngine;
namespace Lix.NightGame.Enemy
{
    public class PoisonEnemy : BasicEnemy
    {
        public override void ContactCalcifer()
        {
            Debug.Log("POISON Contact");
            GameManager.S_Instance.DamageCalcifer(damage);
            base.ContactCalcifer();
        }

    }
}