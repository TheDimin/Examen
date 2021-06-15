using System;
using System.Globalization;
using System.Linq;
using Examen.Level;
using Examen.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Examen.Level
{
    public abstract class HudDrawer<TLevel> : Drawer, ILevelSetter where TLevel : Level
    {
        protected TLevel Level;

        public void SetLevel(Level lvl)
        {
            this.Level = (TLevel)lvl;
        }
    }

    public class GameHudDrawer : HudDrawer<GameLevel>
    {
        private WindDrawer windDrawer;
        private SafeZoneLocationDrawer safeZoneLocationDrawer;
        private ActiveEventDrawer eventDrawer;

        private TextMeshProUGUI timerText;
        private GameLevel gl;


        public GameHudDrawer() : base()
        {
            SafeZoneManager.Instance.OnSafeZoneReached.AddListener(OnSafeZoneReached);
            gl = (GameLevel)GameManager.Instance.level;

            gl.OnOutOfTime.AddListener(OnOutOfTime);
        }


        protected override string Path { get; set; } = "HUD";

        public override void Draw()
        {
            if (gl.LevelData.LocatieHint)
            {
                windDrawer = LoadChild<WindDrawer>(Transform.Find("WindSlot"));
                safeZoneLocationDrawer = LoadChild<SafeZoneLocationDrawer>(Transform);
            }

            eventDrawer = LoadChild<ActiveEventDrawer>(Transform, "", true);
            eventDrawer.DrawHint = gl.LevelData.LocatieHint;
            eventDrawer.Draw();

            timerText = Transform.Find("Timer").GetComponent<TextMeshProUGUI>();
        }

        public override void Tick()
        {
            base.Tick();
            int min = Mathf.FloorToInt(gl.Timer / 60);
            int sec = Mathf.CeilToInt(gl.Timer % 60);

            timerText.text = $"{min}:{sec}";
        }

        public override void Dispose()
        {
            base.Dispose();

            windDrawer = null;
            safeZoneLocationDrawer = null;
            eventDrawer = null;

            SafeZoneManager.Instance.OnSafeZoneReached.RemoveListener(OnSafeZoneReached);
        }
        private void OnOutOfTime()
        {
            var gameOverPanel = LoadChild<EndReached>(Transform, "", true);
            gameOverPanel.Message = "You ran out of time";
            gameOverPanel.Draw();
        }

        private void OnSafeZoneReached(SafeZoneBehaviour _)
        {
            var gameOverPanel = LoadChild<EndReached>(Transform, "", true);
            gameOverPanel.Message = "You reached the correct SafeZone";
            gameOverPanel.Draw();
        }
    }

    public class EndReached : Drawer
    {
        public string Message = "";
        protected override string Path { get; set; } = "GameOver";

        public override void Draw()
        {
            Window.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = Message;
            Window.transform.Find("Button").GetComponent<Button>().onClick
                .AddListener(GameManager.Instance.QuitLevel);
        }
    }
}