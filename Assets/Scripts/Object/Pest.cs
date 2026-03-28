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

            // 🔥 hanya satu yang dihancurkan
            if (this.GetInstanceID() > otherPest.GetInstanceID())
            {
                Destroy(this.gameObject);
            }
        }
    }



}