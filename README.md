# BalloonRaceVR

Balloon Race VR Version1.1 (Free Asset Edition) プロジェクト説明書

# 目次
* 対応機種
* ゲーム概要
* 熱気球レースについて
* 実際のレースと異なる点
* プロジェクトバージョン
* 熱気球のアセットについて
* オンライン要素について
* PlayFabの設定方法
* Photonの設定方法
* 制作期間
* プレイ時の注意点
* プロジェクト構成
* シーン/メニュー構成
* スクリプト構成
* アプリ更新履歴
* リソース
* 謝辞

# 対応機種
* Meta Quest2
* Meta Quest

# ゲーム概要
* カラフルな熱気球で空中散歩に出かけてみませんか？上昇と下降だけのシンプルな操作で風を読んでゴールを目指す、
  ランキングや通信対戦にも対応したレースゲームです。VRで熱いスカイスポーツを体験しましょう！

* 作品紹介ムービー  
https://vracademy.jp/introduction/%e7%ac%ac10%e6%9c%9f%e7%94%9f%e5%84%aa%e7%a7%80%e4%bd%9c%e5%93%81/

# 熱気球レースについて
* このゲームは熱気球競技大会のさまざまな競技(タスク)のうち、
決められたターゲットにマーカーを投下してターゲットからの距離を競う
JDG（ジャッジ・ディクレアード・ゴール）をモデルに制作しました。  
http://www.jballoon.jp/task.html

* 実際の熱気球の競技大会やルールにつきましては日本気球連盟のサイトをご参照下さい。  
http://www.jballoon.jp/
* ※この作品は１人の熱気球ファンが制作した非公式のゲームです。  
  公式の各団体とは無関係の作品ですので、日本気球連盟さまへのお問い合わせなどはご遠慮下さい。

# 実際のレースと異なる点
* この作品はあくまでゲームのため、実際のレースとは異なる点が多数存在します。
（ゲーム化するにあたって意図的に変えている部分と、技術的な理由によって異なる点があります）
* 日中の環境になっていますが、実際のレースは上昇気流の少ない早朝もしくは夕方に開催されます。
* ゲームとしてターゲットに落とす部分に絞っているため、熱気球の立ち上げ・離陸や収納の様子は含まれていません。
* 競技においては離陸場所の選定も重要な要素ですが、ゲーム側で指定したランダムな位置からのスタートになります。
* ゲームとして短時間でプレイできるようにするため、気球の移動速度は実際よりも速くなっており、制限時間も設けています。
* 風向きが立体的に見えるマップはゲームのオリジナル要素です。  
  風の動きは観測が必要で、ヘリウム入りの黒い風船が使われるようです。
* １つのレバーでバーナーを操作していますが、実際は2基のバーナーはそれぞれで操作できます。
* ゲーム内でピンと張られている下降用のロープ（リップライン）は実際には垂れ下がった形状になっています。  
  これを引っ張ることによって球皮上部のリップパネルにすき間ができて熱気球が下降します。
* マーカーは布と砂袋で構成されており、布を持って砂袋を垂らして落とす方法が一般的です。
  棒状になったり地面で跳ね返ったりしてしまっているのはゲーム実装時の技術的な理由によるものです。
* バーナーの音は実際の音とは異なります。音量も実際は非常に大きいですがゲームでは小さめに表現しています。
* 各気球には実際にはパイロットを含む１人以上の人間が乗っており、地上には競技スタッフや観客がいます。  
  また、競技ではバスケットにゼッケンが貼られています。
* クリップボード型のUIは競技の雰囲気をイメージしてオリジナルで作成したものです。
* Free Asset Editionの12種類の球皮は実際のデザインを参考にしつつ、完全オリジナルで作成したものです。  
  世界に存在する気球を全てチェックした訳ではありませんので、もし似たデザインがあっても偶然です。  
  オリジナル版の12種類の球皮はAir Balloon Proのアセットのデフォルトのデザインそのままです。

# プロジェクトバージョン
* このプロジェクトはUnity2021.1.27f1にて作成しました。  
  異なるUnityバージョンでの動作は未確認です。
  
# 熱気球のアセットについて
* 有料アセットの再配布は行えませんのでフリーのアセットに差し替えています。  
Air Balloon Pro (4K + Physics)  
https://assetstore.unity.com/packages/3d/vehicles/air/air-balloon-pro-4k-physics-144281  
3D Air Balloon  
https://assetstore.unity.com/packages/3d/vehicles/air/3d-air-balloon-192983
* オリジナル版の有料アセットに比べると熱気球の揺れ方やバスケットのデザインなどのリアリティは乏しいですが、
ゲームとしてプレイする上では特に大きな違いはありません。  
なお、気球のパーツ構成やテクスチャはカスタマイズを行っています。

