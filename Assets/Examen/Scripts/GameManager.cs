using System;
using System.Collections.Generic;
using Examen.Managers;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private Level level;

    private ScenarioEditor scenarioEditor;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Already instance found");
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
        WindManager.SetWindDirection(Random.Range(0, 365));
        WindManager.SetWindSpeed(Random.Range(0, 100));
    }


    private void FixedUpdate()
    {
        level?.FixedUpdate();
    }

    public void OpenScenarioEditor([CanBeNull] string id = null)
    {
        if (id == null)
            level = ScenarioEditor.CreateScenario();
        else
        {
            level = new ScenarioEditor(id);
        }
    }


    public void LoadLevel(string id)
    {
        level = new GameLevel(id.ToUpper());
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


[System.Serializable]
public struct ScenarioData
{
    [SerializeField]
    public string ID;

    [SerializeField]
    public string Test;


    public ScenarioData(string test)
    {
        this.Test = "SUb";
        this.ID = "INVALID";
    }
}

[System.Serializable]
public struct Saves
{
    [SerializeField]
    public List<string> IDS;

    public Saves(string _ = "")
    {
        IDS = new List<string>();
    }
}

public static class SaveStat
{
    public static string[] GetSaveIDs()
    {
        if (PlayerPrefs.HasKey("Keys"))
        {
            Debug.Log(PlayerPrefs.GetString("Keys"));
            return JsonConvert.DeserializeObject<Saves>(PlayerPrefs.GetString("Keys")).IDS.ToArray();

        }

        return new string[0];
    }

    public static Saves GetSaves()
    {
        if (PlayerPrefs.HasKey("Keys"))
        {
            Debug.Log(PlayerPrefs.GetString("Keys"));
        }


        return PlayerPrefs.HasKey("Keys") ? JsonConvert.DeserializeObject<Saves>(PlayerPrefs.GetString("Keys")) : new Saves();
    }
}


public abstract class Level
{
    protected ScenarioData LevelData;
    protected Drawer hud;
    public string ID { get; protected set; }

    protected Level()
    {
        SceneManager.LoadScene(1);
    }

    protected void LoadScenario(string id)
    {
        string data = PlayerPrefs.GetString(id);
        LevelData = JsonConvert.DeserializeObject<ScenarioData>(data);
        ID = id;

        Awake();
    }


    protected virtual void Awake()
    {

        hud.LoadWindow();
        hud.Draw();
    }

    public virtual void FixedUpdate()
    {
        hud.Tick();
    }

    public virtual void OnDrawGizmos() { }

}

public class GameLevel : Level
{
    private PlayerController pc;

    public GameLevel(string id) : base()
    {
        LoadScenario(id);
    }

    protected override void Awake()
    {
        hud = new GameHudDrawer();

        base.Awake();

        pc = Transform.FindObjectOfType<PlayerController>();
        SchuilPlaatsManager.Reset();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
      //  SchuilPlaatsManager.FindBestSaveZone(pc.transform.position);
    }

    public override void OnDrawGizmos()
    {
       // SchuilPlaatsManager.Instance.OnDrawGizmos();
    }
}

public class ScenarioEditor : Level
{

    public ScenarioEditor(string id) : base()
    {
        LoadScenario(id);
    }

    protected override void Awake()
    {
        hud = new EditorHudDrawer();
        base.Awake();
    }


    public static ScenarioEditor CreateScenario()
    {

        var loadedData = new ScenarioData();

        Guid guid = Guid.Empty;
        string finalId;
        do
        {
            guid = Guid.NewGuid();
            finalId = guid.ToString().Substring(0, 4).ToUpper();
        } while (PlayerPrefs.HasKey(finalId));

        loadedData.ID = finalId;
        string jsonData = JsonConvert.SerializeObject(loadedData);

        PlayerPrefs.SetString(finalId, jsonData);

        {
            var t = SaveStat.GetSaves();

            if (t.IDS == null)
                t.IDS = new List<string>();

            t.IDS.Add(finalId);

            jsonData = JsonConvert.SerializeObject(t);

            PlayerPrefs.SetString("Keys", jsonData);
        }
        PlayerPrefs.Save();

        return new ScenarioEditor(finalId);
    }

}