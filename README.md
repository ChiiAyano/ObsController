# ObsController
OBS Studio に、Web Socket を介していろいろするツール群

## できること
### ObsClock
時刻文字列を特定のテキスト ソースへ送信するツール

### ObsWeather
OpenWeatherMap から得た天気情報を特定のテキスト ソースへ送信するツール

### ObsUserLock
PC のユーザーセッション状態 (ロック・アンロック) を監視して、ロック時に特定のシーンへ切り替えるツール

## 必要なもの
### 共通
* [OBS Studio](https://obsproject.com/)
* [obs-websocket](https://github.com/Palakis/obs-websocket)
* .NET Core 3.1

### ObsWeather
* [Weather Icons](https://erikflowers.github.io/weather-icons/)

### ObsUserLock
* Windows 10 以降

## 使い方
### 共通
1. 最初の起動時 `setting.json` が生成されます。一度アプリを閉じ、別途設定ファイルを編集してください
1. OBS Studio を起動し、使いたいツールで必要なソース・シーンを追加しておきます
1. 準備ができたらアプリを起動してください
1. 2 回目以降は OBS Studio を先に起動してからアプリを起動してください

## OBSClock
1. OBS Studio で `テキスト (GDI+)` を追加して適切な場所へ配置し、名前を設定ファイルにあわせてください

## ObsWeather
1. OBS Studio で `テキスト (GDI+)` を追加して適切な場所へ配置し、名前を設定ファイルにあわせてください
1. 天気を表示するテキスト ソースは、Weather Icons のフォントを指定してください

## ObsUserLock
1. ロックするときに表示するシーンを追加し、画面を構成してください
1. 設定ファイルに指定したシーン名にしてください

## 設定ファイルの設定項目
### ObsClock
| 項目名 | 型 | 説明 | 既定値 |
|--------|---|------|-------|
| `url` | `string` | OBS Studio に接続するアドレスを指定します | ws://127.0.0.1:4444/ |
| `password` | `string?` | 接続パスワードを設定します | 設定無し |
| `sourceName` | `string` | 書き換えるテキスト ソースの名前を指定します | Clock |
| `hoursFormat` | `string` | "時" 表示の仕方を設定します。[設定は C# で指定できるものに依存します](https://docs.microsoft.com/ja-jp/dotnet/standard/base-types/custom-date-and-time-format-strings#hSpecifier) | h |
| `noon` | `int` | 正午 (24 時, 12 時) のときの表記方法を設定します。0 はゼロ時として、1 は 12 時または 0 時として表記します | 0 |
| `fillCharacter` | `string` | "hoursFormat" で `hh` または `HH` にしていない場合の埋め文字を指定します | 空文字 |
| `alwaysShowClockTimes` | `TimeRange[]` | 常に表示する時間帯を設定します | 午前 6 時 ～ 午前 10 時 |
| `showDuration` | `int` | 時報表示をする時間を指定します。常に表示する時間帯では無視されます | 5 (秒) |
| `showInterval` | `int` | 時報表示を表示する間隔 (正時 0 分から換算) を指定します。常に表示する時間帯では無視されます | 30 (分) |

#### TimeRange
| 項目名 | 型 | 説明 |
|--------|---|------|
| `start` | `time (HH:mm:ss)` | 常に表示する時間帯の開始時刻 |
| `end` | `time (HH:mm:ss)` | 常に表示する時間帯の終了時刻 |

----

### ObsWeather
| 項目名 | 型 | 説明 | 既定値 |
|--------|---|------|-------|
| `url` | `string` | OBS Studio に接続するアドレスを指定します | ws://127.0.0.1:4444/ |
| `password` | `string?` | 接続パスワードを設定します | 設定無し |
| `weatherIconSourceName` | `string` | 天気情報アイコンを表示するテキスト ソースの名前を指定します | WeatherIcon |
| `temperatureTextSourceName` | `string` | 温度を表示するテキスト ソースの名前を指定します | TemperatureText |
| `openWeatherMap` | `OpenWeatherMapSetting` | Open Weather Map の情報を定義します | 下記既定値の通り |

#### OpenWeatherMapSetting
| 項目名 | 型 | 説明 | 既定値 |
|--------|---|------|-------|
| `apiKey` | `string` | Open Weather Map API のアクセスキー | 空文字 |
| `latitude` | `double` | 天気を取得する位置の緯度を指定します | 35.685595 おおよそ皇居 |
| `longitude` | `double` | 天気を取得する位置の経度を指定します | 139.753026 おおよそ皇居 |

----
### ObsUserLock
| 項目名 | 型 | 説明 | 既定値 |
|--------|---|------|-------|
| `url` | `string` | OBS Studio に接続するアドレスを指定します | ws://127.0.0.1:4444/ |
| `password` | `string?` | 接続パスワードを設定します | 設定無し |
| `session_locked_scene` | `string` | セッションがロック状態になったときに遷移するシーン名を指定します | Locked |
| `resume_before_scene_unlocked` | `bool` | セッションがアンロック状態になったときに、ロック状態の前に表示していたシーンへ戻すかどうかを指定します | true |
