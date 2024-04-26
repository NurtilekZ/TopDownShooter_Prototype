using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace _current.Services.Logging
{
    public class LoggingService : ILoggingService
    {
        private const string UIColor = "#e346b4";
        private const string DefaultColor = "#e3e3e3";
        private const string GameplayColor = "#4697e3";
        private const string GameStateMachineColor = "#e38d46";
        private const string InfrastructureColor = "#e38d46";

        public void LogMessage(string message, object sender = null, LoggingTag loggingTag = LoggingTag.Logger) => 
            Debug.Log(GetString(message, sender ?? this, loggingTag));

        public void LogWarning(string message, object sender = null, LoggingTag loggingTag = LoggingTag.Logger) => 
            Debug.Log(GetString(message, sender ?? this, loggingTag));

        public void LogError(string message, object sender = null, LoggingTag loggingTag = LoggingTag.Logger) => 
            Debug.Log(GetString(message, sender ?? this, loggingTag));

        private string GetString(string message, object sender, LoggingTag loggingTag)
        {
            var logTag = loggingTag != LoggingTag.Logger ? loggingTag.ToString() : string.Empty; 
            return $"<b><i><color={GetHexColor(sender.GetType())}>[{logTag}({sender.GetType().Name})]</color></i></b> {message}";
        }

        private string GetHexColor(Type sender)
        {
            return sender.Namespace switch
            {
                var x when Regex.IsMatch(x, @".*Infrastructure.") => InfrastructureColor,
                var x when Regex.IsMatch(x, @".*States.") => GameStateMachineColor,
                var x when Regex.IsMatch(x, @".*Core.") => GameplayColor,
                var x when Regex.IsMatch(x, @".*UI.") => UIColor,
                _ => DefaultColor,
            };
        }
    }

    public enum LoggingTag
    {
        Logger,
        Infrastructure,
        StateMachine,
        UI,
        Game,
    }
}