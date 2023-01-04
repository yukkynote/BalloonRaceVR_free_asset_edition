using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using PlayFab.Json;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

/// <summary>
/// シングルプレイシーンのコントローラー
/// </summary>
public class SinglePlayerController:MonoBehaviour
{
    // プレイヤーのバルーン
    public GameObject Balloon1;
    public GameObject Balloon2;
    public GameObject Balloon3;
    public GameObject Balloon4;
    // 敵のゲームオブジェクト格納用配列
    public GameObject[] Enemies;
    // 敵の数
    int enemyCount = 8;
    // 設定メニューボタン
    public GameObject ButtonJa;
    public GameObject ButtonEn;
    public GameObject ButtonLeft;
    public GameObject ButtonRight;
    public GameObject ButtonSmall;
    public GameObject ButtonMiddle;
    public GameObject ButtonLarge;
    public GameObject ButtonVoiceOn;
    public GameObject ButtonVoiceOff;
    // ポーズメニューボタン
    public GameObject ButtonContinue;
    public GameObject ButtonQuit;
    // ポーズボタン
    public GameObject ButtonPause;
    // メニューに戻るボタン
    public GameObject ButtonReturn;
    // 付箋タブベース
    public GameObject Tag1Base;
    public GameObject Tag2Base;
    public GameObject Tag3Base;
    public GameObject Tag4Base;
    public GameObject Tag5Base;
    public GameObject Tag6Base;
    // 付箋タブ(選択時に表示)
    public GameObject Tag1Active;
    public GameObject Tag2Active;
    public GameObject Tag3Active;
    public GameObject Tag4Active;
    public GameObject Tag5Active;
    public GameObject Tag6Active;
    // 付箋タブボタン
    public GameObject Tag1Button;
    public GameObject Tag2Button;
    public GameObject Tag3Button;
    public GameObject Tag4Button;
    public GameObject Tag5Button;
    public GameObject Tag6Button;

    // メニュークリップボード
    public GameObject Clipboard;
    // ハンドコントローラー
    public GameObject LeftHandAnchor;
    public GameObject RightHandAnchor;
    // 付箋タブグループ
    public GameObject StickyGroup;
    // レースメニュー ゲームタイトルパーツ
    public GameObject TitleGroup;

    // マーカーを持たせる右手指先オブジェクト
    public GameObject RIndex;
    // 指先コライダー
    public GameObject RHandIndex;
    public GameObject LHandIndex;
    // アバター
    public GameObject[] avatars;
    // ボタン
    public GameObject burnerButton; // バーナーレバーへの接触判定ボタン
    public GameObject lineButton; // リップライン(ロープ)への接触判定ボタン
    public GameObject markerButton; // マーカーへの接触判定ボタン

    public GameObject burnerCock; // バーナーコック
    public GameObject rope; // ロープ

    public GameObject goalTarget; // ゴールターゲット

    // ミニマップ用のオブジェクト
    public GameObject miniMap; // ミニマップオブジェクト
    public GameObject gridObj; // グリッドプレハブ
    public GameObject gridsObj; // グリッド親オブジェクト

    // ミニマップのUI
    public Canvas miniMapCanvas; // プレイヤーステータスのUIキャンバス
    public Canvas markerCanvas; // マーカー投下地点のUIキャンバス
    public Canvas goalCanvas; // ゴールUIキャンバス
    public GameObject selfWindArrow; // 現地点の風向き
    // ミニマップ配置用プレハブ
    public GameObject balloonPrefab; // プレイヤーのバルーンプレハブ
    public GameObject enemyBalloonPrefab; // ライバルのバルーンプレハブ
    public GameObject goalTargetPrefab; // ゴールターゲットプレハブ
    public GameObject moveRoutePrefab; // プレイヤーの移動経路プレハブ
    public GameObject enemyRoutePrefab; // ライバルの移動経路プレハブ
    // ミニマップの配置先インスタンス
    public GameObject miniMyBalloon; // プレイヤーのバルーンインスタンス
    public GameObject[] miniEnemyBalloons; // ライバルのバルーンインスタンス
    public GameObject miniGoalTarget; // ゴールターゲットインスタンス
    public GameObject[] routes; // プレイヤーの経路インスタンス格納配列
    public GameObject[] enemyRoutes; // ライバルの経路インスタンス格納配列
    // マーカープレハブ
    public GameObject markerPrefab;
    // マーカー投下地点目印プレハブ
    public GameObject markerGroundPointPrefab;
    // 腕時計
    public GameObject watchL;
    public GameObject watchR;
    // クリップボード掛けポイント
    public GameObject clipboardL;
    public GameObject clipboardR;

    // プレイヤーのバルーンの炎パーティクル
    public ParticleSystem particle1;
    public ParticleSystem particle2;
    // オーディオソース
    public AudioSource soundObject; // 効果音
    public AudioSource soundObjectBgm; // BGM
    public AudioSource soundObjectVoice; // ボイスナビ
    public AudioSource soundObjectBurner; // バーナー音
    // オーディオクリップ
    public AudioClip bgm; // BGM
    public AudioClip burnerClip; // バーナー音
    public AudioClip clickClip; // クリック音
    public AudioClip flipClip; // ページめくり音
    public AudioClip startClip; // 開始カウントダウン
    public AudioClip goalClip; // ゴール
    // ボイスナビオーディオクリップ
    public AudioClip v_playguide_ja; // プレイガイド
    public AudioClip v_playguide_en;
    public AudioClip v_pause_ja; // ポーズ
    public AudioClip v_pause_en;
    public AudioClip v_gameclear_ja; // ゲームクリア
    public AudioClip v_gameclear_en;
    public AudioClip v_timeover_ja; // 時間切れ
    public AudioClip v_timeover_en;

    // ポーズナビゲーションUI
    public TextMeshProUGUI pauseNaviTxtTmp;

    // プレイ中メッセージ
    public TextMeshProUGUI messageTxtTmp;

    // ミニマップ内ステータスUI
    public TextMeshProUGUI windTxtTmp; // 風力
    public TextMeshProUGUI timeTxtTmp; // 残り時間
    public TextMeshProUGUI distanceTxtTmp; // ターゲットまでの距離
    public TextMeshProUGUI heightTxtTmp; // 高度
    public TextMeshProUGUI markerGoalTxtTmp; // プレイヤーマーカー記録
    public TextMeshProUGUI goalTxtTmp; // ターゲット

    // ポーズメニューUI
    public TextMeshProUGUI pauseTxtTmp;
    public TextMeshProUGUI pauseTxtTmpEn;
    public TextMeshProUGUI howtoTxtTmp;
    public TextMeshProUGUI howtoTxtTmpEn;
    public TextMeshProUGUI rankingTitleTxtTmp;
    public TextMeshProUGUI rankingTxtTmp;
    public TextMeshProUGUI newsTitleTxtTmp;
    public TextMeshProUGUI newsTimeTxtTmp;
    public TextMeshProUGUI newsBodyTxtTmp;
    public TextMeshProUGUI creditTxtTmp;
    public TextMeshProUGUI creditTxtTmpEn;
    public TextMeshProUGUI configTxtTmp;
    public TextMeshProUGUI configTxtTmpEn;

    // 結果表示UI
    public TextMeshProUGUI resultsTitleTxtTmp;
    public TextMeshProUGUI resultsTxtTmp;
    public TextMeshProUGUI returnButtonTxtTmp;
    public TextMeshProUGUI returnButtonTxtTmpEn;

    // メニューUI
    public TextMeshProUGUI menu1;
    public TextMeshProUGUI menu2;
    public TextMeshProUGUI menu3;
    public TextMeshProUGUI menu4;
    public TextMeshProUGUI menu5;
    public TextMeshProUGUI menu6;
    public TextMeshProUGUI menu7;
    public TextMeshProUGUI menu8;
    public TextMeshProUGUI menu1En;
    public TextMeshProUGUI menu2En;
    public TextMeshProUGUI menu3En;
    public TextMeshProUGUI menu4En;
    public TextMeshProUGUI menu5En;
    public TextMeshProUGUI menu6En;
    public TextMeshProUGUI menu7En;
    public TextMeshProUGUI menu8En;
    // 設定メニューチェックマーク
    public Image checkJa;
    public Image checkEn;
    public Image checkLeft;
    public Image checkRight;
    public Image checkSmall;
    public Image checkMiddle;
    public Image checkLarge;
    public Image checkVoiceOn;
    public Image checkVoiceOff;
    // 現在のメニュー番号
    public int currentMenuNo;
    // 設定
    public string lang; // 言語
    public string dominantHand; // 利き手
    public string mapSize; // マップサイズ
    public string voice; // ボイスナビ設定

    // フラグ
    bool isPlayingSound = false; // バーナー音再生中フラグ
    bool isPlaying = false; // プレイ中フラグ
    bool isStart = false; // ゲーム開始フラグ
    public bool isPause = false; // ポーズ中フラグ
    bool isGoal = false; // ゴールフラグ
    bool isOver = false; // タイムオーバーフラグ
    bool startSoundFlg = false; // プレイ開始音再生フラグ
    bool isMarkerTake = false; // マーカーを手に持ったフラグ
    bool isMarkerDrop = false; // マーカーを落としたフラグ

    bool isBurnerCock = false; // バーナーのレバーが握られているフラグ
    bool isRVibration = false; // 右コントローラーのバイブレーションフラグ
    bool isLVibration = false; // 左コントローラーのバイブレーションフラグ
    public bool playFabLogin; // PlayFabログイン状況
    public bool isLoading; // シーンロード中フラグ

    // 値
    // 現地点のXYZ方向にかかる力
    float xWind = 0.0f;
    float yWind = 0.0f;
    float zWind = 0.0f;
    // 風力
    float windPower = 0.0f;
    // XYZ各方向の加速度
    float xVelocity = 0.0f;
    float yVelocity = 0.0f;
    float zVelocity = 0.0f;
    // ミニマップの拡大率
    float mapScale = 1.0f;
    // フレームごとの経過時間
    float deltaTime;
    // 経過時間(秒)
    float timeElapsed;
    // 処理間隔
    float timeInterval = 0.05f;
    // 一定間隔処理用タイマー
    float tmpTime = 0.0f;
    // 一定間隔処理フラグ
    bool intervalActionFlg;
    // ミニマップの経路オブジェクト配置タイマー(秒)
    float secondTimer;
    // ミニマップの経路オブジェクトの配列格納インデックス
    int sec;
    // プレイヤーからターゲットまでのXZ座標上の距離
    float goalDistance = 0.0f;
    // 制限時間(秒)
    float timeLimit;
    // クリアタイム
    public int clearTime;
    // クリア後の経過時間タイマー
    float afterTimeElapsed;
    // ターゲットまでの距離のレコードデータ(m単位)
    float distanceRecord;
    // ターゲットまでの距離のレコードデータ(mm単位)
    int distanceRecordData;

