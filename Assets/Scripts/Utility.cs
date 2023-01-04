using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

/// <summary>
/// 共通処理
/// </summary>
public partial class Utility
{
    /// <summary>PlayFab利用フラグ</summary>
    /// <Remarks>
    /// このプロジェクトではランキングやニュースのサーバーデータ同期にMicrosoft PlayFabを使用しています。<br />
    /// <br />
    /// このアプリ用のPlayFabサーバーデータ同期を有効にする手順：<br />
    /// 1. PlayFabにアカウント登録します。(https://developer.playfab.com/en-US/login)<br />
    /// 2. ログイン後、右上のMy Profileから言語のタイムゾーンを設定します。<br />
    ///    「新しいスタジオ」から任意の名前のスタジオを作成します。認証プロバイダーはPlayFabです。<br />
    ///    タイトル右の歯車アイコン→タイトル情報の編集で任意の名前のタイトルを設定します。<br />
    /// 3. タイトルの設定→API機能→タイトルIDで表示される文字列と
    ///    タイトルの設定→シークレットキー→シークレットキーで表示される文字列を控えておきます。<br />
    /// 4. このUnityプロジェクトの/Assets/PlayFabSDK/Shared/Public/Resources/PlayFabSharedSettingsの
    ///    Title IdにタイトルIDを、Developer Secret keyにシークレットキーをそれぞれ設定します。<br />
    /// <br />
    /// このアプリ向けにPlayFabランキングを有効にする手順：<br />
    /// 1. タイトルの設定→API機能→「クライアントにプレイヤー統計情報のポストを許可する」にチェック<br />
    /// 2. ランキングに「Course1」(※PLAYFAB_RANKING_NAMEで設定している文字列）を追加します。<br />
    ///    その際にリセット頻度は手動、集計方法は「最大(常に最大の値を使用)」を選択します。<br />
    /// <br />
    /// ニュース記事の登録手順：<br />
    /// 1. コンテンツ→タイトルニュース→新しいタイトルニュースを選択します。<br />
    ///    「続行するには、タイトルの既定の言語を設定してください。」と表示されたら<br />
    ///    設定を開く→規定の言語：Japanese→保存します。<br />
    /// 2. 日本語でタイトルと記事を入力します。<br />
    /// 3. 「言語の追加」からEnglishを選択し、「＞」を押して入力フォームを広げます。<br />
    /// 4. 英語でタイトルと記事を入力します。<br />
    /// 5. 記事を公開状態にしてタイトルニュースを保存します。<br />
    ///    ※ゲーム内では記事は公開状態の最新の1件のみが表示されます。<br />
    /// <br />
    /// ここまで完了したら以下のUSE_PLAYFABをfalseからtrueに切り替えると、ランキングおよびニュースが有効になります。<br />
    /// <br />
    /// PlayFabの利用方法など詳細はREADMEをご参照下さい。
    /// </Remarks>
    public const bool USE_PLAYFAB = false;

    /// <summary>Photon利用フラグ</summary>
    /// <Remarks>
    /// このプロジェクトではマルチプレイにPhotonを利用しています。<br />
    /// マルチプレイを使用する手順：<br />
    /// 1. Photon(https://www.photonengine.com/ja-JP/Photon)にアクセスしてアカウント登録します。<br />
    /// 2. 登録後、ダッシュボードにアクセスします。<br />
    /// 3. 「新しくアプリを作成する」をクリックします。<br />
    /// 4. Photonの種別：PUN、任意のアプリケーション名を入力してアプリを作成します。<br />
    /// 5. ダッシュボードに表示されているアプリケーションIDをクリックして文字列をコピーします。<br />
    /// 6. このUnityプロジェクトの/Assets/Photon/PhotonUnityNetwork/Resources/PhotonServerSettingsの<br />
    /// 「App Id PUN」に、Photonのタイトル一覧に表示されていたアプリケーションIDを登録します。<br />
    /// 7. 以下のUSE_PHOTONをfalseからtrueに切り替えるとマルチプレイが有効になります。<br />
    /// <br />
    /// Photonの利用方法など詳細はREADMEをご参照下さい。
    /// </Remarks>
    public const bool USE_PHOTON = false;
    /// <summary>PlayFabのランキング名</summary>
    public const string PLAYFAB_RANKING_NAME = "Course1";

