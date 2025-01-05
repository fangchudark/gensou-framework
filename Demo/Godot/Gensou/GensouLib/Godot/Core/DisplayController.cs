using System.Threading;
using System.Threading.Tasks;
using GensouLib.GenScript.Interpreters;
using Godot;

namespace GensouLib.Godot.Core
{
    /// <summary>
    /// 文字显示控制器
    /// </summary>
    public partial class DisplayController : VisualNoveCore
    {
        /// <summary>
        /// 显示的文字
        /// </summary>
        [Export]
        public Label TextToDisplay;

        /// <summary>
        /// 是否在打字
        /// </summary>
        [Export]
        public bool IsTyping { get; private set;} = false;
        
        private Task typingTask;
        private CancellationTokenSource tokenSource;

        /// <summary>
        /// 显示一行文字
        /// </summary>
        /// <param name="text">需要显示的文字</param>
        public void DisplayLine(string text)
        {
            tokenSource?.Cancel(); // 取消打字任务
            tokenSource = new CancellationTokenSource(); // 重新创建取消标记
            var token = tokenSource.Token;
            typingTask = TypeText(text, token); // 启动打字任务
        }

        private async Task TypeText(string text, CancellationToken token)
        {
            IsTyping = true; // 标记正在打字
            TextToDisplay.Text = text; // 设置需要显示的文字
            TextToDisplay.VisibleCharacters = 0; // 隐藏文本
            for (int i = 0; i < text.Length; i++)
            {
                if (token.IsCancellationRequested) 
                {
                    return; // 打字任务被取消
                }
                TextToDisplay.VisibleCharacters = i + 1; // 显示文本

                if (OnSkiping) await ToSignal(GetTree().CreateTimer(0.01f), "timeout");
                else await ToSignal(GetTree().CreateTimer(TextDisplaySpeed), "timeout");
            }
            IsTyping = false; // 标记打字结束

            if (OnAutoPlay && !OnSkiping && !ChoiceInterpreter.OnChoosing) 
            {
                await ToSignal(GetTree().CreateTimer(AutoPlayInterval), "timeout"); // 等待自动播放时间
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
            if (typingTask != null && !typingTask.IsCompleted)
            {
                tokenSource?.Cancel(); // 取消打字任务
            }
            if (TextToDisplay.Text != DialogueInterpreter.CurrentDialogue) // 如果当前显示的文本不是当前对话
            {
                TextToDisplay.Text = DialogueInterpreter.CurrentDialogue; // 设置显示文本为当前对话
            }
            TextToDisplay.VisibleCharacters = -1; // 显示所有文本
            IsTyping = false; // 标记打字结束
            if (OnAutoPlay && !ChoiceInterpreter.OnChoosing) BaseInterpreter.ExecuteNextLine(); // 自动播放下一行
            else if (OnSkiping && !ChoiceInterpreter.OnChoosing) BaseInterpreter.ExecuteNextLine(); // 跳过当前行
        }
    }
}