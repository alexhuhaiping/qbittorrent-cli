﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using QBittorrent.Client;

namespace QBittorrent.CommandLineInterface.Commands
{
    [Subcommand("add", typeof(Add))]
    public partial class TorrentCommand
    {
        [Command(Description = "Adds new torrents.")]
        [Subcommand("file", typeof(AddFile))]
        [Subcommand("url", typeof(AddUrl))]
        public class Add : ClientRootCommandBase
        {
            public abstract class Base : AuthenticatedCommandBase
            {
                [Option("-f|--folder <PATH>", "Download folder.", CommandOptionType.SingleValue)]
                public string Folder { get; set; }

                [Option("--cookie <COOKIE>", "Cookie sent to download the .torrent file.", CommandOptionType.SingleValue)]
                public string Cookie { get; set; }

                [Option("-c|--category <CATEGORY>", "Category for the torrent.", CommandOptionType.SingleValue)]
                public string Category { get; set; }

                [Option("-p|--paused", "Add torrents in the paused state.", CommandOptionType.NoValue)]
                public bool Paused { get; set; }

                [Option("--no-check", "Skip hash checking.", CommandOptionType.NoValue)]
                public bool SkipChecking { get; set; }

                [Option("--create-root-folder <BOOL>", "Create root folder (true/false).", CommandOptionType.SingleValue)]
                public bool? CreateRootFolder { get; set; }

                [Option("-r|--rename <NEW_NAME>", "Rename torrent", CommandOptionType.SingleValue)]
                public string Rename { get; set; }

                [Option("-u|--upload-limit <LIMIT>", "Set torrent upload speed limit (bytes/second).", CommandOptionType.SingleValue)]
                public int? UploadLimit { get; set; }

                [Option("-d|--download-limit <LIMIT>", "Set torrent upload speed limit (bytes/second).", CommandOptionType.SingleValue)]
                public int? DownloadLimit { get; set; }

                [Option("-s|--sequential", "Enable sequential download.", CommandOptionType.NoValue)]
                public bool SequentialDownload { get; set; }

                [Option("--first-last-prio", "Prioritize download of the first and the last pieces.", CommandOptionType.SingleValue)]
                public bool FirstLastPiecePrioritized { get; set; }
            }

            [Command(Description = "Adds new torrents from torrent files.")]
            public class AddFile : Base
            {
                [Argument(0, "<file1 file2 ... fileN>", "The list of files.")]
                [Required]
                public List<string> Files { get; set; }

                protected override async Task<int> OnExecuteAuthenticatedAsync(QBittorrentClient client, CommandLineApplication app, IConsole console)
                {
                    var request = new AddTorrentFilesRequest(Files)
                    {
                        Category = Category,
                        Cookie = Cookie,
                        CreateRootFolder = CreateRootFolder,
                        DownloadFolder = Folder,
                        DownloadLimit = DownloadLimit,
                        FirstLastPiecePrioritized = FirstLastPiecePrioritized,
                        Paused = Paused,
                        Rename = Rename,
                        SequentialDownload = SequentialDownload,
                        SkipHashChecking = SkipChecking,
                        UploadLimit = UploadLimit
                    };
                    await client.AddTorrentsAsync(request);
                    return ExitCodes.Success;
                }
            }

            [Command(Description = "Adds new torrents by URLS.")]
            public class AddUrl : Base
            {
                [Argument(0, "<URL_1 URL_2 ... URL_N>", "The list of URLS.")]
                [Required]
                public List<string> Urls { get; set; }

                protected override async Task<int> OnExecuteAuthenticatedAsync(QBittorrentClient client, CommandLineApplication app, IConsole console)
                {
                    var urls = Urls.Select(x => new Uri(x, UriKind.Absolute)).ToList();
                    var request = new AddTorrentUrlsRequest(urls)
                    {
                        Category = Category,
                        Cookie = Cookie,
                        CreateRootFolder = CreateRootFolder,
                        DownloadFolder = Folder,
                        DownloadLimit = DownloadLimit,
                        FirstLastPiecePrioritized = FirstLastPiecePrioritized,
                        Paused = Paused,
                        Rename = Rename,
                        SequentialDownload = SequentialDownload,
                        SkipHashChecking = SkipChecking,
                        UploadLimit = UploadLimit
                    };
                    await client.AddTorrentsAsync(request);
                    return ExitCodes.Success;
                }
            }
        }
    }
}
