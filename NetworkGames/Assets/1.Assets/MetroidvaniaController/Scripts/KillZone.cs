using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SM = UnityEngine.SceneManagement.SceneManager;

public class KillZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            SM.LoadSceneAsync(SM.GetActiveScene().buildIndex);
        }
        else
        {
            Destroy(col.gameObject);
        }
    }
}
