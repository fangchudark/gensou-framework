using UnityEngine;
using UnityEngine.UI;

namespace GensouLib.Unity.Core
{
    /// <summary>
    /// 图片控制器
    /// </summary>
    public class ImageController : VisualNoveCore
    {
        /// <summary>
        /// 立绘位置
        /// </summary>
        public enum FigurePosition
        {
            /// <summary>
            /// 左侧
            /// </summary>
            Left,

            /// <summary>
            /// 中间
            /// </summary>
            Center,

            /// <summary>
            /// 右侧
            /// </summary>
            Right
        }

        /// <summary>
        /// 切换立绘
        /// </summary>
        /// <param name="image">图片</param>
        /// <param name="alpha">透明度</param>
        /// <param name="position">位置</param>
        /// <param name="hide">是否隐藏</param>
        public static void ChangeFigure(Sprite image, float alpha = 1.0f, FigurePosition position = FigurePosition.Center, bool hide = false)
        {
            if (FigureLeft == null || FigureCenter == null || FigureRight == null) // 检查实例是否存在
            {
                Debug.LogError("VisualNoveCore: Missing instances");
                return;
            }

            // 根据位置选择要操作的元素
            Image target = position switch
            {
                FigurePosition.Left => FigureLeft,
                FigurePosition.Right => FigureRight,
                _ => FigureCenter,
            };

            // 调用通用方法
            ChangeElement(target, image, alpha, hide);
        }
    
        /// <summary>
        /// 切换头像
        /// </summary>
        /// <param name="image">图片</param>
        /// <param name="alpha">透明度</param>
        /// <param name="hide">是否隐藏</param>
        public static void ChangePortrait(Sprite image, float alpha = 1.0f, bool hide = false)
            => ChangeElement(Portrait, image, alpha, hide);
    
        /// <summary>
        /// 切换背景
        /// </summary>
        /// <param name="image">图片</param>
        /// <param name="alpha">透明度</param>
        public static void ChangeBackground(Sprite image, float alpha = 1.0f)
            => ChangeElement(Background, image, alpha);

        private static void ChangeElement(Image element, Sprite image, float alpha = 1.0f, bool hide = false)
        {
            if (element == null) // 检查实例是否存在
            {
                Debug.LogError("VisualNoveCore: Missing instance");
                return;
            }

            // 限制透明度范围
            alpha = Mathf.Clamp(alpha, 0.0f, 1.0f);

            if (hide) // 隐藏
            {
                element.gameObject.SetActive(false);
            }
            else
            {
                element.gameObject.SetActive(true);
                element.sprite = image;
                element.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            }
        }
    }
}