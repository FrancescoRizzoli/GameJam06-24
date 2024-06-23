// This class will read and store the game setting. It's porpuse is to avoid frequent I/O operations toword PlayerPrefs.

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

namespace Utility
{
    public static class Settings
    {
        public delegate void OnSettingsChange();
        public static event OnSettingsChange onSettingsChange;



        public static Dictionary<string, int> keysValues = new Dictionary<string, int>()
        {
            { nameof(SettingType.Music), 1 },
            { nameof(SettingType.Sfx), 1 },
            { nameof(SettingType.Vibration), 1 },
            { nameof(SettingType.Quality),1 }
        };


        /// <summary>
        /// initialize playe rpredd
        /// </summary>
        static Settings()
        {

            List<string> keys = new List<string>(keysValues.Keys);


            foreach (string key in keys)
            {
                if (PlayerPrefs.HasKey(key))
                {
                    keysValues[key] = PlayerPrefs.GetInt(key);
                }
            }


        }

        /// <summary>
        /// save player predd
        /// </summary>
        public static void SaveSettings()
        {
            foreach (string key in keysValues.Keys)
            {
                PlayerPrefs.SetInt(key, keysValues[key]);
            }

            PlayerPrefs.Save();
        }

        /// <summary>
        /// change settings dont change player pref
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetInt(string key, int value)
        {
            if (keysValues.ContainsKey(key))
            {
                keysValues[key] = value;
                onSettingsChange?.Invoke();
            }
        }
    }
}
