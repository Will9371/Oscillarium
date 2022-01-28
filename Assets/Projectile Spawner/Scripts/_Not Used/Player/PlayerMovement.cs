using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 7;
    ColorChange colorChange = null;
    BulletSpawner bulletSpawner = null;
    private Player player = null;
    private Rigidbody rb = null;
    
    private void Start()
    {
        player = GetComponent<Player>();
        colorChange = GetComponent<ColorChange>();
        rb = GetComponent<Rigidbody>();
        bulletSpawner = player.bulletSpawner;
    }

    void Update()
    {
        ControllerInput();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vaxis = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(0, vaxis, -hAxis) * movementSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);
    }
    
    private void ChangeColor()
    {
        if (colorChange.colorIndex < 1) colorChange.colorIndex++;
        else colorChange.colorIndex = 0;
    }
    
    private void ControllerInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeColor();
        }
        InputShoot();
    }
    
    private void InputShoot()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            player.shooting.ShootLeft();
            return;
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            player.shooting.ShootRight();
            return;
        }
        bulletSpawner.isFiring = false;
    }
}
