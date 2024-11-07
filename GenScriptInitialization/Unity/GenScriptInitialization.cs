using GensouLib.GenScript;
using UnityEngine;

public class GenScriptInitialization : MonoBehaviour
{
    private void Awake()
    {
        if (FindObjectsOfType<GenScriptInitialization>().Length > 1)
        {
            Destroy(gameObject); // 防止创建多个实例
            return;
        }

        DontDestroyOnLoad(gameObject); // 保持在场景切换时不被销毁
        ScriptReader.Initialization();        
    }
}
