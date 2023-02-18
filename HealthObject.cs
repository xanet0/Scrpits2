using System.Collections;
using UnityEngine;

public class HealthObject : MonoBehaviour
{
    public int health;
    public GameObject drop;
    public void Fall()
    {
        Rigidbody rig = GetComponent<Rigidbody>();
        rig.isKinematic = false;
        Destroy(gameObject, 4f);
        StartCoroutine(SleepBeforeSpawn());

    }
    IEnumerator SleepBeforeSpawn()
    {
        yield return new WaitForSeconds(2.5f);
        Instantiate(drop, new Vector3(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y+0.1f, GetComponent<Transform>().position.z), GetComponent<Transform>().rotation);
    }
}
