using UnityEngine;
namespace Lix.NightGame.Enemy
{
    /// <summary>
    /// Spawn point - has a collider to check if any enemy is in the zone
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class SpawnPoint : MonoBehaviour
    {       
        ///<summary>if multiple respawns occurs simultaneously then there will be no firing on trigger functionality to ensure zones are marked as occupied, hence the need for a dirty variable
        /// </summary>
        private bool m_IsDirty = false;
        ///<summary>Is the zone empty and not marked as dirty
        /// </summary>
        public bool IsEmptyZone
        {
            get { return !m_IsDirty; }
        }
        public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag.Equals(Consts.TAG_ENEMY)){
                m_IsDirty = false;
            }
        }
        /// <summary>
        /// Used to set the spawn point to dirty to prevent simultaneous spawns from occurring at the same point 
        /// </summary>
        public void SetDirty()
        {
            m_IsDirty = true;
        }
        void OnDrawGizmos()
        {
            if (m_IsDirty)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.green;
            }
            Gizmos.DrawSphere(transform.position, 0.3f);
        }
    }
}