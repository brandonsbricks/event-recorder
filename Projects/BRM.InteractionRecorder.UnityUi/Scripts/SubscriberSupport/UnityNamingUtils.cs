using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BRM.InteractionRecorder.UnityUi
{
    public static class UnityNamingUtils
    {
        private const string _indexCharacterSeparator = ";i=";

        public static string GetHierarchyName(Transform tran)
        {
            var names = GetUniqueNamesToRoot(tran);
            return BuildName(names);
        }

        /// <summary>
        /// names ordered from the given transform up until the root transform
        /// Non-unique transforms in the hierarchy are modified to indicate their sibling index
        /// </summary>
        private static List<string> GetUniqueNamesToRoot(Transform tran)
        {
            List<string> names = new List<string>();
            Transform currentTransform = tran;
            while (currentTransform != null)
            {
                var uniqueName = currentTransform.name;
                if (!IsUniqueName(currentTransform))
                {
                    int sibIndex = currentTransform.GetSiblingIndex();
                    uniqueName = $"{uniqueName}{_indexCharacterSeparator}{sibIndex}";
                }
                names.Add(uniqueName);
                currentTransform = currentTransform.parent;
            }

            return names;
        }

        private static string BuildName(List<string> names)
        {
            StringBuilder finalName = new StringBuilder();
            for (int i = names.Count - 1; i >= 0; i--)
            {
                finalName.Append(names[i]);
                if (i > 0)
                {
                    finalName.Append("/");
                }
            }

            return finalName.ToString();
        }

        public static string GetComponentTypeNames(List<Component> components)
        {
            if (components == null)
            {
                return null;
            }

            var builder = new StringBuilder();
            for (int i = 0; i < components.Count; i++)
            {
                var comp = components[i];
                builder.Append(comp.GetType());//todo: measure performance, maybe work around the reflection
                if (i < components.Count - 1)
                {
                    builder.Append(';');
                }
            }

            return builder.ToString();
        }
        
        
        private static bool IsUniqueName(Transform tran)//bug: when two root element gameobjects share a name, this will not distinguish them
        {
            var parent = tran.parent;
            if (ReferenceEquals(parent, null))
            {
                return true;
            }

            var childCount = parent.childCount;

            for (int i = 0; i < childCount; i++)
            {
                var child = parent.GetChild(i);
                if (!ReferenceEquals(child, tran) && child.name == tran.name)
                {
                    return false;
                }
            }

            return true;
        }
    }
}