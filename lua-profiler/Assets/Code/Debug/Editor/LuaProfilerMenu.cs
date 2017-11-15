
/********************************************************************
created:    2017-11-15
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/

using Unique;
using System;
using UnityEditor;

namespace Lua.Menus
{
	static class LuaProfilerMenu
    {
		[MenuItem ("*Lua/Attach Lua Profiler")]
		private static void _ExecuteAttach ()
		{
            if (_isAttached)
            {
                return;
            }

            _isAttached = true;

            string script = @"
            local ClientProfiler = Unique.ClientProfiler
            local debug = debug

            local _cache = {}
            local _id_generator = 0
            local _ignore_count = 0

            local function lua_profiler_hook (event, line)
                if event == 'call' then
                    local func = debug.getinfo (2, 'f').func
                    local id = _cache[func]

                    if id then
                        ClientProfiler.BeginSample (id)
                    else
                        local ar = debug.getinfo (2, 'Sn')
                        local method_name = ar.name
                        local linedefined = ar.linedefined

                        if linedefined ~= -1 or (method_name and method_name ~= '__index')  then
                            local short_src = ar.short_src
                            method_name = method_name or '[unknown]'

                            local index = short_src:match ('^.*()[/\\]')
                            local filename  = index and short_src:sub (index + 1) or short_src
                            local show_name = filename .. ':' .. method_name .. ' '.. linedefined

                            local id = _id_generator + 1
                            _id_generator = id
                            _cache[func] = id

                            ClientProfiler.BeginSample (id, show_name)
                        else
                            _ignore_count = _ignore_count + 1
                        end
                    end
                elseif event == 'return' then
                    if _ignore_count == 0 then
                        ClientProfiler.EndSample ()
                    else
                        _ignore_count = _ignore_count - 1
                    end
                end
            end

            debug.sethook (lua_profiler_hook, 'cr', 0)
            ";

            LuaInterface.LuaDLL.luaL_dostring (Client.MBGame.GetLuaStatePtr(), script);
		}

		[MenuItem("*Lua/Detach Lua Profiler")]
		private static void _ExecuteDetach ()
		{
            if (!_isAttached)
            {
                return;
            }

            _isAttached = false;
            var script = "debug.sethook (nil)";
            LuaInterface.LuaDLL.luaL_dostring (Client.MBGame.GetLuaStatePtr(), script);
		}

        private static bool _isAttached;
    }
}