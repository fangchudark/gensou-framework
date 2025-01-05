## 线程取消标记
class_name CancellationToken extends Thread

var _is_canceled: bool

## 标记
var token: CancellationToken

func _init():
	token = self
	_is_canceled = false

## 将标记设置为取消
func cancel() -> void:
	_is_canceled = true

## 判断是否已取消
func is_cancelled() -> bool:
	return _is_canceled
