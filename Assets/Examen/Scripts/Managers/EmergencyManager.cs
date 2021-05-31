using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Examen.Managers
{

    public class LocationEvent : UnityEvent<EventLocation> { }
    public class EmergencyManager : Singelton<EmergencyManager>
    {
        private List<EventLocation> eventLocations = new List<EventLocation>();

        public LocationEvent onEventActivated = new LocationEvent();
        public EventLocation currentEvent { get; private set; }

        public override void Awake()
        {
            GetRandomEventLocation();
        }

        public EventLocation CreatEventLocation(Vector3 pos)
        {
            var eventloc = GameObject.Instantiate(Resources.Load<EventLocation>("EventLocation"), pos, Quaternion.identity);
            eventLocations.Add(eventloc);
            currentEvent = eventloc;
            onEventActivated.Invoke(eventloc);
            return eventloc;
        }


        public void GetRandomEventLocation()
        {
            if (eventLocations.Count == 0) return;

            currentEvent = eventLocations[Random.Range(0, eventLocations.Count)];
            onEventActivated.Invoke(currentEvent);
        }

    }
}
