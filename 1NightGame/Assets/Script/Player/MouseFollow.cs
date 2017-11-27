using UnityEngine;
namespace Lix.NightGame.Player
{
    public class MouseFollow : MonoBehaviour
    {
        private Vector3 mousePosition;
        public float moveSpeed = 10f;

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            if (Input.GetMouseButton(0))
            {
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = Vector2.MoveTowards(transform.position, mousePosition, moveSpeed);
            }

#endif
            ///TODO mobile control
        }
    }
}