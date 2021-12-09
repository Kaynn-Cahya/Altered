using System;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static T instance;

    public static T Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<T>();
            }
            return instance;
        }
    }

	protected void InitializeSingleton(bool persistent = true) {
		if (instance == null) {
			instance = (T)Convert.ChangeType(this, typeof(T));
			if (persistent)
				DontDestroyOnLoad(instance);
		} else {
			Debug.LogWarning($"Another instance of Singleton<{typeof(T).Name}> detected on GO {name}. Destroyed", gameObject);
			Destroy(this);
		}
	}
}
