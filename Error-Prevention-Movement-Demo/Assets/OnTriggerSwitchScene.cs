using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnTriggerSwitchScene : MonoBehaviour
{

    private int _nextScene;

    private void Start()
    {
        _nextScene = SceneManager.GetActiveScene().buildIndex + 1;
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        SceneManager.LoadScene(_nextScene);
    }
}
