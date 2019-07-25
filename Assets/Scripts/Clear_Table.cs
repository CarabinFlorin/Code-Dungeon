using UnityEngine;
using UnityEngine.SceneManagement;

public class Clear_Table : MonoBehaviour
{
    public void Clear()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