# オンライン要素について
* オンライン要素については各サービスを利用した実装になっているため、プロジェクトのデータでは各サービスにアクセスするためのキーは外しています。
* 各自で利用登録を行ってキーを設定頂ければ有効になります。

# PlayFabの設定方法
* このプロジェクトではランキングやニュースのサーバーデータ同期にMicrosoft PlayFabを使用しています。
* サーバーデータ同期を有効にするにはPlayFabに登録の上、「新しいスタジオ」からスタジオを作成し、タイトルを作成します。
* タイトルの設定→API機能→タイトルIDと
タイトルの設定→シークレットキー→シークレットキーを控えておきます。

* /Assets/PlayFabSDK/Shared/Public/Resources/PlayFabSharedSettingsに
Title IdとDeveloper Secret keyを設定してください。

* PlayFabマスターへの道 - 【初心者向け】PlayFabの始め方【環境構築から初回ログインまで】  
https://playfab-master.com/playfab-how-to-start  
※PlayFabのログイン画面はバージョンによって上記と異なる場合があります。

* タイトルの設定→API機能→クライアントにプレイヤー統計情報のポストを許可するにチェック

* ランキングにCourse1を追加（最大・常に最大の値を使用を選択）

* コンテンツ→タイトルニュースで記事を追加、English記事も追加して公開状態にする

# Photonの設定方法
* このプロジェクトではマルチプレイにPhoton Unity Networking 2を使用しています。
* マルチプレイを有効にする手順
* Photonにアカウント登録の上、アプリを追加してApp Idを控えておきます。  
  /Assets/Photon/PhotonUnityNetwork/Resources/PhotonServerSettingsの「App Id PUN」にApp Idを登録してください。

* Photonの登録手順は以下ご参照下さい。  
  PUN2（Photon Unity Networking 2）で始めるオンラインゲーム開発入門 - 初期設定とチュートリアル  
  https://zenn.dev/o8que/books/bdcb9af27bdd7d/viewer/c04ad5

# 制作期間
* この作品は2021年11月～2022年2月までの約3か月（約１か月弱のチーム制作課題の期間を除く）で制作しました。

# プレイ時の注意点
* このゲームに限ったことではないと思いますが、スリープ状態からの復帰時など正面がずれる場合は
  Oculus Linkの場合は右コントローラーのOculusボタンを押して正面をリセット、
  ビルドしたアプリの場合はガーディアン設定を更新してください。

# 実機とPCのみでマルチプレイを確認する方法
* 検証用にUnityEditor単体でマルチプレイをプレイ可能にしています。
* ビルドしたアプリをSideQuestでQuest2実機にインストールしてマルチプレイを実行した状態で
  UnityEditorで実行すればマルチプレイの確認が可能です。

# プロジェクト構成
* ヒエラルキーは３つのシーンで構成されています。  
  ※シングルとマルチは共通の処理も多いのですが、全く同じ仕様という訳でもなく
  各モードの機能実装に伴う影響が読めなかったため、分けた実装になっています。  
MenuScene：プレイ開始時のメニューシーン  
SingleScene：シングルプレイ用シーン  
MultiScene：マルチプレイ用シーン  

