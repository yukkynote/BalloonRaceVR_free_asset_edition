using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ロープ(リップライン)ボタンコントローラー
/// </summary>
public class LineController : MonoBehaviour
{
    /// <summary>ロープボタンに指先が入っていればtrue</summary>
    public bool isHand;

    /// <summary>
    /// 初期実行処理
    /// </summary>
    private void Start() {
        isHand = false;
    }

    /// <summary>
    /// ロープボタンに指先が入っているか
    /// </summary>
    /// <return>ロープボタンに指先が入っているか</return>
    public bool isLineHand() {
        return isHand;
    }

    /// <summary>
    /// ロープボタンに指先が入った状態の場合のイベント処理
    /// </summary>
    /// <param name="other">Collider</param>
    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Hand")) {
            isHand = true;
        }

    }

    /// <summary>
    /// ロープボタンに指先が入った時のイベント処理
    /// </summary>
    /// <param name="other">Collider</param>
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Hand")) {
            isHand = true;
        }

    }

    /// <summary>
    /// ロープボタンから指先が出た時のイベント処理
    /// </summary>
    /// <param name="other">Collider</param>
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Hand")) {
            isHand = false;
        }

    }
}
