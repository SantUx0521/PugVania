    using System.Collections;
    using UnityEngine;
    using UnityEngine.Localization.Settings;

    public class LanguajeFunction : MonoBehaviour
    {
        private bool _active = false;
        void Start()
        {
            int ID = PlayerPrefs.GetInt("Language", 0);
            ChangeLocale(ID);
        }

        public void ChangeLocale(int localID)
        {
            if (_active)
            {
                return;
            }
            StartCoroutine(setLocaleID(localID));
        }

        public IEnumerator setLocaleID(int localID)
        {
            _active = true;
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localID];
            PlayerPrefs.SetInt("Language", localID);
            _active = false;
        }
    }
