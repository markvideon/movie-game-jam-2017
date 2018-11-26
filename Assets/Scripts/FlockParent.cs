using UnityEngine;
using System.Collections;

public class FlockParent : MonoBehaviour
{
    private Vector3[] flockVelocities;
    // Use this for initialization
    void Start()
    {
        flockVelocities = new Vector3[this.transform.childCount];

        for (int i = 0; i < this.transform.childCount; i++) {
            flockVelocities[i] = this.transform.GetChild(i).GetComponent<Rigidbody>().velocity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            flockVelocities[i] = this.transform.GetChild(i).GetComponent<Rigidbody>().velocity;
        }
    }

    public Vector3[] getFlockVelocities() {
        return flockVelocities;
    }
}
