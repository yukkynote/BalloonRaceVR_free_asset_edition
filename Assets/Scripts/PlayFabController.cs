using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabController:MonoBehaviour
{
    [SerializeField] GetPlayerCombinedInfoRequestParams InfoRequestParams;
    void Start() {
        if (!Utility.USE_PLAYFAB) {
            return;
        }
        InfoRequestParams.GetUserData = true; // プレイヤーデータを取得する
        InfoRequestParams.GetTitleData = true; // タイトルデータを取得する

        PlayFabAuthService.Instance.InfoRequestParams = InfoRequestParams; // ここを追加!!
        PlayFabAuthService.OnLoginSuccess += PlayFabLogin_OnLoginSuccess;
        PlayFabAuthService.Instance.Authenticate(Authtypes.Silent);
    }
    void OnEnable() {
    }
    private void PlayFabLogin_OnLoginSuccess(LoginResult result) {
    }
    private void OnDisable() {
        if (!Utility.USE_PLAYFAB) {
            return;
        }
        PlayFabAuthService.OnLoginSuccess -= PlayFabLogin_OnLoginSuccess;
    }
    private void Update() {
    }
}