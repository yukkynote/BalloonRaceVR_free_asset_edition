using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// メニューシーンの各ボタンのコントローラー
/// </summary>
public class MenuButtonController:MonoBehaviour
{
    // オーディオソース
    public AudioSource soundObject; // 効果音用
    // オーディオクリップ
    public AudioClip clickClip; // クリック音
    public AudioClip flipClip; // ページめくり音

    // フェード処理
    [SerializeField] OVRScreenFade fade;

    /// <summary>
    /// ボタンに他のオブジェクトが接触した時の処理
    /// </summary>
    /// <param name="other">接触したオブジェクト</param>
    void OnTriggerEnter(Collider other) {
        // ハンドトリガーを押しているとHandが接触したように判定されてしまう不具合の回避処理
        if (OVRInput.Get(OVRInput.RawButton.LHandTrigger) ||
            OVRInput.Get(OVRInput.RawButton.RHandTrigger)) {
            return;
        }
        // 接触したオブジェクトのタグが"Hand"のときのみ実行
        if (other.CompareTag("Hand")) {
            MenuController menuController = GameObject.Find("MenuController").GetComponent<MenuController>();
            // ロード中に反応しないようにする処理
            if (menuController.isLoading) {
                return;
            }
            switch (this.name) {
                case "ButtonSingle": // シングルプレイ
                    fade.FadeOut();
                    if (clickClip != null) {
                        soundObject.PlayOneShot(clickClip);
                    }
                    menuController.isLoading = true;
                    StartCoroutine("LoadSingleSceneCoroutine");
                    break;
                case "ButtonMulti": // マルチプレイ
                    fade.FadeOut();
                    if (clickClip != null) {
                        soundObject.PlayOneShot(clickClip);
                    }
                    menuController.isLoading = true;
                    StartCoroutine("LoadMultiSceneCoroutine");
                    break;
                case "Tag1": // メニュー1
                    menuController.TouchTag(1);
                    break;
                case "Tag2": // メニュー2
                    menuController.TouchTag(2);
                    break;
                case "Tag3": // メニュー3
                    menuController.TouchTag(3);
                    break;
                case "Tag4": // メニュー4
                    menuController.TouchTag(4);
                    break;
                case "Tag5": // メニュー5
                    menuController.TouchTag(5);
                    break;
                case "Tag6": // メニュー6
                    menuController.TouchTag(6);
                    break;
                case "ButtonJa": // 言語：日本語
                    menuController.SetLang("ja");
                    break;
                case "ButtonEn": // 言語：英語
                    menuController.SetLang("en");
                    break;
                case "ButtonLeft": // 利き手：左手
                    menuController.SetHand("left");
                    break;
                case "ButtonRight": // 利き手：右手
                    menuController.SetHand("right");
                    break;
                case "ButtonSmall": // マップサイズ：小
                    menuController.SetMap("small");
                    break;
                case "ButtonMiddle": // マップサイズ：中
                    menuController.SetMap("middle");
                    break;
                case "ButtonLarge": // マップサイズ：大
                    menuController.SetMap("large");
                    break;
                case "ButtonVoiceOn": // ボイスナビ：ON
                    menuController.SetVoice("on");
                    break;
                case "ButtonVoiceOff": // ボイスナビ：OFF
                    menuController.SetVoice("off");
                    break;
                default:// どれにも合致しない場合は処理を抜ける
                    return;
            }
            // 両手にバイブレーション
            StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.LTouch));
            StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.RTouch));
        }
    }

    /// <summary>
    /// シングルプレイメニューの非同期ロード処理
    /// </summary>
    IEnumerator LoadSingleSceneCoroutine() {
        // フェード時間を稼ぐために3秒間待機
        yield return new WaitForSeconds(3.0f);
        // 非同期でロードを開始
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SingleScene");

        // 非同期ロードが完了するまで待つ
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }

    /// <summary>
    /// マルチプレイメニューの非同期ロード処理
    /// </summary>
    IEnumerator LoadMultiSceneCoroutine() {
        // フェード時間を稼ぐために3秒間待機
        yield return new WaitForSeconds(3.0f);
        // 非同期でロードを開始
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MultiScene");

        // 非同期ロードが完了するまで待つ
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }
}
