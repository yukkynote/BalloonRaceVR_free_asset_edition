using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マーカーコントローラー
/// </summary>
public class MarkerController : MonoBehaviour
{
    /// <summary>マーカーが着地したらtrue</summary>
    public bool isGround;

    /// <summary>
    /// 初期実行処理
    /// </summary>
    private void Start() {
        isGround = false;
    }


    /// <summary>
    /// マーカーが地面に着地したか
    /// </summary>
    /// <return>マーカーが地面に着地したか</return>
    public bool isGroundEnter() {
        return isGround;
    }

    /// <summary>
    /// マーカーが地面に着地した状態の場合のイベント処理
    /// </summary>
    /// <param name="other">Collider</param>
    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Ground")) {
            isGround = true;
        }

    }

    /// <summary>
    /// マーカーが地面に着地した時のイベント処理
    /// </summary>
    /// <param name="other">Collider</param>
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Ground")) {
            isGround = true;
        }

    }

    /// <summary>
    /// マーカーが地面にから離れた時のイベント処理
    /// </summary>
    /// <param name="other">Collider</param>
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Ground")) {
            isGround = false;
        }

    }
}
