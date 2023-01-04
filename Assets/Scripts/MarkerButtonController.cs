using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マーカーボタンコントローラー
/// </summary>
public class MarkerButtonController : MonoBehaviour
{
    /// <summary>マーカーボタンに指先が入っていればtrue</summary>
    public bool isHand;

    /// <summary>
    /// 初期実行処理
    /// </summary>
    private void Start() {
        isHand = false;
    }

    /// <summary>
    /// マーカーボタンに指先が入っているか
    /// </summary>
    /// <return>マーカーボタンに指先が入っているか</return>
    public bool isMarkerHand() {
        return isHand;
    }

    /// <summary>
    /// マーカーボタンに指先が入った状態の場合のイベント処理
    /// </summary>
    /// <param name="other">Collider</param>
    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Hand")) {
            isHand = true;
        }

    }

    /// <summary>
    /// マーカーボタンに指先が入った時のイベント処理
    /// </summary>
    /// <param name="other">Collider</param>
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Hand")) {
            isHand = true;
        }

    }

    /// <summary>
    /// マーカーボタンから指先が出た時のイベント処理
    /// </summary>
    /// <param name="other">Collider</param>
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Hand")) {
            isHand = false;
        }

    }
}
