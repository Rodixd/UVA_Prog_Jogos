using UnityEngine;

public class KeyElementUI : MonoBehaviour
{
    public GameObject keyElement;
    void Update()
    {
        PlayerController player = GetComponent<PlayerController>();
        if (player.key == 1)
        {
            keyElement.SetActive(true);
        }
    }
}
