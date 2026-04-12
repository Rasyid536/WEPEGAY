using UnityEngine;

public class Pest : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("AntiPest"))
        {
            Destroy(this.gameObject);
        }

        if (collider.CompareTag("Pest"))
        {
            Pest otherPest = collider.GetComponent<Pest>();

            if (otherPest == null) return;

            if (this.GetInstanceID() > otherPest.GetInstanceID())
            {
                Debug.Log("Pest collide");
                Destroy(this.gameObject);
            }
        }
    }



}