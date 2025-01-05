## 截取游戏画面
class_name ScreenshotToTextureRect extends VisualNoveCore

## 截取的图片
static var screenshot: Texture2D

## 从字节数组加载截图 [br]
## [br]
## [param data] : [br]
## 图片数据 [br]
## 返回加载的图片
static func load_screenshot_form_bytes(data: PackedByteArray) -> Texture2D:
	var texture: Image = Image.new()
	if texture.load_png_from_buffer(data) == OK:
		return ImageTexture.create_from_image(texture)
	else:
		push_error("Failed to load texture from byte array.")
		return null

## 截取游戏画面并返回Texture2D
static func capture_screenshot() -> Texture2D:
	if not game_manager_node:
		push_error("The node on the NodeTree is not set.")
		return null
	var viewport = game_manager_node.get_viewport()
	var texture = viewport.get_texture()

	var img: Image = texture.get_image()
	screenshot = ImageTexture.create_from_image(img)
	return screenshot

## 获取截图的字节数组
static func get_screenshot_bytes() -> PackedByteArray:
	if not screenshot:
		return []
	return screenshot.get_image().save_png_to_buffer()
