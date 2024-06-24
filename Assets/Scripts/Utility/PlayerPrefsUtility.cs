using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

namespace RPG.Utility
{
    public static class PlayerPrefsUtility
    {
        public static void SetString(string key, List<string> value)
        {
            string formattedValue = String.Join(",", value);

            PlayerPrefs.SetString(key, formattedValue);
        }

        public static List<string> GetString(string key)
        {
            string unformattedValue = PlayerPrefs.GetString(key);

            List<string> formattedValue = new List<string>(unformattedValue.Split(','));

            if (unformattedValue.Length == 0 && formattedValue.Count == 1)
                formattedValue.RemoveAt(0);

            return formattedValue;
        }


    }
}