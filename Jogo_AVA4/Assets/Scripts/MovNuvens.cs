using UnityEngine;

public class MovNuvens : MonoBehaviour
{
    private Rigidbody2D corpo;
    public float velocidade = 1.0f;

    void Start()
    {
        corpo = GetComponent<Rigidbody2D>();
        corpo.linearVelocity = new Vector2(-velocidade, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x <= -20)
        {
            transform.Translate(133, 0, 0);
        }
    }
}
