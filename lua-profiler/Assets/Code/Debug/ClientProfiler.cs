
/********************************************************************
created:    2017-11-13
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Profiling;

namespace Unique
{
    public static class ClientProfiler
    {
        [Conditional("UNITY_EDITOR")]
        public static void BeginSample (int id)
        {
            {
                string name;
                _showNames.TryGetValue(id, out name);
                name = name ?? string.Empty;

                Profiler.BeginSample(name);
                ++_sampleDepth;
            }
        }

        [Conditional("UNITY_EDITOR")]
        public static void BeginSample (int id, string name)
        {
            {
                name = name ?? string.Empty;
                _showNames[id] = name;

                Profiler.BeginSample(name);
                ++_sampleDepth;
            }
        }

        [Conditional("UNITY_EDITOR")]
        internal static void BeginSample (string name)
        {
            name = name ?? string.Empty;
            Profiler.BeginSample(name);
            ++_sampleDepth;
        }

        [Conditional("UNITY_EDITOR")]
        public static void EndSample ()
        {
            if (_sampleDepth > 0)
            {
                --_sampleDepth;
                Profiler.EndSample();
            }
        }

        private static int _sampleDepth;
        // private const int  _maxSampleDepth = 100;
        private static readonly Dictionary<int, string> _showNames = new Dictionary<int, string>();
    }
}