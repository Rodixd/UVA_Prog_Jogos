using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [Header("Visuals")]
    public Sprite openDoorSprite;

    [Header("Scene Management")]
    public string sceneToLoad;

    private SpriteRenderer spriteRenderer;
    private Collider2D doorCollider;
    private bool isOpened = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isOpened) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player != null && player.key == 1)
            {
                OpenDoorAndLoadScene();
            }
            else
            {
                Debug.Log("Door is locked! Find the key.");
            }
        }
    }

    private void OpenDoorAndLoadScene()
    {
        SoundEffectManager.Play("Door");
        isOpened = true;

        if (openDoorSprite != null)
        {
            spriteRenderer.sprite = openDoorSprite;
        }

        doorCollider.enabled = false;

        SceneManager.LoadScene(sceneToLoad);
    }
}