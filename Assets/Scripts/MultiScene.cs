using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// マルチシーンコントローラー
/// </summary>
/// <Remarks>
/// MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにするクラス
/// </Remarks>
public class MultiScene:MonoBehaviourPunCallbacks
{
    public GameObject[] startPos;
    public GameObject customHandLeft;
    public GameObject customHandRight;

    // フェード処理
    [SerializeField] OVRScreenFade fade;

    /// <summary>
    /// 初期実行処理
    /// </summary>
    private void Start() {
        // プレイヤー自身の名前を"Player"に設定する
        PhotonNetwork.NickName = "Player";

        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
        //PhotonNetwork.SendRate = 30;
        //PhotonNetwork.SerializationRate = 10;

        // フェード処理
        fade.SetExplicitFade(1);
    }

    /// <summary>
    /// マスターサーバーへの接続が成功した時に呼ばれるコールバック
    /// </summary>
    public override void OnConnectedToMaster() {
        // ランダムなルームに参加する
        PhotonNetwork.JoinRandomRoom();
    }

    /// <summary>
    /// ランダムマッチへの参加に失敗した場合の処理
    /// </summary>
    public override void OnJoinRandomFailed(short returnCode, string message) {
        // ルームの同時参加人数を4人までに設定する
        var roomOptions = new RoomOptions();
        roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        // 新規にルームを作成する
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    /// <summary>
    /// Photonのサーバーから切断された時に呼ばれるコールバック
    /// </summary>
    public override void OnDisconnected(DisconnectCause cause) {
        Debug.Log($"サーバーとの接続が切断されました: {cause.ToString()}");
        // メニューに戻る非同期処理を呼び出す
        StartCoroutine("LoadMenuSceneCoroutine");
    }

    /// <summary>
    /// ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    /// </summary>
    public override void OnJoinedRoom() {
        // 0-3までのランダムな開始位置を選択
        int n = Random.Range(0, 4);
        // 現在の部屋のプレイヤー数を取得する
        int num = PhotonNetwork.CurrentRoom.PlayerCount;
        Vector3 position = startPos[n].transform.position;
        // Avatar（ネットワークオブジェクト）を生成する
        GameObject avatar = PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
        // サーバー時刻を取得
        int currentTime = PhotonNetwork.ServerTimestamp;
        /*
        if (PhotonNetwork.IsMasterClient) {
            PhotonNetwork.CurrentRoom.SetStartTime(PhotonNetwork.ServerTimestamp);
        }
        */
        // ルームが満員になったら、以降そのルームへの参加を不許可にする
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers) {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        // フェード処理
        fade.SetExplicitFade(0);
        fade.FadeIn();
        // カメラをAvatar内のViewpointの子にしてプレイヤー視点で見えるようにする
        GameObject camera = GameObject.Find("OVRCameraRig");
        GameObject childObj = avatar.transform.Find("Viewpoint").gameObject;
        camera.transform.SetParent(childObj.transform, false);
        // カメラはViewPointの座標から若干ずらす
        camera.transform.position = new Vector3(childObj.transform.position.x+0.2f, childObj.transform.position.y, childObj.transform.position.z+0.4f);
        camera.transform.rotation = childObj.transform.rotation;

    }

    /// <summary>
    /// ルームから退出した時に呼ばれるコールバック
    /// </summary>
    public override void OnLeftRoom() {
        Debug.Log("ルームから退出しました");
        Debug.Log("Go to Menu");
        // Photonから切断する
        PhotonNetwork.Disconnect();
    }

    /// <summary>
    /// メニューシーンの非同期読み込み処理
    /// </summary>
    IEnumerator LoadMenuSceneCoroutine() {
        // 非同期でロードを開始
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MenuScene");

        // 非同期ロードが完了するまで待つ
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }
}