# AudioManager

継承：[Node](https://docs.godotengine.org/ja/stable/classes/class_node.html)

## 説明

オーディオの再生をコントロールするオーディオマネージャです。

GDScriptの特性上、このクラスは匿名クラスであるため、`AudioManager`クラス名を直接使用するのではなく、Autoloadで定義したノード名を使用してクラスにアクセスしてください。

このクラスのスクリプトは、オートロードに追加されたシーンにアタッチする必要があります。

提供されたAudioManager.tscnシーンを使用するか、

`Node`型のルートノードと4つの`AudioStreamPlayer`型の子ノードを持つシーンを作成してください。

オーディオを再生する際には、`AudioStream`を継承する`AudioStreamPlaylist`のようなリソースではなく、オーディオファイルそのものを使用してください。

## 信号

|[bgm_fade_completed_and_changed](#audiomanagerbgm_fade_completed_and_changed)|フェードアウトしてBGMの切り替えが完了したときに発せされます|
|:---|:---|
|[fade_completed](#audiomanagerfade_completed)|プレーヤーのフェードインとフェードアウトが完了したときに発せされます。|

## プロパティ

|[res_path](#audiomanagerres_path)|オーディオファイルが配置されているパス|
|:---|:---|
|[bgm_node_path](#audiomanagerbgm_node_path)|BGMプレーヤーノードのパス|
|[bgs_node_path](#audiomanagerbgs_node_path)|環境音プレーヤーノードのパス|
|[sfx_node_path](#audiomanagersfx_node_path)|効果音プレーヤーノードのパス|
|[voice_node_path](#audiomanagervoice_node_path)|ボイスプレーヤーノードのパス.|
|[master_volume](#audiomanagermaster_volume)|マスターボリューム|
|[bgm_volume](#audiomanagerbgm_volume)|BGMボリューム|
|[bgs_volume](#audiomanagerbgs_volume)|環境音ボリューム|
|[sfx_volume](#audiomanagersfx_volume)|効果音ボリューム|
|[voice_volume](#audiomanagervoice_volume)|ボイスボリューム|
|[is_muted](#audiomanageris_muted)|ミュート状態かどうか|

## メソッド

|[play_bgm](#audiomanagerplay_bgm)|BGMを再生します|
|:---|:---|
|[play_bgs](#audiomanagerplay_bgs)|環境音を再生します|
|[play_voice](#audiomanagerplay_voice)|ボイスを再生します|
|[play_sfx](#audiomanagerplay_sfx)|効果音を再生します|
|[fade_out_and_play_new_music](#audiomanagerfade_out_and_play_new_music)|フェードアウトして新しいBGMを再生します|
|[fade_bgm](#audiomanagerfade_bgm)|BGMをフェードインまたはフェードアウトします。|
|[fade_bgs](#audiomanagerfade_bgs)|環境音をフェードインまたはフェードアウトします。|
|[fade_voice](#audiomanagerfade_voice)|ボイスをフェードインまたはフェードアウトします。|
|[stop_bgm](#audiomanagerstop_bgm)|BGMを停止します|
|[stop_bgs](#audiomanagerstop_bgs)|環境音を停止します|
|[stop_voice](#audiomanagerstop_voice)|ボイスを停止します|
|[set_mute](#audiomanagerset_mute)|ミュートを設定します|

---

# AudioManager.bgm_fade_completed_and_changed

`signal bgm_fade_completed_and_changed()`

## 説明

フェードアウトしてBGMの切り替えが完了したときに発せされます。

この信号は、メソッド [fade_out_and_play_new_music](#audiomanagerfade_out_and_play_new_music) がフェードアウトを完了し、BGM を切り替えた後に発せされます。

---

# AudioManager.fade_completed

`signal fade_completed(player: String)`

## 説明

プレーヤーのフェードインとフェードアウトが完了したときに発せされます。

メソッド [fade_bgm](#audiomanagerfade_bgm)、[fade_bgs](#audiomanagerfade_bgs)、[fade_voice](#audiomanagerfade_voice)でフェードインとフェードアウトが完了すると、この信号が発せられます

`player`パラメータはプレーヤーの名前です。以下のいずれかです。

- `bgm` 

- `bgs`

- `voice`

---

# AudioManager.res_path

`var res_path: String`

## 説明

オーディオファイルが配置されているパス。デフォルト値は`res://Audio/`です。このパス以下にあるオーディオファイルをロードします。

---

# AudioManager.bgm_node_path

`var bgm_node_path: NodePath`

## 説明

BGMを再生するための`AudioStreamPlayer`ノードのパス。

デフォルトではルートノード相対の`BGMPlayer`です。

---

# AudioManager.bgs_node_path

`var bgs_node_path: NodePath`

## 説明

環境音を再生するための`AudioStreamPlayer`ノードのパス。

デフォルトではルートノード相対の`BGSPlayer`です。

---

# AudioManager.sfx_node_path

`var sfx_node_path: NodePath`

## 説明

効果音を再生するための`AudioStreamPlayer`ノードのパス。

デフォルトではルートノード相対の`SFXPlayer`です。

---

# AudioManager.voice_node_path

`var voice_node_path: NodePath`

## 説明

ボイスを再生するための`AudioStreamPlayer`ノードのパス。

デフォルトではルートノード相対の`VoicePlayer`です。

---

# AudioManager.master_volume

`var master_volume: float`

## 説明

マスターボリューム。デフォルトのMasterオーディオバスの音量に依存します。

このプロパティを変更するとオーディオバスの音量が変わります。

---

# AudioManager.bgm_volume

`var bgm_volume: float`

## 説明

BGMのボリューム。

このプロパティを変更するとBGM用`AudioStreamPlayer`ノードの音量が変わります。

---

# AudioManager.bgs_volume

`var bgs_volume: float`

## 説明

環境音のボリューム。

このプロパティを変更すると環境音用`AudioStreamPlayer`ノードの音量が変わります。

---

# AudioManager.sfx_volume

`var sfx_volume: float`

## 説明

効果音のボリューム。

このプロパティを変更すると効果音用`AudioStreamPlayer`ノードの音量が変わります。

---

# AudioManager.voice_volume

`var voice_volume: float`

## 説明

ボイスのボリューム。

このプロパティを変更するとボイス用`AudioStreamPlayer`ノードの音量が変わります。

---

# AudioManager.is_muted

`var is_muted: bool`

## 説明

読み取り専用プロパティで、現在ミュート状態かどうかを示します。

---

# AudioManager.play_bgm

`func play_bgm(file_name: String) -> void`

## パラメーター

|`file_name`|オーディオファイル名。拡張子を含み、`ogg`、`wav`、`mp3`形式をサポートします。|
|:---|:---|

## 描述

BGM用`AudioStreamPlayer`ノードで指定したオーディオを再生します。

ループ再生したい場合は、ファイルのインポート設定でループを有効にしてください。

---

# AudioManager.play_bgs

`func play_bgs(file_name: String) -> void`

## パラメーター

|`file_name`|オーディオファイル名。拡張子を含み、`ogg`、`wav`、`mp3`形式をサポートします。|
|:---|:---|

## 説明

環境音用`AudioStreamPlayer`ノードで指定したオーディオを再生します。

ループ再生したい場合は、ファイルのインポート設定でループを有効にしてください。

---

# AudioManager.play_voice

`func play_voice(file_name: String) -> void`

## パラメーター

|`file_name`|オーディオファイル名。拡張子を含み、`ogg`、`wav`、`mp3`形式をサポートします。|
|:---|:---|

## 説明

ボイス用`AudioStreamPlayer`ノードで指定したオーディオを再生します。

ループ再生したい場合は、ファイルのインポート設定でループを有効にしてください。

---

# AudioManager.play_sfx

`func play_sfx(file_name: String) -> void`

## パラメーター

|`file_name`|オーディオファイル名。拡張子を含み、`ogg`、`wav`、`mp3`形式をサポートします。|
|:---|:---|

## 説明

効果音用`AudioStreamPlayer`ノードで指定したオーディオを再生します。

ループ再生したい場合は、ファイルのインポート設定でループを有効にしてください。

---

# AudioManager.fade_out_and_play_new_music

`func fade_out_and_play_new_music(new_music_name: String, duration: float) -> bool`

## パラメーター

|`new_music_name`|新しい音楽ファイル名（拡張子を含む）。対応フォーマットは`ogg`、`wav`、`mp3`です。|
|:---|:---|
|`duration`|フェードアウト時間（秒単位）。|

## 説明

待機可能。

指定された時間で、背景音楽を再生する`AudioStreamPlayer`ノードの現在再生中の音楽をフェードアウトし、新しい音楽を再生します。

## 戻り値

成功した場合は`true`を返し、失敗した場合は`false`を返します。

---

# AudioManager.fade_bgm

`func fade_bgm(target_volume: float, duration: float) -> bool`

## パラメーター

|`target_volume`|目標音量 (負ではない数値)。|
|:---|:---|
|`duration`|フェードインまたはフェードアウト時間 (秒単位)。|

## 説明

待機可能。

指定された時間で、背景音楽を再生する`AudioStreamPlayer`ノードの音量を目標音量までフェードインまたはフェードアウトします。

## 戻り値

フェードアウトまたはフェードアウト完了後に`true`に戻り、目標音量がマイナスの場合に`false`に戻ります。

---

# AudioManager.fade_bgs

`func fade_bgs(target_volume: float, duration: float) -> bool`

## パラメーター

|`target_volume`|目標音量 (負ではない数値)。|
|:---|:---|
|`duration`|フェードインまたはフェードアウト時間 (秒単位)。|

## 説明

待機可能。

指定された時間で、環境音楽を再生する`AudioStreamPlayer`ノードの音量を目標音量までフェードインまたはフェードアウトします。

## 戻り値

フェードアウトまたはフェードアウト完了後に`true`に戻り、目標音量がマイナスの場合に`false`に戻ります。

---

# AudioManager.fade_voice

`func fade_voice(target_volume: float, duration: float) -> bool`

## パラメーター

|`target_volume`|目標音量 (負ではない数値)。|
|:---|:---|
|`duration`|フェードアウト時間（秒単位）。|

## 説明

待機可能。

指定された時間で、ボイスを再生する`AudioStreamPlayer`ノードの音量を目標音量までフェードインまたはフェードアウトします。

## 戻り値

フェードアウトまたはフェードアウト完了後に`true`に戻り、目標音量がマイナスの場合に`false`に戻ります。

---

# AudioManager.stop_bgm

`func stop_bgm() -> void`

## 説明

背景音楽を再生する`AudioStreamPlayer`ノードで再生中の音楽を停止します。

---

# AudioManager.stop_bgs

`func stop_bgs() -> void`

## 説明

環境音楽を再生する`AudioStreamPlayer`ノードで再生中の音楽を停止します。

---

# AudioManager.stop_voice

`func stop_voice() -> void`

## 説明

ボイスを再生する`AudioStreamPlayer`ノードで再生中の音声を停止します。

---

# AudioManager.set_mute

`func set_mute(mute: bool) -> void`

## パラメーター

|`mute`|ミュートの設定。`true`でミュート、`false`でミュート解除。|
|:---|:---|

## 説明

ミュートを設定します。