* MenuScene  
　MenuController：プレイヤーのコントローラー  
　PlayFabController：PlayFabのコントローラー  
　Cover：外周を囲んでいるコライダー  
　Stage1：この環境のTerrainおよび霧  
　　TerrainA(3)：メニュー視点のTerrain（草をなびかせるためにTerrainを残しています）  
　　TerrainObject/Terrain*：外周を囲んでいるポリゴン化したTerrain(軽量化のためポリゴン化したものを配置しています)  
　3DAirBalloon1：プレイヤー  
　　OVRCameraRig：プレイヤーカメラおよびハンドコントローラー  
　　　TrackingSpace/LeftHandAnchor/Clipboard：ボードメニュー  
　　　TrackingSpace/RightHandAnchor/以下のRHand：タッチ用の指先コライダー  
　　　（開始後は指先hands_coll:b_r_index3に割り当て）  
　3DAirBalloon2-12：各ライバルプレイヤー  
* SingleScene  
　PlayFabController：PlayFabのコントローラー  
　Stage1：この環境のTerrainおよび霧  
　　TerrainObject/Terrain*：軽量化のためポリゴン化したTerrainを配置  
　Player1：プレイヤーのコントローラー  
　　3DAirBalloon1-4：ランダムで選択される  
　　　Pivot/Balloon_parts_cloth：球皮(布)  
　　　Balloon_parts_basket：バスケット  
　　　Flame1-2：バーナーの炎パーティクル  
　　　Frame：衝突防止用の外周コライダー  
　　LineButton：ロープの左手接触判定ボタン  
　　MarkerPrefab：マーカー  
　　OVRCameraRig：プレイヤーカメラおよびハンドコントローラー  
　　　TrackingSpace/LeftHandAnchor/WatchL/ButtonPause：腕時計型ポーズボタン  
　　MarkerButton：マーカーの右手接触判定ボタン  
　　clipboardL/Clipboard：メニューボードUI（ポーズ時はOVRCameraRig/TrackingSpace/LeftHandAnchorに移動）  
　　clipboardR：左利きの場合はボードがこちらに移動  
　　miniMap：ミニマップ  
　　　mapBase：土台  
　　　Terrain/default：ポリゴン化して縮小したTerrain  
　　　Grids：風マップがこの配下にインスタンス配置される  
　　　MiniMapCanvas:プレイヤー情報UI  
　　　MarkerCanvas：マーカー投下位置UI  
　　　GoalCanvas：ターゲット位置UI  
　　　プレイ中は以下がインスタンス配置されます  
　　　GoalTarget(Clone)：ゴール  
　　　myPosition(Clone)：プレイヤー位置  
　　　enemyPosition(Clone)：ライバル位置  
　　　moveRoute(Clone)：プレイヤー移動経路  
　　　enemyRoute(Clone)：ライバル移動経路  
　　　markerGroundPoint(Clone)：マーカー投下地点  
　　BurnerButton：バーナーレバーの右手接触判定ボタン  
　　BurnerTrigger：レバーを押すと動くレバー  
　　Line：左手でつかむと少し動くロープ(リップライン)  
　Player12-19：ライバルプレイヤーのコントローラー  
　Cube*：外周を囲んでいるコライダー  
　StartPos0-3：開始位置をこの中からランダムに決定してPlayer1を配置  
　GoalTarget：ターゲット（近くにマーカーを落としたら勝ち）  
* MultiScene  
　Stage1：この環境のTerrainおよび霧  
　Cube*：外周を囲んでいるコライダー  
　MultiScene：シーンコントローラー  
　　OVRCameraRig：プレイヤーカメラおよびハンドコントローラー（プレイ時はAvatar（Clone)以下に移動）  
　StartPos0-3：開始位置をこの中からランダムに決定してPlayer1を配置  
　GoalTarget：ターゲット（近くにマーカーを落としたら勝ち）  
　PlayFabController：PlayFabのコントローラー  
   
