using UnityEngine;

namespace CG.Bootstrapper
{
    internal class MagicMenuBootStrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void InitializeMagicMenu()
        {
            //Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Magic Menu")));
        }
    }
}