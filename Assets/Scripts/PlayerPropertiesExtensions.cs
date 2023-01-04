using ExitGames.Client.Photon;
using Photon.Realtime;

/// <summary>
/// Photonのカスタムプロパティを扱うための拡張メソッド
/// </summary>
public static class PlayerPropertiesExtensions
{
    /// <summary>スコアキー名</summary>
    private const string ScoreKey = "Score";

    /// <summary>ハッシュテーブル</summary>
    private static readonly Hashtable propsToSet = new Hashtable();

    /// <summary>
    /// プレイヤーのスコアを取得する処理
    /// </summary>
    /// <param name="player">Playerオブジェクト</param>
    /// <returns>スコアデータ</returns>
    public static int GetScore(this Player player) {
        // 値が取得できれば返す、デフォルトは0
        return (player.CustomProperties[ScoreKey] is int score) ? score : 0;
    }

    /// <summary>
    /// プレイヤーのスコアを更新する処理
    /// </summary>
    /// <param name="player">Playerオブジェクト</param>
    /// <param name="value">スコア</param>
    public static void UpdateScore(this Player player, int value) {
        // 引数をカスタムプロパティとして更新
        propsToSet[ScoreKey] = value;
        player.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }
    /// <summary>
    /// プレイヤーのカスタムプロパティを送信する処理
    /// </summary>
    /// <param name="player">Playerオブジェクト</param>
    public static void SendPlayerProperties(this Player player) {
        // 送信する要素があれば送信
        if (propsToSet.Count > 0) {
            player.SetCustomProperties(propsToSet);
            propsToSet.Clear();
        }
    }
}