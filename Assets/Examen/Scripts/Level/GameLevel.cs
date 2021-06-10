using System.Collections;
using Examen.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace Examen.Level
{

    public class GameLevel : Level
    {
        public PlayerController pc { get; private set; }
        public override int Scene { get; protected set; } = 1;

        public float Timer { get; private set; } = 60;

        public UnityEvent OnOutOfTime = new UnityEvent();

        private Coroutine awaitInputCoroutine;
        public GameLevel(string id) : base()
        {
            LoadScenario(id);
        }

        internal override void Awake()
        {
            hud = new GameHudDrawer();

            base.Awake();

            pc = Transform.FindObjectOfType<PlayerController>();
            Camera.main.gameObject.AddComponent<CameraPlayerSnap>();

            RemoveInputHint();
            pc.OnMove.AddListener(RemoveInputHint);

            awaitInputCoroutine = pc.StartCoroutine(AwaitInput());
            pc.StartCoroutine(AudioManager.Instance.PlayCode(LevelData.AlarmLocation));
            //   
        }

        IEnumerator AwaitInput()
        {
            yield return new WaitForSeconds(5);
            hud.Transform.Find("HintSlot").gameObject.SetActive(true);

        }

        void RemoveInputHint()
        {
            if (awaitInputCoroutine != null)
                pc.StopCoroutine(awaitInputCoroutine);

            hud.Transform.Find("HintSlot").gameObject.SetActive(false);
        }

        public override void Update(float delta)
        {
            base.Update(delta);
            Timer -= delta;

            if (Timer <= 0)
            {
                OnOutOfTime?.Invoke();
                OnOutOfTime = null;
            }
        }

        public override void OnDrawGizmos()
        {
            // SchuilPlaatsManager.Instance.OnDrawGizmos();
        }

        public override void OnLoad(ScenarioData data)
        {
            Debug.Log("Spawn player");
            GameObject.Instantiate(Resources.Load("Player"), data.StartLocation, Quaternion.identity);

            EmergencyManager.Instance.CreatEventLocation(data.FireLocation);
            SafeZoneManager.Instance.FindClosest(data.SafeZone);
            WindManager.Instance.SetWindDirection((int)data.wind);
        }

    }
}
