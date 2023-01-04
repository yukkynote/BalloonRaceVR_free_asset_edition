using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シングルプレイシーンのライバルプレイヤーコントローラー
/// </summary>
public class EnemyController : MonoBehaviour
{
    // フラグ
    bool isPlaying = false; // プレイ中フラグ
    bool isStart = false; // ゲーム開始フラグ
    public bool isGoal = false; // ゴールフラグ
    public bool isMarkerDrop = false; // マーカーを落としたフラグ
    bool isPlayingSound = false; // バーナー音再生中フラグ

    // 値
    // 現地点のXYZ方向にかかる力
    public float xWind;
    public float yWind;
    public float zWind;
    // XYZ各方向の加速度
    float xVelocity = 0.0f;
    float yVelocity = 0.0f;
    float zVelocity = 0.0f;
    // 各高度の風力(十の位以上)と45度単位風向き(一の位)設定
    int[] windPowers;
    // 風力
    int windPower;
    // フレームごとの経過時間
    float deltaTime;
    // 経過時間(秒)
    public float timeElapsed;
    // 切り替えの経過時間タイマー
    public float changeTimeElapsed;
    // クリアタイム
    public int clearTime;
    // 制限時間
    float timeLimit;
    // ターゲットまでの距離のレコードデータ(m単位)
    float distanceRecord;
    // ターゲットまでの距離のレコードデータ(mm単位)
    int distanceRecordData;
    // プレイヤーからターゲットまでのXZ座標上の距離
    float goalDistance = 0.0f;
    // 1判定前のプレイヤーからターゲットまでのXZ座標上の距離
    float beforeGoalDistance = 0.0f;
    // 将来的なゴールの角度
    float futureGoalAngle = 0.0f;
    // ゴールの角度
    float goalAngle = 0.0f;
    // ターゲットへの角度
    float currentAngleDistance; // 現在の高度
    float lowerAngleDistance; // 1段階下の高度
    float upperAngleDistance; // 1段階上の高度
    // 乱数値
    float randVal;

    // バルーンの炎パーティクル
    public ParticleSystem particle1;
    public ParticleSystem particle2;

    // オーディオソース
    public AudioSource soundObjectBurner; // バーナー音
    // オーディオクリップ
    public AudioClip burnerClip; // バーナー音

    // ゲームオブジェクト
    // ゴールターゲット
    public GameObject goalTarget;
    // マーカープレハブ
    public GameObject markerPrefab;
    // プレイヤーオブジェクト
    GameObject playerObj;
    // マーカー
    GameObject sandbag;

    // 各コントローラー
    SinglePlayerController playerScript;// プレイヤーコントローラー
    MarkerController markerController; // マーカーコントローラー

    /// <summary>
    /// 初期実行処理
    /// </summary>
    void Start() {
        // バーナーの炎パーティクルの初期化
        particle1.Stop();
        particle2.Stop();
        // 制限時間(秒)
        timeLimit = Utility.TIME_LIMIT;
        // 開始前タイマー設定
        timeElapsed = 3.5f;
        // クリアタイム初期化
        clearTime = 0;
        // プレイヤーのスコアデータ初期化
        PlayerPrefs.SetInt("Score_" + this.name, 999999);
        // プレイヤーオブジェクト取得
        playerObj = GameObject.Find("Player1");
        // プレイヤーコントローラー取得
        playerScript = playerObj.GetComponent<SinglePlayerController>();
        // マーカーコントローラー取得
        markerController = markerPrefab.GetComponent<MarkerController>();
        // 乱数値セット
        randVal = UnityEngine.Random.value;
        // プレイ開始座標をランダム値を元に更新
        transform.position = new Vector3(transform.position.x + (randVal * 100.0f) - 100, transform.position.y + (randVal * 10.0f) + 10, transform.position.z + (randVal * 100.0f) - 100);
    }

