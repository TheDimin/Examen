using System;
using System.Collections;
using Examen.Level;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{//
    public static GameManager Instance { get; private set; }
    public Level level { get; private set; }

    private ScenarioEditor scenarioEditor;

    private void Awake()
    {
        if (Instance != null)
        {
            // Debug.LogWarning("Already instance found");
            Destroy(gameObject);
            return;
        }


        Instance = this;
        DontDestroyOnLoad(gameObject);

        //    NavManager.StartPoint = transform.position;
        //  NavManager.Awake();

    }

    private void Start()
    {
        WindManager.Instance.SetWindSpeed(Random.Range(0, 100));
    }

    private void Update()
    {
        level?.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        level?.FixedUpdate();
    }

    public void OpenScenarioEditor([CanBeNull] string id = null)
    {
        level?.Dispose();
        if (id == null)
            level = ScenarioEditor.CreateScenario();
        else
        {
            level = new ScenarioEditor(id);
        }
        StartCoroutine(WaitForScene());
    }


    public void LoadLevel(string id)
    {

        level = new GameLevel(id.ToUpper());

        StartCoroutine(WaitForScene());
    }

    public void QuitLevel()
    {
        level.Dispose();
        level = null;
        SceneManager.LoadScene(0);
    }

    IEnumerator WaitForScene()
    {
        yield return SceneManager.LoadSceneAsync(1);
        //  while (!await.isDone) yield return null;
        yield return new WaitForEndOfFrame();
        level.Awake();
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {

    }

    private void OnDrawGizmos()
    {
        //   NavManager.StartPoint = transform.position;
        //   NavManager.OnDrawGizmos();
        level?.OnDrawGizmos();

    }
#endif
}