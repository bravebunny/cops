using UnityEngine;
using System.Collections;

// scripts that need to be run when a map is loaded should extend this
public abstract class RunOnMapBuild : MonoBehaviour {
    public abstract void Execute();

    public void Run() {
        Execute();
        // check if this script generated another one to run
        foreach (Transform child in transform) {
            RunOnMapBuild[] scripts = child.GetComponents<RunOnMapBuild>();
            foreach (RunOnMapBuild script in scripts)
                script.Run();
        }
    }
}
