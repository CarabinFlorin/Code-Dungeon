using UnityEngine;
using UnityEngine.UI;

public class Resize : MonoBehaviour
{
    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            //Resize  height
            transform.GetChild(i).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 30);
            //Set scale to 1
            transform.GetChild(i).localScale = new Vector3(1, 1, 1);
        }

    }
}

