using UnityEngine;
using UnityEngine.UI;

namespace GensouLib.Unity.Core
{
    /// <summary>
    /// 截取游戏画面
    /// </summary>
    public class ScreenshotToRawImage : VisualNoveCore
    {
        /// <summary>
        /// 截图的相机
        /// </summary>
        public static Camera TargetCamera { get; set; }      // 截图的相机
        
        /// <summary>
        /// 用于显示截图的 RawImage
        /// </summary>
        public static RawImage DisplayImage { get; set; }    // 用于显示截图的 RawImage
        
        /// <summary>
        /// 截取的图片
        /// </summary>
        public static Texture2D Screenshot { get; private set; }
        
        /// <summary>
        /// 截图的宽度
        /// </summary>
        public static int ScreenshotWidth { get; set; } = 1920; // 截图宽度
        
        /// <summary>
        /// 截图的高度
        /// </summary>
        public static int ScreenshotHeight { get; set; } = 1080; // 截图高度

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="targetCamera">截图的相机</param>
        /// <param name="screenshotWidth">截图的宽度</param>
        /// <param name="screenshotHeight">截图的高度</param>
        /// <param name="displayImage">用于显示截图的 RawImage</param>
        public static void Init(
            Camera targetCamera, 
            int screenshotWidth, 
            int screenshotHeight, 
            RawImage displayImage = null
        )
        {
            TargetCamera = targetCamera;
            ScreenshotWidth = screenshotWidth;
            ScreenshotHeight = screenshotHeight;
            DisplayImage = displayImage;
        }

        /// <summary>
        /// 从字节数组加载图片
        /// </summary>
        /// <param name="data">图片数据</param>
        /// <returns>加载的图片</returns>
        public static Texture2D LoadScreenshotFormBytes(byte[] data)
        {
            Texture2D texture = new(2, 2); // 创建一个默认大小的空纹理
            if (texture.LoadImage(data)) // 加载字节数组为纹理
            {
                return texture; // 成功加载返回纹理
            }
            else
            {
                Debug.LogError("Failed to load texture from byte array.");
                return null;
            }
        }

        /// <summary>
        /// 截取游戏画面并显示到 RawImage 中
        /// </summary>
        /// <returns>截取的游戏画面</returns>
        public static Texture2D CaptureScreenshot()
        {
            if (TargetCamera == null)
            {
                Debug.LogError("TargetCamera is not set.");
                return null;
            }
            // 创建 RenderTexture
            RenderTexture renderTexture = new(ScreenshotWidth, ScreenshotHeight, 24);
            TargetCamera.targetTexture = renderTexture;

            // 渲染相机视图到 RenderTexture
            TargetCamera.Render();

            // 创建 Texture2D 并从 RenderTexture 读取像素数据
            RenderTexture.active = renderTexture;
            Texture2D screenshot = new(ScreenshotWidth, ScreenshotHeight, TextureFormat.RGB24, false);
            screenshot.ReadPixels(new Rect(0, 0, ScreenshotWidth, ScreenshotHeight), 0, 0);
            screenshot.Apply();

            // 清理 RenderTexture
            TargetCamera.targetTexture = null;
            RenderTexture.active = null;
            Destroy(renderTexture);

            // 赋值截图
            if (DisplayImage != null) 
            {
                DisplayImage.texture = screenshot; 
                //DisplayImage.SetNativeSize(); // 根据截图大小自动调整 RawImage 大小
            }
            Screenshot = screenshot;
            return screenshot;
        }
    }
}