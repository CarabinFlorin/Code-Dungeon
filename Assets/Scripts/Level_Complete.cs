using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Complete : MonoBehaviour
{
    bool once = false;

    public GameObject UI;
    private void Update()
    {
        if (!once && Vector2.Distance(GameObject.FindWithTag("Player").transform.position, transform.position) < .5)
        {
            once = true;
            UI.SetActive(true);
        }
    }
}