* プロジェクトの構成は以下の通りです。  
Assets/以下  
_Bad_Raccoon/_3D Realistic Terrain Free：3D 環境TerrainアセットのRealistic Terrain Free  
Air Balloon Pro：熱気球アセットAir Balloon Pro (4K + Physics)※有料アセットにつき外しています  
Audio：追加したオーディオ  
　asahi：メニューBGM（甘茶の音楽工房：朝日）  
　countdown：スタート時効果音（効果音ラボ：カウントダウン電子音）  
　flipCard：メニュータブ切り替え効果音（効果音ラボ：カードをめくる）  
　studium1：ゴール歓声効果音（効果音ラボ：スタジアムの歓声1）  
　submit32：メニュータッチ効果音（効果音ラボ：決定、ボタン押下32）  
　takibi：バーナー音（効果音ラボ：たき火）  
　umioiku：ゲームプレイBGM（甘茶の音楽工房：海を行く）  
　v_clipboard_*：ボイスナビ（メニュー開始後のクリップボード案内）  
　v_gameclear_*：ボイスナビ（ゲームクリア）  
　v_pause_*：ボイスナビ（ポーズ）  
　v_playguide2_*：ボイスナビ（プレイ開始時のガイド修正版）  
　v_playguide_*：ボイスナビ（プレイ開始時のガイド）  
　v_timeover_*：ボイスナビ（タイムオーバー）  
　v_welcome_*：ボイスナビ（メニュー開始時のあいさつ）  
Balloon：3D Air Balloonのアセット  
　Materials/Basket1-2：バスケット用自作マテリアル(仮)  
　Materials/Cloth1-2：球皮用自作マテリアル(仮)  
　Materials/Parts_Basket：バスケット用自作マテリアル  
　Materials/Sphere1-12：球皮用自作マテリアル  
　Models/Balloon_parts_basket：テクスチャ再構築用のUVを含むバスケットパーツ  
　Models/Balloon_parts_cloth：テクスチャ再構築用のUVを含む球皮パーツ  
　Prefabs/Balloon_parts_basket：テクスチャを再構築したバスケットパーツプレハブ  
　Prefabs/Balloon_parts_cloth：テクスチャを再構築した球皮パーツプレハブ  
Clipboard：Clipboardアセット  
Materials：追加したマテリアル  
Models：ポリゴン化したTerrain  
Oculus：Oculus Integration  
Photon：PUN 2 Free  
PlayFabSDK：PlayFabSDK  
Plugins/UniRx：UniRxプラグイン  
Prefabs：各プレハブ  
　3DAirBalloon：3D Air Balloonのプレハブ  
　BurnerPipe：3D Air Balloon用に作成したバーナーパーツ  
　BurnerTrigger：バーナーのレバープレハブ  
　CloudPrefab  
　enemyPosition：ミニマップに配置するためのライバルプレイヤー位置を表すプレハブ  
　enemyRoute：ミニマップに配置するためのライバルプレイヤーに移動経路を表すプレハブ  
　Frame：熱気球の周囲を囲んで衝突を防ぐためのコライダープレハブ  
　GoalTarget：ミニマップに配置するためのターゲットプレハブ  
　Grid：ミニマップに配置するための風向きを表示するプレハブ  
　markerGroundPoint：ミニマップに配置するためのマーカー投下地点を示すプレハブ  
　MarkerPrefab：プレイヤーが投下するためのマーカープレハブ。落とすと黄色い布部分が広がる  
　moveRoute：ミニマップに配置するためのプレイヤーに移動経路を表すプレハブ  
　myPosition：ミニマップに配置するためのプレイヤー位置を表すプレハブ  
　Stage1：プレハブ化した3D Realistic Terrain Free  
　Terrain：ミニマップ用のポリゴン化Terrain  
　Watch：腕時計  
Resources：  
　Avatar：Photonでインスタンス配置するためのプレイヤーオブジェクト  
Scenes：各シーン  
Scripts：追加したスクリプト  
Shader：追加したシェーダー  
　Mask：指定範囲を非表示にするシェーダー(マップで使用を検討したが不採用)  
　standard_cullOff：裏面を表示できるスタンダードシェーダー。3D Air Balloonに使用  
Skybox：Free HDR Sky  
Terrain：ポリゴン化したTerrain  
TextMesh Pro：TextMesh Proおよびフォントアセット  
　Fonts/azuki*あずきフォント  
Textures：追加した各テクスチャです  
　Balloon_parts_basket：3D Air Balloonのバスケットテクスチャ  
　Balloon_parts_cloth1-12：3D Air Balloonの球皮のテクスチャ  
　GameTitle  
　stickeyBg  
　checkMark  
　rope  
XR：XR-Plugin  

# シーン/メニュー構成
* MenuScene  
　メニュー  
　　レース：各プレイメニューへの遷移  
　　　シングルプレイ→SingleSceneへ  
　　　マルチプレイ→MultiSceneへ  
　　遊び方：ゲーム概要と簡単な操作方法を表示  
　　ランキング：PlayFabから取得した過去のベストレコードのランキングを15件まで表示  
　　ニュース：PlayFabから取得したニュースデータを表示  
　　クレジット：作者名・使用アセットなどを表示  
　　設定：  
　　　言語設定：日本語/English タッチでUIおよびボイスが切り替わる  
　　　利き手：左手/右手 タッチでボードをもつ手とふせんタブが切り替わる。プレイコントローラーは変更なし  
　　　マップ表示：広域/標準/詳細 3Dマップのサイズが切り替わる(プレイ中のみ表示)  
　　　ボイスナビ：オン/オフ ボイスナビのオンオフが切り替わる  
　ボイスナビ：手元のボードで操作できることを伝える（初回プレイで気づかない場合があるため）  
　ライバルの熱気球：ランダムに上昇・下降してシーン上を動きまわる  
* SingleScene  
　プレイ中：  
　　何もしない：緩やかに下降  
　　上部レバー：右手を近づけてトリガーで上昇  
　　左側ロープ：左手を近づけてトリガーで下降  
　　右側マーカー：右手を近づけてトリガーで持つ、離して投下、着地でゲームクリア  
　　腕時計：利き手と反対側に表示・タッチでポーズ画面になり手元にボードが移動  
　　ボード：利き手と反対側にゲームの説明を表示・タッチでポーズ画面になり手元にボードが移動  
　　ボイスナビ：プレイ方法を順に案内する  
　　3Dマップ：  
　　　プレイヤー：位置と経路と現在の風向き、風力・高度・ゴールまでの平面上の距離・残り時間を表示  
　　　ライバル：位置と経路を表示  
　　　ターゲット：ターゲットの位置を表示  
　　　投下地点：投下すると地点とターゲットとの距離を表示  
　　　風向き：それぞれの高さの風向きをマップ上に表示  
　ポーズ中：ゲームが一時停止される  
　　メニュー：  
　　　ゲームに戻る：ポーズを解除してプレイに戻る  
　　　プレイをやめる：ゲームをやめてメニューに戻る  
　　（他メニューはメニューシーンと同様）  
　クリア後：  
　　レース結果：投下した各プレイヤーのターゲットからの投下地点までの距離を表示  
　　ランキング：過去のベストレコードのランキングを表示(更新があれば反映)  
* MultiScene  
　プレイ中：シングルプレイと同様  
　ポーズ中：マルチプレイにつきゲームは進行する。メニューはシングルプレイと同様  
　クリア後：  
　　レース結果：同じルームにログイン中のクリア済みプレイヤーのレース結果を表示  
　　ランキング：過去のベストレコードのランキングを表示(更新があれば反映)  

