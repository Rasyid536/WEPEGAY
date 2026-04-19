using UnityEngine;

public class Pest : MonoBehaviour
{

    void Start()
    {
        GlobalVariable.pestAmount += 1;
    }
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

    void OnDestroy() {GlobalVariable.pestAmount -= 1;}

}