using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public void BackToMenu()
    {
        if (Input.GetButtonDown("BackButton"))
        {
            SceneManager.LoadScene(0);
        }
    }
}
