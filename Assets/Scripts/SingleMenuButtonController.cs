using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シングルプレイシーンの各ボタンのコントローラー
/// </summary>
public class SingleMenuButtonController:MonoBehaviour
{
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
        // 接触したオブジェクトのタグが"Hand"のときのみ実行
        if (other.CompareTag("Hand")) {
            SinglePlayerController singlePlayerController = GameObject.Find("Player1").GetComponent<SinglePlayerController>();

            // ロード中に反応しないようにする処理
            if (singlePlayerController.isLoading) {
                return;
            }
            switch (this.name) {
                case "Tag1": // メニュー1
                    singlePlayerController.TouchTag(1);
                    break;
                case "Tag2": // メニュー2
                    singlePlayerController.TouchTag(2);
                    break;
                case "Tag3": // メニュー3
                    singlePlayerController.TouchTag(3);
                    break;
                case "Tag4": // メニュー4
                    singlePlayerController.TouchTag(4);
                    break;
                case "Tag5": // メニュー5
                    singlePlayerController.TouchTag(5);
                    break;
                case "Tag6": // メニュー6
                    singlePlayerController.TouchTag(6);
                    break;
                case "ButtonJa": // 言語：日本語
                    singlePlayerController.SetLang("ja");
                    break;
                case "ButtonEn": // 言語：英語
                    singlePlayerController.SetLang("en");
                    break;
                case "ButtonLeft": // 利き手：左手
                    singlePlayerController.SetHand("left");
                    break;
                case "ButtonRight": // 利き手：右手
                    singlePlayerController.SetHand("right");
                    break;
                case "ButtonSmall": // マップサイズ：小
                    singlePlayerController.SetMap("small");
                    break;
                case "ButtonMiddle": // マップサイズ：中
                    singlePlayerController.SetMap("middle");
                    break;
                case "ButtonLarge": // マップサイズ：大
                    singlePlayerController.SetMap("large");
                    break;
                case "ButtonVoiceOn": // ボイスナビ：ON
                    singlePlayerController.SetVoice("on");
                    break;
                case "ButtonVoiceOff": // ボイスナビ：OFF
                    singlePlayerController.SetVoice("off");
                    break;
                case "ButtonContinue": // ポーズ：ゲームに戻る
                    StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.LTouch));
                    StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.RTouch));
                    vibrateFlg = false;
                    singlePlayerController.Continue();
                    break;
                case "ButtonQuit": // ポーズ：ゲームをやめる
                case "ButtonReturn": // メニューに戻る
                    StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.LTouch));
                    StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.RTouch));
                    vibrateFlg = false;
                    singlePlayerController.isLoading = true;
                    singlePlayerController.Quit();
                    break;
                case "ButtonPause": // ポーズ
                    StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.LTouch));
                    StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.RTouch));
                    vibrateFlg = false;
                    singlePlayerController.Pause();
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
