using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private HealthManager healthManager = null;
    private int currentState = 0;
    [SerializeField] private GameObject[] franticNavPoints = null;
    [SerializeField] private GameObject[] verticalNavPoints = null;
    [SerializeField] private float speed = 0.005f;
    private Vector3 nextPoint;
    private float timeBetweenMovements = 10;
    private float movementTimer = 10;


    private void Start()
    {
        healthManager = this.GetComponent<HealthManager>();
    }

    private void Update()
    {
        MoveToNext(nextPoint);
        SwitchBossStates();
        //still side
        if (healthManager.health > 18000)
        {
            currentState = 0;
        }
        //vertical
        if (healthManager.health > 15000 && healthManager.health <18000)
        {
            currentState = 1;
        }
        //still middle
        if (healthManager.health <= 15000 && healthManager.health > 10000)
        {
            currentState = 2;
        }
        //frantic
        if (healthManager.health <= 10000)
        {
            currentState = 3;
        }
    }

    private void SwitchBossStates()
    {
        switch (currentState)
        {
            case 0:
                StillSide();
                break;
            case 1:
                Vertical();
                break;
            case 2:
                StillMiddle();
                break;
            case 3:
                Frantic();
                break;

        }
            
    }
    private void StillSide()
    {
        SwitchPoint(new Vector3(0,0,-12.5f));
    }
    private void StillMiddle()
    {
        speed = 0.01f;
        timeBetweenMovements = 8;
        SwitchPoint(new Vector3(0,0,0));
    }
    private void Vertical()
    {
        speed = 0.01f;
        timeBetweenMovements = 1;
        SwitchPoint(verticalNavPoints[Random.Range(0,verticalNavPoints.Length)].transform.position);
    }
    private void Frantic()
    {
        speed = 0.05f;
        timeBetweenMovements = 5;
        SwitchPoint(franticNavPoints[Random.Range(0, franticNavPoints.Length)].transform.position);
    }
    private void MoveToNext(Vector3 point)
    {
        transform.position = Vector3.MoveTowards(transform.position, point, speed);
    }
    private void SwitchPoint(Vector3 point)
    {
        //is movement time up?
        if (movementTimer > 0)
        {
            movementTimer -= Time.deltaTime;
            return;
        }
        movementTimer = timeBetweenMovements;
        nextPoint = point;
    }
}
