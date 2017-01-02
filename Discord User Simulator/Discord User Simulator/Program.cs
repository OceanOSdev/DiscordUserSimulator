using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

//A bot can join a server by sending join [inviteurl] to it.
//however, the API does not support that.To make a bot join using the api, use the following URL: https://discordapp.com/oauth2/authorize?&client_id=265610724049420289&scope=bot&permissions=0
namespace Discord_User_Simulator
{
    public class Program
    {
        private DiscordClient _client;

        static void Main(string[] args) => new Program().Run(args);

        public void Run(string[] args)
        {
            // Init the bot
            _client = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Verbose;
            });

            // --- Logging Set Up ---
            _client.Log.Message += (s, e) => Console.WriteLine($"[{e.Severity}] {e.Source}: {e.Message}");

            _client.ExecuteAndWait(async () =>
            {
                await _client.Connect(ConfigurationManager.AppSettings["token"], TokenType.Bot); // yeah yeah, I know, storing in plain text

                //if (!_client.Servers.Any())
                //    throw new Exception("This bot is not subbed to any servers");

                _client.UsingCommands(x =>
                {
                    x.PrefixChar = '~'; // All commands will be prefixed with '~'
                    x.HelpMode = HelpMode.Private;
                });

                _client.GetService<CommandService>().CreateCommand("simulate")
                .Alias("sim")
                .Description("Simulates a user's speech.")
                .Parameter("SimulatedUser", ParameterType.Required)
                .Do(async e =>
                    {
                        await e.Channel.SendMessage($"Learning {e.GetArg("SimulatedUser")}");

                    });
            });
        }
    }
}
