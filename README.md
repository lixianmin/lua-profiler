

### [lua-profiler](https://github.com/lixianmin/lua-profiler)

---
#### 问题由来

由于在线更新或bugfix的原因，lua在Unity3d手游中得到大量应用，但是缺乏配套的调试工具，使得lua相关的一些性能问题很难诊断，往往重度依赖程序人员分析代码，这对于简单的情景尚可应对，对复杂的游戏逻辑很多情况下会束手束脚。

Unity3d本身自带图形化Profiler工具，但是默认只会分析C#代码，对于lua代码则无法提供帮助。

----
#### 设计思路

本插件的设计思路很简单：

1. Unity3d引擎中有Profiler相关的库方法，可以让我们通过手动编码分析代码的性能热点。

2. lua的hook机制可以让我们在function的call和return时刻插入自己的代码

3. 这样，通过hook机制，我们在function的call时刻调用Profiler.BeginSample()，在return时刻调用Profiler.EndSample()，就可以借助Unity3d的Profiler机制分析lua代码的性能热点

---
#### 测试环境

项目使用的Unity3d版本是Unity 2017.1.0f3，如果你的Unity3d恰好是这个版本（或者更高一些），那么你可以直接打开WinMain.unity场景运行测试；如果你的Unity3d版本略低，也没有关系，你只是需要新建一个场景并且把MBGame这个脚本拖到某个GameObject上运行就可以了。

项目使用的lua环境是[tolua 1.0.7.381](https://github.com/topameng/tolua)的版本，或者说是当前时间2017-11-15的最新版本。

项目使用的操作系统是mac OS 10.12.6。在mac系统中tolua默认使用的是luavm库，如果你使用的是windows，则tolua默认使用的是luajit库，因此看到的lua调用栈可能与后面的测试截图不同。

----
#### 代码与文件

项目中，你可能需要关注的文件有：

name | description
---   |---
WinMain.unity | 测试场景文件
MBGame.cs | 启动脚本，挂到WinMain场景中的Game对象上即可
ClientProfiler.cs  | 对UnityEngine.Profiling.Profiler的封装
LuaProfilerMenu.cs | 加入*Lua下面的菜单项，同时hook相关的代码也在这里
Main.lua  | 测试用到的lua代码

如果要把lua-profiler集成到你自己的开发环境中，你需要做的事情包括：

1. 将Unique.ClientProfiler这个类导出到lua中，在tolua中就是将其加入到CustomSettings.cs中，然后导出

2. 在Attach或Detach本lua-profiler的时候，需要调用某种形式的 luaL_dostring (IntPtr luaState, string chunck)，你需要保证该函数调用到正确的luaState

---
#### 应用与问题

该lua-profiler方案最早应用于完美乐逍遥的《火炬之光移动版》项目，不过当时是在C代码中调用的sethook。

该方案的主要优点是C代码的性能会更好，但是也会有两个问题：

1. 一是需要自己编译各平台的lua库；

2. 二是由于luajit优化了对tail call，导致该方案只能使用luavm

本项目采用的是在lua中sethook的方案，这样在luajit中也是可以使用的。

---
#### 测试截图

1. 首先在unity3d中，通过Window/Profiler菜单，打开 Profiler窗口

2.  游戏启动后，点击*Lua/Attach Lua Profiler菜单

3. 然后在Profiler窗口中展开CPU Usage一项，就可以看到各lua方法在每一帧的调用开销了

![attach](https://raw.githubusercontent.com/lixianmin/lua-profiler/master/images/attach.jpeg)

![profiler](https://raw.githubusercontent.com/lixianmin/lua-profiler/master/images/profiler.jpeg)


