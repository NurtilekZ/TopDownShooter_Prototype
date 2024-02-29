using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Services.Logging
{
    class LoggingService : ILoggingService
    {
        private const string DefaultColor        = "#e3e3e3";
        private const string InfrastructureColor = "#e38d46";
        private const string MetaColor           = "#e346b4";
        private const string GameplayColor       = "#4697e3";

        public void LogMessage(string message, object sender = null) => 
            Debug.Log(GetString(message, sender ?? this));

        public void LogWarning(string message, object sender = null) => 
            Debug.Log(GetString(message, sender ?? this));

        public void LogError(string message, object sender = null) => 
            Debug.Log(GetString(message, sender ?? this));

        private string GetString(string message, object sender)
        {
            return $"<b><i><color={GetHexColor(sender.GetType())}>{sender.GetType().Name}: </color></i></b> {message}";
        }

        private string GetHexColor(Type sender)
        {
            return sender.Namespace switch
            {
                var x when Regex.IsMatch(x, @".*Infrastructure.") => InfrastructureColor,
                var x when Regex.IsMatch(x, @".*Meta.") => MetaColor,
                var x when Regex.IsMatch(x, @".*Gameplay.") => GameplayColor,
                _ => DefaultColor,
            };
        }
    }
}