# スクリプト構成
* 全てProjectのAssets/Scripts以下に配置。
* MenuScene用  
MenuController：メニューシーンのコントローラー。MenuScene/MenuControllerに適用  
MenuButtonController：メニューシーンのメニューボタンコントローラー。  
  MenuScene/3DAirBalloon1/OVRCameraRig/TrackingSpace/LeftHandAnchor/Clipboard/Canvas/以下のButton*およびStickyGroup/Tag1-6に適用  
MenuEnemyController：メニューシーンのライバルAI熱気球コントローラー。MenuScene/3DAirBalloon2-12に適用  
* SigleScene用  
SinglePlayerController：シングルプレイのプレイヤーコントローラー。SingleScene/Player1に適用  
SingleMenuButtonController：シングルプレイのメニューボタンコントローラー。  
SingleScene/clipboardL/Clipboard/Button*に適用  
(ポーズ中はSingleScene/Player1/OVRCameraRig/TrackingSpace/LeftまたはRightHandAnchor/Clipboard/以下のButton*およびStickyGroup/Tag1-6に適用)  
EnemyController：シングルプレイのライバルAIプレイヤーコントローラー。SingleScene/Player12-19に適用  
* MultiScene用  
MultiScene：マルチプレイのシーンコントローラー。MultiScene/MutiSceneに適用  
PlayerController：マルチプレイのプレイヤーコントローラー。ProjectのAssets/Resources/Avatarに適用。  
プレイ中はMultiScene/Avatar(Clone)としてインスタンス化される  
MultiMenuButtonController：マルチプレイのメニューボタンコントローラー。  
プレイ前はMultiScene/MutiScene/OVRCameraRig/TrackingSpace/LeftHandAnchor/Clipboard/Canvas/以下のButton*およびStickyGroup/Tag1-6  
プレイ中は上記MultiScene/Avatar(Clone)/Viewpoint/OVRCameraRig/TrackingSpace/LeftHandAnchor/Clipboard/Canvas/Button*およびStickyGroup/Tag1-6に適用  
* シーン共通  
BurnerController：バーナーのレバーをつかむ際の判定ボタンコントローラー。SingleScene/Player1/BurnerButtonに適用  
LineController：ロープ(リップライン)をつかむ際の判定ボタンコントローラー。SingleScene/Player1/LineButtonに適用  
MarkerButtonController：マーカーをつかむ際の判定ボタンコントローラー。SingleScene/Player1/MarkerButtonに適用  
MarkerController：マーカーのコントローラー。SingleScene/Player1/MarkerPrefabに適用  
PlayFabController：PlayFabコントローラー。各シーン直下のPlayFabControllerに適用。  
PlayFabAuthServices：PlayFabの認証サービス。ゲームオブジェクトへの適用はなく、クラス名指定で呼び出される。  
Utility：共通設定や共通メソッド。ゲームオブジェクトへの適用はなく、クラス名指定で呼び出される。  
ExportTerrain：Terrainデータのポリゴン出力スクリプト。開発用のため機能はコメントアウト状態。  
コメントアウトを外して有効にするとMenu>Terrain>Export To Obj...からTerrainのポリゴン書き出しが可能。  
# アプリ更新履歴
* Version 1.1 2022/3/17  
各方面から頂いたフィードバックを受けて不具合の修正および改修を加えたバージョンです。  
プロジェクトデータの公開に際して、Free Asset Editionとして、
有料アセットを無料アセットに差し替えて、PlayFabおよびPhotonのアプリIDを外しています。  
    * ハンドトリガーを押した時の不具合を修正（シーン内のボタンが順次選択されてしまう症状への暫定対応）
    * どちらのトリガーでも操作できるように修正（ユーザーがハンドトリガーを押す傾向があることを参考に見直し）
    * コースのTerrainをポリゴンに変換して処理を軽量化（ご指摘頂いたTerrainが重い問題の解決）
    * 3DバーチャルマップのテクスチャのX軸とZ軸が反転していた不具合を修正(環境Terrainのポリゴン化に伴う発見および解決)
    * マーカーが地面に隠れてしまう不具合を修正(マーカーのコライダー制御を見直して解決)
    * 気球が重なってしまう不具合の修正と初期位置の調整(コライダーを接触させないように見直して未解決だった問題を回避)
    * 制限時間を3分から5分に見直し（体験で制限時間内にクリアできない方が多かったので初回プレイ体験の向上のため見直し）
    * 順次理解して進められるようにプレイガイドボイスナビの文章再生間隔を見直し(実際に操作を確かめられる余裕を持たせる)
    * ガイド内の文章中にマーカーの色を明記(マーカーがどれかをより明確に伝える改善)
    * 「操作方法」の英語表現を一般的な表現に見直し（英訳に関して頂いたフィードバックを参考に見直し）
    * 調整に伴いガイドテキストおよびボイスナビを更新
    * ライバルの熱気球も上昇時にバーナーが有効になるように調整(リアリティの向上のため)
    * ライバルのバーナー音が3Dサウンドで聞こえるように調整(臨場感の改善のため)
    * シーン遷移時のフェード処理を追加（切り替え時に表示していると画面がゆらいで見えてしまう問題の解決のため）
    * 自分の気球がランダムで4種類から選択されるように調整(複数回プレイするユーザー体験向上のため)
    * AirBalloonProのアセットを3D Air Balloonに差し替え（有料アセットを二次配布できないため）
    * 熱気球のテクスチャおよびバーナーまわりのパーツを自作して実装(アセットのビジュアルがシンプルだったため)

