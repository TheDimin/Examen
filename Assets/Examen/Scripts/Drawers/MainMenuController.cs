using System;
using System.Collections;
using System.Collections.Generic;
using Examen.Level;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace MainMenu
{
    public class MainDrawer : Drawer
    {

        private UnityAction OnPlayPressed;
        public UnityAction OnCreatePressed;

        protected override string Path { get; set; } = "MainMenu/Main";

        public MainDrawer(UnityAction OnPlayPressed, UnityAction OnCreatePressed)
        {
            this.OnPlayPressed = OnPlayPressed;
            this.OnCreatePressed = OnCreatePressed;
        }


        public override void Draw()
        {
            Transform.Find("Panel/PlayB").GetComponent<Button>().onClick.AddListener(OnPlayPressed);
            Transform.Find("Panel/CreateB").GetComponent<Button>().onClick.AddListener(OnCreatePressed);
            Transform.Find("Panel/QuitB").GetComponent<Button>().onClick.AddListener(() => Application.Quit());
        }
        public override void Dispose()
        {
            Transform.Find("Panel/PlayB").GetComponent<Button>().onClick.RemoveAllListeners();
            Transform.Find("Panel/CreateB").GetComponent<Button>().onClick.RemoveAllListeners();
            Transform.Find("Panel/QuitB").GetComponent<Button>().onClick.RemoveAllListeners();
            base.Dispose();
        }
    }

    public class PlayDrawer : Drawer
    {

        private UnityAction back;


        protected override string Path { get; set; } = "MainMenu/PlayScenario";

        public PlayDrawer(UnityAction Back)
        {
            this.back = Back;
        }


        public override void Draw()
        {
            Transform.Find("ErrorText").gameObject.SetActive(false);
            Transform.Find("Panel/Play").GetComponent<Button>().onClick.AddListener(() =>
            {
                string key = Transform.Find("Panel/InputField").GetComponent<TMP_InputField>().text.ToUpper();
                if (!PlayerPrefs.HasKey(key))
                {
                    Transform.Find("ErrorText").gameObject.SetActive(true);
                    return;
                }
                Debug.Log("Should work?");

                GameManager.Instance.LoadLevel(key);
            });
            Transform.Find("Panel/Back").GetComponent<Button>().onClick.AddListener(back);
        }

        public override void Dispose()
        {
            Transform.Find("Panel/Back").GetComponent<Button>().onClick.RemoveAllListeners();
            //  Transform.Find("Play")
            base.Dispose();
        }

    }

    public class CreateDrawer : Drawer
    {

        private UnityAction onLevelOpen;
        private UnityAction back;


        protected override string Path { get; set; } = "MainMenu/CreateScenario";

        public CreateDrawer(UnityAction Back)
        {
            //  this.onLevelOpen = OnLevelOpen;
            this.back = Back;
        }


        public override void Draw()
        {
            // Transform.Find("Panel/PlayB").GetComponent<Button>().onClick.AddListener(onLevelOpen);
            // Transform.Find("Panel/CreateB").GetComponent<Button>().onClick.AddListener(OnCreatePressed);
            Transform.Find("Panel/Back").GetComponent<Button>().onClick.AddListener(back);
            Transform.Find("Panel/CreateNew").GetComponent<Button>().onClick.AddListener((() => GameManager.Instance.OpenScenarioEditor()));
            foreach (var saveId in SaveStat.GetSaveIDs())
            {
                var entry = LoadChild<CreateEntryDrawer>(Transform.Find("Scroll View/Viewport/Content"), "", true);
                entry.ID = saveId;
                entry.Draw();
            }

        }

        public override void Dispose()
        {
            Transform.Find("Panel/Back").GetComponent<Button>().onClick.RemoveAllListeners();
            //  Transform.Find("Play")
            base.Dispose();
        }

    }

    public class CreateEntryDrawer : Drawer
    {
        public string ID = "NONE";
        protected override string Path { get; set; } = "MainMenu/ScenarioEntry";


        public CreateEntryDrawer()
        {
        }


        public override void Draw()
        {
            Transform.GetChild(0).GetComponent<Text>().text = ID;
            Transform.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.OpenScenarioEditor(ID));
        }

        public override void Dispose()
        {

        }

    }


    public class MainMenuController : MonoBehaviour
    {
        private Drawer child;

        private void Awake()
        {
            OpenMainPanel();
        }

        public void OpenMainPanel()
        {
            child?.Dispose();

            child = new MainDrawer(OpenLevelLoader, OpenScenarioCreator);
            child.LoadWindow(transform.Find("Panel"));
            child.Draw();
        }

        public void OpenLevelLoader()
        {
            child.Dispose();
            child = new PlayDrawer(OpenMainPanel);
            child.LoadWindow(transform.Find("Panel"));
            child.Draw();
        }

        public void OpenScenarioCreator()
        {
            child.Dispose();
            child = new CreateDrawer(OpenMainPanel);
            child.LoadWindow(transform.Find("Panel"));
            child.Draw();
        }
    }
}