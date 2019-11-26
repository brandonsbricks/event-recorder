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
            List<string> names = new List<string> {tran.name};
            Transform currentTransform = tran;
            while (currentTransform.parent != null)
            {
                var parent = currentTransform.parent;
                names.Add(parent.name);
                currentTransform = parent;
            }
            StringBuilder finalName = new StringBuilder();
            for (int i = names.Count - 1; i >= 0; i--)
            {
                finalName.Append(names[i]);
                if (i > 0)
                {
                    finalName.Append("/");
                }
            }

            if (!IsUniqueName(tran))
            {
                int sibIndex = tran.GetSiblingIndex();
                finalName.Append($"{_indexCharacterSeparator}{sibIndex}");
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
        
        
        private static bool IsUniqueName(Transform tran)
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