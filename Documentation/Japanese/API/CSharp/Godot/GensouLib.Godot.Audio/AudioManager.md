# AudioManager

継承：[Node](https://docs.godotengine.org/ja/stable/classes/class_node.html)

## 説明

シングルトンのオーディオマネージャーで、オーディオの再生を制御します。

このクラスのインスタンスは`AudioManager.Instance`を使用してアクセスしてください。

このクラスのスクリプトは、オートロードに追加されたシーンにアタッチする必要があります。

提供されたAudioManager.tscnシーンを使用するか、

`Node`型のルートノードと4つの`AudioStreamPlayer`型の子ノードを持つシーンを作成してください。

オーディオを再生する際には、`AudioStream`を継承する`AudioStreamPlaylist`のようなリソースではなく、オーディオファイルそのものを使用してください。

## 静的プロパティ

|[Instance](#audiomanagerinstance)|AudioManagerのインスタンス|
|:---|:---|

## 信号

|[BGMFadeCompletedAndChanged](#audiomanagerbgmfadecompletedandchanged)|フェードアウトしてBGMの切り替えが完了したときに発せされます|
|:---|:---|
|[FadeCompleted](#audiomanagerfadecompleted)|プレーヤーのフェードインとフェードアウトが完了したときに発せされます。|

## プロパティ

|[ResPath](#audiomanagerrespath)|オーディオファイルが配置されているパス|
|:---|:---|
|[BGMNodePath](#audiomanagerbgmnodepath)|BGMプレーヤーノードのパス|
|[BGSNodePath](#audiomanagerbgsnodepath)|環境音プレーヤーノードのパス|
|[SFXNodePath](#audiomanagersfxnodepath)|効果音プレーヤーノードのパス|
|[VoiceNodePath](#audiomanagervoicenodepath)|ボイスプレーヤーノードのパス|
|[MasterVolume](#audiomanagermastervolume)|マスターボリューム|
|[BGMVolume](#audiomanagerbgmvolume)|BGMボリューム|
|[BGSVolume](#audiomanagerbgsvolume)|環境音ボリューム|
|[SFXVolume](#audiomanagersfxvolume)|効果音ボリューム|
|[VoiceVolume](#audiomanagervoicevolume)|ボイスボリューム|
|[IsMuted](#audiomanagerismuted)|ミュート状態かどうか|

## メソッド

|[PlayBGM](#audiomanagerplaybgm)|BGMを再生します|
|:---|:---|
|[PlayBGS](#audiomanagerplaybgs)|環境音を再生します|
|[PlayVoice](#audiomanagerplayvoice)|ボイスを再生します|
|[PlaySFX](#audiomanagerplaysfx)|効果音を再生します|
|[FadeOutAndPlayNewMusic](#audiomanagerfadeoutandplaynewmusic)|フェードアウトして新しいBGMを再生します|
|[FadeBGM](#audiomanagerfadebgm)|BGMをフェードインまたはフェードアウトします。|
|[FadeBGS](#audiomanagerfadebgs)|環境音をフェードインまたはフェードアウトします。|
|[FadeVoice](#audiomanagerfadevoice)|ボイスをフェードインまたはフェードアウトします。|
|[StopBGM](#audiomanagerstopbgm)|BGMを停止します|
|[StopBGS](#audiomanagerstopbgs)|環境音を停止します|
|[StopVoice](#audiomanagerstopvoice)|ボイスを停止します|
|[SetMute](#audiomanagersetmute)|ミュートを設定します|

---

# AudioManager.Instance

`public static AudioManager Instance`

## 説明

`AudioManager`のインスタンス。このプロパティを通じてオーディオの再生を制御します。

---

# AudioManager.BGMFadeCompletedAndChanged

`public delegate void BGMFadeCompletedAndChangedEventHandler()`

## 説明

フェードアウトしてBGMの切り替えが完了したときに発せされます。

この信号は、メソッド [FadeOutAndPlayNewMusic](#audiomanagerfadeoutandplaynewmusic) がフェードアウトを完了し、BGM を切り替えた後に発せされます。

---

# AudioManager.FadeCompleted

`public delegate void FadeCompletedEventHandler(string player)`

## 説明

プレーヤーのフェードインとフェードアウトが完了したときに発せされます。

メソッド [FadeBGM](#audiomanagerfadebgm)、[FadeBGS](#audiomanagerfadebgs)、[FadeVoice](#audiomanagerfadevoice) でフェードインとフェードアウトが完了すると、この信号が発せられます

`player` パラメータはプレーヤーの名前です。以下のいずれかです。

- `BGM`

- `BGS`

- `Voice`

---

# AudioManager.ResPath

`public string ResPath`

## 説明

オーディオファイルが配置されているパス。デフォルト値は`res://Audio/`です。このパス以下にあるオーディオファイルをロードします。

---

# AudioManager.BGMNodePath

`public NodePath BGMNodePath`

## 説明

BGMを再生するための`AudioStreamPlayer`ノードのパス。

デフォルトではルートノード相対の`BGMPlayer`です。

---

# AudioManager.BGSNodePath

`public NodePath BGSNodePath`

## 説明

環境音を再生するための`AudioStreamPlayer`ノードのパス。

デフォルトではルートノード相対の`BGSPlayer`です。

---

# AudioManager.SFXNodePath

`public NodePath SFXNodePath`

## 説明

効果音を再生するための`AudioStreamPlayer`ノードのパス。

デフォルトではルートノード相対の`SFXPlayer`です。

---

# AudioManager.VoiceNodePath

`public NodePath VoiceNodePath`

## 説明

ボイスを再生するための`AudioStreamPlayer`ノードのパス。

デフォルトではルートノード相対の`VoicePlayer`です。

---

# AudioManager.MasterVolume

`public float MasterVolume`

## 説明

マスターボリューム。デフォルトのMasterオーディオバスの音量に依存します。

このプロパティを変更するとオーディオバスの音量が変わります。

---

# AudioManager.BGMVolume

`public float BGMVolume`

## 説明

BGMのボリューム。

このプロパティを変更するとBGM用`AudioStreamPlayer`ノードの音量が変わります。

---

# AudioManager.BGSVolume

`public float BGSVolume`

## 説明

環境音のボリューム。

このプロパティを変更すると環境音用`AudioStreamPlayer`ノードの音量が変わります。

---

# AudioManager.SFXVolume

`public float SFXVolume`

## 説明

効果音のボリューム。

このプロパティを変更すると効果音用`AudioStreamPlayer`ノードの音量が変わります。

---

# AudioManager.VoiceVolume

`public float VoiceVolume`

## 説明

ボイスのボリューム。

このプロパティを変更するとボイス用`AudioStreamPlayer`ノードの音量が変わります。

---

# AudioManager.IsMuted

`public bool IsMuted`

## 説明

読み取り専用プロパティで、現在ミュート状態かどうかを示します。

---

# AudioManager.PlayBGM

`public void PlayBGM(string name)`

## パラメーター

|`name`|オーディオファイル名。拡張子を含み、`ogg`、`wav`、`mp3`形式をサポートします。|
|:---|:---|

## 説明

BGM用`AudioStreamPlayer`ノードで指定したオーディオを再生します。

ループ再生したい場合は、ファイルのインポート設定でループを有効にしてください。

---

# AudioManager.PlayBGS

`public void PlayBGS(string name)`

## パラメーター

|`name`|オーディオファイル名。拡張子を含み、`ogg`、`wav`、`mp3`形式をサポートします。|
|:---|:---|

## 説明

環境音用`AudioStreamPlayer`ノードで指定したオーディオを再生します。

ループ再生したい場合は、ファイルのインポート設定でループを有効にしてください。

---

# AudioManager.PlayVoice

`public void PlayVoice(string name)`

## パラメーター

|`name`|オーディオファイル名。拡張子を含み、`ogg`、`wav`、`mp3`形式をサポートします。|
|:---|:---|

## 説明

ボイス用`AudioStreamPlayer`ノードで指定したオーディオを再生します。

ループ再生したい場合は、ファイルのインポート設定でループを有効にしてください。

---

# AudioManager.PlaySFX

`public void PlaySFX(string name)`

## パラメーター

|`name`|オーディオファイル名。拡張子を含み、`ogg`、`wav`、`mp3`形式をサポートします。|
|:---|:---|

## 説明

効果音用`AudioStreamPlayer`ノードで指定したオーディオを再生します。

ループ再生したい場合は、ファイルのインポート設定でループを有効にしてください。

---

# AudioManager.FadeOutAndPlayNewMusic

`public async Task<bool> FadeOutAndPlayNewMusic(string newMusicName, float duration)`

## パラメーター

|`newMusicName`|新しい音楽ファイル名（拡張子を含む）。対応フォーマットは`ogg`、`wav`、`mp3`です。|
|:---|:---|
|`duration`|フェードアウト時間（秒単位）。|

## 説明

指定された時間で、背景音楽を再生する`AudioStreamPlayer`ノードの現在再生中の音楽をフェードアウトし、新しい音楽を再生します。

## 戻り値

非同期タスク。成功した場合は`true`を返し、失敗した場合は`false`を返します。

---

# AudioManager.FadeBGM

`public async Task<bool> FadeBGM(float targetVolume, float duration)`

## パラメーター

|`targetVolume`|目標音量 (負ではない数値)。|
|:---|:---|
|`duration`|フェードインまたはフェードアウト時間 (秒単位)。|

## 説明

指定された時間で、背景音楽を再生する`AudioStreamPlayer`ノードの音量を目標音量までフェードインまたはフェードアウトします。

## 戻り値

非同期タスクは、フェードアウトまたはフェードアウト完了後に`true`に戻り、目標音量がマイナスの場合に`false`に戻ります。

---

# AudioManager.FadeBGS

`public async Task<bool> FadeBGS(float targetVolume, float duration)`

## パラメーター

|`targetVolume`|目標音量 (負ではない数値)。|
|:---|:---|
|`duration`|フェードインまたはフェードアウト時間 (秒単位)。|

## 説明

指定された時間で、環境音楽を再生する`AudioStreamPlayer`ノードの音量を目標音量までフェードインまたはフェードアウトします。

## 戻り値

非同期タスクは、フェードアウトまたはフェードアウト完了後に`true`に戻り、目標音量がマイナスの場合に`false`に戻ります。

---

# AudioManager.FadeVoice

`public async Task<bool> FadeVoice(float targetVolume, float duration)`

## パラメーター

|`targetVolume`|目標音量 (負ではない数値)。|
|:---|:---|
|`duration`|フェードアウト時間（秒単位）。|

## 説明

指定された時間で、ボイスを再生する`AudioStreamPlayer`ノードの音量を目標音量までフェードインまたはフェードアウトします。

## 戻り値

非同期タスクは、フェードアウトまたはフェードアウト完了後に`true`に戻り、目標音量がマイナスの場合に`false`に戻ります。

---

# AudioManager.StopBGM

`public void StopBGM()`

## 説明

背景音楽を再生する`AudioStreamPlayer`ノードで再生中の音楽を停止します。

---

# AudioManager.StopBGS

`public void StopBGS()`

## 説明

環境音楽を再生する`AudioStreamPlayer`ノードで再生中の音楽を停止します。

---

# AudioManager.StopVoice

`public void StopVoice()`

## 説明

ボイスを再生する`AudioStreamPlayer`ノードで再生中の音声を停止します。

---

# AudioManager.SetMute

`public void SetMute(bool mute)`

## パラメーター

|`mute`|ミュートの設定。`true`でミュート、`false`でミュート解除。|
|:---|:---|

## 説明

ミュートを設定します。

