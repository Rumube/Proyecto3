using UnityEditor.Scripting.Python;
using UnityEditor;
using UnityEngine;

public class Stadistics : MonoBehaviour
{
    //    [MenuItem("Python/Hello World")]
    //    static void PrintHelloWorldFromPython()
    //    {
    //        PythonRunner.RunString(@"
    //                import UnityEngine;
    //                UnityEngine.Debug.Log('hello world')
    //                ");
    //    }
    [MenuItem("Python/Ensure Naming")]
    static void RunEnsureNaming()
    {
        PythonRunner.RunFile($"{Application.dataPath}/Stadistics.py");
    }

    static void prueba()
    {
        PythonRunner.RunString(@"
                    import Stadistics
                    Stadistics.prueba()
                    ");
    }
    private void Start()
    {
        print(Application.dataPath);
        prueba();
    }
}
