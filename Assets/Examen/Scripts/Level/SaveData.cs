using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Examen.Level
{
   
    [System.Serializable]
    public struct ScenarioData
    {
        [SerializeField] public string ID;

        [SerializeField] public float wind;
        [SerializeField] public Vector3 StartLocation;
        [SerializeField] public Vector3 FireLocation;
        [SerializeField] public Vector3 SafeZone;

        public ScenarioData(string _)
        {
            this.ID = "INVALID";
            wind = 0;
            StartLocation = Vector3.zero;
            FireLocation = Vector3.zero;
            SafeZone = Vector3.zero;
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
}
