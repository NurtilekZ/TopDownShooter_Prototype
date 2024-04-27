using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace _current.Services.Logging
{
    public class LoggingService : ILoggingService
    {
        private const string UIColor = "#156ebd";
        private const string DefaultColor = "#ffea00";
        private const string GameplayColor = "#bd8a15";
        private const string GameStateMachineColor = "#15b7bd";
        private const string InfrastructureColor = "#50bd15";
        
        private const string WarningMessageColor = "#ffd000";
        private const string ErrorMessageColor = "#ff0000";

        public void LogMessage(string message, object sender = null, LoggingTag loggingTag = LoggingTag.Logger) => 
            Debug.Log(GetString(message, sender ?? this, loggingTag));

        public void LogWarning(string message, object sender = null, LoggingTag loggingTag = LoggingTag.Logger) => 
            Debug.Log(GetString($"<color={WarningMessageColor}>{message}</c>", sender ?? this, loggingTag));

        public void LogError(string message, object sender = null, LoggingTag loggingTag = LoggingTag.Logger) => 
            Debug.Log(GetString($"<color={ErrorMessageColor}>{message}</c>", sender ?? this, loggingTag));

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