using UnityEngine;

public class GhostFollow : MonoBehaviour
{
    //public Transform player;
    public float speed = 1f;
    private Transform target;


    void Update()
    {

        target = GhostTargetManager.Instance?.GetTarget();

        if (target != null) {
            Vector3 dir;

            //ifelse za da li se plase ili jok
            if (GhostTargetManager.Instance.AreGhostsAfraid())
            {
                dir = (transform.position - target.position).normalized; //od
            }
            else 
            {
                dir = (target.position - transform.position).normalized; //ka
            }

            transform.position += dir * speed * Time.deltaTime;

            if (dir.x != 0) {
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x) * (dir.x > 0 ? 1 : -1);
                transform.localScale = scale;
            }
        }
    }
}
