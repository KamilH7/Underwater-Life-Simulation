using UnityEngine;

public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
   private static T instance;

   public static T Instance 
   {
      get
      {
         if (instance == null)
         {
            T[] results = Resources.FindObjectsOfTypeAll<T>();

            if (results.Length == 0)
            {
               Debug.LogError("NO INSTANCE OF "+ typeof(T) +" HAS BEEN FOUND!");
            }
            else if(results.Length == 1)
            {
               instance = results[0];
               instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
            }
            else
            {
               Debug.LogError("MULTIPLE INSTANCES OF "+ typeof(T) +" HAS BEEN FOUND!");
            }
         }

         return instance;
      }
   }
}
