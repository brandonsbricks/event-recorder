using System;
using BRM.EventRecorder.UnityUi;
using UnityEngine;

namespace BRM.EventAnalysis.UnityPlayback
{
    public static class GameObjectFinder
    {
        public static GameObject Find(string indexedHierarchyName)
        {
            if (!indexedHierarchyName.Contains(UnityNamingUtils.IndexCharacterSeparator))
            {
                return GameObject.Find(indexedHierarchyName);
            }

            var gameObjectNames = indexedHierarchyName.Split(UnityNamingUtils.GameObjectHierarchySeparatorChr);
            Transform currentChild = null;
            for (int i = 0; i < gameObjectNames.Length; i++)
            {
                var currentGoName = gameObjectNames[i];

                if (TryGetChildIndex(currentGoName, out var childIndex))
                {
                    currentGoName = GetCleanedGameObjectName(currentGoName);
                    currentChild = currentChild != null ? currentChild.transform.GetChild(childIndex) : GameObject.Find(currentGoName).transform;
                }
                else
                {
                    currentChild = currentChild != null ? currentChild.transform.Find(currentGoName) : GameObject.Find(currentGoName).transform;
                }
            }

            return currentChild.gameObject;
        }

        private static bool TryGetChildIndex(string gameObjectName, out int childIndex)
        {
            childIndex = -1;
            var indexOfSeparator = gameObjectName.IndexOf(UnityNamingUtils.IndexCharacterSeparator, StringComparison.Ordinal);
            if (indexOfSeparator > 0)
            {
                var stringChildIndex = gameObjectName[gameObjectName.Length - 1].ToString();
                return int.TryParse(stringChildIndex, out childIndex);
            }

            return false;
        }

        private static string GetCleanedGameObjectName(string gameObjectName)
        {
            var indexOfSeparator = gameObjectName.IndexOf(UnityNamingUtils.IndexCharacterSeparator, StringComparison.Ordinal);
            if (indexOfSeparator <= 0)
            {
                return gameObjectName;
            }

            var separatorLength = UnityNamingUtils.IndexCharacterSeparator.Length;
            return gameObjectName.Substring(0, gameObjectName.Length - separatorLength);
        }
    }
}