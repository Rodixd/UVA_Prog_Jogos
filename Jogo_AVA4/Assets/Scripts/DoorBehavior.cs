using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes!

public class Door : MonoBehaviour
{
    [Header("Visuals")]
    public Sprite openDoorSprite; // Drag your "open door" sprite here in the Inspector

    [Header("Scene Management")]
    public string sceneToLoad; // Type the name of the scene you want to load

    private SpriteRenderer spriteRenderer;
    private Collider2D doorCollider;
    private bool isOpened = false;

    private void Awake()
    {
        // Get references to the components on this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();
    }

    // Unity calls this when another collider bumps into this one
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the door is already open, do nothing.
        if (isOpened) return;

        // Check if the object we collided with is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            // Check if the player exists and if they have the key
            if (player != null && player.key == 1)
            {
                OpenDoorAndLoadScene();
            }
            else
            {
                Debug.Log("Door is locked! Find the key.");
                // Optional: Play a "locked" sound effect here
            }
        }
    }

    private void OpenDoorAndLoadScene()
    {
        SoundEffectManager.Play("Door");
        isOpened = true;

        // 1. Change the sprite to the open door
        if (openDoorSprite != null)
        {
            spriteRenderer.sprite = openDoorSprite;
        }

        // 2. Disable the collider so the player can pass through
        doorCollider.enabled = false;

        // 3. Load the new scene
        // Make sure the scene name is correct and it's in the Build Settings!
        SceneManager.LoadScene(sceneToLoad);
    }
}