using UnityEngine;

namespace DefaultNamespace
{
    public class PickupItem : MonoBehaviour
    {
        public int xpAmount = 25;
        
        private void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                print("Picked up pickup");
                player.GainXp(xpAmount);
                Destroy(gameObject);
            }
        }
    }
}