    /// <summary>
    /// 毎フレーム実行処理
    /// </summary>
    void Update() {
        // フレームごとの加算時間を取得して保管
        deltaTime = Time.deltaTime;
        // プレイ中であれば
        if (isPlaying) {
            // transformを取得
            Transform myTransform = this.transform;
            // ターゲットオブジェクトがあれば
            if (goalTarget != null) {
                // 高さを考慮しないXZ座標軸上でのターゲットとの距離を算出
                goalDistance = (
                    new Vector3(goalTarget.transform.position.x,0.0f, goalTarget.transform.position.z)
                    -
                    new Vector3(transform.position.x, 0.0f, transform.position.z)
                    ).magnitude;
                // 乱数地を元にターゲットとの距離および残り時間でマーカー投下可否のフィルタ条件を設定
                if ((goalDistance < (50.0f * randVal / 2.0f) + 5.0f) || timeElapsed > 120.0f + (60.0f * randVal)) {
                    // ターゲットとの距離が離れ始めるタイミングであれば最接近していると判断
                    if (beforeGoalDistance < goalDistance) {
                        // マーカー投下
                        // レンダラー・コライダー・物理演算・重力を有効にする
                        markerPrefab.gameObject.transform.Find("Sandbag").GetComponent<MeshRenderer>().enabled = true;
                        markerPrefab.gameObject.transform.Find("Cloth").GetComponent<MeshRenderer>().enabled = true;
                        markerPrefab.gameObject.transform.Find("Handle").GetComponent<BoxCollider>().enabled = true;
                        markerPrefab.gameObject.transform.Find("Sandbag").GetComponent<BoxCollider>().enabled = true;
                        markerPrefab.GetComponent<BoxCollider>().enabled = true;
                        markerPrefab.AddComponent<Rigidbody>();
                        markerPrefab.GetComponent<Rigidbody>().useGravity = true;
                        markerPrefab.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                        // 親子関係を解除して自由落下させる
                        markerPrefab.transform.parent = null;
                        // マーカーを落としたフラグを立てる
                        isMarkerDrop = true;
                        // マーカーのsandbagとターゲット間の距離をスコアとして記録する(移動したら都度更新)
                        sandbag = markerController.transform.Find("Sandbag").gameObject;
                        distanceRecord = (goalTarget.transform.position - sandbag.transform.position).magnitude;
                        // スコアデータはmm単位の整数値にする
                        distanceRecordData = (int)(distanceRecord * 1000.0f);
                        // プレイヤー名を添えたPlayerPrefsデータに記録する
                        PlayerPrefs.SetInt("Score_" + this.name, distanceRecordData);
                    }
                }
                // 1フレーム前のターゲットとの距離を保管する
                beforeGoalDistance = goalDistance;
                // 将来的なゴールとの角度を予測
                futureGoalAngle = GetAngle(
                    new Vector2(transform.position.x + xWind * (100.0f * randVal + 50.0f), transform.position.z + zWind * (100.0f * randVal + 50.0f)),
                    new Vector2(goalTarget.transform.position.x, goalTarget.transform.position.z)
                );
                // ゴールとの角度を計算
                goalAngle = GetAngle(
                    new Vector2(transform.position.x, transform.position.z),
                    new Vector2(goalTarget.transform.position.x, goalTarget.transform.position.z)
                );
            }
            // 座標を取得
            Vector3 pos = myTransform.position;

            // 現在座標をグリッドの座標に置き換える
            int iX = (int)Math.Floor((transform.position.x + 480.0f) / 2.0f / 20.0f);
            int iY = (int)Math.Floor(transform.position.y / 1.0f / 20.0f);
            int iZ = (int)Math.Floor((transform.position.z + 480.0f) / 2.0f / 20.0f);
            // タイマーを加算
            timeElapsed += deltaTime;
            changeTimeElapsed += deltaTime;
            // 十の位以上が風力、一の位が45度単位の風向きになっている高度別の風力設定データを取得
            windPowers = Utility.WIND_POWERS;
            // 風向きを取得
            int dirId = windPowers[iY] % 10;
            float yRot = (float)(dirId * 45); // Y軸の角度に変換する
            windPower = (int)Math.Floor((double)(windPowers[iY] / 10)); // 風力
            xWind = (float)Math.Cos(yRot * Mathf.Deg2Rad) * windPower * -1; // X方向の力
            zWind = (float)Math.Sin(yRot * Mathf.Deg2Rad) * windPower; // Z方向の力
            // 現在の高度での将来的なゴールへの角度を算出
            currentAngleDistance = (float)Math.Cos((futureGoalAngle - yRot) * Mathf.Deg2Rad);
            lowerAngleDistance = (float)Math.Cos((futureGoalAngle - (float)(windPowers[iY - 1] % 10 * 45)) * Mathf.Deg2Rad);
            upperAngleDistance = (float)Math.Cos((futureGoalAngle - (float)(windPowers[iY + 1] % 10 * 45)) * Mathf.Deg2Rad);

            // 高度の下限に達していればそれ以上は下がらないようにする
            if (iY > 1) {
                // 一段階下の高度で将来的なゴールへの角度を算出
                lowerAngleDistance = (float)Math.Cos((goalAngle - (float)(windPowers[iY - 1] % 10 * 45)) * Mathf.Deg2Rad);
            } else {
                lowerAngleDistance = -1.0f;
            }
            // 高度の上限に達していればそれ以上は上がらないようにする
            if (iY < windPowers.Length - 1) {
                // 一段階上の高度で将来的なゴールへの角度を算出
                upperAngleDistance = (float)Math.Cos((futureGoalAngle - (float)(windPowers[iY + 1] % 10 * 45)) * Mathf.Deg2Rad);
            } else {
                upperAngleDistance = -1.0f;
            }
            // 上の高度の角度よりも下の高度の角度の方が大きく
            if (lowerAngleDistance > upperAngleDistance) {
                // 現在の高度の角度よりも下の高度の角度の方が大きければ
                if (lowerAngleDistance > currentAngleDistance) {
                    // 降下させる
                    yWind = -3.0f;
                    // バーナーパーティクルは停止する
                    particle1.Stop();
                    particle2.Stop();
                    // バーナー音も止める
                    if (isPlayingSound == true) {
                        AudioSource soundObjectBurner = GetComponent<AudioSource>();
                        soundObjectBurner.Stop();
                        isPlayingSound = false;
                    }
                // 現在の高度の角度よりも下の高度の角度の方が小さければ
                } else {
                    // 高度を維持する
                    yWind = 0.0f;
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
            // 上の高度の角度よりも下の高度の角度の方が小さく
            } else {
                // 現在の高度の角度よりも上の高度の角度の方が大きければ
                if (upperAngleDistance > currentAngleDistance) {
                    // 上昇させる
                    yWind = 3.0f;
                    // バーナーパーティクルを再生する
                    particle1.Play();
                    particle2.Play();
                    // バーナー音を再生する
                    if (isPlayingSound == false) {
                        AudioSource soundObjectBurner = GetComponent<AudioSource>();
                        soundObjectBurner.PlayOneShot(burnerClip, 0.5f);
                        isPlayingSound = true;
                    }
                // 現在の高度の角度よりも上の高度の角度の方が小さければ
                } else {
                    // 高度を維持する
                    yWind = 0.0f;
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
            }
            // プレイヤーがポーズ状態でなければ
            if (!playerScript.isPause) {
                // XYZ方向の加速度を算出
                xVelocity = xWind * deltaTime * 3.0f;
                yVelocity = yWind * deltaTime * 3.0f;
                zVelocity = zWind * deltaTime * 3.0f;
                // XYZ方向の移動距離を算出
                pos.x += xVelocity * deltaTime * 4.0f;
                pos.y += yVelocity * deltaTime * 4.0f;
                pos.z += zVelocity * deltaTime * 4.0f;
                myTransform.position = pos;  // 座標を設定
                // マーカーを落として地面と接触したらゴール判定
                if ((!isGoal && isMarkerDrop && markerController.isGroundEnter())) {
                    isGoal = true;
                }
            }
        // プレイ中でなければ
        } else {
            // 未スタートであれば
            if (!isStart) {
                // カウントダウン処理
                timeElapsed -= deltaTime;
                // 0に達したら開始処理
                if (timeElapsed <= 0.0f) {
                    timeElapsed = 0.0f;
                    changeTimeElapsed = 0.0f;
                    isPlaying = true;
                    isStart = true;
                // それまでは何もしない
                } else if (timeElapsed < 3.0f) {
                }
            }
        }

    }

    /// <summary>
    /// 2次元座標間の角度算出処理
    /// </summary>
    /// <param name="start">基点座標</param>
    /// <param name="target">ターゲット座標</param>
    /// <returns>基点とターゲット座標間の角度</returns>
    float GetAngle(Vector2 start, Vector2 target) {
        // 2点間ベクトルを計算
        Vector2 dt = target - start;
        // ラジアン値を算出
        float rad = Mathf.Atan2(dt.y, dt.x);
        // 度数に変換
        float degree = rad * Mathf.Rad2Deg;
        // 角度として返す
        return degree;
    }

    /// <summary>
    /// 他コントローラーからの呼び出しでゴール状態を返す処理
    /// </summary>
    public bool isEnemyGoal() {
        return isGoal;
    }
}
