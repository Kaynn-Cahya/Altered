using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using io.newgrounds.components;
using io.newgrounds;
using io.newgrounds.objects;
using io.newgrounds.results;

public class NewgroundsManager : Singleton<NewgroundsManager> {

    [SerializeField]
    private core newgroundCore;

    [SerializeField]
    private bool isNewgroundsBuild;

    private bool isLoggedIn;

    void Start() {
        isLoggedIn = false;

        if (!isNewgroundsBuild) {
            return;
        }
        CheckAndRequestLogin();
    }

    #region LoginLogout

    void onLoggedIn() {
        user player = newgroundCore.current_user;
        isLoggedIn = true;
    }

    void onLoginFailed() {
        error error = newgroundCore.login_error;
    }

    void onLoginCancelled() {
    }

    void requestLogin() {
        newgroundCore.requestLogin(onLoggedIn, onLoginFailed, onLoginCancelled);
    }

    void cancelLogin() {
        newgroundCore.cancelLoginRequest();
    }

    void CheckAndRequestLogin() {
        newgroundCore.onReady(() => {

            // Call the server to check login status
            newgroundCore.checkLogin((bool logged_in) => {

                if (logged_in) {
                    onLoggedIn();
                } else {
                    requestLogin();
                }
            });
        });
    }

    // Check if the user has a saved login when your game starts

    #endregion
}
