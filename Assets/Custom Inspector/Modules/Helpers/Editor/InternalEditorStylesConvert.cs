using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using CustomInspector.Extensions;
using System;

namespace CustomInspector.Helpers
{
    public static class InternalEditorStylesConvert
    {
        public static GUIContent IconNameToGUIContent(string iconName)
        {
            Texture2D iconTexture = (Texture2D)typeof(EditorGUIUtility)
                            .GetMethod("LoadIcon", BindingFlags.NonPublic | BindingFlags.Static)
                            .Invoke(null, new object[] { iconName });

            if (iconTexture != null)
                return new GUIContent() { image = iconTexture };
            else
                return new GUIContent(iconName);
        }
        public static MessageType ToUnityMessageType(MessageBoxType type)
        {
            return type switch
            {
                MessageBoxType.None => MessageType.None,
                MessageBoxType.Info => MessageType.Info,
                MessageBoxType.Warning => MessageType.Warning,
                MessageBoxType.Error => MessageType.Error,
                _ => throw new NotImplementedException(type.ToString())
            };
        }
    }
}