* Version 1.0 2022/2/24  
2022/2/26の第10期Vアカオーディション・VRフェスに出展したバージョンです。  
フィードバックを元に大幅なてこ入れを行った結果、
作業スケジュールの関係で操作面の不具合が残ってしまったため、最新版で修正しています。  
    * 実際の競技同様に、落としたマーカーからターゲットまでの距離を競う形に変更（より競技として楽しめるように見直し）
    * クリップボード型メニューUIを実装（フィードバックを元に考えたアイデアで実装）
    * ポーズ用の腕時計UIを実装（プレイ状態からの切り替えアイデア）
    * 右利き・左利きの切り替えに対応（手持ちUIにしたことによる副次的対応）
    * 3Dバーチャルマップを実装（環境内の風向きが見えず手探りプレイになってしまう点の改善）
    * マップのサイズ設定を実装（もっと拡大して見たい、もしくは邪魔になるケースへの対応）
    * 日英ボイスナビを実装（UIを表示せずに案内するための施策）
    * ボイスナビのON/OFFを実装（プレイ方法が理解できたら不要になるため）
    * 日英対応および切り替え機能を実装（グローバル展開を意識した対応）
    * ゲームAIの移動経路や落下条件など強化（敵の動きを面白くするため、先読みして上下させるなど見直し）
    * マルチプレイを１ルームあたり4人上限のランダムマッチに変更
    * UIのタッチ反応バイブレーションを実装（感覚的に必要性を感じて実装）
    * ゴール後にもそのまま移動を続けるように修正（フィードバック、すぐ止まるとユーザー体験が良くない件の対応）
    * コースの配置や移動経路を見直し（ゲームとして戦略的に楽しめるように見直し）
    * テキスト全般をTextMeshProに変更し、あずきフォントのフォントアセットを作成して反映（UIの品質改善）
    * タイムアップ処理を実装（制限時間経過後の例外対応）
    * シーンを非同期で切り替えるように修正（頂いたフィードバック、フリーズして見える問題の対応）

