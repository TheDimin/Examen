using UnityEngine;
using UnityEngine.Events;

namespace Examen.Managers
{

    public class LocationEvent : UnityEvent<EventLocation> { }
    public class EmergencyManager : Singelton<EmergencyManager>
    {
        private EventLocation[] eventLocations;

        public LocationEvent onEventActivated = new LocationEvent();
        public EventLocation currentEvent { get; private set; }

        public override void Awake()
        {
            GetEmergencyLocations();
            GetRandomEventLocation();
        }

        private void GetEmergencyLocations()
        {
            eventLocations = Transform.FindObjectsOfType<EventLocation>();
        }

        public void GetRandomEventLocation()
        {
            if (eventLocations.Length == 0) return;

            currentEvent = eventLocations[Random.Range(0, eventLocations.Length)];
            onEventActivated.Invoke(currentEvent);
        }

    }
}
