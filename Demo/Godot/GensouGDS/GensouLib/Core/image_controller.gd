## 图片控制器
class_name ImageController extends VisualNoveCore

## 立绘位置
enum FigurePosition
{
	## 左侧
	Left,
	## 中间
	Center,
	## 右侧
	Right
}

## 切换立绘 [br]
## [br]
## [param image] : [br]
## 图片 [br]
## [br]
## [param alpha] : [br]
## 透明度 [br]
## [br]
## [param position] : [br]
## 位置 [br]
## [br]
## [param hide] : [br]
## 是否隐藏 [br]
static func change_figure(image: Texture, alpha: float = 1.0, position: FigurePosition = FigurePosition.Center, hide: bool = false) -> void:
	if figure_left == null or figure_center == null or figure_right == null: # 检查实例是否存在
		push_error("VisualNoveCore: Missing instances")
		return

	# 根据位置选择要操作的元素
	var target : TextureRect
	match position:
		FigurePosition.Left:
			target = figure_left
		FigurePosition.Right:
			target = figure_right
		_:
			target = figure_center

	# 调用通用方法
	_change_element(target, image, alpha, hide)

## 切换头像 [br]
## [br]
## [param image] : [br]
## 图片 [br]
## [br]
## [param alpha] : [br]
## 透明度 [br]
## [br]
## [param hide] : [br]
## 是否隐藏
static func change_portrait(image: Texture, alpha: float = 1.0, hide: bool = false) -> void:
	_change_element(portrait, image, alpha, hide)

## 切换背景 [br]
## [br]
## [param image] : [br]
## 图片 [br]
## [br]
## [param alpha] : [br]
## 透明度 [br]
static func change_background(image: Texture, alpha: float = 1.0) -> void:
	_change_element(background, image, alpha)


static func _change_element(element: TextureRect, image: Texture, alpha: float = 1.0, hide: bool = false) -> void:
	if not element: # 检查实例是否存在
		push_error("VisualNoveCore: Missing instance")
		return

	# 限制透明度范围
	alpha = clamp(alpha, 0.0, 1.0)
	if hide: # 隐藏
		element.visible = false
	else:
		element.visible = true
		element.texture = image
		element.modulate = Color(1.0, 1.0, 1.0, alpha)