    // PlayFab
    string selfPlayerId; // プレイヤーID

    // 各コントローラー
    BurnerController burnerController; // バーナーコントローラー
    LineController lineController; // リップライン(ロープ)コントローラー
    MarkerButtonController markerButtonController; // マーカーボタンコントローラー
    MarkerController markerController; // マーカーコントローラー

    // 配列データ
    int[,,] windArray = new int[25, 10, 25]; // 風向きデータの3次元配列
    GameObject[,,] mapGrids = new GameObject[25, 10, 25]; // ミニマップの風向きグリッドオブジェクトの格納配列
    int[] windPowers; // 各高度の風力(十の位以上)と45度単位風向き(一の位)設定

    // フェード処理
    [SerializeField] OVRScreenFade fade;

    // 指先のカプセルコライダー
    CapsuleCollider rIndexCollider;
    CapsuleCollider lIndexCollider;

    /// <summary>
    /// 初期実行処理
    /// </summary>
    void Start() {
        // メニュー初期設定
        InitMenu();
        // バーナーの炎パーティクルの初期化
        particle1.Stop();
        particle2.Stop();
        // フラグの初期化
        isPlaying = false;
        isStart = false;
        // 制限時間(秒)
        timeLimit = Utility.TIME_LIMIT;
        // 開始前タイマー設定
        timeElapsed = 3.5f;
        // クリアタイム初期化
        clearTime = 0;
        // ミニマップの経路オブジェクト格納配列初期化
        routes = new GameObject[310];
        enemyRoutes = new GameObject[310 * enemyCount];
        // 各UI表示初期化
        timeTxtTmp.enabled = false;
        heightTxtTmp.enabled = false;
        distanceTxtTmp.enabled = false;
        resultsTitleTxtTmp.enabled = false;
        resultsTxtTmp.enabled = false;
        rankingTitleTxtTmp.enabled = false;
        rankingTxtTmp.enabled = false;
        markerCanvas.enabled = false; // ミニマップのマーカーUI
        miniMapCanvas.enabled = false; // ミニマップのステータスUI
        goalCanvas.enabled = false; // ターゲットのUI
        // 現地点の風向き非表示
        selfWindArrow.SetActive(false);
        selfWindArrow.transform.Find("Cone").GetComponent<MeshRenderer>().enabled = (false);
        selfWindArrow.transform.Find("Cylinder").GetComponent<MeshRenderer>().enabled = (false);
        windTxtTmp.enabled = false;
        // ポーズボタンを有効化
        ButtonPause.SetActive(true);
        // 各コントローラーを取得
        burnerController = burnerButton.GetComponent<BurnerController>();
        lineController = lineButton.GetComponent<LineController>();
        markerButtonController = markerButton.GetComponent<MarkerButtonController>();
        markerController = markerPrefab.GetComponent<MarkerController>();
        // メニューの付箋タブ非表示
        StickyGroup.SetActive(false);

        windPowers = Utility.WIND_POWERS;
        // 風の強さの三次元配列
        for (var i = 0; i < 25; i++) {
            for (var j = 0; j < 10; j++) {
                for (var k = 0; k < 25; k++) {
                    // 高度によって風の向きと強さを設定
                    windArray[i, j, k] = windPowers[j]; // 高度別の風力と風向きデータを格納
                    float yRot = (float)((windArray[i, j, k] % 10) * 45); // 風向きのY軸の角度
                    windPower = (int)Math.Floor((double)(windArray[i, j, k] / 10)); // 風力
                    // 風向きグリッドの間隔を空けて表示するため、風向きごとに条件を指定
                    // プレイ領域外の場合は非表示にする
                    if ((((yRot % 180.0f == 90.0f) && i % 2 == 1 && k % 4 == 2) ||
                         ((yRot % 180.0f == 0.0f) && i % 4 == 2 && k % 2 == 1) ||
                         ((yRot % 180.0f == 45.0f) && ((i % 4 == 0 && k % 4 == 1) || (i % 4 == 2 && k % 4 == 3))) ||
                         ((yRot % 180.0f == 135.0f) && ((i % 4 == 1 && k % 4 == 2) || (i % 4 == 3 && k % 4 == 0))))
                        && j >= 1) {
                        if (i <= 5 || i >= 20 || k <= 5 || k >= 20) {
                            continue;
                        }
                        // 風向きグリッドをインスタンス化して配置して配列に格納
                        mapGrids[i, j, k] = Instantiate(gridObj, Vector3.zero, Quaternion.Euler(0.0f, 0.0f, 0.0f));
                        // ミニマップ内のグリッドの子として座標とサイズと向きを指定
                        mapGrids[i, j, k].transform.parent = gridsObj.transform;
                        mapGrids[i, j, k].transform.localPosition = new Vector3(
                            k * 2.0f / 50.0f - 0.5f + 0.025f,
                            j * 1.0f / 50.0f + 0.0f + 0.025f,
                            i * 2.0f / 50.0f - 0.5f + 0.025f
                        );
                        mapGrids[i, j, k].transform.localScale = new Vector3(
                            1 * 2.0f * 2.0f / 1000.0f * windPower / 5.0f,
                            1 * 2.0f * 2.0f / 1000.0f * windPower / 5.0f,
                            1 * 2.0f * 2.0f / 1000.0f * windPower / 5.0f
                        );
                        mapGrids[i, j, k].transform.Find("Arrow").transform.localRotation = Quaternion.Euler(0.0f, yRot, 0.0f);
                    }
                }
            }
        }
        // ミニマップ内のターゲットを配置
        miniGoalTarget = Instantiate(goalTargetPrefab, Vector3.zero, Quaternion.identity);
        miniGoalTarget.transform.parent = miniMap.transform;
        miniGoalTarget.transform.position = Vector3.zero;
        miniGoalTarget.transform.localPosition = new Vector3(
            (goalTarget.transform.position.x + 0.0f) / 1000.0f - 0.0f + 0.0f,
            (goalTarget.transform.position.y + 0.0f) / 1000.0f + 0.0f,
            (goalTarget.transform.position.z + 0.0f) / 1000.0f - 0.0f + 0.0f
        );
        miniGoalTarget.transform.localScale = new Vector3(2 / 1000.0f, 2 / 1000.0f, 2 / 1000.0f);
        // ターゲットのUIを表示
        goalCanvas.enabled = true;
        goalCanvas.transform.parent = miniGoalTarget.transform;
        goalCanvas.transform.localPosition = new Vector3(0, 0, 0);
        if (lang == "ja") {
            goalTxtTmp.text = Utility.STRING_TARGET_JA;
        } else {
            goalTxtTmp.text = Utility.STRING_TARGET_EN;
        }

        // BGMを再生
        soundObjectBgm = gameObject.AddComponent<AudioSource>();
        soundObjectBgm.clip = bgm;
        soundObjectBgm.loop = true;
        soundObjectBgm.volume = 0.3f;
        soundObjectBgm.Play();

        // ボイスナビの再生準備
        soundObjectVoice = gameObject.AddComponent<AudioSource>();

        // バーナー音の再生準備
        soundObjectBurner = GetComponent<AudioSource>();

        // 指先のコライダーを取得
        rIndexCollider = RHandIndex.GetComponent<CapsuleCollider>();
        lIndexCollider = LHandIndex.GetComponent<CapsuleCollider>();

    }

