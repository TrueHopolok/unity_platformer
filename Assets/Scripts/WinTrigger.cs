using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("WIN!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
