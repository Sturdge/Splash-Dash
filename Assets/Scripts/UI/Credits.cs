using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField]
    private bool canPressBtn = true;

    public void Update()
    {
        if (Input.GetButtonDown("BackButton"))
        {
            canPressBtn = true;
            SceneManager.LoadScene(0);
        }
    }
}
