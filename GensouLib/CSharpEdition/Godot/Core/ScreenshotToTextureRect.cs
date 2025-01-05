using Godot;

namespace GensouLib.Godot.Core
{
    /// <summary>
    /// 截取游戏画面
    /// </summary>
    public partial class ScreenshotToTextureRect : VisualNoveCore
    {
        /// <summary>
        /// 截取的图片
        /// </summary>
        public static Texture2D Screenshot { get; private set; }

        /// <summary>
        /// 从字节数组加载截图
        /// </summary>
        /// <param name="data">图片数据</param>
        /// <returns>加载的图片</returns>
        public static Texture2D LoadScreenshotFormBytes(byte[] data)
        {
            Image texture = new();
            if (texture.LoadPngFromBuffer(data) == Error.Ok)
            {
                return ImageTexture.CreateFromImage(texture);
            }
            else
            {
                GD.PushError("Failed to load texture from byte array.");
                return null;
            }
        }

        /// <summary>
        /// 截取游戏画面
        /// </summary>
        /// <returns>截取的游戏画面</returns>
        public static Texture2D CaptureScreenshot()
        {
            if (GameManagerNode == null)
            {
                GD.PushError("The node on the NodeTree is not set.");
                return null;
            }
            // 获取主 Viewport
            var viewport = GameManagerNode.GetViewport();
            var texture = viewport.GetTexture();
            
            // 获取 Image 对象并返回
            Image image = texture.GetImage();
            Screenshot = ImageTexture.CreateFromImage(image);
            return Screenshot;
        }

        /// <summary>
        /// 获取截图的字节数组
        /// </summary>
        /// <returns>获取的图像数据</returns>
        public static byte[] GetScreenshotBytes()
        {
            if (Screenshot == null)
                return null;
            return Screenshot.GetImage().SavePngToBuffer();
        }
    }
}