* Version 0.3 2022/1/28  
2022/1/29のVアカプレゼン時に提出したバージョンです。  
この時点ではUIは空間上にありましたが、見づらかったりプレイ環境によっては衝突するとのフィードバックを参考に
その後のバージョンで手持ちUIにしています。  
操作方法の分かりやすさについても課題があったため、その後改善を行いました。  
    * 熱気球を加速度で動かすように修正（頂いたフィードバック、操作してすぐ動きに反映されていた件の対応）
    * バーナー付近でレバーを押したら上昇する機能を実装（ゲーム体験としてのリアリティを重視）
    * ロープを引いたら下降する機能を実装（ゲーム体験としてのリアリティを重視）
    * コントロール時のバイブレーションを実装（操作に対してのゲームからの反応を返す）
    * PlayFabオンラインニュースを実装（運営からの最新情報の提示を可能にする）
    * ポーズ機能を実装（プレイ中にやり直したいケース）

* Version 0.2 2022/1/7  
2022/1/8の進捗報告時にプレイ動画を提出した時のバージョンです。  
オンライン機能は検証も兼ねて先行実装しています。  
メニュー遷移を含めた一通りのゲームサイクルができた状態になっています。  
操作方法についてはリアリティを感じられた方が良いのではとのアドバイスを頂き、その後見直しています。  
    * ライバル熱気球の移動処理を実装
    * PlayFabオンラインランキング実装
    * Photonの通信対戦処理実装
    * メニューおよびシーン遷移を実装
    * シングルプレイモードを追加
    * リザルト表示を実装
    * プレイ方法実装
    * クレジット実装（使用アセットなど記載）
    * 風向き表示を実装
    * BGM・効果音を実装

* Version 0.1 2021/11/12  
初期プロトタイプバージョンです。  
ゲームとしてのビジュアル面や基本的な動きを確認していました。  
映像として見て頂いて反応が良かったこともあり、この方向で引き続き制作を進めました。  
    * プロジェクトを作成してOculus向けアプリとしての基本設定準備
    * 熱気球アセットを選定して実装
    * 環境アセットを選定して実装
    * ビジュアル面の方向性を確定
    * 一定方向に熱気球が移動する処理を実装
    * ボタン操作で高さを変化させて風向きによって熱気球が移動する処理を実装

* 企画決定 2021/10/30  
「現実ではまずやれないのVRでやってみようと思う」とのご意見や、
各ストアを調査しても競合製品がほぼ存在しなかったことから
最も独自性があってVRでしかできない体験として楽しめそうなVR熱気球レースゲームに決定しました。  
ゲームそのものの実装は比較的容易で、実装したいオンライン機能の実装に注力できそうであるとの見積もりもできており
必要なアセットやオンラインサービスについてもこの段階で見込みが立っていました。  
短時間でデモンストレーション可能なプレイ体験へのアレンジが課題でした。  

* 企画発案 2021/10/25 - 2021/10/29  
駄案含めて50案程度を出した中から5案程度練って相談させていただきました。  
それぞれ2行程度でどんなことをする企画でどんなメリットがあるかをざっと記載しました。  

# リソース
* Photon  
https://www.photonengine.com/ja-JP/PUN  
マルチプレイの実装でいくつかのサービスを比較検討の上、お勧め頂いたPhotonを使用しています。  
以下サイトを参考に独習してプロトタイプ時点で実装しました。  

* PUN 2 - FREE  
https://assetstore.unity.com/packages/tools/network/pun-2-free-119922


* PlayFab  
オンラインデータ同期を簡単に実装するためいくつかのBaaSを比較検討しました。  
参考情報や書籍などの情報のあったPlayFabを採用して実装しています。  
以下サイト・書籍・動画を参考に独習してプロトタイプ時点で実装しました。  
    * Microsoft PlayFab  
      https://developer.playfab.com/ja-JP/  
    * PlayFabゲーム開発テクニック  
      https://www.amazon.co.jp/dp/4798064076  
    * ランキング実装についてはこちらを参考にしました。  
      https://zenn.dev/studio_shimazu/articles/733a0eb3d95636  


* Air Balloon Pro (4K + Physics) ※このレポジトリでは差し替えていますので含まれていません  
https://assetstore.unity.com/packages/3d/vehicles/air/air-balloon-pro-4k-physics-144281  
有料の熱気球アセット。動きに合わせた球皮のゆらぎやバスケットのリアルさが魅力だが扱いが少し難しかったです  
* 3D Air Balloon  
https://assetstore.unity.com/packages/3d/vehicles/air/3d-air-balloon-192983  
無料の熱気球アセット。シンプルすぎてリアリティに欠けますが、シンプルなのでカスタマイズすればある程度使えます。  
* 3D Realistic Terrain Free  
https://assetstore.unity.com/packages/3d/environments/landscapes/3d-realistic-terrain-free-182593  
無料のTerrainアセット。つなげてシームレスに広げたり霧のパーティクルが実装されているのですぐ使えました。  
ポリゴン化して使うと少し軽くなります  

