using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using PlayFab.ProfilesModels;
using UnityEngine.Events;
using System;

/// <summary>
/// メニューシーンのコントローラー
/// </summary>
public class MenuController:MonoBehaviour
{
    // 設定
    public string lang; // 言語(ja/en)
    public string dominantHand; // 利き手(left/right)
    public string mapSize; // マップサイズ(small/middle/large)
    public string voice; // ボイスナビ(on/off)

    // 選択中のメニュー番号(1-6)
    public int currentMenuNo;

    // メニュークリップボード
    public GameObject Clipboard;
    // 付箋タブグループ
    public GameObject StickyGroup;
    // 付箋タブ(選択時に表示)
    public GameObject Tag1Active;
    public GameObject Tag2Active;
    public GameObject Tag3Active;
    public GameObject Tag4Active;
    public GameObject Tag5Active;
    public GameObject Tag6Active;
    // レースメニュー ゲームタイトルパーツ
    public GameObject TitleGroup;
    // レースメニューボタン
    public GameObject ButtonSingle;
    public GameObject ButtonMulti;
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

    // UIテキスト
    // 付箋タブメニューテキスト
    public TextMeshProUGUI menu1;
    public TextMeshProUGUI menu1En;
    public TextMeshProUGUI menu2;
    public TextMeshProUGUI menu2En;
    public TextMeshProUGUI menu3;
    public TextMeshProUGUI menu3En;
    public TextMeshProUGUI menu4;
    public TextMeshProUGUI menu4En;
    public TextMeshProUGUI menu5;
    public TextMeshProUGUI menu5En;
    public TextMeshProUGUI menu6;
    public TextMeshProUGUI menu6En;

    // 各メニューテキスト
    public TextMeshProUGUI raceTxtTmp; // レースメニュー
    public TextMeshProUGUI raceTxtTmpEn;
    public TextMeshProUGUI howtoTxtTmp; // 遊び方メニュー
    public TextMeshProUGUI howtoTxtTmpEn;
    public TextMeshProUGUI rankingTxtTmp; // ランキングメニュー
    public TextMeshProUGUI newsTitleTxtTmp; // ニュースメニュー
    public TextMeshProUGUI newsTimeTxtTmp;
    public TextMeshProUGUI newsBodyTxtTmp;
    public TextMeshProUGUI creditTxtTmp; // クレジットメニュー
    public TextMeshProUGUI creditTxtTmpEn;
    public TextMeshProUGUI configTxtTmp; // 設定メニュー
    public TextMeshProUGUI configTxtTmpEn;
    string SinglePlayStringJa = "シングルプレイ";
    string SinglePlayStringEn = "Single Play";
    // メニュー設定画面UIチェックマーク
    public Image checkJa;
    public Image checkEn;
    public Image checkLeft;
    public Image checkRight;
    public Image checkSmall;
    public Image checkMiddle;
    public Image checkLarge;
    public Image checkVoiceOn;
    public Image checkVoiceOff;

    // ハンドコントローラー
    public GameObject LeftHandAnchor;
    public GameObject RightHandAnchor;
    // 指先オブジェクト
    public GameObject RIndex;
    public GameObject LIndex;
    // 指先コライダー
    public GameObject RHand;
    public GameObject LHand;
    // 指先コライダー設定状況
    public bool initIndex;
    // シーンロード中フラグ
    public bool isLoading;

    // オーディオソース
    public AudioSource soundObject; // 効果音用
    public AudioSource soundObjectBgm; // BGM用
    public AudioSource soundObjectVoice; // ボイスナビ用

    // オーディオクリップ
    public AudioClip bgm; // BGM
    public AudioClip clickClip; // タッチ音
    public AudioClip flipClip; // めくり音
    public AudioClip v_welcome_ja; // ボイスナビ あいさつ(日本語)
    public AudioClip v_welcome_en; // ボイスナビ あいさつ(英語)
    public AudioClip v_clipboard_ja; // ボイスナビ メニュー案内(日本語)
    public AudioClip v_clipboard_en; // ボイスナビ メニュー案内(英語)

    // PlayFab
    string selfPlayerId; // プレイヤーID
    [SerializeField] GetPlayerCombinedInfoRequestParams InfoRequestParams; // PlayFabリクエスト設定
    public bool playFabLogin; // ログイン状態

