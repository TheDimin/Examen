using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Examen.Level;
using Examen.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetLocationDrawer : Drawer
{
    private bool ShouldTick = false;
    public Vector3 WorldPos;

    public Vector3 OldWorldPos;

    protected override string Path { get; set; } = "Editor/PlayerStart";

    private Camera cam;
    private float size;

    public override void Draw()
    {
        cam = Camera.main;
        size = Window.GetComponent<RectTransform>().sizeDelta.x;
    }

    public void EnableTick()
    {
        ShouldTick = true;
        OldWorldPos = WorldPos;
    }

    public override void Tick()
    {
        Vector3 screenUnclamped = cam.WorldToScreenPoint(WorldPos);
        Transform.position = new Vector3(Mathf.Clamp(screenUnclamped.x, size / 2, Screen.width - size / 2),
            Mathf.Clamp(screenUnclamped.y, size / 2, Screen.height - size / 2));

        if (!ShouldTick)
            return;

        WorldPos = CalculateWorldPos(cam.ScreenToWorldPoint(Input.mousePosition));

        if (Input.GetMouseButton(0))
        {
            ShouldTick = false;
        }
        else if (Input.GetMouseButton(1))
        {
            ShouldTick = false;
            WorldPos = OldWorldPos;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="MousePos">Location of users mouse in world space</param>
    /// <returns></returns>
    protected virtual Vector3 CalculateWorldPos(Vector3 MousePos)
    {
        return MousePos;
    }
}


public class SetLocationWithCollisionDrawer : SetLocationDrawer
{
    private PolygonCollider2D collider;

    public override void Draw()
    {
        base.Draw();
        collider = GameObject.FindObjectOfType<PolygonCollider2D>();
    }

    protected override Vector3 CalculateWorldPos(Vector3 MousePos)
    {
        return !collider.OverlapPoint(MousePos) ? MousePos : WorldPos;
    }
}

public class SafeZoneSetLocationDrawer : SetLocationDrawer
{
    private Vector3[] locs;

    public override void Draw()
    {
        base.Draw();
        locs = SafeZoneManager.Instance.schuilplaatsen.Select(behaviour => behaviour.transform.position)
            .ToArray();
    }

    protected override Vector3 CalculateWorldPos(Vector3 MousePos)
    {
        Vector3 bestLoc = Vector3.zero;
        float distance = -1;
        foreach (var loc in locs)
        {
            var nmag = (loc - MousePos).magnitude;
            if (distance == -1 || nmag < distance)
            {
                bestLoc = loc;
                distance = nmag;
            }
        }

        if (distance == -1)
            throw new Exception("Failed to find proper distance");

        return bestLoc;
    }
}


public class EditorHudDrawer : HudDrawer<ScenarioEditor>
{
    private Slider WindSlider;
    private TMP_InputField AlarmCode;
    private SetLocationDrawer spawnLocation;
    private SetLocationDrawer fireLocation;
    private SetLocationDrawer BestSafeZoneLocation;
    private Toggle HintToggle;
    protected override string Path { get; set; } = "Editor/Hud";

    public override void Draw()
    {
        spawnLocation = LoadChild<SetLocationWithCollisionDrawer>(Transform, "Editor/PlayerStart");
        fireLocation = LoadChild<SetLocationDrawer>(Transform, "Editor/FireLocation");
        BestSafeZoneLocation = LoadChild<SafeZoneSetLocationDrawer>(Transform, "Editor/SafeZone");

        //  GameObject.DontDestroyOnLoad(Transform);
        WindSlider = Transform.Find("Panel/Wind/Slider").GetComponent<Slider>();
        AlarmCode = Transform.Find("Panel/AlarmCode/InputField").GetComponent<TMP_InputField>();
        HintToggle = Transform.Find("Panel/LocationHint/Toggle").GetComponent<Toggle>();

        WindSlider.onValueChanged.AddListener(arg0 => SetWind(arg0));
        AlarmCode.characterLimit = 3;

        Transform.Find("Panel/Save").GetComponent<Button>().onClick.AddListener(
            () =>
            {
                Save(ref Level.LevelData);
            }
        );

        Transform.Find("Panel/SaveAndExit").GetComponent<Button>().onClick.AddListener(
            () =>
            {
                Save(ref Level.LevelData);
                GameManager.Instance.QuitLevel();
            }
        );

        Transform.Find("Panel/SpawnPoint").GetComponent<Button>().onClick.AddListener(spawnLocation.EnableTick);
        Transform.Find("Panel/FirePoint").GetComponent<Button>().onClick.AddListener(fireLocation.EnableTick);
        Transform.Find("Panel/TargetSafeZone").GetComponent<Button>().onClick
            .AddListener(BestSafeZoneLocation.EnableTick);

        Load(ref Level.LevelData);
    }

    //https://answers.unity.com/questions/227736/clamping-a-wrapping-rotation.html
    private static float ClampAngle(float angle)
    {
        if (angle < 0f)
            return angle + (360f * (int)((angle / 360f) + 1));
        else if (angle > 360f)
            return angle - (360f * (int)(angle / 360f));
        else
            return angle;
    }

    private void SetWind(float value, bool FromSave = false)
    {
        if (FromSave)
            WindSlider.value = value;
        Transform.Find("Panel/Wind/Text").GetComponent<TextMeshProUGUI>().text =
            value.ToString(CultureInfo.InvariantCulture);
    }

    private void Load(ref ScenarioData saveData)
    {
        SetWind(saveData.wind, true);
        spawnLocation.WorldPos = saveData.StartLocation;
        fireLocation.WorldPos = saveData.FireLocation;
        BestSafeZoneLocation.WorldPos = saveData.SafeZone;
        AlarmCode.text = saveData.AlarmLocation;
        HintToggle.isOn = saveData.LocatieHint;
    }

    private void Save(ref ScenarioData saveData)
    {
        saveData.wind = ClampAngle(WindSlider.value);
        saveData.StartLocation = spawnLocation.WorldPos;
        saveData.FireLocation = fireLocation.WorldPos;
        saveData.SafeZone = BestSafeZoneLocation.WorldPos;
        saveData.AlarmLocation = AlarmCode.text;
        saveData.LocatieHint = HintToggle.isOn;
        Level.Save();
    }
}
