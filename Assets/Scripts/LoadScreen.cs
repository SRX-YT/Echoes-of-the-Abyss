using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private Slider     slider;

    private void Start()
    {
        Loading();
    }

    public void Loading()
    {
        StartCoroutine(LoadAsync());
    }

    IEnumerator LoadAsync()
    {
        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(2);
        loadAsync.allowSceneActivation = false;

        while (!loadAsync.isDone)
        {
            slider.value = loadAsync.progress;

            if (loadAsync.progress >= .9f && !loadAsync.allowSceneActivation)
            {
                yield return new WaitForSeconds(2.2f);
                loadAsync.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
