using UnityEngine;
namespace Lix.NightGame.Enemy
{
    public class NewEnemy : BasicEnemy
    {
        public override void ContactCalcifer()
        {
            Debug.Log("override ContactCalcifer");
            base.ContactCalcifer();
        }
    

    }
}
