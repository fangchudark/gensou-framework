using System.Collections;
using GensouLib.GenScript.Interpreters;
using TMPro;
using UnityEngine;

namespace GensouLib.Unity.Core
{
    /// <summary>
    /// 文字显示控制器
    /// </summary>
    public class DisplayController : VisualNoveCore
    {
        /// <summary>
        /// 显示的文字
        /// </summary>
        public TextMeshProUGUI TextToDisplay; 

        /// <summary>
        /// 只读，是否正在打字
        /// </summary>
        public bool IsTyping { get; private set; } = false;
        private Coroutine typingCoroutine; // 打字机协程
        public DisplayController(TextMeshProUGUI textToDisplay)
        {
            TextToDisplay = textToDisplay; // 绑定显示文本
        }

        /// <summary>
        /// 显示一行文字
        /// </summary>
        /// <param name="text">需要显示的文字</param>
        public void DisplayLine(string text)
        {
            if (typingCoroutine != null) // 停止之前的打字机协程
            {
                StopCoroutine(typingCoroutine);
            }
            typingCoroutine = StartCoroutine(TypeText(text)); // 启动新的打字机协程
        }

        private IEnumerator TypeText(string text)
        {
            IsTyping = true; // 标记正在打字
            TextToDisplay.text = text; // 设置需要显示的文本
            TextToDisplay.maxVisibleCharacters = 0; // 隐藏文本
            for (int i = 0; i < text.Length; i++)
            {
                TextToDisplay.maxVisibleCharacters = i + 1; // 逐渐增加可见字符数
                if (OnSkiping) yield return new WaitForSeconds(0.01f);
                else yield return new WaitForSeconds(TextDisplaySpeed); // 等待一段时间
            }
            IsTyping = false; // 标记打字结束
            
            if (OnAutoPlay && !OnSkiping && !ChoiceInterpreter.OnChoosing) 
            {
                yield return new WaitForSeconds(AutoPlayInterval); // 等待自动播放时间
                BaseInterpreter.ExecuteNextLine(); // 自动播放下一行
            }
            else if (OnSkiping && !ChoiceInterpreter.OnChoosing)
            {
                BaseInterpreter.ExecuteNextLine(); // 跳过当前行
            }
        }

        /// <summary>
        /// 停止打字，显示完整的对话
        /// </summary>
        public void DisplayCompleteLine()
        {
            if (typingCoroutine != null) // 停止之前的打字机协程
            {
                StopCoroutine(typingCoroutine);
            }
            if (TextToDisplay.text != DialogueInterpreter.CurrentDialogue) // 如果当前显示的文本不是当前对话
            {
                TextToDisplay.text = DialogueInterpreter.CurrentDialogue; // 设置显示文本为当前对话
            }
            TextToDisplay.maxVisibleCharacters = TextToDisplay.text.Length; // 显示所有文本
            IsTyping = false; // 标记打字结束
            if (OnAutoPlay && !ChoiceInterpreter.OnChoosing) BaseInterpreter.ExecuteNextLine(); // 自动播放下一行
            else if (OnSkiping && !ChoiceInterpreter.OnChoosing) BaseInterpreter.ExecuteNextLine(); // 跳过当前行
        }
 
    }
}