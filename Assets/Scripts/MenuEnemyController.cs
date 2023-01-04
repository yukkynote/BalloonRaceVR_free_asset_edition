using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// メニューシーンのライバルプレイヤーコントローラー
/// </summary>
public class MenuEnemyController : MonoBehaviour
{
    // 値
    // 現地点のXYZ方向にかかる力
    public float xWind;
    public float yWind;
    public float zWind;
    // 経過時間(秒)
    public float timeElapsed;
    // フレームごとの経過時間
    float deltaTime;
    // 高度変化を維持する時間
    private float timeOut = 10.0f;
    // 風向きリスト
    private float[] yWindList = { -2.0f, -0.1f, 2.0f };

    // バルーンの炎パーティクル
    public ParticleSystem particle1;
    public ParticleSystem particle2;

    // オーディオソース
    public AudioSource soundObjectBurner; // バーナー音
    // オーディオクリップ
    public AudioClip burnerClip; // バーナー音

    // フラグ
    bool isPlayingSound = false; // バーナー音再生中フラグ

    /// <summary>
    /// 初期実行処理
    /// </summary>
    void Start() {
        // バーナーの炎パーティクルの初期化
        particle1.Stop();
        particle2.Stop();
    }

    /// <summary>
    /// 毎フレーム実行処理
    /// </summary>
    void Update() {
        // フレームごとの加算時間を取得して保管
        deltaTime = Time.deltaTime;
        // transformを取得
        Transform myTransform = this.transform;

        // 座標を取得
        Vector3 pos = myTransform.position;

        // タイマーを加算
        timeElapsed += deltaTime;
        // 一定時間高度変化を維持したら更新する
        if (timeElapsed >= timeOut) {
            // 上昇または下降をランダムで計算
            yWind = yWindList[Random.Range(0, yWindList.Length)];
            // 上昇する場合
            if (yWind > 0.0f) {
                // バーナーパーティクルを再生する
                particle1.Play();
                particle2.Play();
                // バーナー音を再生する
                if (isPlayingSound == false) {
                    AudioSource soundObjectBurner = GetComponent<AudioSource>();
                    soundObjectBurner.PlayOneShot(burnerClip, 0.5f);
                    isPlayingSound = true;
                }
            } else {
                // バーナーパーティクルは停止する
                particle1.Stop();
                particle2.Stop();
                // バーナー音も止める
                if (isPlayingSound == true) {
                    AudioSource soundObjectBurner = GetComponent<AudioSource>();
                    soundObjectBurner.Stop();
                    isPlayingSound = false;
                }
            }
            // タイマーをリセットする
            timeElapsed = 0.0f;
        }
        // 高度によってで風向きを切り替える
        if (transform.position.y < 1.0f) {
            xWind = 0.0f;
            zWind = 0.0f;
        } else if (transform.position.y < 50.0f) {
            xWind = 2.0f;
            zWind = 0.0f;
        } else if (transform.position.y < 100.0f) {
            xWind = 0.0f;
            zWind = 2.0f;
        } else if (transform.position.y < 150.0f) {
            xWind = -2.0f;
            zWind = 0.0f;
        } else if (transform.position.y < 200.0f) {
            xWind = 0.0f;
            zWind = -2.0f;
        } else {
            xWind = 0.0f;
            zWind = 0.0f;
        }
        // XYZ方向の移動距離を算出
        pos.x += xWind * deltaTime;
        pos.y += yWind * deltaTime;
        pos.z += zWind * deltaTime;

        myTransform.position = pos;  // 座標を設定

    }
}
