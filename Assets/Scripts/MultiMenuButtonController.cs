using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// マルチプレイシーンの各ボタンのコントローラー
/// </summary>
public class MultiMenuButtonController:MonoBehaviour
{
    // アバターオブジェクト
    public GameObject AvatarObj;
    bool vibrateFlg = true;
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
        // アバターオブジェクトを取得
        if (AvatarObj == null) {
            AvatarObj = GameObject.Find("Avatar(Clone)");
        }
        // 接触したオブジェクトのタグが"Hand"のときのみ実行
        if (other.CompareTag("Hand")) {
            MultiPlayerController multiPlayerController = AvatarObj.GetComponent<MultiPlayerController>();

            // ロード中に反応しないようにする処理
            if (multiPlayerController.isLoading) {
                return;
            }
            switch (this.name) {
                case "Tag1": // メニュー1
                    multiPlayerController.TouchTag(1);
                    break;
                case "Tag2": // メニュー2
                    multiPlayerController.TouchTag(2);
                    break;
                case "Tag3": // メニュー3
                    multiPlayerController.TouchTag(3);
                    break;
                case "Tag4": // メニュー4
                    multiPlayerController.TouchTag(4);
                    break;
                case "Tag5": // メニュー5
                    multiPlayerController.TouchTag(5);
                    break;
                case "Tag6": // メニュー6
                    multiPlayerController.TouchTag(6);
                    break;
                case "ButtonJa": // 言語：日本語
                    multiPlayerController.SetLang("ja");
                    break;
                case "ButtonEn": // 言語：英語
                    multiPlayerController.SetLang("en");
                    break;
                case "ButtonLeft": // 利き手：左手
                    multiPlayerController.SetHand("left");
                    break;
                case "ButtonRight": // 利き手：右手
                    multiPlayerController.SetHand("right");
                    break;
                case "ButtonSmall": // マップサイズ：小
                    multiPlayerController.SetMap("small");
                    break;
                case "ButtonMiddle": // マップサイズ：中
                    multiPlayerController.SetMap("middle");
                    break;
                case "ButtonLarge": // マップサイズ：大
                    multiPlayerController.SetMap("large");
                    break;
                case "ButtonVoiceOn": // ボイスナビ：ON
                    multiPlayerController.SetVoice("on");
                    break;
                case "ButtonVoiceOff": // ボイスナビ：OFF
                    multiPlayerController.SetVoice("off");
                    break;
                case "ButtonContinue": // ポーズ：ゲームに戻る
                    StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.LTouch));
                    StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.RTouch));
                    vibrateFlg = false;
                    multiPlayerController.Continue();
                    break;
                case "ButtonQuit": // ポーズ：ゲームをやめる
                case "ButtonReturn": // メニューに戻る
                    StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.LTouch));
                    StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.RTouch));
                    vibrateFlg = false;
                    multiPlayerController.isLoading = true;
                    multiPlayerController.Quit();
                    break;
                case "ButtonPause": // ポーズ
                    StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.LTouch));
                    StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.RTouch));
                    vibrateFlg = false;
                    multiPlayerController.Pause();
                    break;
                default:// どれにも合致しない場合は処理を抜ける
                    return;
            }
            // 両手にバイブレーション
            if (vibrateFlg) {
                StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.LTouch));
                StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.RTouch));
            }
        }
    }
}
