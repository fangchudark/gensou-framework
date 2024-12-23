using GensouLib.GenScript;
using UnityEngine;

public class GenScriptInitialization : MonoBehaviour
{
    private void Awake()
    {
        if (FindObjectsOfType<GenScriptInitialization>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        ScriptReader.ReadAndExecute();        
    }
}