    /// <summary>制限時間</summary>
    public const int TIME_LIMIT = 300;
    /// <summary>高度別の風向き</summary>
    /// <Remarks>十の位以上=風力 / 一の位=45度単位の風向き</Remarks>
    public static readonly int[] WIND_POWERS = { 30, 50, 66, 77, 62, 53, 54, 55, 56, 35 };
    // システム内テキスト
    /// <summary>プレイガイドメッセージ(日本語)</summary>
    public const string GUIDE_MESSAGE_JA = 
        "レーススタート！\n" +
        "制限時間は5分です。\n" +
        "\n" +
        "上のレバーに右手を合わせて握ると、気球がバーナーで暖められて上昇します。\n" +
        "左のロープに左手を合わせてつかむと、暖かい空気が抜けて下降します。\n" +
        "\n" +
        "マップで風の方向を確認して、向こうに見える赤いターゲットに近づけるように操縦しましょう。\n" +
        "近づいたら右にある白いマーカーをつかんで落としたら終了です。\n" +
        "\n" +
        "手元の腕時計かクリップボードに触れるとポーズできます。\n" +
        "\n" +
        "それではレースを楽しみましょう！\n";
    /// <summary>プレイガイドメッセージ(英語)</summary>
    public const string GUIDE_MESSAGE_EN =
        "Race start!\n" +
        "The time limit is 5 minutes.\n" +
        "\n" +
        "Place your right hand on the upper lever and hold it to warm the balloon with a burner and raise it.\n" +
        "If you put your left hand on the left rope and grab it, warm air will escape and you will descend.\n" +
        "\n" +
        "Check the direction of the wind on the map and steer closer to the red target you can see over there.\n" +
        "When you get close to the target, grab the white marker on the right and drop it to finish.\n" +
        "\n" +
        "You can pause by touching your wristwatch or clipboard.\n" +
        "\n" +
        "Let's enjoy the race!\n";
    public const string STRING_TARGET_JA = "ターゲット";
    public const string STRING_TARGET_EN = "Target";
    public const string STRING_UNTIL_TARGET_JA = "ターゲットまで：";
    public const string STRING_UNTIL_TARGET_EN = " to the Target";
    public const string STRING_ALTITUDE_JA = "高度";
    public const string STRING_ALTITUDE_EN = "Altitude";
    public const string STRING_REMAIN_TIME_JA = "残り時間";
    public const string STRING_REMAIN_TIME_EN = "left";
    public const string STRING_WIND_SPEED_JA = "風速";
    public const string STRING_WIND_SPEED_EN = "Wind speed";
    public const string STRING_TIME_OUT = "Time Out!";
    public const string STRING_GAME_CLEAR = "Game Clear!";
    public const string STRING_RECORD_JA = "記録";
    public const string STRING_RECORD_EN = "Record";
    // エラーメッセージ
    public const string NEWS_PLAYFAB_ERROR_JA = "※PlayFabの設定が必要です(Assets/Scripts/Utility)";
    public const string NEWS_PLAYFAB_ERROR_EN = "* Please set PlayFab from Assets/Scripts/Utility.";
    public const string RANKING_PLAYFAB_ERROR_JA = "※PlayFabの設定が必要です(Assets/Scripts/Utility)";
    public const string RANKING_PLAYFAB_ERROR_EN = "* Please set PlayFab from Assets/Scripts/Utility.";
    public const string MULTI_PHOTON_ERROR_JA = "※マルチプレイはPhotonの設定が必要です(Assets/Scripts/Utility)";
    public const string MULTI_PHOTON_ERROR_EN = "* Photon settings are required to enable multiplayer(Assets/Scripts/Utility).";

    /// <summary>
    /// PlayFabのプレイヤー言語の更新処理
    /// </summary>
    public static void SetProfileLanguage() {
        // 言語設定をPlayerPrefsから取得
        string lang = PlayerPrefs.GetString("Lang", "ja");
        // PlayFabが有効であれば
        if (Utility.USE_PLAYFAB) {
            // PlayFabのプレイヤー言語を更新する
            PlayFabProfilesAPI.SetProfileLanguage(new PlayFab.ProfilesModels.SetProfileLanguageRequest {
                Entity = new PlayFab.ProfilesModels.EntityKey {
                    Id = PlayFabSettings.staticPlayer.EntityId,
                    Type = PlayFabSettings.staticPlayer.EntityType
                },
                Language = lang
            }, result => {
            }, error => {
            });
        }
    }

    /// <summary>
    /// PlayFabへのレコード送信処理
    /// </summary>
    /// <Remark>
    /// 値の小さい順ランキングを実現するために、
    /// PlayFabのリーダーボードのランキングデータはマイナスの値で大きい順に保管しているため
    /// 送信時にスコアデータにマイナスをかける
    /// </Remark>
    /// <param name="record">スコアデータ</param>
    public static void SubmitRecord(int record) {
        // PlayFabが使用可能であれば
        if (USE_PLAYFAB) {
            // ランキングにデータを送信
            PlayFabClientAPI.UpdatePlayerStatistics(
            new UpdatePlayerStatisticsRequest {
                Statistics = new List<StatisticUpdate>() {
                    new StatisticUpdate{
                        StatisticName = PLAYFAB_RANKING_NAME,
                        Value = record * -1
                    }
                }
            },
            result => {
            }, error => {
            }
            );
        }
    }

    /// <summary>
    /// バイブレーション処理
    /// </summary>
    /// <param name="duration">振動時間(デフォルト:0.1)</param>
    /// <param name="frequency">振幅(デフォルト:0.1)</param>
    /// <param name="amplitude">振動数(デフォルト:0.1)</param>
    /// <param name="controller">対象のコントローラー</param>
    public static IEnumerator Vibrate(
        float duration = 0.1f,
        float frequency = 0.1f,
        float amplitude = 0.1f,
        OVRInput.Controller controller = OVRInput.Controller.Active
    ) {
        //コントローラーを振動させる
        OVRInput.SetControllerVibration(frequency, amplitude, controller);

        //指定された時間待つ
        yield return new WaitForSeconds(duration);

        //コントローラーの振動を止める
        OVRInput.SetControllerVibration(0, 0, controller);
    }
}
