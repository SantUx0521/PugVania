using System.Collections.Generic;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace UnityEngine.Localization.Samples
{
    [RequireComponent(typeof(Dropdown))]
    public class LanguageSelectionMenuUGUIDropdown : MonoBehaviour
    {
        Dropdown m_Dropdown;
        AsyncOperationHandle m_InitializeOperation;

        void Start()
        {
            m_Dropdown = GetComponent<Dropdown>();
            m_Dropdown.onValueChanged.AddListener(OnSelectionChanged);

            m_Dropdown.ClearOptions();
            m_Dropdown.options.Add(new Dropdown.OptionData("Loading..."));
            m_Dropdown.interactable = false;

            m_InitializeOperation = LocalizationSettings.SelectedLocaleAsync;
            if (m_InitializeOperation.IsDone)
            {
                InitializeCompleted(m_InitializeOperation);
            }
            else
            {
                m_InitializeOperation.Completed += InitializeCompleted;
            }
        }

        void InitializeCompleted(AsyncOperationHandle obj)
        {
            var options = new List<string>();
            int selectedOption = 0;
            var locales = LocalizationSettings.AvailableLocales.Locales;
            for (int i = 0; i < locales.Count; ++i)
            {
                var locale = locales[i];
                if (LocalizationSettings.SelectedLocale == locale)
                    selectedOption = i;

                options.Add(locales[i].ToString());
            }

            if (options.Count == 0)
            {
                options.Add("No Locales Available");
                m_Dropdown.interactable = false;
            }
            else
            {
                m_Dropdown.interactable = true;
            }

            m_Dropdown.ClearOptions();
            m_Dropdown.AddOptions(options);
            m_Dropdown.SetValueWithoutNotify(selectedOption);

            LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
        }

        void OnSelectionChanged(int index)
        {
            LocalizationSettings.SelectedLocaleChanged -= LocalizationSettings_SelectedLocaleChanged;

            var locale = LocalizationSettings.AvailableLocales.Locales[index];
            LocalizationSettings.SelectedLocale = locale;

            LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
        }

        void LocalizationSettings_SelectedLocaleChanged(Locale locale)
        {
            var selectedIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(locale);
            m_Dropdown.SetValueWithoutNotify(selectedIndex);
        }
    }
}
