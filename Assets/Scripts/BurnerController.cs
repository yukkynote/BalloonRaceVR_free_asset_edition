using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バーナーボタンコントローラー
/// </summary>
public class BurnerController : MonoBehaviour
{
    /// <summary>バーナーボタンに指先が入っていればtrue</summary>
    public bool isHand;

    /// <summary>
    /// 初期実行処理
    /// </summary>
    private void Start() {
        isHand = false;
    }

    /// <summary>
    /// バーナーボタンに指先が入っているか
    /// </summary>
    /// <return>バーナーボタンに指先が入っているか</return>
    public bool isBurnerHand() {
        return isHand;
    }

    /// <summary>
    /// バーナーボタンに指先が入った状態の場合のイベント処理
    /// </summary>
    /// <param name="other">Collider</param>
    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Hand")) {
            isHand = true;
        }

    }

    /// <summary>
    /// バーナーボタンに指先が入った時のイベント処理
    /// </summary>
    /// <param name="other">Collider</param>
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Hand")) {
            isHand = true;
        }

    }
    /// <summary>
    /// バーナーボタンから指先が出た時のイベント処理
    /// </summary>
    /// <param name="other">Collider</param>
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Hand")) {
            isHand = false;
        }

    }
}
