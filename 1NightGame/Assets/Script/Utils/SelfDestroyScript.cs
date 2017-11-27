using UnityEngine;

public class SelfDestroyScript : MonoBehaviour {

	 public void SelfDestroy()
    {
        ObjectPool.Recycle(this);
    }
} 
