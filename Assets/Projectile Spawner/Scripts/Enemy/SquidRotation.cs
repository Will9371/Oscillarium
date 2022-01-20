using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidRotation : MonoBehaviour
{
    void Update()
    {
        this.transform.Rotate(Random.Range(0f, 0.7f),0,0);
        this.transform.position += new Vector3(0, Random.Range(-0.005f, 0.005f), 0);
    }
}
