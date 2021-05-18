using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudDrawer : Drawer
{
    private WindDrawer windDrawer;
    private SafeZoneLocationDrawer safeZoneLocationDrawer;
    private ActiveEventDrawer eventDrawer;
    public HudDrawer() : base()
    {
        SchuilPlaatsManager.OnSafeZoneReached.AddListener(OnSafeZoneReached);
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
        SchuilPlaatsManager.OnSafeZoneReached.RemoveListener(OnSafeZoneReached);
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
