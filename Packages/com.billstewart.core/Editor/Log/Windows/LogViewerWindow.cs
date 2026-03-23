#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Bill.Mutant.Core.Editor
{
    public class LogViewerWindow : EditorWindow
    {
        private Vector2 _scroll;
        private string _search = string.Empty;
        private string _categoryFilter = string.Empty;
        private bool _autoRefresh = true;
        private double _lastRepaintTime;

        private readonly Dictionary<LogLevel, bool> _levelFilters = new Dictionary<LogLevel, bool>
        {
            { LogLevel.Trace, true },
            { LogLevel.Debug, true },
            { LogLevel.Info, true },
            { LogLevel.Warning, true },
            { LogLevel.Error, true },
            { LogLevel.Fatal, true }
        };

        [MenuItem("Mutant/Core/Log Viewer")]
        public static void ShowWindow()
        {
            GetWindow<LogViewerWindow>("Mutant Log Viewer");
        }

        private void OnEnable()
        {
            EditorApplication.update += OnEditorUpdate;
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
        }

        private void OnEditorUpdate()
        {
            if (!_autoRefresh) return;

            if (EditorApplication.timeSinceStartup - _lastRepaintTime > 0.5d)
            {
                _lastRepaintTime = EditorApplication.timeSinceStartup;
                Repaint();
            }
        }

        private void OnGUI()
        {
            DrawToolbar();
            DrawFilterBar();
            DrawLogList();
        }

        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            if (GUILayout.Button("Refresh", EditorStyles.toolbarButton, GUILayout.Width(60)))
            {
                Repaint();
            }

            if (GUILayout.Button("Clear", EditorStyles.toolbarButton, GUILayout.Width(60)))
            {
                Logger.ClearMemory();
            }

            _autoRefresh = GUILayout.Toggle(_autoRefresh, "Auto", EditorStyles.toolbarButton, GUILayout.Width(60));

            GUILayout.FlexibleSpace();

            GUILayout.Label($"Entries: {Logger.GetMemoryEntries().Count}", EditorStyles.miniLabel, GUILayout.Width(80));

            EditorGUILayout.EndHorizontal();
        }

        private void DrawFilterBar()
        {
            EditorGUILayout.BeginVertical("box");

            _search = EditorGUILayout.TextField("Search", _search);
            _categoryFilter = EditorGUILayout.TextField("Category", _categoryFilter);

            EditorGUILayout.BeginHorizontal();
            DrawLevelToggle(LogLevel.Trace);
            DrawLevelToggle(LogLevel.Debug);
            DrawLevelToggle(LogLevel.Info);
            DrawLevelToggle(LogLevel.Warning);
            DrawLevelToggle(LogLevel.Error);
            DrawLevelToggle(LogLevel.Fatal);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        private void DrawLevelToggle(LogLevel level)
        {
            _levelFilters[level] = GUILayout.Toggle(_levelFilters[level], level.ToString(), "Button", GUILayout.Height(22));
        }

        private void DrawLogList()
        {
            IReadOnlyList<LogEntry> entries = Logger.GetMemoryEntries();

            _scroll = EditorGUILayout.BeginScrollView(_scroll);

            for (int i = entries.Count - 1; i >= 0; i--)
            {
                var entry = entries[i];

                if (!PassFilter(entry))
                    continue;

                DrawEntry(entry);
            }

            EditorGUILayout.EndScrollView();
        }

        private bool PassFilter(LogEntry entry)
        {
            if (!_levelFilters.TryGetValue(entry.Level, out bool enabled) || !enabled)
                return false;

            if (!string.IsNullOrWhiteSpace(_search))
            {
                if ((entry.Message == null || entry.Message.IndexOf(_search, StringComparison.OrdinalIgnoreCase) < 0) &&
                    (entry.Source == null || entry.Source.IndexOf(_search, StringComparison.OrdinalIgnoreCase) < 0))
                    return false;
            }

            if (!string.IsNullOrWhiteSpace(_categoryFilter))
            {
                if (entry.Category == null || entry.Category.IndexOf(_categoryFilter, StringComparison.OrdinalIgnoreCase) < 0)
                    return false;
            }

            return true;
        }

        private void DrawEntry(LogEntry entry)
        {
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(GetLevelTag(entry.Level), GUILayout.Width(60));
            GUILayout.Label(entry.Timestamp.ToString("HH:mm:ss.fff"), GUILayout.Width(90));
            GUILayout.Label(entry.Category ?? string.Empty, GUILayout.Width(100));
            GUILayout.Label(entry.Source ?? string.Empty, GUILayout.Width(120));

            GUIStyle msgStyle = new GUIStyle(EditorStyles.label)
            {
                wordWrap = true
            };
            EditorGUILayout.LabelField(entry.Message ?? string.Empty, msgStyle);

            if (GUILayout.Button("Copy", GUILayout.Width(50)))
            {
                EditorGUIUtility.systemCopyBuffer = FormatEntry(entry);
            }

            EditorGUILayout.EndHorizontal();

            if (!string.IsNullOrEmpty(entry.StackTrace))
            {
                EditorGUILayout.Space(2);
                EditorGUILayout.LabelField("StackTrace:", EditorStyles.boldLabel);
                EditorGUILayout.TextArea(entry.StackTrace, GUILayout.MinHeight(40));
            }

            EditorGUILayout.EndVertical();
        }

        private string GetLevelTag(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Trace: return "[Trace]";
                case LogLevel.Debug: return "[Debug]";
                case LogLevel.Info: return "[Info]";
                case LogLevel.Warning: return "[Warn]";
                case LogLevel.Error: return "[Error]";
                case LogLevel.Fatal: return "[Fatal]";
                default: return "[?]";
            }
        }

        private string FormatEntry(LogEntry entry)
        {
            return $"[{entry.Timestamp:yyyy-MM-dd HH:mm:ss.fff}][{entry.Level}][{entry.Category}][{entry.Source}] {entry.Message}\n{entry.StackTrace}";
        }
    }
}
#endif
