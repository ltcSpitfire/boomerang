using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderScript : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public int iLevelToLoad;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        GameObject collisionGameObject = collision.gameObject;

        if(collisionGameObject.name == "Player")
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));

        }

    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(iLevelToLoad);
    }
}