    /// <summary>
    /// 毎フレーム実行処理
    /// </summary>
    void Update() {
        // フレームごとの加算時間を取得して保管
        deltaTime = Time.deltaTime;
        // 一定時間経過ごとに有効にするフラグ
        tmpTime += deltaTime;
        if (tmpTime >= timeInterval) {
            intervalActionFlg = true;
            tmpTime = 0.0f;
        } else {
            intervalActionFlg = false;
        }
        if (intervalActionFlg) {
            // CustomHandの指先のコライダーが無効化されたら都度有効化する
            if (!rIndexCollider.enabled) {
                rIndexCollider.enabled = true;
            }
            if (!lIndexCollider.enabled) {
                lIndexCollider.enabled = true;
            }
        }
        // プレイ中であれば
        if (isPlaying) {
            // ポーズ中でなければ
            if (!isPause) {
                // プレイ時間のタイマーを加算
                timeElapsed += deltaTime;
                // 移動経路オブジェクト配置用のタイマーを加算
                secondTimer += deltaTime;
                // 時間制限を超えたら時間切れにする
                if (timeElapsed >= timeLimit) {
                    isOver = true;
                    if (lang == "ja") {
                        timeTxtTmp.text = Utility.STRING_REMAIN_TIME_JA + "：0s";
                    } else {
                        timeTxtTmp.text = "0 sec. " + Utility.STRING_REMAIN_TIME_EN;
                    }
                    // 超えていなければ残り時間を表示する
                } else {
                    if (intervalActionFlg) {
                        if (lang == "ja") {
                            timeTxtTmp.text = Utility.STRING_REMAIN_TIME_JA + "：" + (timeLimit - timeElapsed).ToString("f2") + "s";
                        } else {
                            timeTxtTmp.text = (timeLimit - timeElapsed).ToString("f2") + "sec. " + Utility.STRING_REMAIN_TIME_EN;
                        }
                    }
                }
            }
            if (intervalActionFlg) {
                // ターゲットまでのXZ表面上の距離を表示する
                if (goalTarget != null) {
                    goalDistance = (
                        new Vector3(goalTarget.transform.position.x, 0.0f, goalTarget.transform.position.z)
                        -
                        new Vector3(transform.position.x, 0.0f, transform.position.z)
                        ).magnitude;
                    if (lang == "ja") {
                        distanceTxtTmp.text = Utility.STRING_UNTIL_TARGET_JA + goalDistance.ToString("F3") + "m";
                    } else {
                        distanceTxtTmp.text = goalDistance.ToString("F3") + "m" + Utility.STRING_UNTIL_TARGET_EN;
                    }
                }
                // 高度を表示する
                if (lang == "ja") {
                    heightTxtTmp.text = Utility.STRING_ALTITUDE_JA + "：" + transform.position.y.ToString("F3") + "m";
                } else {
                    heightTxtTmp.text = Utility.STRING_ALTITUDE_EN + ": " + transform.position.y.ToString("F3") + "m";
                }
            }
            // transformを取得
            Transform myTransform = this.transform;

            // 座標を取得
            Vector3 pos = myTransform.position;

            // 現在座標をグリッドの座標に置き換える
            int iX = (int)Math.Floor((pos.x + 480.0f) / 2.0f / 20.0f);
            int iY = (int)Math.Floor(pos.y / 1.0f / 20.0f);
            int iZ = (int)Math.Floor((pos.z + 480.0f) / 2.0f / 20.0f);
            // 該当座標の風力と風向きを取得
            if (windArray[iX, iY, iZ] != 0) {
                // 十の位以上が風力、一の位が45度単位の風向きになっているデータを取得
                int windData = windArray[iX, iY, iZ];
                // 風向きを取得
                int dirId = windData % 10;
                float yRot = (float)(dirId * 45); // Y軸の角度に変換する
                windPower = (int)Math.Floor((double)(windData / 10)); // 風力
                xWind = (float)Math.Cos(yRot * Mathf.Deg2Rad) * windPower * -1; // X方向の力
                zWind = (float)Math.Sin(yRot * Mathf.Deg2Rad) * windPower; // Z方向の力
                if (intervalActionFlg) {
                    // 風速を表示
                    if (lang == "ja") {
                        windTxtTmp.text = Utility.STRING_WIND_SPEED_JA + "：" + (int)windPower + "m";
                    } else {
                        windTxtTmp.text = Utility.STRING_WIND_SPEED_EN + ": " + (int)windPower + "m";
                    }
                    // 風向きUIの角度を更新する
                    selfWindArrow.transform.localRotation = Quaternion.Euler(90.0f, yRot, 0.0f);
                }
            }
            // ポーズ中でなければ
            if (!isPause) {
                // ゴールに到達していないがマーカーを落として地面と接触した、もしくは時間切れの場合
                if ((!isGoal && isMarkerDrop && markerController.isGroundEnter()) || isOver) {
                    // ゴール効果音を再生
                    if (goalClip != null) {
                        soundObject.PlayOneShot(goalClip);
                    }
                    // バイブレーション
                    StartCoroutine(Utility.Vibrate(duration: 0.5f, controller: OVRInput.Controller.LTouch));
                    isLVibration = true;
                    StartCoroutine(Utility.Vibrate(duration: 0.5f, controller: OVRInput.Controller.RTouch));
                    isRVibration = true;
                    // タイムオーバーの場合
                    if (isOver) {
                        // メッセージを更新
                        messageTxtTmp.color = new Color(0, 0, 0, 1);
                        messageTxtTmp.alignment = TextAlignmentOptions.Top;
                        messageTxtTmp.fontSize = 10;
                        messageTxtTmp.text = Utility.STRING_TIME_OUT;
                        // プレイ状態を無効化
                        isPlaying = false;
                        // ボイスナビを再生
                        // 残念、タイムオーバーです！お手元のクリップボードからメニューに戻れます。
                        if (voice != "off") {
                            soundObjectVoice.Stop();
                            this.UpdateAsObservable().First().DelaySubscription(System.TimeSpan.FromSeconds(0.5f)).Subscribe(_ => {
                                if (lang == "ja") {
                                    if (v_timeover_ja != null)
                                        soundObjectVoice.PlayOneShot(v_timeover_ja, 1.0f);
                                } else {
                                    if (v_timeover_en != null)
                                        soundObjectVoice.PlayOneShot(v_timeover_en, 1.0f);
                                }
                            }).AddTo(this);
                        }
                        // タイムオーバーになっていなければ
                    } else {
                        // ゲームクリア表示
                        messageTxtTmp.color = new Color(0, 0, 0, 1);
                        messageTxtTmp.alignment = TextAlignmentOptions.Top;
                        messageTxtTmp.fontSize = 10;
                        messageTxtTmp.text = Utility.STRING_GAME_CLEAR;
                        // プレイ状態を無効化
                        isPlaying = false;
                        // ゴール状態を有効化
                        isGoal = true;
                        // ボイスナビを再生
                        // ゲームクリアです！お手元のクリップボードからメニューに戻れます。
                        if (voice != "off") {
                            // 既に再生されているボイスを停止
                            soundObjectVoice.Stop();
                            // 0.5秒後に再生する
                            this.UpdateAsObservable().First().DelaySubscription(System.TimeSpan.FromSeconds(0.5f)).Subscribe(_ => {
                                if (lang == "ja") {
                                    if (v_gameclear_ja != null)
                                        soundObjectVoice.PlayOneShot(v_gameclear_ja, 1.0f);
                                } else {
                                    if (v_gameclear_en != null)
                                        soundObjectVoice.PlayOneShot(v_gameclear_en, 1.0f);
                                }
                            }).AddTo(this);
                        }
                    }
                    // 風向き表示を無効化
                    selfWindArrow.SetActive(false);
                    selfWindArrow.transform.Find("Cone").GetComponent<MeshRenderer>().enabled = false;
                    selfWindArrow.transform.Find("Cylinder").GetComponent<MeshRenderer>().enabled = false;
                    // 風向表示を無効化
                    windTxtTmp.enabled = false;
                    // ボードのメッセージテキストを有効化
                    messageTxtTmp.enabled = true;
                    // 経過時間タイマーからクリアタイムを算出
                    clearTime = (int)((timeLimit - timeElapsed) * 100);
                    // クリアタイムが0以下であれば0とする
                    if (clearTime <= 0) {
                        clearTime = 0;
                    }
                    // ゲームオーバーになっていなければ
                    if (!isOver) {
                        // マーカーからターゲットまでのXZ表面上の距離を取得
                        GameObject sandbag = markerController.transform.Find("Sandbag").gameObject;
                        distanceRecord = (
                            new Vector3(goalTarget.transform.position.x, 0.0f, goalTarget.transform.position.z)
                            -
                            new Vector3(sandbag.transform.position.x, 0.0f, sandbag.transform.position.z)
                            ).magnitude;
                        // 距離をmm単位に変換してスコアデータとして保管
                        distanceRecordData = (int)(distanceRecord * 1000.0f);
                        // ランキングにスコアデータを送信
                        Utility.SubmitRecord(distanceRecordData);
                        // ミニマップの対応座標にマーカー投下位置を表示
                        GameObject markerGoalPoint = Instantiate(markerGroundPointPrefab, Vector3.zero, Quaternion.identity);
                        markerGoalPoint.transform.parent = miniMap.transform;
                        markerGoalPoint.transform.position = Vector3.zero;
                        markerGoalPoint.transform.localPosition = new Vector3(
                            (sandbag.transform.position.x + 0.0f) / 1000.0f - 0.0f + 0.0f,
                            sandbag.transform.position.y / 1000.0f + 0.0f,
                            (sandbag.transform.position.z - 0.0f) / 1000.0f - 0.0f + 0.0f
                        );
                        markerGoalPoint.transform.localScale = new Vector3(2 / 500.0f, 2 / 500.0f, 2 / 500.0f);
                        // ミニマップ上のステータス表示を非表示にする
                        miniMapCanvas.enabled = false;
                        // ミニマップ上のマーカー投下表示
                        markerCanvas.enabled = true;
                        markerCanvas.transform.parent = markerGoalPoint.transform;
                        markerCanvas.transform.localPosition = new Vector3(0, 0, 0);
                        if (lang == "ja") {
                            markerGoalTxtTmp.text = Utility.STRING_RECORD_JA + "：" + distanceRecord.ToString("F3") + "m";
                        } else {
                            markerGoalTxtTmp.text = Utility.STRING_RECORD_EN + ": " + distanceRecord.ToString("F3") + "m";
                        }
                    }
                    // 腕時計型のポーズUIを非アクティブにする
                    ButtonPause.SetActive(false);
                    // クリップボードUIを手元に表示
                    if (dominantHand == "left" || dominantHand == "Left") {
                        Clipboard.transform.parent = RightHandAnchor.transform;
                        Clipboard.transform.localPosition = new Vector3(-0.18f, 0.07f, 0.19f);
                        Clipboard.transform.localRotation = Quaternion.Euler(42.2f, -221.8f, -85.0f);
                    } else {
                        Clipboard.transform.parent = LeftHandAnchor.transform;
                        Clipboard.transform.localPosition = new Vector3(0.18f, 0.07f, 0.19f);
                        Clipboard.transform.localRotation = Quaternion.Euler(42.2f, 221.8f, 85.0f);
                    }
                    // 付箋UIグループを有効化
                    StickyGroup.SetActive(true);
                    // メニュー番号を初期化
                    currentMenuNo = 1;
                    // ゲームクリアメニューを表示
                    if (lang == "ja" || lang == "Ja") {
                        menu1.enabled = false;
                        menu2.enabled = false;
                        menu3.enabled = false;
                        menu4.enabled = false;
                        menu5.enabled = false;
                        menu6.enabled = false;
                        menu7.enabled = true;
                        menu8.enabled = true;
                    } else {
                        menu1En.enabled = false;
                        menu2En.enabled = false;
                        menu3En.enabled = false;
                        menu4En.enabled = false;
                        menu5En.enabled = false;
                        menu6En.enabled = false;
                        menu7En.enabled = true;
                        menu8En.enabled = true;
                    }
                    TitleGroup.SetActive(false);
                    Tag1Base.SetActive(true);
                    Tag2Base.SetActive(true);
                    Tag3Base.SetActive(false);
                    Tag4Base.SetActive(false);
                    Tag5Base.SetActive(false);
                    Tag6Base.SetActive(false);
                    Tag1Button.SetActive(true);
                    Tag2Button.SetActive(true);
                    Tag3Button.SetActive(false);
                    Tag4Button.SetActive(false);
                    Tag5Button.SetActive(false);
                    Tag6Button.SetActive(false);
                    Tag1Active.SetActive(true);
                    Tag2Active.SetActive(false);
                    Tag3Active.SetActive(false);
                    Tag4Active.SetActive(false);
                    Tag5Active.SetActive(false);
                    Tag6Active.SetActive(false);
                    // ポーズ表示を無効化
                    pauseNaviTxtTmp.enabled = false;
                    // メニューに戻るボタンを有効化
                    ButtonReturn.SetActive(true);
                    if (lang == "ja") {
                        returnButtonTxtTmp.enabled = true;
                    } else {
                        returnButtonTxtTmpEn.enabled = true;
                    }
                    // 結果表示を有効化
                    resultsTxtTmp.enabled = true;
                }
                // 右手トリガーが押されていれば
                if (
                OVRInput.Get(OVRInput.RawButton.RIndexTrigger) || OVRInput.Get(OVRInput.RawButton.RHandTrigger)) {
                    // バーナーレバーボタンに指先が入っていれば
                    if (burnerController.isBurnerHand()) {
                        // 右手のバイブレーションを有効化
                        OVRInput.SetControllerVibration(0.1f, 0.1f, OVRInput.Controller.RTouch);
                        isRVibration = true;
                        // バーナーレバーを握った状態にする
                        burnerCock.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                        isBurnerCock = true;
                        // マーカーは持っていない状態にリセット
                        isMarkerTake = false;
                        // マーカーボタンに指先が入っていれば
                    } else if (markerButtonController.isMarkerHand()) {
                        // 右手のバイブレーションを有効化
                        OVRInput.SetControllerVibration(0.1f, 0.1f, OVRInput.Controller.RTouch);
                        isRVibration = true;
                        // バーナーレバーはリセット
                        if (isBurnerCock) {
                            burnerCock.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -30.0f);
                            isBurnerCock = false;
                        }
                        // マーカーは持っている状態に切り替え
                        isMarkerTake = true;

                    } else if (!isMarkerTake) {
                        // 右手のバイブレーションを無効化
                        if (isRVibration) {
                            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
                            isRVibration = false;
                        }
                        // バーナーレバーはリセット
                        if (isBurnerCock) {
                            burnerCock.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -30.0f);
                            isBurnerCock = false;
                        }
                        // マーカーは持っていない状態にリセット
                        isMarkerTake = false;
                    }
                    // マーカーを持っている状態であれば
                    if (isMarkerTake) {
                        // 右手の指先にマーカーを追従させる
                        markerPrefab.transform.parent = RIndex.transform;
                        markerPrefab.transform.localPosition = new Vector3(0, 0, 0);
                    }
                    // 右手トリガーが押されていなければ
                } else {
                    // 右手のバイブレーションを無効化
                    if (isRVibration) {
                        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
                        isRVibration = false;
                    }
                    // バーナーレバーはリセット
                    if (isBurnerCock) {
                        burnerCock.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -30.0f);
                        isBurnerCock = false;
                    }
                }
                // 右手トリガーのいずれかが離された時
                if ((OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger) && !OVRInput.Get(OVRInput.RawButton.RHandTrigger)) ||
                    (OVRInput.GetUp(OVRInput.RawButton.RHandTrigger) && !OVRInput.Get(OVRInput.RawButton.RIndexTrigger))) {
                    // かつマーカーを持っている状態であれば
                    if (isMarkerTake) {
                        // マーカーを落とす処理
                        // マーカーの各コライダーと物理演算と重力を有効にする
                        markerPrefab.gameObject.transform.Find("Sandbag").GetComponent<MeshRenderer>().enabled = true;
                        markerPrefab.gameObject.transform.Find("Cloth").GetComponent<MeshRenderer>().enabled = true;
                        markerPrefab.gameObject.transform.Find("Handle").GetComponent<BoxCollider>().enabled = true;
                        markerPrefab.gameObject.transform.Find("Sandbag").GetComponent<BoxCollider>().enabled = true;
                        markerPrefab.GetComponent<BoxCollider>().enabled = true;
                        markerPrefab.AddComponent<Rigidbody>();
                        markerPrefab.GetComponent<Rigidbody>().useGravity = true;
                        // 地面を突き抜けないように連続的に衝突判定する処理
                        markerPrefab.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                        // マーカーを指先から離す
                        markerPrefab.transform.parent = null;
                        // マーカーボタンを無効化する
                        markerButton.SetActive(false);
                        // マーカーは持っていない状態にリセット
                        isMarkerTake = false;
                        // マーカーを落としたフラグを立てる
                        isMarkerDrop = true;
                    }
                }
                // いずれかの左手トリガーが押されていれば
                if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) || OVRInput.Get(OVRInput.RawButton.LHandTrigger)) {
                    // 何もしない
                    // そうでなければ
                } else {
                    // 左手のバイブレーションを無効化
                    if (isLVibration) {
                        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
                        isLVibration = false;
                    }
                    // ロープ(リップライン)の位置をリセット
                    rope.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                }
            }
            // Escapeキーが押されたら
            if (Input.GetKey(KeyCode.Escape)) {
                // Unityエディタモードの場合はポーズメニュー表示切り替え
#if UNITY_EDITOR
                // ポーズ中の場合はポーズ解除
                if (isPause) {
                    StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.LTouch));
                    isLVibration = true;
                    StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.RTouch));
                    isRVibration = true;
                    isPause = false;
                    // ポーズ中でなければポーズに切り替える
                } else {
                    StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.LTouch));
                    isLVibration = true;
                    StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.RTouch));
                    isRVibration = true;
                    isPause = true;
                }
