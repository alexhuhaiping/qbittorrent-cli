﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using QBittorrent.Client;

namespace QBittorrent.CommandLineInterface.Commands
{
    [Subcommand("torrent", typeof(TorrentCommand))]
    public partial class GetCommand
    {
        [Subcommand("properties", typeof(Properties))]
        public class TorrentCommand : ClientRootCommandBase
        {
            public class Properties : TorrentSpecificCommandBase
            {
                protected override async Task<int> OnExecuteTorrentSpecificAsync(QBittorrentClient client, CommandLineApplication app, IConsole console)
                {
                    var props = await client.GetTorrentPropertiesAsync(Hash);
                    console.PrintObject(props);
                    return 0;
                }
            }
        }
    }
}