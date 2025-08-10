using UnityEngine;

public class ScrollPickup : MonoBehaviour
{
    public GameObject pickupEffectPref;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            string scrollType = gameObject.tag;
            Debug.Log("Collected scroll: " + scrollType);

            //triger sta se desava s playerom kad se pokupi

            var abilityComponent = other.GetComponent<PlayerAbillities>();
            if (abilityComponent != null) 
            {
                abilityComponent.UnlockAbility(scrollType);
            }

            //goustuj goustove
            if (GhostSpawner.Instance != null) {
                GhostSpawner.Instance.DestroyAllGhosts();
            }

            //eff
            if (pickupEffectPref != null) 
            {
                Instantiate(pickupEffectPref, transform.position, Quaternion.identity);
            }

            //ubi scroll
            Destroy(gameObject);
        }

    }

}