#endif
                // 上記以外でいずれかの左トリガーが押されていれば(右トリガー同時押しの場合も含む)
            } else if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) || OVRInput.Get(OVRInput.RawButton.LHandTrigger)) {
                // 左のロープ(リップライン)に左手の指先が入っていれば
                if (lineController.isLineHand()) {
                    // 左手のバイブレーションを有効化
                    OVRInput.SetControllerVibration(0.1f, 0.1f, OVRInput.Controller.LTouch);
                    isLVibration = true;
                    // 下向きの力を加える
                    yWind = -3.0f;
                    // ロープを少し動かす
                    rope.transform.localPosition = new Vector3(0.0f, 0.0f, -0.5f);
                }
                // 左トリガーが押されておらず右トリガーが押されていれば
            } else if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) || OVRInput.Get(OVRInput.RawButton.RHandTrigger)) {
                // バーナーレバーのボタンに右の指先が入っていれば
                if (burnerController.isBurnerHand()) {
                    // 上向きの力を加える
                    yWind = 3.0f;
                    // バーナーのパーティクルを再生する
                    particle1.Play();
                    particle2.Play();
                    // バーナー音が再生中でなければ再生する
                    if (isPlayingSound == false) {
                        OVRInput.SetControllerVibration(0.1f, 0.1f, OVRInput.Controller.RTouch);
                        isRVibration = true;
                        soundObjectBurner.PlayOneShot(burnerClip, 1.0f);
                        isPlayingSound = true;
                    }
                    // バーナーレバーのボタンに右の指先が入っていなければ
                } else {
                    // 少し下向きの力を加える
                    yWind = -1.0f;
                    // バーナーのパーティクルを停止する
                    particle1.Stop();
                    particle2.Stop();
                    // バーナー音が再生中であれば停止する
                    if (isPlayingSound == true) {
                        soundObjectBurner.Stop();
                        isPlayingSound = false;
                    }
                }
                // どちらのトリガーも押されていなければ
            } else {
                // 少し下向きの力を加える
                yWind = -1.0f;
                // バーナーのパーティクルを停止する
                particle1.Stop();
                particle2.Stop();
                // バーナー音が再生中であれば停止する
                if (isPlayingSound == true) {
                    soundObjectBurner.Stop();
                    isPlayingSound = false;
                }
            }
            // ポーズ中でなければ
            if (!isPause) {
                // XYZ方向の風向きを加速度に変換
                xVelocity = xWind * deltaTime * 3.0f;
                yVelocity = yWind * deltaTime * 3.0f;
                zVelocity = zWind * deltaTime * 3.0f;
                // 現在座標から風向き方向に加速させる
                pos.x += xVelocity * deltaTime * 4.0f;
                pos.y += yVelocity * deltaTime * 4.0f;
                pos.z += zVelocity * deltaTime * 4.0f;
                // 現在座標を更新
                myTransform.position = pos;
                // 定期的な実行のタイミングであれば
                if (intervalActionFlg) {
                    // ミニマップの現在地を同期する
                    miniMyBalloon.transform.localPosition = new Vector3(
                        (pos.x + 0.0f) / 1000.0f - 0.0f + 0.0f,
                        pos.y / 1000.0f + 0.0f,
                        (pos.z + 0.0f) / 1000.0f - 0.0f + 0.0f);
                    miniMyBalloon.transform.localScale = new Vector3(2 / 50.0f, 2 / 50.0f, 2 / 50.0f);
                    // 全てのライバルプレイヤーに対して実行
                    for (int i = 0; i < enemyCount; i++) {
                        // ミニマップのライバルプレイヤーの座標を同期する
                        miniEnemyBalloons[i].transform.localPosition = new Vector3(
                            (Enemies[i].transform.position.x + 0.0f) / 1000.0f - 0.0f + 0.0f,
                            Enemies[i].transform.position.y / 1000.0f + 0.0f,
                            (Enemies[i].transform.position.z + 0.0f) / 1000.0f - 0.0f + 0.0f);
                        miniEnemyBalloons[i].transform.localScale = new Vector3(1 / 50.0f, 1 / 50.0f, 1 / 50.0f);
                    }

                }
                // マップの経路表示のタイミングであれば
                if (secondTimer >= 2.0f && sec < 1200) {
                    // ミニマップにプレイヤーの経路オブジェクトを配置
                    routes[sec] = Instantiate(moveRoutePrefab, Vector3.zero, Quaternion.identity);
                    routes[sec].transform.parent = miniMap.transform;
                    routes[sec].transform.position = Vector3.zero;
                    routes[sec].transform.localPosition = new Vector3(
                        (this.transform.position.x + 0.0f) / 1000.0f - 0.0f + 0.0f,
                        this.transform.position.y / 1000.0f + 0.0f,
                        (this.transform.position.z - 0.0f) / 1000.0f - 0.0f + 0.0f
                    );
                    routes[sec].transform.localScale = new Vector3(2 / 500.0f, 2 / 500.0f, 2 / 500.0f);

                    // ミニマップのプレイヤー座標を同期する
                    miniMyBalloon.transform.localPosition = new Vector3(
                        (pos.x + 0.0f) / 1000.0f - 0.0f + 0.0f,
                        pos.y / 1000.0f + 0.0f,
                        (pos.z + 0.0f) / 1000.0f - 0.0f + 0.0f);
                    // 全てのライバルプレイヤーに対して実行
                    for (int i = 0; i < enemyCount; i++) {
                        // ミニマップのライバルプレイヤーの経路オブジェクトを配置
                        enemyRoutes[sec * i] = Instantiate(enemyRoutePrefab, Vector3.zero, Quaternion.identity);
                        enemyRoutes[sec * i].transform.parent = miniMap.transform;
                        enemyRoutes[sec * i].transform.position = Vector3.zero;
                        enemyRoutes[sec * i].transform.localPosition = new Vector3(
                            (Enemies[i].transform.position.x + 0.0f) / 1000.0f - 0.0f + 0.0f,
                            Enemies[i].transform.position.y / 1000.0f + 0.0f,
                            (Enemies[i].transform.position.z - 0.0f) / 1000.0f - 0.0f + 0.0f
                        );
                        enemyRoutes[sec * i].transform.localScale = new Vector3(1 / 500.0f, 1 / 500.0f, 1 / 500.0f);
                    }
                    // マップの経路表示のタイミングをリセット
                    secondTimer -= 2.0f;
                    // 累計時間を加算
                    sec++;
                }
            }
            // プレイ中でなければ
        } else {
            // ゲームを開始していなければ
            if (!isStart) {
                // カウントダウン用にタイマーを減算
                timeElapsed -= deltaTime;
                // カウントダウンが0に達したら
                if (timeElapsed <= 0.0f) {
                    // 指先コライダーを有効化
                    if (!rIndexCollider.enabled) {
                        rIndexCollider.enabled = true;
                    }
                    if (!lIndexCollider.enabled) {
                        lIndexCollider.enabled = true;
                    }
                    // ナビメッセージを表示
                    StartMessage();
                    // ミニマップ上のステータス表示を初期化
                    timeTxtTmp.enabled = true;
                    heightTxtTmp.enabled = true;
                    distanceTxtTmp.enabled = true;
                    // タイマーを初期化
                    timeElapsed = 0.0f;
                    // 定期実行用のタイマーを初期化
                    secondTimer = 0.0f;
                    // 累積時間を初期化
                    sec = 0;
                    // プレイ中ステータスに切り替え
                    isPlaying = true;
                    // スタート済みステータスに切り替え
                    isStart = true;
                    // レーススタート！制限時間は3分です。
                    if (voice != "off") {
                        // 0.5秒経過後にボイスナビを再生
                        this.UpdateAsObservable().First().DelaySubscription(System.TimeSpan.FromSeconds(0.5f)).Subscribe(_ => {
                            if (lang == "ja") {
                                if (v_playguide_ja != null)
                                    soundObjectVoice.PlayOneShot(v_playguide_ja, 1.0f);
                            } else {
                                if (v_playguide_en != null)
                                    soundObjectVoice.PlayOneShot(v_playguide_en, 1.0f);
                            }
                        }).AddTo(this);
                    }
                    // 現在の風向き表示を有効化
                    selfWindArrow.SetActive(true);
                    selfWindArrow.transform.Find("Cone").GetComponent<MeshRenderer>().enabled = true;
                    selfWindArrow.transform.Find("Cylinder").GetComponent<MeshRenderer>().enabled = true;
                    // 風力表示を有効化
                    windTxtTmp.enabled = true;
                    // バイブレーションを有効化
                    StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.LTouch));
                    isLVibration = true;
                    StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.RTouch));
                    isRVibration = true;
                    // ミニマップの現在地を同期して表示を有効化
                    miniMyBalloon = Instantiate(balloonPrefab, Vector3.zero, Quaternion.identity);
                    miniMyBalloon.SetActive(false);
                    miniMyBalloon.transform.parent = miniMap.transform;
                    miniMyBalloon.transform.position = Vector3.zero;
                    miniMyBalloon.transform.localPosition = new Vector3(
                        (this.transform.position.x + 0.0f) / 1000.0f - 0.0f + 0.0f,
                        (this.transform.position.y + 0.0f) / 1000.0f + 0.0f,
                        (this.transform.position.z + 10.0f) / 1000.0f - 0.0f + 0.0f
                    );

                    miniMyBalloon.transform.localScale = new Vector3(2 / 50.0f, 2 / 50.0f, 2 / 50.0f);

                    miniMyBalloon.SetActive(true);
                    // ミニマップのステータス表示を有効化
                    miniMapCanvas.enabled = true;
                    miniMapCanvas.transform.parent = miniMyBalloon.transform;
                    miniMapCanvas.transform.localPosition = new Vector3(0, 0, 0);

                    // 各ライバルプレイヤーに対して順次実行
                    for (int i = 0; i < enemyCount; i++) {
                        // ミニマップ上の各ライバルプレイヤーの座標を同期して表示
                        miniEnemyBalloons[i] = Instantiate(enemyBalloonPrefab, Vector3.zero, Quaternion.identity);
                        miniEnemyBalloons[i].SetActive(false);
                        miniEnemyBalloons[i].transform.parent = miniMap.transform;
                        miniEnemyBalloons[i].transform.position = Vector3.zero;
                        miniEnemyBalloons[i].transform.localPosition = new Vector3(
                            (Enemies[i].transform.position.x + 0.0f) / 1000.0f - 0.0f + 0.0f,
                            (Enemies[i].transform.position.y + 0.0f) / 1000.0f + 0.0f,
                            (Enemies[i].transform.position.z + 10.0f) / 1000.0f - 0.0f + 0.0f
                        );
                        miniEnemyBalloons[i].transform.localScale = new Vector3(1 / 50.0f, 1 / 50.0f, 1 / 50.0f);
                        miniEnemyBalloons[i].SetActive(true);
                    }
                    // プレイ開始の3秒前から開始するまでの間は
                } else if (timeElapsed < 3.0f) {
                    // カウントダウンサウンドが再生されていなければ再生
                    if (startClip != null && !startSoundFlg) {
                        soundObject.PlayOneShot(startClip);
                        startSoundFlg = true;
                    }
                    if (intervalActionFlg) {
                        // カウントダウンテキストを時間経過に合わせて更新
                        // フォントサイズと不透明度を毎秒単位でアニメーションする
                        messageTxtTmp.text = "" + Mathf.Ceil(timeElapsed);
                        messageTxtTmp.color = new Color(0, 0, 0, timeElapsed - Mathf.Floor(timeElapsed));
                        messageTxtTmp.fontSize = Mathf.FloorToInt((1 + (timeElapsed - Mathf.Floor(timeElapsed))) * 25.0f);
                        // ミニマップの現在座標を非表示のままで同期しておく
                        Vector3 pos = this.transform.position;
                        miniMyBalloon.SetActive(false);
                        miniMyBalloon.transform.localPosition = new Vector3(
                            (pos.x + 0.0f) / 1000.0f - 0.0f + 0.025f,
                            pos.y / 1000.0f + 0.025f,
                            (pos.z - 0.0f) / 1000.0f - 0.0f + 0.025f
                        );
                        miniMyBalloon.transform.localScale = new Vector3(2 / 50.0f, 2 / 50.0f, 2 / 50.0f);
                    }
                }
            }
        }
        // ゴール状態もしくはゲームオーバーの場合に制御なしで移動を続ける処理
        if (isGoal || isOver) {
            // ゴール後の定期実行用のタイマーを加算
            afterTimeElapsed += deltaTime;
            // メニュー番号が2番でなければ(結果表示であれば)
            if (currentMenuNo != 2) {
                // 1秒単位で実行
                if (afterTimeElapsed >= 1.0f) {
                    // タグ指定でプレイヤー一覧を取得
                    avatars = GameObject.FindGameObjectsWithTag("Player");
                    // 結果テキストをリセット
                    resultsTxtTmp.text = "";
                    int i = 1;
                    // ライバルプレイヤーを順次実行
                    foreach (GameObject avatar in avatars) {
                        // PlayerPrefsに保管されたライバルプレイヤーのスコアを取得
                        int enemyScore = PlayerPrefs.GetInt("Score_" + avatar.name);
                        // 有効な値のスコアが記録されていれば
                        if (enemyScore >= 0 && enemyScore != 999999) {
                            // プレイヤー自身であれば
                            if (avatar.name == this.name) {
                                // 有効な値のスコアが記録されていれば
                                if (distanceRecordData != 0 && distanceRecordData != 999999) {
                                    // ターゲットからの距離が300m未満であれば
                                    if (distanceRecordData < 300000) {
                                        // スコアを表示
                                        resultsTxtTmp.text += this.name + "　" + ((float)distanceRecordData / 1000).ToString("F3") + "m" + " (You)\n";
                                    } else {
                                        // スコアをデフォルト表示
                                        resultsTxtTmp.text += this.name + "　- m" + " (You)\n";
                                    }
                                }
                                // ライバルプレイヤーであれば
                            } else {
                                // 有効な値のスコアが記録されていれば
                                if (enemyScore != 0 && enemyScore != 999999) {
                                    if (enemyScore < 300000) {
                                        // スコアを表示
                                        resultsTxtTmp.text += avatar.name + "　" + ((float)enemyScore / 1000).ToString("F3") + "m" + "\n";
                                    } else {
                                        // スコアをデフォルト表示
                                        resultsTxtTmp.text += avatar.name + "　- m" + "\n";
                                    }
                                }
                            }
                        }
                        i++;
                    }
                    // 実行間隔タイマーをリセット
                    afterTimeElapsed = 0.0f;
                }
            }


            // transformを取得
            Transform myTransform = this.transform;

            // 座標を取得
            Vector3 pos = myTransform.position;

            // 現在座標をグリッドの座標に置き換える
            int iX = (int)Math.Floor((pos.x + 480.0f) / 2.0f / 20.0f);
            int iY = (int)Math.Floor(pos.y / 1.0f / 20.0f);
            int iZ = (int)Math.Floor((pos.z + 480.0f) / 2.0f / 20.0f);
            // 該当座標の風力と風向を取得
            if (windArray[iX, iY, iZ] != 0) {
                // 十の位以上が風力、一の位が45度単位の風向きになっているデータを取得
                int windData = windArray[iX, iY, iZ];
                // 風向きを取得
                int dirId = windData % 10;
                float yRot = (float)(dirId * 45); // Y軸の角度に変換する
                windPower = (int)Math.Floor((double)(windData / 10)); // 風力
                xWind = (float)Math.Cos(yRot * Mathf.Deg2Rad) * windPower * -1; // X方向の力
                zWind = (float)Math.Sin(yRot * Mathf.Deg2Rad) * windPower; // Z方向の力
                if (intervalActionFlg) {
                    // 風速を表示
                    if (lang == "ja") {
                        windTxtTmp.text = Utility.STRING_WIND_SPEED_JA + "：" + (int)windPower + "m";
                    } else {
                        windTxtTmp.text = Utility.STRING_WIND_SPEED_EN + ": " + (int)windPower + "m";
                    }
                    selfWindArrow.SetActive(windPower != 0);
                    selfWindArrow.transform.Find("Cone").GetComponent<MeshRenderer>().enabled = (windPower != 0);
                    selfWindArrow.transform.Find("Cylinder").GetComponent<MeshRenderer>().enabled = (windPower != 0);
                    // 風向きUIの角度を更新する
                    selfWindArrow.transform.localRotation = Quaternion.Euler(90.0f, yRot, 0.0f);//後
                }
            }
            // XYZ方向の風向きを加速度に変換
            xVelocity = xWind * deltaTime * 3.0f;
            yVelocity = yWind * deltaTime * 3.0f;
            zVelocity = zWind * deltaTime * 3.0f;
            // 現在座標から風向き方向に加速させる
            pos.x += xVelocity * deltaTime * 4.0f;
            pos.y += yVelocity * deltaTime * 4.0f;
            pos.z += zVelocity * deltaTime * 4.0f;
            // 現在座標を更新
            myTransform.position = pos;
            // 定期的な実行のタイミングであれば
            if (intervalActionFlg) {
                // ミニマップの現在地を同期する
                miniMyBalloon.transform.localPosition = new Vector3(
                    (pos.x + 0.0f) / 1000.0f - 0.0f + 0.0f,
                    pos.y / 1000.0f + 0.0f,
                    (pos.z + 0.0f) / 1000.0f - 0.0f + 0.0f);
                miniMyBalloon.transform.localScale = new Vector3(2 / 50.0f, 2 / 50.0f, 2 / 50.0f);

                // 各ライバルプレイヤーに対して順次実行
                for (int i = 0; i < enemyCount; i++) {
                    // ミニマップのライバルプレイヤーの座標を同期する
                    miniEnemyBalloons[i].transform.localPosition = new Vector3(
                        (Enemies[i].transform.position.x + 0.0f) / 1000.0f - 0.0f + 0.0f,
                        Enemies[i].transform.position.y / 1000.0f + 0.0f,
                        (Enemies[i].transform.position.z + 0.0f) / 1000.0f - 0.0f + 0.0f);
                    miniEnemyBalloons[i].transform.localScale = new Vector3(1 / 50.0f, 1 / 50.0f, 1 / 50.0f);
                }
            }
        }
    }

    void OnEnable() {
        // ランダムに1～4の値を取得
        int id = Random.Range(1, 5);
        // idを4で割った余り0～3で分岐する
        switch (id % 4) {
            case 1:
                Balloon2.SetActive(false);
                Balloon3.SetActive(false);
                Balloon4.SetActive(false);
                particle1 = Balloon1.transform.Find("Flame 1").GetComponent<ParticleSystem>();
                particle2 = Balloon1.transform.Find("Flame 2").GetComponent<ParticleSystem>();
                break;
            case 2:
                Balloon1.SetActive(false);
                Balloon3.SetActive(false);
                Balloon4.SetActive(false);
                particle1 = Balloon2.transform.Find("Flame 1").GetComponent<ParticleSystem>();
                particle2 = Balloon2.transform.Find("Flame 2").GetComponent<ParticleSystem>();
                break;
            case 3:
                Balloon1.SetActive(false);
                Balloon2.SetActive(false);
                Balloon4.SetActive(false);
                particle1 = Balloon3.transform.Find("Flame 1").GetComponent<ParticleSystem>();
                particle2 = Balloon3.transform.Find("Flame 2").GetComponent<ParticleSystem>();
                break;
            case 0:
            default:
                Balloon1.SetActive(false);
                Balloon2.SetActive(false);
                Balloon3.SetActive(false);
                particle1 = Balloon4.transform.Find("Flame 1").GetComponent<ParticleSystem>();
                particle2 = Balloon4.transform.Find("Flame 2").GetComponent<ParticleSystem>();
                break;
        }
        particle1.Stop();
        particle2.Stop();
        // PlayFabからプレイヤーデータを取得
        GetPlayerData();
        // ランダムに0～3の値を取得
        int n = Random.Range(0, 4);
        // 値に対応する開始位置に移動させる
        Vector3 pos = GameObject.Find("startPos" + n).transform.position;
        if (pos != null) {
            transform.position = pos;
        }
    }

    /// <summary>
    /// PlayFabのログインに成功した場合の処理
    /// </summary>
    /// <param name="result">LoginResultオブジェクト</param>
    private void PlayFabLogin_OnLoginSuccess(LoginResult result) {
        // PlayFabログインフラグを有効化
        playFabLogin = true;
    }

    /// <summary>
    /// オブジェクトが無効化された場合の処理
    /// </summary>
    private void OnDisable() {
        // PlayFabを有効にしている場合は
        if (Utility.USE_PLAYFAB) {
            PlayFabAuthService.OnLoginSuccess -= PlayFabLogin_OnLoginSuccess;
        }
    }

    /// <summary>
    /// PlayFabからプレイヤーデータを取得する処理
    /// </summary>
    private async void GetPlayerData() {
        // PlayFabを有効にしている場合は
        if (Utility.USE_PLAYFAB) {
            // PlayerProfileを取得する
            PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest() {
                PlayFabId = ""
            }, result => {
                // Profileの取得に成功したらプレイヤーIDを取得
                var playerProfile = result.PlayerProfile;
                selfPlayerId = playerProfile.PlayerId;
            }, error => Debug.Log(error.GenerateErrorReport()));
        }
    }

    /// <summary>
    /// PlayFabからランキングデータを取得する処理
    /// </summary>
    void RequestLeaderBoard() {
        rankingTxtTmp.text = "";
        // PlayFabを有効にしている場合は
        if (Utility.USE_PLAYFAB) {
            // PlayFabからランキングデータを最大15件取得
            PlayFabClientAPI.GetLeaderboard(
            new GetLeaderboardRequest {
                StatisticName = Utility.PLAYFAB_RANKING_NAME,
                StartPosition = 0,
                MaxResultsCount = 15
            },
            result => {
                // ランキングデータの取得に成功したら言語に応じてランキング表示を更新
                result.Leaderboard.ForEach(
                    x => rankingTxtTmp.text += ((x.Position + 1) +
                    ((lang == "ja" || lang == "Ja") ? "位 " : ((x.Position + 1) % 10 == 1) ? "st " : ((x.Position + 1) % 10 == 2) ? "nd " : ((x.Position + 1) % 10 == 3) ? "rd " : "th ") +
                    ((float)x.StatValue * -1 / 1000).ToString("F3") + "m " + ((selfPlayerId == x.PlayFabId) ? "(You)" : "") + "\n")
                    );
            }, error => {
                Debug.Log(error.GenerateErrorReport());
            }
            );
            // PlayFabを有効にしていない場合は
        } else {
            // 案内メッセージを表示
            lang = PlayerPrefs.GetString("Lang");
            if (lang == "ja") {
                rankingTxtTmp.text = Utility.RANKING_PLAYFAB_ERROR_JA;
            } else {
                rankingTxtTmp.text = Utility.RANKING_PLAYFAB_ERROR_EN;
            }
        }
    }

    /// <summary>
    /// メニューシーンの非同期読み込み処理
    /// </summary>
    IEnumerator LoadMenuSceneCoroutine() {
        // フェード時間を稼ぐために3秒間待機
        yield return new WaitForSeconds(3.0f);
        // 非同期でロードを開始
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MenuScene");

        // 非同期ロードが完了するまで待つ
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }

    /// <summary>
    /// クリップボードメニューの初期化処理
    /// </summary>
    void InitMenu() {
        // 各オブジェクトの非表示処理
        // タイトル
        TitleGroup.SetActive(false);
        // タブメニュー
        Tag1Base.SetActive(false);
        Tag2Base.SetActive(false);
        Tag3Base.SetActive(false);
        Tag4Base.SetActive(false);
        Tag5Base.SetActive(false);
        Tag6Base.SetActive(false);
        Tag1Button.SetActive(false);
        Tag2Button.SetActive(false);
        Tag3Button.SetActive(false);
        Tag4Button.SetActive(false);
        Tag5Button.SetActive(false);
        Tag6Button.SetActive(false);
        Tag1Active.SetActive(false);
        Tag2Active.SetActive(false);
        Tag3Active.SetActive(false);
        Tag4Active.SetActive(false);
        Tag5Active.SetActive(false);
        Tag6Active.SetActive(false);
        // ボタン
        ButtonContinue.SetActive(false);
        ButtonQuit.SetActive(false);
        ButtonJa.SetActive(false);
        ButtonEn.SetActive(false);
        ButtonLeft.SetActive(false);
        ButtonRight.SetActive(false);
        ButtonSmall.SetActive(false);
        ButtonMiddle.SetActive(false);
        ButtonLarge.SetActive(false);
        ButtonVoiceOn.SetActive(false);
        ButtonVoiceOff.SetActive(false);
        // PlayerPrefsから言語設定を取得して更新(設定がない場合の初期値はja)
        lang = PlayerPrefs.GetString("Lang", "ja");
        PlayerPrefs.SetString("Lang", lang);
        // PlayFabの言語設定を更新(ログイン済みの場合のみ・ニュースの言語が切り替わる)
        if (playFabLogin) {
            Utility.SetProfileLanguage();
        }
        // PlayerPrefsからマップサイズを取得して更新(初期値はmiddle)
        mapSize = PlayerPrefs.GetString("Map");
        if (mapSize == null) {
            mapSize = "middle";
            mapScale = 1.0f;
            // ミニマップの拡大率を設定に合わせて更新
            miniMap.transform.localScale = new Vector3(mapScale, mapScale, mapScale);
            PlayerPrefs.SetString("Map", mapSize);
        }
        // PlayerPrefsからボイスナビ設定を取得して更新(初期値はon)
        voice = PlayerPrefs.GetString("Voice");
        if (voice == null) {
            voice = "on";
            PlayerPrefs.SetString("Voice", voice);
        }
        // 各タブメニューを無効化
        menu1.enabled = false;
        menu2.enabled = false;
        menu3.enabled = false;
        menu4.enabled = false;
        menu5.enabled = false;
        menu6.enabled = false;
        menu1En.enabled = false;
        menu2En.enabled = false;
        menu3En.enabled = false;
        menu4En.enabled = false;
        menu5En.enabled = false;
        menu6En.enabled = false;
        // 利き手設定をPlayerPrefsから取得して更新(初期値はright)
        dominantHand = PlayerPrefs.GetString("Hand");
        if (dominantHand == null) {
            dominantHand = "right";
            PlayerPrefs.SetString("Hand", dominantHand);
        }
        // 利き手に応じて腕時計とクリップボードメニューの向きを更新
        if (dominantHand == "left" || dominantHand == "Left") {
            watchL.SetActive(false);
            watchR.SetActive(true);
            Clipboard.transform.parent = clipboardR.transform;
            Clipboard.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            Clipboard.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            StickyGroup.transform.localPosition = new Vector3(-49.49f, 42.4f, 1.69f);
        } else {
            watchL.SetActive(true);
            watchR.SetActive(false);
            Clipboard.transform.parent = clipboardL.transform;
            Clipboard.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            Clipboard.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            StickyGroup.transform.localPosition = new Vector3(49.49f, 42.4f, 1.69f);
        }

    }

    /// <summary>
    /// 言語設定の更新処理
    /// </summary>
    /// <param name="language">言語(ja/en)</param>
    public void SetLang(string language) {
        // 言語設定に変更があればPlayerPrefsのデータを更新
        lang = PlayerPrefs.GetString("Lang");
        // 設定の更新がなければスキップ
        if (language == lang) {
            return;
        }
        lang = language;
        PlayerPrefs.SetString("Lang", lang);
        // PlayFabの言語を更新(ニュースの言語が切り替わる)
        Utility.SetProfileLanguage();
        // クリック音再生
        if (clickClip != null) {
            soundObject.PlayOneShot(clickClip);
        }
        // 言語に応じて各表示を更新
        if (lang == "ja" || lang == "Ja") {
            configTxtTmp.enabled = true;
            configTxtTmpEn.enabled = false;
            checkJa.enabled = true;
            checkEn.enabled = false;
            menu1.enabled = true;
            menu2.enabled = true;
            menu3.enabled = true;
            menu4.enabled = true;
            menu5.enabled = true;
            menu6.enabled = true;
            menu1En.enabled = false;
            menu2En.enabled = false;
            menu3En.enabled = false;
            menu4En.enabled = false;
            menu5En.enabled = false;
            menu6En.enabled = false;
            goalTxtTmp.text = Utility.STRING_TARGET_JA;
        } else {
            configTxtTmp.enabled = false;
            configTxtTmpEn.enabled = true;
            checkJa.enabled = false;
            checkEn.enabled = true;
            menu1.enabled = false;
            menu2.enabled = false;
            menu3.enabled = false;
            menu4.enabled = false;
            menu5.enabled = false;
            menu6.enabled = false;
            menu1En.enabled = true;
            menu2En.enabled = true;
            menu3En.enabled = true;
            menu4En.enabled = true;
            menu5En.enabled = true;
            menu6En.enabled = true;
            goalTxtTmp.text = Utility.STRING_TARGET_EN;
        }
        // ボードのメッセージを更新
        StartMessage();
        // PlayFabからプレイヤーデータを取得
        GetPlayerData();
        // PlayFabからニュースデータを取得
        GetNews();
    }

    /// <summary>
    /// クリップボードUIの案内メッセージ更新処理
    /// </summary>
    void StartMessage() {
        messageTxtTmp.color = new Color(0, 0, 0, 1);
        messageTxtTmp.alignment = TextAlignmentOptions.TopLeft;
        if (lang == "ja") {
            messageTxtTmp.fontSize = 3.8f;
            messageTxtTmp.text = Utility.GUIDE_MESSAGE_JA;
        } else {
            messageTxtTmp.fontSize = 3.5f;
            messageTxtTmp.text = Utility.GUIDE_MESSAGE_EN;
        }
    }

    /// <summary>
    /// 利き手設定の更新処理
    /// </summary>
    /// <param name="hand">利き手設定(right/left)</param>
    public void SetHand(string hand) {
        // 利き手設定がPlayerPrefsの設定と異なっている場合は更新
        dominantHand = PlayerPrefs.GetString("Hand");
        // 設定の更新がなければスキップ
        if (hand == dominantHand) {
            return;
        }
        dominantHand = hand;
        PlayerPrefs.SetString("Hand", dominantHand);
        // クリック音再生
        if (clickClip != null) {
            soundObject.PlayOneShot(clickClip);
        }
        // 利き手設定のチェックを切り替え
        // 腕時計とクリップボードを利き手の反対の手に持ち換えて付箋タブの位置を利き手側に移動する
        if (dominantHand == "left" || dominantHand == "Left") {
            watchL.SetActive(false);
            watchR.SetActive(false);
            checkLeft.enabled = true;
            checkRight.enabled = false;
            Clipboard.transform.parent = RightHandAnchor.transform;
            Clipboard.transform.localPosition = new Vector3(-0.25f, 0.1f, 0.15f);
            Clipboard.transform.localRotation = Quaternion.Euler(42.2f, -221.8f, -50.45f);
            StickyGroup.transform.localPosition = new Vector3(-49.49f, 42.4f, 1.69f);
        } else {
            watchL.SetActive(false);
            watchR.SetActive(false);
            checkRight.enabled = true;
            checkLeft.enabled = false;
            Clipboard.transform.parent = LeftHandAnchor.transform;
            Clipboard.transform.localPosition = new Vector3(0.25f, 0.1f, 0.15f);
            Clipboard.transform.localRotation = Quaternion.Euler(42.2f, 221.8f, 50.45f);
            StickyGroup.transform.localPosition = new Vector3(49.49f, 42.4f, 1.69f);
        }
    }

    /// <summary>
    /// マップサイズ設定の更新処理
    /// </summary>
    /// <param name="size">マップサイズ(small/middle/large)</param>
    public void SetMap(string size) {
        // マップサイズ設定をPlayerPrefsから取得
        mapSize = PlayerPrefs.GetString("Map");
        // 設定が変更されている場合のみ更新
        if (size == mapSize) {
            return;
        }
        mapSize = size;
        // PlayerPrefsのマップサイズ設定を更新
        PlayerPrefs.SetString("Map", mapSize);
        // クリック音再生
        if (clickClip != null) {
            soundObject.PlayOneShot(clickClip);
        }
        // マップサイズ設定に応じて各表示を更新
        if (mapSize == "small") {
            mapScale = 0.5f;
            checkSmall.enabled = true;
            checkMiddle.enabled = false;
            checkLarge.enabled = false;
        } else if (mapSize == "large") {
            mapScale = 2.0f;
            checkSmall.enabled = false;
            checkMiddle.enabled = false;
            checkLarge.enabled = true;
        } else {
            mapScale = 1.0f;
            checkSmall.enabled = false;
            checkMiddle.enabled = true;
            checkLarge.enabled = false;
        }
        // ミニマップの拡大率を設定
        miniMap.transform.localScale = new Vector3(mapScale, mapScale, mapScale);
    }

    /// <summary>
    /// ボイスナビ設定の更新処理
    /// </summary>
    /// <param name="voiceSetting">ボイスナビ設定(on/off)</param>
    public void SetVoice(string voiceSetting) {
        // ボイスナビ設定をPlayerPrefsから取得して切り替わっていれば更新
        string voice = PlayerPrefs.GetString("Voice");
        // 設定が変更されている場合のみ更新
        if (voiceSetting == voice) {
            return;
        }
        voice = voiceSetting;
        PlayerPrefs.SetString("Voice", voice);
        // クリック音を再生
        if (clickClip != null) {
            soundObject.PlayOneShot(clickClip);
        }
        // ボイスナビが無効であれば
        if (voice == "off") {
            // ボイスナビの再生を停止
            soundObjectVoice.Stop();
            // ボイスナビ設定のチェックを切り替え
            checkVoiceOn.enabled = false;
            checkVoiceOff.enabled = true;
            // ボイスナビが有効であれば
        } else {
            // ボイスナビ設定のチェックを切り替え
            checkVoiceOn.enabled = true;
            checkVoiceOff.enabled = false;
        }
    }

    /// <summary>
    /// ポーズ
    /// </summary>
    public void Pause() {
        // ポーズ表示に切り替え
        pauseNaviTxtTmp.enabled = false;
        messageTxtTmp.enabled = false;
        ButtonPause.SetActive(false);
        StickyGroup.SetActive(true);
        Tag1Base.SetActive(true);
        Tag2Base.SetActive(true);
        Tag3Base.SetActive(true);
        Tag4Base.SetActive(true);
        Tag5Base.SetActive(true);
        Tag6Base.SetActive(true);
        Tag1Button.SetActive(true);
        Tag2Button.SetActive(true);
        Tag3Button.SetActive(true);
        Tag4Button.SetActive(true);
        Tag5Button.SetActive(true);
        Tag6Button.SetActive(true);
        Tag1Active.SetActive(true);
        Tag2Active.SetActive(false);
        Tag3Active.SetActive(false);
        Tag4Active.SetActive(false);
        Tag5Active.SetActive(false);
        Tag6Active.SetActive(false);
        ButtonContinue.SetActive(true);
        ButtonQuit.SetActive(true);
        // 腕時計メニューは非アクティブに切り替え
        watchL.SetActive(false);
        watchR.SetActive(false);
        if (lang == "ja" || lang == "Ja") {
            pauseTxtTmp.enabled = true;
            pauseTxtTmpEn.enabled = false;
        } else {
            pauseTxtTmp.enabled = false;
            pauseTxtTmpEn.enabled = true;
        }
        howtoTxtTmp.enabled = false;
        howtoTxtTmpEn.enabled = false;
        rankingTxtTmp.enabled = false;
        configTxtTmp.enabled = false;
        configTxtTmpEn.enabled = false;
        creditTxtTmp.enabled = false;
        creditTxtTmpEn.enabled = false;
        newsTitleTxtTmp.enabled = false;
        newsTimeTxtTmp.enabled = false;
        newsBodyTxtTmp.enabled = false;
        if (lang == "ja" || lang == "Ja") {
            menu1.enabled = true;
            menu2.enabled = true;
            menu3.enabled = true;
            menu4.enabled = true;
            menu5.enabled = true;
            menu6.enabled = true;
            menu7.enabled = false;
            menu8.enabled = false;
        } else {
            menu1En.enabled = true;
            menu2En.enabled = true;
            menu3En.enabled = true;
            menu4En.enabled = true;
            menu5En.enabled = true;
            menu6En.enabled = true;
            menu7En.enabled = false;
            menu8En.enabled = false;
        }
        checkJa.enabled = false;
        checkEn.enabled = false;
        checkLeft.enabled = false;
        checkRight.enabled = false;
        checkSmall.enabled = false;
        checkMiddle.enabled = false;
        checkLarge.enabled = false;
        checkVoiceOn.enabled = false;
        checkVoiceOff.enabled = false;
        // メニュー番号をリセット
        currentMenuNo = 1;
        // クリップボードUIを手元に移動させる
        if (dominantHand == "left" || dominantHand == "Left") {
            Clipboard.transform.parent = RightHandAnchor.transform;
            Clipboard.transform.localPosition = new Vector3(-0.18f, 0.07f, 0.19f);
            Clipboard.transform.localRotation = Quaternion.Euler(42.2f, -221.8f, -85.0f);
        } else {
            Clipboard.transform.parent = LeftHandAnchor.transform;
            Clipboard.transform.localPosition = new Vector3(0.18f, 0.07f, 0.19f);
            Clipboard.transform.localRotation = Quaternion.Euler(42.2f, 221.8f, 85.0f);
        }
        // バイブレーションを有効化
        StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.LTouch));
        isLVibration = true;
        StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.RTouch));
        isRVibration = true;
        isPause = true;
        // ボイスナビを再生
        // ポーズしました。お手元のクリップボードでメニューを選択できます。
        soundObjectVoice.Stop();
        this.UpdateAsObservable().First().DelaySubscription(System.TimeSpan.FromSeconds(0.5f)).Subscribe(_ => {
            if (lang == "ja") {
                if (v_pause_ja != null)
                    soundObjectVoice.PlayOneShot(v_pause_ja, 1.0f);
            } else {
                if (v_pause_en != null)
                    soundObjectVoice.PlayOneShot(v_pause_en, 1.0f);
            }
        }).AddTo(this);
    }

    /// <summary>
    /// プレイ再開
    /// </summary>
    public void Continue() {
        // プレイ中表示に切り替え
        pauseNaviTxtTmp.enabled = true;
        messageTxtTmp.enabled = true;
        StickyGroup.SetActive(false);
        ButtonPause.SetActive(true);
        Tag1Base.SetActive(false);
        Tag2Base.SetActive(false);
        Tag3Base.SetActive(false);
        Tag4Base.SetActive(false);
        Tag5Base.SetActive(false);
        Tag6Base.SetActive(false);
        Tag1Button.SetActive(false);
        Tag2Button.SetActive(false);
        Tag3Button.SetActive(false);
        Tag4Button.SetActive(false);
        Tag5Button.SetActive(false);
        Tag6Button.SetActive(false);
        Tag1Active.SetActive(false);
        Tag2Active.SetActive(false);
        Tag3Active.SetActive(false);
        Tag4Active.SetActive(false);
        Tag5Active.SetActive(false);
        Tag6Active.SetActive(false);
        ButtonContinue.SetActive(false);
        ButtonQuit.SetActive(false);
        pauseTxtTmp.enabled = false;
        pauseTxtTmpEn.enabled = false;
        menu1.enabled = false;
        menu2.enabled = false;
        menu3.enabled = false;
        menu4.enabled = false;
        menu5.enabled = false;
        menu6.enabled = false;
        menu1En.enabled = false;
        menu2En.enabled = false;
        menu3En.enabled = false;
        menu4En.enabled = false;
        menu5En.enabled = false;
        menu6En.enabled = false;
        // ポーズ状態であれば
        if (isPause) {
            // バイブレーションを有効化
            StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.LTouch));
            isLVibration = true;
            StartCoroutine(Utility.Vibrate(duration: 0.1f, controller: OVRInput.Controller.RTouch));
            isRVibration = true;
            // ポーズ状態を解除
            isPause = false;
        }
        if (dominantHand == "left" || dominantHand == "Left") {
            watchL.SetActive(false);
            watchR.SetActive(true);
            Clipboard.transform.parent = clipboardR.transform;
            Clipboard.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            Clipboard.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            StickyGroup.transform.localPosition = new Vector3(-49.49f, 42.4f, 1.69f);
        } else {
            watchL.SetActive(true);
            watchR.SetActive(false);
            Clipboard.transform.parent = clipboardL.transform;
            Clipboard.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            Clipboard.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            StickyGroup.transform.localPosition = new Vector3(49.49f, 42.4f, 1.69f);
        }
    }

    /// <summary>
    /// メニューに戻る処理
    /// </summary>
    public void Quit() {
        // クリック音を再生
        if (clickClip != null) {
            soundObject.PlayOneShot(clickClip);
        }
        // フェードアウト処理
        fade.FadeOut();
        // メニューシーンをロード
        StartCoroutine("LoadMenuSceneCoroutine");
    }

    /// <summary>
    /// 指定したタブメニューへの切り替え処理
    /// </summary>
    /// <param name="tagId">タグ番号</param>
    public void TouchTag(int tagId) {
        // 現在の番号と同じであればスキップ
        if (tagId == currentMenuNo) {
            return;
        }
        // クリック音を再生
        if (flipClip != null) {
            soundObject.PlayOneShot(flipClip);
        }
        // 表示を初期化
        TitleGroup.SetActive(false);
        Tag1Active.SetActive(false);
        Tag2Active.SetActive(false);
        Tag3Active.SetActive(false);
        Tag4Active.SetActive(false);
        Tag5Active.SetActive(false);
        Tag6Active.SetActive(false);
        ButtonContinue.SetActive(false);
        ButtonQuit.SetActive(false);
        ButtonJa.SetActive(false);
        ButtonEn.SetActive(false);
        ButtonLeft.SetActive(false);
        ButtonRight.SetActive(false);
        ButtonSmall.SetActive(false);
        ButtonMiddle.SetActive(false);
        ButtonLarge.SetActive(false);
        ButtonVoiceOn.SetActive(false);
        ButtonVoiceOff.SetActive(false);
        pauseTxtTmp.enabled = false;
        pauseTxtTmpEn.enabled = false;
        howtoTxtTmp.enabled = false;
        howtoTxtTmpEn.enabled = false;
        resultsTxtTmp.enabled = false;
        rankingTxtTmp.enabled = false;
        configTxtTmp.enabled = false;
        configTxtTmpEn.enabled = false;
        creditTxtTmp.enabled = false;
        creditTxtTmpEn.enabled = false;
        newsTitleTxtTmp.enabled = false;
        newsTimeTxtTmp.enabled = false;
        newsBodyTxtTmp.enabled = false;
        checkJa.enabled = false;
        checkEn.enabled = false;
        checkLeft.enabled = false;
        checkRight.enabled = false;
        checkSmall.enabled = false;
        checkMiddle.enabled = false;
        checkLarge.enabled = false;
        checkVoiceOn.enabled = false;
        checkVoiceOff.enabled = false;
        // クリア済みもしくはゲームオーバーであれば
        if (isGoal || isOver) {
            // 2タブメニューを表示
            switch (tagId) {
                // 結果表示
                case 1:
                    currentMenuNo = 1;
                    TitleGroup.SetActive(false);
                    Tag1Active.SetActive(true);
                    ButtonReturn.SetActive(true);
                    messageTxtTmp.enabled = true;
                    resultsTxtTmp.enabled = true;
                    break;
                // ランキング表示
                case 2:
                    currentMenuNo = 2;
                    Tag2Active.SetActive(true);
                    GetPlayerData(); // PlayFabのデータを取得
                    messageTxtTmp.enabled = false;
                    rankingTxtTmp.enabled = true;
                    RequestLeaderBoard(); // PlayFabのランキングデータを更新
                    break;
                default:
                    break;
            }
            // そうでなければ
        } else {
            // 6タブメニューを表示
            switch (tagId) {
                // ポーズメニュー
                case 1:
                    currentMenuNo = 1;
                    TitleGroup.SetActive(false);
                    Tag1Active.SetActive(true);
                    ButtonContinue.SetActive(true);
                    ButtonQuit.SetActive(true);
                    if (lang == "ja" || lang == "Ja") {
                        pauseTxtTmp.enabled = true;
                    } else {
                        pauseTxtTmpEn.enabled = true;
                    }
                    break;
                // 使い方メニュー
                case 2:
                    currentMenuNo = 2;
                    Tag2Active.SetActive(true);
                    if (lang == "ja" || lang == "Ja") {
                        howtoTxtTmp.enabled = true;
                        howtoTxtTmpEn.enabled = false;
                    } else {
                        howtoTxtTmp.enabled = false;
                        howtoTxtTmpEn.enabled = true;
                    }
                    break;
                // ランキングメニュー
                case 3:
                    currentMenuNo = 3;
                    Tag3Active.SetActive(true);
                    GetPlayerData(); // PlayFabのデータを取得
                    rankingTxtTmp.enabled = true;
                    RequestLeaderBoard(); // PlayFabのランキングデータを更新
                    break;
                // ニュースメニュー
                case 4:
                    currentMenuNo = 4;
                    Tag4Active.SetActive(true);
                    GetNews(); // PlayFabのニュースデータを取得
                    newsTitleTxtTmp.enabled = true;
                    newsTimeTxtTmp.enabled = true;
                    newsBodyTxtTmp.enabled = true;
                    break;
                // クレジットメニュー
                case 5:
                    currentMenuNo = 5;
                    Tag5Active.SetActive(true);
                    if (lang == "ja" || lang == "Ja") {
                        creditTxtTmp.enabled = true;
                        creditTxtTmpEn.enabled = false;
                    } else {
                        creditTxtTmp.enabled = false;
                        creditTxtTmpEn.enabled = true;
                    }
                    break;
                // 設定メニュー
                case 6:
                    currentMenuNo = 6;
                    Tag6Active.SetActive(true);
                    ButtonJa.SetActive(true);
                    ButtonEn.SetActive(true);
                    ButtonLeft.SetActive(true);
                    ButtonRight.SetActive(true);
                    ButtonSmall.SetActive(true);
                    ButtonMiddle.SetActive(true);
                    ButtonLarge.SetActive(true);
                    ButtonVoiceOn.SetActive(true);
                    ButtonVoiceOff.SetActive(true);
                    // 言語設定
                    lang = PlayerPrefs.GetString("Lang");
                    if (lang == "ja" || lang == "Ja") {
                        configTxtTmp.enabled = true;
                        checkJa.enabled = true;
                    } else {
                        configTxtTmpEn.enabled = true;
                        checkEn.enabled = true;
                    }
                    // 利き手設定
                    dominantHand = PlayerPrefs.GetString("Hand");
                    if (dominantHand == "left" || dominantHand == "Left") {
                        checkLeft.enabled = true;
                    } else {
                        checkRight.enabled = true;
                    }
                    // マップサイズ設定
                    mapSize = PlayerPrefs.GetString("Map");
                    if (mapSize == "small") {
                        mapScale = 0.5f;
                        checkSmall.enabled = true;
                    } else if (mapSize == "large") {
                        mapScale = 2.0f;
                        checkLarge.enabled = true;
                    } else {
                        mapScale = 1.0f;
                        checkMiddle.enabled = true;
                    }
                    miniMap.transform.localScale = new Vector3(mapScale, mapScale, mapScale);
                    // ボイスナビ設定
                    voice = PlayerPrefs.GetString("Voice");
                    if (voice == "off") {
                        checkVoiceOff.enabled = true;
                    } else {
                        checkVoiceOn.enabled = true;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// ニュースの読み込み処理
    /// </summary>
    private void GetNews() {
        // PlayFabが有効であれば
        if (Utility.USE_PLAYFAB) {
            // PlayFabからニュースデータを取得
            PlayFabClientAPI.GetTitleNews(new GetTitleNewsRequest(), result => {
                var newsTitle = result.News[0].Title; // ニュースタイトル
                newsTitleTxtTmp.text = newsTitle;
                var newsBody = result.News[0].Body; // ニュース本文
                newsBodyTxtTmp.text = newsBody;
                var newsTime = result.News[0].Timestamp.ToLocalTime(); // ニュース日時は現地時刻に変換
                newsTimeTxtTmp.text = "" + newsTime;
            }, error => Debug.LogError(error.GenerateErrorReport()));
            // PlayFabが有効でなければ
        } else {
            // 案内メッセージを表示
            lang = PlayerPrefs.GetString("Lang");
            if (lang == "ja") {
                newsTitleTxtTmp.text = Utility.NEWS_PLAYFAB_ERROR_JA;
            } else {
                newsTitleTxtTmp.text = Utility.NEWS_PLAYFAB_ERROR_EN;
            }
        }
    }
}
