using System.Collections;
using System.Collections.Generic;
using Examen.Managers;
using UnityEngine;

public abstract class HudDrawer<TLevel> : Drawer where TLevel : Level
{
    protected TLevel Level;

    public void SetLevel(TLevel level)
    {
        this.Level = level;
    }
}

public class EditorHudDrawer : HudDrawer<ScenarioEditor>
{
    protected override string Path { get; set; } = "Editor/Hud";
    public override void Draw()
    {
    }
}

public class GameHudDrawer : HudDrawer<GameLevel>
{
    private WindDrawer windDrawer;
    private SafeZoneLocationDrawer safeZoneLocationDrawer;
    private ActiveEventDrawer eventDrawer;
    public GameHudDrawer() : base()
    {
        SchuilPlaatsManager.Instance.OnSafeZoneReached.AddListener(OnSafeZoneReached);
    }


    protected override string Path { get; set; } = "HUD";
    public override void Draw()
    {
        windDrawer = LoadChild<WindDrawer>(Transform.Find("WindSlot"));
        safeZoneLocationDrawer = LoadChild<SafeZoneLocationDrawer>(Transform);
        eventDrawer = LoadChild<ActiveEventDrawer>(Transform);
    }

    public override void Dispose()
    {
        base.Dispose();
        windDrawer = null;
        safeZoneLocationDrawer = null;
        SchuilPlaatsManager.Instance.OnSafeZoneReached.RemoveListener(OnSafeZoneReached);
    }


    private void OnSafeZoneReached(SchuilPlaatsBehaviour arg0)
    {
        LoadChild<EndReached>(Transform);
    }
}

public class EndReached : Drawer
{
    protected override string Path { get; set; } = "GameOver";
    public override void Draw()
    {
        throw new System.NotImplementedException();
    }
}
