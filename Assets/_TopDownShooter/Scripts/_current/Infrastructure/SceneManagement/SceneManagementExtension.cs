using System;

namespace _current.Infrastructure.SceneManagement
{
    public static class SceneManagementExtension
    {
        private const string Bootstrap = "Bootstrap";
        private const string Meta = "Meta";
        private const string Core = "Core";

        public static SceneName ToSceneName(this string sceneName)
        {
            return sceneName switch
            {
                Bootstrap => SceneName.Bootstrap,
                Meta => SceneName.Meta,
                Core => SceneName.Core,
                _ => throw new ArgumentOutOfRangeException(nameof(sceneName), sceneName, null)
            };
        }

        public static string ToSceneString(this SceneName sceneName)
        {
            return sceneName switch
            {
                SceneName.Bootstrap => Bootstrap,
                SceneName.Meta => Meta,
                SceneName.Core => Core,
                _ => throw new ArgumentOutOfRangeException(nameof(sceneName), sceneName, null)
            };
        }

        public static bool IsGamePlayScene(this SceneName sceneName)
        {
            return sceneName is SceneName.Core;
        }
    }
}