﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using QBittorrent.Client;

namespace QBittorrent.CommandLineInterface.Commands
{
    [Command("torrent")]
    [Subcommand("properties", typeof(Properties))]
    [Subcommand("content", typeof(Content))]
    [Subcommand("pause", typeof(Pause))]
    [Subcommand("resume", typeof(Resume))]
    [Subcommand("delete", typeof(Delete))]
    [Subcommand("move", typeof(Move))]
    [Subcommand("rename", typeof(Rename))]
    public partial class TorrentCommand : ClientRootCommandBase
    {
        [Command(Description = "Shows the torrent properties.")]
        public class Properties : TorrentSpecificCommandBase
        {
            protected override async Task<int> OnExecuteTorrentSpecificAsync(QBittorrentClient client, CommandLineApplication app, IConsole console)
            {
                var props = await client.GetTorrentPropertiesAsync(Hash);
                console.PrintObject(props);
                return ExitCodes.Success;
            }
        }

        [Command(Description = "Shows the torrent content.")]
        public class Content : TorrentSpecificCommandBase
        {
            protected override async Task<int> OnExecuteTorrentSpecificAsync(QBittorrentClient client, CommandLineApplication app, IConsole console)
            {
                var contents = await client.GetTorrentContentsAsync(Hash);
                foreach (var content in contents)
                {
                    console.PrintObject(content);
                    console.WriteLine(string.Empty);
                }
                return ExitCodes.Success;
            }
        }

        [Command(Description = "Pauses the specified torrent or all torrents.")]
        public class Pause : TorrentSpecificCommandBase
        {
            protected override bool AllowAll => true;

            [Argument(0, "<HASH|ALL>", "Full or partial torrent hash, or keyword ALL to pause all torrents.")]
            public override string Hash { get; set; }

            protected override async Task<int> OnExecuteTorrentSpecificAsync(QBittorrentClient client, CommandLineApplication app, IConsole console)
            {
                await (IsAll ? client.PauseAllAsync() : client.PauseAsync(Hash));
                return ExitCodes.Success;
            }
        }

        [Command(Description = "Resumes the specified torrent or all torrents.")]
        public class Resume : TorrentSpecificCommandBase
        {
            protected override bool AllowAll => true;

            [Argument(0, "<HASH|ALL>", "Full or partial torrent hash, or keyword ALL to resume all torrents.")]
            public override string Hash { get; set; }

            protected override async Task<int> OnExecuteTorrentSpecificAsync(QBittorrentClient client, CommandLineApplication app, IConsole console)
            {
                await (IsAll ? client.ResumeAllAsync() : client.ResumeAsync(Hash));
                return ExitCodes.Success;
            }
        }

        [Command(Description = "Deletes the torrent.")]
        public class Delete : TorrentSpecificCommandBase
        {
            [Option("-f|--delete-files|--with-files", "Delete downloaded files", CommandOptionType.NoValue)]
            public bool DeleteFiles { get; set; }

            protected override async Task<int> OnExecuteTorrentSpecificAsync(QBittorrentClient client, CommandLineApplication app, IConsole console)
            {
                await client.DeleteAsync(Hash, DeleteFiles);
                return ExitCodes.Success;
            }
        }

        [Command(Description = "Moves the downloaded files to the other folder.")]
        public class Move : TorrentSpecificCommandBase
        {
            [Argument(1, "<FOLDER>", "The folder to move the torrent to.")]
            [Required]
            public string Folder { get; set; }

            protected override async Task<int> OnExecuteTorrentSpecificAsync(QBittorrentClient client, CommandLineApplication app, IConsole console)
            {
                await client.SetLocationAsyc(Hash, Folder);
                return ExitCodes.Success;
            }
        }

        [Command(Description = "Renames the torrent.")]
        public class Rename : TorrentSpecificCommandBase
        {
            [Argument(1, "<NAME>", "The new torrent name.")]
            [Required]
            public string Name { get; set; }

            protected override async Task<int> OnExecuteTorrentSpecificAsync(QBittorrentClient client, CommandLineApplication app, IConsole console)
            {
                await client.RenameAsync(Hash, Name);
                return ExitCodes.Success;
            }
        }
    }
}