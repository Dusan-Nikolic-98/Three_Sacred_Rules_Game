using UnityEngine;

public class Ghost : MonoBehaviour
{
    public GameObject deathEffectPref;

    public void PlayDeathEffectAndDestroy()
    {
        if (deathEffectPref != null) { 
            Instantiate(deathEffectPref, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
