using UnityEngine;

public class DecorativeObjectCuller : MonoBehaviour
{
    void Start()
    {
        if (transform.position.z < 30)
        {
            gameObject.SetActive(false);
            return;
        }

        if (transform.position.z > 150)
        {
            if (Random.Range(0f, 1f) < 0.3f)
            {
                gameObject.SetActive(false);
                return;
            }
        }

        if (transform.position.z > 300)
        {
            if (Random.Range(0f, 1f) < 0.5f)
            {
                gameObject.SetActive(false);
                return;
            }
        }

        if (transform.position.z > 600)
        {
            if (Random.Range(0f, 1f) < 0.7f)
            {
                gameObject.SetActive(false);
                return;
            }
        }
    }


}
