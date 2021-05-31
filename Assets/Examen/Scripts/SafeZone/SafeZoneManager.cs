using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Examen.Managers
{

    public class SafeZoneManager : Singelton<SafeZoneManager>
    {
        public SafeZoneBehaviour[] schuilplaatsen { get; private set; }
        private Vector2[] locs;
        public SafeZoneEvent OnBestSafeZoneFound = new SafeZoneEvent();
        public SafeZoneEvent OnSafeZoneReached = new SafeZoneEvent();
        public SafeZoneBehaviour LastBestSafeZone { get; private set; }

        public override void Awake()
        {
            GenerateSchuilplaatsen();
        }

        private void GenerateSchuilplaatsen()
        {
            schuilplaatsen = Transform.FindObjectsOfType<SafeZoneBehaviour>();
            locs = schuilplaatsen.Select(behaviour => (Vector2)behaviour.transform.position).ToArray();
        }

        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            if (schuilplaatsen == null) return;

            foreach (var t in schuilplaatsen)
            {
                if (t == null) return;
                Transform scp = t.transform;
                Gizmos.DrawCube(scp.position, scp.GetComponent<BoxCollider2D>().size * scp.localScale);
            }
        }

        public SafeZoneBehaviour FindClosest(Vector3 loc)
        {
            SafeZoneBehaviour closestSafeZone = null;
            float closestDistance = 96999999999;
            foreach (var schuilPlaats in schuilplaatsen)
            {
                float mag = (schuilPlaats.transform.position - loc).magnitude;
                if (closestSafeZone == null || (mag < closestDistance))
                {
                    closestSafeZone = schuilPlaats;
                    closestDistance = mag;
                }
            }

            if (closestSafeZone == null)
                throw new Exception("Failed to find safezone");

            if (closestSafeZone == LastBestSafeZone) return closestSafeZone;

            LastBestSafeZone = closestSafeZone;
            OnBestSafeZoneFound.Invoke(LastBestSafeZone);

            return LastBestSafeZone;
        }

        /*
        private SchuilPlaatsBehaviour FindBestSaveZoneInternal(Vector2 playerLocation)
        {
            Debug.Log("FindBestSaveZone");
            EventLocation eventLocation = EmergencyManager.Instance.currentEvent;
            SchuilPlaatsBehaviour closestSchuilPlaats = null;
            float closestDistance = -1;
            foreach (var schuilPlaats in schuilplaatsen)
            {
                float mag = ((Vector2)schuilPlaats.transform.position - playerLocation).magnitude;
                if (closestSchuilPlaats == null || (mag < closestDistance &&
                                                    (eventLocation.transform.position - schuilPlaats.transform.position)
                                                    .magnitude > 100))
                {
                    closestSchuilPlaats = schuilPlaats;
                    closestDistance = mag;
                }
            }

            if (closestSchuilPlaats == null)
                throw new Exception("Failed to find safezone");

            if (closestSchuilPlaats == LastBestSchuilPlaats) return closestSchuilPlaats;

            LastBestSchuilPlaats = closestSchuilPlaats;
            OnBestSafeZoneFound.Invoke(LastBestSchuilPlaats);

            return LastBestSchuilPlaats;
        }*/
    }
}
