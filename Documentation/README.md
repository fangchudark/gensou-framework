# Genscirpt 语法

注释使用`;`，`;`后的命令将被忽略，`;`可使用`\;`进行转义

## 变量

变量操作使用`var`命令，加上`-global`参数使之成为全局变量，全局变量将不保存到存档当中。

> var:[var_name]  
> var:[var_name] -global

需要获得随机数时，将`random()`赋值给变量，将会获得一个0到1之间的随机数。

> var:[rand_num]=random()

变量支持常见的四种类型：字符串，整数，浮点数和布尔值。  

赋值为字符串时，不必须要双引号包裹

> var:[str_var]="hello world"  
> var:[int_var]=10  
> var:[float_var]=3.14  
> var:[bool_var]=true

后续不需要使用的变量可以使用`release`命令删除
> release:[var_name]

**数学表达式中不要用空格来分开运算符和操作数。**

## 条件

任何命令都有参数`when`，使用`when`来控制是否执行该行命令


**相等比较使用`==`，而不是`=`**

**条件表达式中不要用空格来分开运算符和操作数，字符串操作数必须使用双引号包裹**

> [command] -when=[condition]

## 切换立绘

使用`changeFigure`命令切换立绘，需要指定有效路径的文件名，对于Godot平台，需要指定扩展名。

> changeFigure:[figure_file_name]

默认切换中间立绘，使用`-left`，`-right`参数可以切换左右立绘。

> changeFigure:[figure_file_name] -left  
> changeFigure:[figure_file_name] -right

使用`-alpha`参数来指定透明度，范围为0到1。

> changeFigure:[figure_file_name] -alpha=[alpha_value]

命令如果指定的是`none`，将会清除指定位置的立绘，不指定方向时，默认清除中间立绘。

> changeFigure:none -left

## 切换头像

使用`changePortrait`命令切换对话框左下角小头像，需要指定有效路径的文件名，对于Godot平台，需要指定扩展名。

> changePortrait:[portrait_file_name]

使用`-alpha`参数来指定透明度，范围为0到1。

> changePortrait:[portrait_file_name] -alpha=[alpha_value]

命令如果指定的是`none`，将会清除头像。

> changePortrait:none

## 切换背景

使用`changeBg`命令切换背景，需要指定有效路径的文件名，对于Godot平台，需要指定扩展名。

> changeBg:[bg_file_name]

使用`-alpha`参数来指定透明度，范围为0到1。

> changeBg:[bg_file_name] -alpha=[alpha_value]

命令如果指定的是`none`，将会清除背景。

> changeBg:none

## 切换音频

使用`bgm`，`bgs`，`se`，命令切换对应音频，需要指定有效路径的文件名，对于Godot平台，需要指定扩展名。

> bgm:[bgm_file_name]  
> bgs:[bgs_file_name]  
> se:[se_file_name]

命令如果指定的是`none`，将会停止播放对应音频。

> bgm:none  
> bgs:none  
> se:none

使用`-volume`参数来指定音量，范围为0到1。

> bgm:[bgm_file_name] -volume=[volume_value]  
> bgs:[bgs_file_name] -volume=[volume_value]  
> se:[se_file_name] -volume=[volume_value]

使用`-enter`参数来指定淡入时间，单位为秒。

> bgm:[bgm_file_name] -enter=[fade_time]  
> bgs:[bgs_file_name] -enter=[fade_time]  
> se:[se_file_name] -enter=[fade_time]

在音频被指定为`none`时，使用`-enter`，则会指定淡出时间

可以在任何语句使用`-voice`参数播放指定语音，用法同上。

> [command] -voice=[voice_file_name]

## 分支选项

使用`choose`命令创建一些剧情分支按钮

> choose:[option1]->line:[line_index]|[option2]->[story_file_name]->[line_index]

使用`|`来分隔选项，`->`指定选项目标，当指定为`line:[line_index]`时，会跳转到当前剧本对应行索引处（索引就是行号减一），当指定为`[story_file_name]->[line_index]`时，会跳转到指定剧本对应行索引处，剧本文件名不需要指定扩展名。

**跳转到其他剧本时，必须指定行索引，如果要从头开始执行，就将行索引指定为`0`**

## 剧本跳转

使用`call`命令跳转到其他剧本从头开始执行，需要指定剧本文件名，不需要指定扩展名。

> call:[story_file_name]

## 隐藏对话框

使用`setTextbox`命令，并将参数设置为`hide`来隐藏对话框。

> setTextbox:hide

设置除`hide`外的其他任何值都能显示对话框。

被这样隐藏的对话框，无法被玩家打开，除非使用`setTextbox`命令再次显示。

## 设置字体大小

使用`fontSize`参数设置字体大小，默认为24。

> [command] -fontSize=[font_size]

## 返回标题

使用`end`命令返回标题画面。

> end

## 对话

无法被解析的命令会被解析为对话，可以使用以下三种语法创建对话：

指定说话者，并指定对话内容：
> \[speaker]:[dialogue]   

不指定说话者，只指定对话内容，说话者一栏将什么也不显示：
> \:[dialogue]  

不指定说话者，只指定对话内容，说话者一栏将显示上一句对话的说话者：
> [dialogue]  