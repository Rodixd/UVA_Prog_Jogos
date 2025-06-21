using UnityEngine;

public class MovLua : MonoBehaviour
{
    private Rigidbody2D corpo;
    public float velocidade = 1.0f;
    public float posicao_inicial = -121.0f;
    public float posicao_final = 108.0f;

    void Start()
    {
        corpo = GetComponent<Rigidbody2D>();
        corpo.linearVelocity = new Vector2(velocidade, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x >= posicao_final)
        {
            transform.Translate(posicao_inicial, 0, 0);
        }
    }
}
