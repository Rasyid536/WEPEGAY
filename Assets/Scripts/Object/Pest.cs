using UnityEngine;

public class Pest : MonoBehaviour
{
    [SerializeField] Sprite snake, rat, monkey;
    private SpriteRenderer thisSprite;

    void Awake()
    {
        thisSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        int rand = Random.Range(1, 4);

        if(rand == 1)
            thisSprite.sprite = snake;
        else if(rand == 2 )
            thisSprite.sprite = rat;
        else if(rand < 5 && rand > 2)
            thisSprite.sprite = monkey;        

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