    // フェード処理
    [SerializeField] OVRScreenFade fade;

    /// <summary>
    /// 初期実行処理
    /// </summary>
    void Start() {
        fade.SetExplicitFade(1);

        if (Utility.USE_PLAYFAB) {
            // PlayFabリクエスト処理
            InfoRequestParams.GetUserData = true; // プレイヤーデータを取得する
            InfoRequestParams.GetTitleData = true; // タイトルデータを取得する
            PlayFabAuthService.Instance.InfoRequestParams = InfoRequestParams;
            PlayFabAuthService.OnLoginSuccess += PlayFabLogin_OnLoginSuccess;
            PlayFabAuthService.Instance.Authenticate(Authtypes.Silent);
        }


        // 初期設定
        InitConfig(); // 言語設定初期化
        InitHand(); // 利き手設定初期化
        InitVoice(); // ボイスナビ設定初期化

        // メニュー初期設定
        // 付箋タブアクティブ設定初期化
        Tag1Active.SetActive(true);
        Tag2Active.SetActive(false);
        Tag3Active.SetActive(false);
        Tag4Active.SetActive(false);
        Tag5Active.SetActive(false);
        Tag6Active.SetActive(false);

        // メニュー内ボタンアクティブ設定初期化
        ButtonSingle.SetActive(true);
        if (Utility.USE_PHOTON) {
            ButtonMulti.SetActive(true);
        } else {
            ButtonMulti.SetActive(false);
        }
        ButtonJa.SetActive(false);
        ButtonEn.SetActive(false);
        ButtonLeft.SetActive(false);
        ButtonRight.SetActive(false);
        ButtonSmall.SetActive(false);
        ButtonMiddle.SetActive(false);
        ButtonLarge.SetActive(false);
        ButtonVoiceOff.SetActive(false);
        ButtonVoiceOn.SetActive(false);

        // メニュー内オブジェクトアクティブ設定初期化
        TitleGroup.SetActive(true);
        if (lang == "ja" || lang == "Ja") {
            raceTxtTmp.enabled = true;
            raceTxtTmpEn.enabled = false;
            if (!Utility.USE_PHOTON) {
                raceTxtTmp.text = "□" + SinglePlayStringJa + "\n\n" + Utility.MULTI_PHOTON_ERROR_JA;
            }
        } else {
            raceTxtTmp.enabled = false;
            raceTxtTmpEn.enabled = true;
            if (!Utility.USE_PHOTON) {
                raceTxtTmpEn.text = "□" + SinglePlayStringEn + "\n\n" + Utility.MULTI_PHOTON_ERROR_EN;
            }
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
        checkJa.enabled = false;
        checkEn.enabled = false;
        checkLeft.enabled = false;
        checkRight.enabled = false;

        // メニュー内テキスト表示初期化
        rankingTxtTmp.text = "";

        // メニュー番号初期化
        currentMenuNo = 1;

        // 指先コライダー設定状況初期化
        initIndex = false;

        // BGMを再生開始
        soundObjectBgm = gameObject.AddComponent<AudioSource>();
        soundObjectBgm.clip = bgm;
        soundObjectBgm.loop = true;
        soundObjectBgm.volume = 0.3f;
        soundObjectBgm.Play();

        fade.SetExplicitFade(0);
        fade.FadeIn();

        // 一定時間経過後にボイスナビ再生開始
        soundObjectVoice = gameObject.AddComponent<AudioSource>();
        // ボイスナビの再生設定が有効であれば
        if (voice != "off") {
            // 再生中のボイスナビは停止
            soundObjectVoice.Stop();
            // 0.5秒後に1回だけ再生
            // バルーンレースVRへようこそ。熱気球レースを楽しみましょう！
            this.UpdateAsObservable().First().DelaySubscription(System.TimeSpan.FromSeconds(2.5f)).Subscribe(_ => {
                if (lang == "ja") {
                    if (v_welcome_ja != null)
                        soundObjectVoice.PlayOneShot(v_welcome_ja, 1.0f);
                } else {
                    if (v_welcome_en != null)
                        soundObjectVoice.PlayOneShot(v_welcome_en, 1.0f);
                }
            }).AddTo(this);
            // 6秒後に1回だけ再生
            // お手元のクリップボードでメニューを選択できます。
            this.UpdateAsObservable().First().DelaySubscription(System.TimeSpan.FromSeconds(8.0f)).Subscribe(_ => {
                if (lang == "ja") {
                    if (v_welcome_ja != null)
                        soundObjectVoice.PlayOneShot(v_clipboard_ja, 1.0f);
                } else {
                    if (v_welcome_en != null)
                        soundObjectVoice.PlayOneShot(v_clipboard_en, 1.0f);
                }
            }).AddTo(this);
        }
    }

    /// <summary>
    /// フレームごとの実行処理
    /// </summary>
    void Update() {
        // 指先コライダーが未設定であれば設定(Startより後に1回だけ実行する必要あり)
        if (!initIndex) {
            RHand.transform.parent = RIndex.transform;
            RHand.transform.localPosition = Vector3.zero;
            LHand.transform.parent = LIndex.transform;
            LHand.transform.localPosition = Vector3.zero;
            RHand.GetComponent<SphereCollider>().enabled = true;
            LHand.GetComponent<SphereCollider>().enabled = true;
            initIndex = true;
        }
#if UNITY_EDITOR
        // PCでマルチプレイ開始する処理(デバッグ用)
        if (Input.GetKeyDown(KeyCode.Return)) {
            // fade.FadeOut();
            StartCoroutine("LoadMultiSceneCoroutine");
        }
#endif
    }
    /// <summary>
    /// 各設定の初期化処理
    /// </summary>
    void InitConfig() {
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
            PlayerPrefs.SetString("Map", mapSize);
        }
        // PlayerPrefsからボイスナビ設定を取得して更新(初期値はon)
        voice = PlayerPrefs.GetString("Voice");
        if (voice == null) {
            voice = "on";
            PlayerPrefs.SetString("Voice", voice);
        }
        // 言語設定に応じてメニューの付箋タブの表示言語を切り替える
        if (lang == "ja" || lang == "Ja") {
            menu1.enabled = true;
            menu2.enabled = true;
            menu3.enabled = true;
            menu4.enabled = true;
            menu5.enabled = true;
            menu6.enabled = true;
        } else {
            menu1En.enabled = true;
            menu2En.enabled = true;
            menu3En.enabled = true;
            menu4En.enabled = true;
            menu5En.enabled = true;
            menu6En.enabled = true;
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
        // 言語設定に応じて各表示を更新
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
        }
        // PlayFabからランキングデータを取得
        GetPlayerData();
        // PlayFabからニュースデータを取得
        GetNews();
        // 言語を切り替えに伴って改めて指定言語でボイスナビを再生する
        // ボイスナビの再生設定が有効であれば
        if (voice != "off") {
            // 再生中のボイスナビは停止
            soundObjectVoice.Stop();
            // 0.5秒後に1回だけ再生
            // バルーンレースVRへようこそ。熱気球レースを楽しみましょう！
            this.UpdateAsObservable().First().DelaySubscription(System.TimeSpan.FromSeconds(0.5f)).Subscribe(_ => {
                if (lang == "ja") {
                    if (v_welcome_ja != null)
                        soundObjectVoice.PlayOneShot(v_welcome_ja, 1.0f);
                } else {
                    if (v_welcome_en != null)
                        soundObjectVoice.PlayOneShot(v_welcome_en, 1.0f);
                }
            }).AddTo(this);
            // 6秒後に1回だけ再生
            // お手元のクリップボードでメニューを選択できます。
            this.UpdateAsObservable().First().DelaySubscription(System.TimeSpan.FromSeconds(6.0f)).Subscribe(_ => {
                if (lang == "ja") {
                    if (v_welcome_ja != null)
                        soundObjectVoice.PlayOneShot(v_clipboard_ja, 1.0f);
                } else {
                    if (v_welcome_en != null)
                        soundObjectVoice.PlayOneShot(v_clipboard_en, 1.0f);
                }
            }).AddTo(this);
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
        // 選択に応じてマップサイズ設定メニューのチェックボックスを切り替える
        if (mapSize == "small") {
            checkSmall.enabled = true;
            checkMiddle.enabled = false;
            checkLarge.enabled = false;
        } else if (mapSize == "large") {
            checkSmall.enabled = false;
            checkMiddle.enabled = false;
            checkLarge.enabled = true;
        } else {
            checkSmall.enabled = false;
            checkMiddle.enabled = true;
            checkLarge.enabled = false;
        }
    }
    /// <summary>
    /// ボイスナビ設定の初期化処理
    /// </summary>
    void InitVoice() {
        // ボイスナビ設定をPlayerPrefsから取得して更新(初期値はon)
        string voiceSetting = PlayerPrefs.GetString("Hand");
        if (voiceSetting == null) {
            voice = "on";
            PlayerPrefs.SetString("Hand", voice);
        }
    }
    /// <summary>
    /// ボイスナビ設定の更新処理
    /// </summary>
    /// <param name="voiceSetting">ボイスナビ設定</param>
    public void SetVoice(string voiceSetting) {
        // ボイスナビ設定をPlayerPrefsから取得して切り替わっていれば更新
        string voice = PlayerPrefs.GetString("Voice");
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
            // ONへの切り替えに応じて改めて再生する
            if (voice != "off") {
                // 一旦ボイスナビの再生を停止
                soundObjectVoice.Stop();
                // バルーンレースVRへようこそ。熱気球レースを楽しみましょう！
                this.UpdateAsObservable().First().DelaySubscription(System.TimeSpan.FromSeconds(0.5f)).Subscribe(_ => {
                    if (lang == "ja") {
                        if (v_welcome_ja != null)
                            soundObjectVoice.PlayOneShot(v_welcome_ja, 1.0f);
                    } else {
                        if (v_welcome_en != null)
                            soundObjectVoice.PlayOneShot(v_welcome_en, 1.0f);
                    }
                }).AddTo(this);
                // お手元のクリップボードでメニューを選択できます。
                this.UpdateAsObservable().First().DelaySubscription(System.TimeSpan.FromSeconds(6.0f)).Subscribe(_ => {
                    if (lang == "ja") {
                        if (v_welcome_ja != null)
                            soundObjectVoice.PlayOneShot(v_clipboard_ja, 1.0f);
                    } else {
                        if (v_welcome_en != null)
                            soundObjectVoice.PlayOneShot(v_clipboard_en, 1.0f);
                    }
                }).AddTo(this);
            }
        }
    }
    /// <summary>
    /// 利き手設定の初期化処理
    /// </summary>
    void InitHand() {
        // 利き手設定をPlayerPrefsから取得して更新(初期値はright)
        dominantHand = PlayerPrefs.GetString("Hand");
        if (dominantHand == null) {
            dominantHand = "right";
            PlayerPrefs.SetString("Hand", dominantHand);
        }
        // クリップボードを利き手の反対の手に持ち換えて付箋タブの位置を利き手側に移動する
        if (dominantHand == "left" || dominantHand == "Left") {
            Clipboard.transform.parent = RightHandAnchor.transform;
            Clipboard.transform.localPosition = new Vector3(-0.18f, 0.07f, 0.19f);
            Clipboard.transform.localRotation = Quaternion.Euler(42.2f, -221.8f, -85.0f);
            StickyGroup.transform.localPosition = new Vector3(-49.49f, 42.4f, 1.69f);
        } else {
            Clipboard.transform.parent = LeftHandAnchor.transform;
            Clipboard.transform.localPosition = new Vector3(0.18f, 0.07f, 0.19f);
            Clipboard.transform.localRotation = Quaternion.Euler(42.2f, 221.8f, 85.0f);
            StickyGroup.transform.localPosition = new Vector3(49.49f, 42.4f, 1.69f);
        }
    }
    /// <summary>
    /// 利き手設定の更新処理
    /// </summary>
    /// <param name="hand">利き手設定(right/left)</param>
    public void SetHand(string hand) {
        // 利き手設定がPlayerPrefsの設定と異なっている場合は更新
        dominantHand = PlayerPrefs.GetString("Hand");
        if (hand == dominantHand) {
            return;
        }
        dominantHand = hand;
        // PlayerPrefsの利き手設定を更新
        PlayerPrefs.SetString("Hand", dominantHand);
        // クリック音再生
        if (clickClip != null) {
            soundObject.PlayOneShot(clickClip);
        }
        // 利き手設定のチェックを切り替え
        // 腕時計とクリップボードを利き手の反対の手に持ち換えて付箋タブの位置を利き手側に移動する
        if (dominantHand == "left" || dominantHand == "Left") {
            checkLeft.enabled = true;
            checkRight.enabled = false;
            Clipboard.transform.parent = RightHandAnchor.transform;
            Clipboard.transform.localPosition = new Vector3(-0.25f, 0.1f, 0.15f);
            Clipboard.transform.localRotation = Quaternion.Euler(42.2f, -221.8f, -50.45f);
            StickyGroup.transform.localPosition = new Vector3(-49.49f, 42.4f, 1.69f);
        } else {
            checkRight.enabled = true;
            checkLeft.enabled = false;
            Clipboard.transform.parent = LeftHandAnchor.transform;
            Clipboard.transform.localPosition = new Vector3(0.25f, 0.1f, 0.15f);
            Clipboard.transform.localRotation = Quaternion.Euler(42.2f, 221.8f, 50.45f);
            StickyGroup.transform.localPosition = new Vector3(49.49f, 42.4f, 1.69f);
        }
    }
    /// <summary>
    /// マルチプレイシーンの非同期ロード処理
    /// </summary>
    IEnumerator LoadMultiSceneCoroutine() {
        // 非同期でロードを開始
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MultiScene");

        // 非同期ロードが完了するまで待つ
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }
    /// <summary>
    /// アクティブになったときの処理
    /// </summary>
    void OnEnable() {
    }
    /// <summary>
    /// PlayFabのログイン成功時の処理
    /// </summary>
    /// <param name="result">ログイン結果</param>
    private void PlayFabLogin_OnLoginSuccess(LoginResult result) {
        playFabLogin = true;
        // PlayFabの言語設定を更新
        Utility.SetProfileLanguage();
        // PlayFabからランキングデータを取得
        GetPlayerData();
        // PlayFabからニュースデータを取得
        GetNews();
    }
    /// <summary>
    /// 非アクティブになったときの処理
    /// </summary>
    private void OnDisable() {
        if (Utility.USE_PLAYFAB) {
            // PlayFabログイン成功時の処理の紐付けをいったん解除
            PlayFabAuthService.OnLoginSuccess -= PlayFabLogin_OnLoginSuccess;
        }
    }
    /// <summary>
    /// PlayFabからプレイヤーデータを非同期読み込み
    /// </summary>
    private async void GetPlayerData() {
        // PlayFabが有効であれば
        if (Utility.USE_PLAYFAB) {
            PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest() {
                PlayFabId = ""
            }, result => {
                var playerProfile = result.PlayerProfile;
                selfPlayerId = playerProfile.PlayerId;
            }, error => Debug.Log(error.GenerateErrorReport()));
            rankingTxtTmp.text = "";
            // PlayFabからランキングデータを読み込み
            RequestLeaderBoard();
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
    /// PlayFabからランキングデータを読み込み
    /// </summary>
    void RequestLeaderBoard() {
        // PlayFabが有効であれば
        if (Utility.USE_PLAYFAB) {
            // PlayFabのランキングのCourse1のランキングデータを0番目から15件まで取得
            PlayFabClientAPI.GetLeaderboard(
            new GetLeaderboardRequest {
                StatisticName = Utility.PLAYFAB_RANKING_NAME,
                StartPosition = 0,
                MaxResultsCount = 15,

            },
            result => {
                rankingTxtTmp.text = "";
                // 順位表記を言語に応じて切り替える(英語の場合はPosition+1の一の位の値に応じて1st,2nd,3rd,4th…の表記)
                lang = PlayerPrefs.GetString("Lang");
                // 取得結果を元にランキング表示を更新(自身のPlayFabIdであればYou表示を添える)
                // StatValueは1000倍のマイナスの整数で格納されているため、正の数に反転させて1000で除算する
                result.Leaderboard.ForEach(
                    x => rankingTxtTmp.text +=
                    ((x.Position + 1) + ((lang == "ja" || lang == "Ja") ? "位 " : ((x.Position + 1) % 10 == 1) ? "st " : ((x.Position + 1) % 10 == 2) ? "nd " : ((x.Position + 1) % 10 == 3) ? "rd " : "th ") +
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
            // PlayFabを有効にしていない場合は
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
    /// <summary>
    /// クリップボードメニューの各付箋タブをタッチした時の処理
    /// </summary>
    /// <param name="tagId">タグID</param>
    public void TouchTag(int tagId) {
        // メニューIDと異なる場合のみ実行
        if (tagId == currentMenuNo) {
            return;
        }
        // ページめくり音再生
        if (flipClip != null) {
            soundObject.PlayOneShot(flipClip);
        }
        // 各表示・ボタンを一旦全てオフに更新
        TitleGroup.SetActive(false);
        Tag1Active.SetActive(false);
        Tag2Active.SetActive(false);
        Tag3Active.SetActive(false);
        Tag4Active.SetActive(false);
        Tag5Active.SetActive(false);
        Tag6Active.SetActive(false);
        ButtonSingle.SetActive(false);
        ButtonMulti.SetActive(false);
        ButtonJa.SetActive(false);
        ButtonEn.SetActive(false);
        ButtonLeft.SetActive(false);
        ButtonRight.SetActive(false);
        ButtonSmall.SetActive(false);
        ButtonMiddle.SetActive(false);
        ButtonLarge.SetActive(false);
        ButtonVoiceOn.SetActive(false);
        ButtonVoiceOff.SetActive(false);
        raceTxtTmp.enabled = false;
        raceTxtTmpEn.enabled = false;
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
        checkJa.enabled = false;
        checkEn.enabled = false;
        checkLeft.enabled = false;
        checkRight.enabled = false;
        checkSmall.enabled = false;
        checkMiddle.enabled = false;
        checkLarge.enabled = false;
        checkVoiceOn.enabled = false;
        checkVoiceOff.enabled = false;

        // タグIDに応じて更新
        switch (tagId) {
            // レースメニュー
            case 1:
                currentMenuNo = 1;
                Tag1Active.SetActive(true);
                // ゲームタイトルパーツを表示
                TitleGroup.SetActive(true);
                // 各ボタンを有効化
                ButtonSingle.SetActive(true);
                if (Utility.USE_PHOTON) {
                    ButtonMulti.SetActive(true);
                }
                if (lang == "ja" || lang == "Ja") {
                    raceTxtTmp.enabled = true;
                    if (!Utility.USE_PHOTON) {
                        raceTxtTmp.text = "□" + SinglePlayStringJa + "\n\n" + Utility.MULTI_PHOTON_ERROR_JA;
                    }
                } else {
                    raceTxtTmpEn.enabled = true;
                    if (!Utility.USE_PHOTON) {
                        raceTxtTmpEn.text = "□" + SinglePlayStringEn + "\n\n" + Utility.MULTI_PHOTON_ERROR_EN;
                    }
                }
                break;
            // 遊び方メニュー
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
                // PlayFabからランキングデータを取得
                GetPlayerData();
                rankingTxtTmp.enabled = true;
                break;
            // ニュースメニュー
            case 4:
                currentMenuNo = 4;
                Tag4Active.SetActive(true);
                // PlayFabからニュースデータを取得
                GetNews();
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
                // 各設定ボタンを有効化
                ButtonJa.SetActive(true);
                ButtonEn.SetActive(true);
                ButtonLeft.SetActive(true);
                ButtonRight.SetActive(true);
                ButtonSmall.SetActive(true);
                ButtonMiddle.SetActive(true);
                ButtonLarge.SetActive(true);
                ButtonVoiceOn.SetActive(true);
                ButtonVoiceOff.SetActive(true);
                // 言語設定に応じて表示更新
                lang = PlayerPrefs.GetString("Lang");
                if (lang == "ja" || lang == "Ja") {
                    configTxtTmp.enabled = true;
                    checkJa.enabled = true;
                } else {
                    configTxtTmpEn.enabled = true;
                    checkEn.enabled = true;
                }
                // 利き手設定に応じて表示更新
                dominantHand = PlayerPrefs.GetString("Hand");
                if (dominantHand == "left" || dominantHand == "Left") {
                    checkLeft.enabled = true;
                } else {
                    checkRight.enabled = true;
                }
                // マップサイズ設定に応じて表示更新
                mapSize = PlayerPrefs.GetString("Map");
                if (mapSize == "small") {
                    checkSmall.enabled = true;
                } else if (mapSize == "large") {
                    checkLarge.enabled = true;
                } else {
                    checkMiddle.enabled = true;
                }
                // ボイスナビ設定に応じて表示更新
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