* Free HDR Sky  
https://assetstore.unity.com/packages/2d/textures-materials/sky/free-hdr-sky-61217  
日中と夕暮れの２つのリアルな全天球画像が入っており、このうちDaytimeを使っています。  
Sunsetも良いのですが雰囲気が合わなかったので断念しました

* Clipboard  
https://assetstore.unity.com/packages/3d/props/clipboard-137662  
手元UI用に使用した無料アセットです  

* UniRx  
https://assetstore.unity.com/packages/tools/integration/unirx-reactive-extensions-for-unity-17276  
習得時期の関係で新規実装箇所のボイスナビ再生タイミング制御用にのみ使用しています。

* Unity Probuilder  
https://xr-hub.com/archives/5463  
簡易的なモデリングツール。Unity単体でモデリングしたい方には良いかもしれません  
本作ではUnity標準で作れないプリミティブなモデル(風向きUI矢印の円錐)の作成に使用した程度です  
  
* Standard-2Sided.shader  
https://gist.github.com/unitycoder/469118783af9d2fd0e2b36becc7dd347  
StandardシェーダーではBackface Cullingが有効なためポリゴンの背面が非表示になるのですが、
3D Air Balloonで背面を表示させたかったため、こちらのシェーダーを追加して対応しました。  

* あずきフォント  
http://azukifont.com/font/azuki.html  
クリップボードデザインのメニューのため、
手書き感を出せそうなフリーフォントということで選定しました。  
azukiP / azukiB  

* 甘茶の音楽工房  
https://amachamusic.chagasi.com/  
熱気球ゲームということで雄大そうなフリーのBGMを選定しました。  
朝日→メニューBGM  
海を行く→ゲームプレイBGM  

* 効果音ラボ  
https://soundeffect-lab.info/  
プロトタイプで仮設定した効果音ですが大きな違和感はなかったです  
たき火→バーナー音  
カウントダウン電子音→開始音  
スタジアムの歓声1→クリア音  
カードをめくる→メニュー切り替え音  
決定、ボタン押下32→タッチ音  

* 日本語音声：VOICEVOX:ずんだもん  
https://voicevox.hiroshiba.jp/
* 英語音声：音読さん  
https://ondoku3.com/ja/  
English：Aria  
ボイスナビ用にフリーの合成音声サービスを使用しました。  
日本語についてはより親しみを持てそうなVOICEVOXを採用しましたが、
VOICEVOXは英語に未対応だったため音読さんを採用しました。  

* Adobe Photoshop CC  
タイトルロゴ画像の作成や各テクスチャの調整などに使用しました。

* Adobe After Effects CC / Adobe Media Encoder CC  
ボイスナビサウンドデータの編集に使用しました。  
https://www.adobe.com/jp/creativecloud.html  

* Blender 3.0  
https://www.blender.org/download/releases/3-0/  
※Free Asset Editionにて3D Air BalloonのモデルのUV展開に使用しました。

* UV Squares  
https://3dcg-school.pro/blender-uv-squares/  
※BlenderでのUVの整列にこちらのプラグインを使用しました。

* テクスチャ作成  
Substance Painter 2020 (Steam版)  
https://store.steampowered.com/app/1775390/Substance_3D_Painter_2022/  
※Free Asset Editionの熱気球のテクスチャ作成に使用しました。  

* Terrain-To-Object-Exporter  
https://github.com/burakdaskin/Terrain-To-Object-Exporter  
※Terrainのポリゴン化に使用。  
  Terrainをそのままでは縮小できなかったり重かったりするためポリゴン化しました。  
  有料のアセットも存在するようですが、こちらの無料のスクリプトを使用しました。  
  Script以下に配置し、使用する時以外はコメントアウトしています。  

* free asset tiled rope texture  
https://jp.3dexport.com/free-3dmodel-free-asset-tiled-rope-texture-126553.htm  
ロープのテクスチャ画像

* Google翻訳  
https://translate.google.co.jp/
* DeepL翻訳  
https://www.deepl.com/ja/translator  

# クレジット
(c)2022 yukkynote All Rights Reserved.
