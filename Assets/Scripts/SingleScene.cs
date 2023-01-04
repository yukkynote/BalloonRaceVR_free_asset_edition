using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シングルシーンコントローラー
/// </summary>
/// <Remarks>
/// MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにするクラス
/// </Remarks>
public class SingleScene:MonoBehaviour
{
    /// <summary>プレイヤー開始地点配列</summary>
    public GameObject[] startPos;

    /// <summary>
    /// 初期実行処理
    /// </summary>
    private void Start() {
        // ランダムな座標に自身のアバター（ネットワークオブジェクト）を生成する
        int n = Random.Range(0, 4);
        Vector3 position = startPos[n].transform.position;
        GameObject sa = GameObject.FindGameObjectWithTag("SingleAvatar");
        GameObject avatar = Instantiate(sa, position, Quaternion.identity);
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.transform.parent = avatar.transform;
        camera.transform.position = new Vector3(avatar.transform.position.x, avatar.transform.position.y + 2.0f, avatar.transform.position.z);
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