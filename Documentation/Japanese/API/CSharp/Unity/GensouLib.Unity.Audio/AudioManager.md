# AudioManager

## 説明

オーディオマネージャーは、オーディオの再生を管理します。

## 静的プロパティ

|[MasterVolume](#audiomanagermastervolume)|マスター音量。|
|:---|:---|
|[BGMVolume](#audiomanagerbgmvolume)|バックグラウンドミュージックの音量。|
|[BGSVolume](#audiomanagerbgsvolume)|環境音の音量。|
|[SFXVolume](#audiomanagersfxvolume)|効果音の音量。|
|[VoiceVolume](#audiomanagervoicevolume)|ボイスの音量。|
|[BGMSource](#audiomanagerbgmsource)|バックグラウンドミュージックのソース。|
|[BGSSource](#audiomanagerbgssource)|環境音のソース。|
|[SFXSource](#audiomanagersfxsource)|効果音のソース。|
|[VoiceSource](#audiomanagervoicesource)|ボイスのソース。|
|[BGMClip](#audiomanagerbgmclip)|バックグラウンドミュージックのクリップ。|
|[BGSClip](#audiomanagerbgsclip)|環境音のクリップ。|
|[SFXClip](#audiomanagersfxclip)|効果音のクリップ。|
|[VoiceClip](#audiomanagervoiceclip)|ボイスのクリップ。|
|[IsMuted](#audiomanagerismuted)|ミュート状態かどうか。|

## 静的メソッド

|[PlayBGM](#audiomanagerplaybgm)|バックグラウンドミュージックを再生します。|
|:---|:---|
|[PlayBGS](#audiomanagerplaybgs)|環境音を再生します。|
|[PlayVoice](#audiomanagerplayvoice)|ボイスを再生します。|
|[PlaySFX](#audiomanagersplaysfx)|効果音を再生します。|
|[FadeOutAndPlayNewMusic](#audiomanagerfadeoutandplaynewmusic)|フェードアウトし、新しいバックグラウンドミュージックを再生します。|
|[FadeBGM](#audiomanagerfadebgm)|バックグラウンドミュージックをフェードアウトします。|
|[FadeBGS](#audiomanagerfadebgs)|環境音をフェードアウトします。|
|[FadeVoice](#audiomanagerfadevoice)|ボイスをフェードアウトします。|
|[StopBGM](#audiomanagerstopbgm)|バックグラウンドミュージックの再生を停止します。|
|[StopBGS](#audiomanagerstopbgs)|環境音の再生を停止します。|
|[StopVoice](#audiomanagerstopvoice)|ボイスの再生を停止します。|
|[SetMute](#audiomanagersetmute)|ミュート状態を設定します。|

---

# AudioManager.MasterVolume

`public static float MasterVolume`

## 説明

ゲーム全体の音量、範囲は0〜1。

このプロパティを変更すると、他の `AudioSource` コンポーネントの音量制限が変更されます。

---

# AudioManager.BGMVolume

`public static float BGMVolume`

## 説明

バックグラウンドミュージックの音量、マスター音量に影響を受け、範囲は0〜1。

このプロパティを変更すると、BGM の再生に使用される `AudioSource` コンポーネントの音量が変更されます。

---

# AudioManager.BGSVolume

`public static float BGSVolume`

## 説明

環境音の音量、マスター音量に影響を受け、範囲は0〜1。

このプロパティを変更すると、環境音の再生に使用される `AudioSource` コンポーネントの音量が変更されます。

---

# AudioManager.SFXVolume

`public static float SFXVolume`

## 説明

効果音の音量、マスター音量に影響を受け、範囲は0〜1。

このプロパティを変更すると、効果音の再生に使用される `AudioSource` コンポーネントの音量が変更されます。

---

# AudioManager.VoiceVolume

`public static float VoiceVolume`

## 説明

ボイスの音量、マスター音量に影響を受け、範囲は0〜1。

このプロパティを変更すると、ボイスの再生に使用される `AudioSource` コンポーネントの音量が変更されます。

---

# AudioManager.BGMSource

`public static AudioSource BGMSource`

## 説明

BGM の再生に使用される`AudioSource`コンポーネント。

---

# AudioManager.BGSSource

`public static AudioSource BGSSource`

## 説明

環境音の再生に使用される`AudioSource`コンポーネント。

---

# AudioManager.SFXSource

`public static AudioSource SFXSource`

## 説明

効果音の再生に使用される`AudioSource`コンポーネント。

---

# AudioManager.VoiceSource

`public static AudioSource VoiceSource`

## 説明

ボイスの再生に使用される`AudioSource`コンポーネント。

---

# AudioManager.BGMClip

`public static AudioClip BGMClip`

## 説明

BGM のオーディオクリップ。

対応する `AudioSource` コンポーネントによって再生されるオーディオ クリップ。

---

# AudioManager.BGSClip

`public static AudioClip BGSClip`

## 説明

環境音のオーディオクリップ。

対応する `AudioSource` コンポーネントによって再生されるオーディオ クリップ。

---

# AudioManager.SFXClip

`public static AudioClip SFXClip`

## 説明

効果音のオーディオクリップ。

対応する `AudioSource` コンポーネントによって再生されるオーディオ クリップ。

---

# AudioManager.VoiceClip

`public static AudioClip VoiceClip`

## 説明

ボイスのオーディオクリップ。

対応する `AudioSource` コンポーネントによって再生されるオーディオ クリップ。

---

# AudioManager.IsMuted

`public static bool IsMuted`

## 説明

読み取り専用プロパティ、ミュート状態かどうか。

---

# AudioManager.PlayBGM

`public static void PlayBGM(string name)`

## パラメーター

|`name`|オーディオリソースのパスまたはアドレス、[リソースのロード方法](../GensouLib.Unity.ResourceLoader/AssetLoader.md/#assetloaderloadresource)による。|
|:---|:---|

## 説明

BGM の再生に使用される `AudioSource` コンポーネントで指定されたオーディオを再生します。

---

# AudioManager.PlayBGS

`public static void PlayBGS(string name)`

## パラメーター

|`name`|オーディオリソースのパスまたはアドレス、[リソースのロード方法](../GensouLib.Unity.ResourceLoader/AssetLoader.md/#assetloaderloadresource)による。|
|:---|:---|

## 説明

環境音の再生に使用される `AudioSource` コンポーネントで指定されたオーディオを再生します。

---

# AudioManager.PlayVoice

`public static void PlayVoice(string name)`

## パラメーター

|`name`|オーディオリソースのパスまたはアドレス、[リソースのロード方法](../GensouLib.Unity.ResourceLoader/AssetLoader.md/#assetloaderloadresource)による。|
|:---|:---|

## 説明

ボイスの再生に使用される `AudioSource` コンポーネントで指定されたオーディオを再生します。

---

# AudioManager.PlaySFX

`public static void PlaySFX(string name)`

## パラメーター

|`name`|オーディオリソースのパスまたはアドレス、[リソースのロード方法](../GensouLib.Unity.ResourceLoader/AssetLoader.md/#assetloaderloadresource)による。|
|:---|:---|

## 説明

効果音の再生に使用される `AudioSource` コンポーネントで指定されたオーディオを再生します。

---

# AudioManager.FadeOutAndPlayNewMusic

`public static IEnumerator FadeOutAndPlayNewMusic(string newMusicName, float duration)`

## パラメーター

|`newMusicName`|新しい音楽のリソースパスまたはアドレス、[リソースのロード方法](../GensouLib.Unity.ResourceLoader/AssetLoader.md/#assetloaderloadresource)による。|
|:---|:---|
|`duration`|フェードアウトの時間（秒単位）。|

## 説明

BGM の再生に使用される `AducioSource` コンポーネントの現在再生中の BGM を指定した時間にフェードアウトし、指定した新しい BGM を再生します。

`MonoBehaviour.StartCoroutine(IEnumerator)`メソッドを使って呼び出してください。

## 戻り値

コルーチンイテレータ。

---

# AudioManager.FadeBGM

`public static IEnumerator FadeBGM(float targetVolume, float duration)`

## パラメーター

|`targetVolume`|目標音量。|
|:---|:---|
|`duration`|フェードイン/フェードアウトの時間（秒単位）。|

## 説明

BGM の再生に使用される `AudioSource` コンポーネントの音量を、指定した時間に従って目標の音量までフェードさせます。

`MonoBehaviour.StartCoroutine(IEnumerator)`メソッドを使って呼び出してください。

## 戻り値

コルーチンイテレータ。

---

# AudioManager.FadeBGS

`public static IEnumerator FadeBGS(float targetVolume, float duration)`

## パラメーター

|`targetVolume`|目標音量。|
|:---|:---|
|`duration`|フェードイン/フェードアウトの時間（秒単位）。|

## 説明

環境音の再生に使用される `AudioSource` コンポーネントの音量を、指定した時間に従って目標の音量までフェードさせます。

`MonoBehaviour.StartCoroutine(IEnumerator)`メソッドを使って呼び出してください。

## 戻り値

コルーチンイテレータ。

---

# AudioManager.FadeVoice

`public static IEnumerator FadeVoice(float targetVolume, float duration)`

## パラメーター

|`targetVolume`|目標音量。|
|:---|:---|
|`duration`|フェードイン/フェードアウトの時間（秒単位）。|

## 説明

ボイスの再生に使用される `AudioSource` コンポーネントの音量を、指定した時間に従って目標の音量までフェードさせます。

`MonoBehaviour.StartCoroutine(IEnumerator)`メソッドを使って呼び出してください。

## 戻り値

コルーチンイテレータ。

---

# AudioManager.StopBGM

`public static void StopBGM()`

## 説明

BGM の再生に使用される `AudioSource` コンポーネントによって再生されているオーディオの再生を停止します。

---

# AudioManager.StopBGS

`public static void StopBGS()`

## 説明

環境音の再生に使用される `AudioSource` コンポーネントによって再生されているオーディオの再生を停止します。

---

# AudioManager.StopVoice

`public static void StopVoice()`

## 説明

ボイスの再生に使用される `AudioSource` コンポーネントによって再生されているオーディオの再生を停止します。

---

# AudioManager.SetMute

`public static void SetMute(bool isMuted)`

## パラメーター

|`isMuted`|ミュートするかどうか。`true` はミュート、`false` はミュート解除を意味します。|
|:---|:---|

## 説明

ミュートの設定を行います。

