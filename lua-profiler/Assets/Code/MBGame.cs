
/********************************************************************
created:    2017-11-15
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/

using System;
using UnityEngine;
using LuaInterface;

namespace Client
{
    public class MBGame : LuaClient
    {
        protected override void OnLoadFinished ()
        {
            base.OnLoadFinished();
//            _luaUpdateFunction = luaState.GetFunction("Update");
        }

        private void Update ()
        {
            if (null != _luaUpdateFunction)
            {
//                _luaUpdateFunction.Call();
            }
        }

        public static IntPtr GetLuaStatePtr ()
        {
            return Instance._GetLuaStatePtr ();
        }

        private static LuaFunction _luaUpdateFunction;
    }
}