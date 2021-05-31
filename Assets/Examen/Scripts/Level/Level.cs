using System;
using Examen.Managers;
using Newtonsoft.Json;
using UnityEngine;

namespace Examen.Level
{
    public interface ILevelSetter
    {
        public void SetLevel(Level level);
    }

    public abstract class Level : IDisposable
    {
        public ScenarioData LevelData;
        protected Drawer hud;
        public abstract int Scene { get; protected set; }
        public string ID { get; protected set; }

        protected void LoadScenario(string id)
        {
            string data = PlayerPrefs.GetString(id);
            Debug.Log(data);
            LevelData = JsonConvert.DeserializeObject<ScenarioData>(data);
            ID = id;
        }

        public void Save()
        {
            string jsonData = JsonConvert.SerializeObject(LevelData);
            Debug.Log(jsonData);
            PlayerPrefs.SetString(ID, jsonData);
            PlayerPrefs.Save();
        }

        internal virtual void Awake()
        {
            OnLoad(LevelData);

            ((ILevelSetter)hud).SetLevel(this);
            hud.LoadWindow();
            UnityEngine.Object.DontDestroyOnLoad(hud.Transform);
            hud.Draw();
        }

        public virtual void Update(float delta){}

        public virtual void FixedUpdate()
        {
            hud?.Tick();
        }

        public virtual void OnDrawGizmos() { }

        public virtual void Dispose()
        {
            hud.Dispose();

            SafeZoneManager.Dispose();
            EmergencyManager.Dispose();
            WindManager.Dispose();

        }
        public virtual void OnLoad(ScenarioData data)
        {

        }
    }
}
