using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Examen.Level
{
   
    public class ScenarioEditor : Level
    {
        public override int Scene { get; protected set; } = 1;

        public ScenarioEditor(string id) : base()
        {
            LoadScenario(id);
        }

        internal override void Awake()
        {
            hud = new EditorHudDrawer();
            base.Awake();
            Camera.main.gameObject.AddComponent<CameraPlayerController>();
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
}
