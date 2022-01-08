using UnityEngine;

public class Enemy_Sideways : MonoBehaviour
{
    //Movement Saw
    [SerializeField] private float movemementDistance;
    [SerializeField] private float speed;
    private bool movingLeft;
    private float leftEdge;
    private float rightEdge;

    private void Awake()
    {
        leftEdge = transform.position.x - movemementDistance;
        rightEdge = transform.position.x + movemementDistance;
    }

    private void Update()
    {
        // Movement SAW
        if(movingLeft)
        {
            if (transform.position.x > leftEdge)
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                movingLeft = false;
        } else
        {
            if (transform.position.x < rightEdge)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);

            }
            else
                movingLeft = true;
        }
       
    }


    [SerializeField] private float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }


}
