﻿// Copyright 2007-2017 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.Transports.InMemory.Topology.Topologies
{
    using System.Text;
    using MassTransit.Topology;
    using MassTransit.Topology.Topologies;
    using NewIdFormatters;
    using Util;


    public class InMemoryHostTopology :
        HostTopology
    {
        static readonly INewIdFormatter _formatter = new ZBase32Formatter();
        readonly IInMemoryPublishTopology _publishTopology;

        public InMemoryHostTopology(IMessageTopology messageTopology, ISendTopology sendTopology, IInMemoryPublishTopology publishTopology)
            : base(messageTopology, sendTopology, publishTopology)
        {
            _publishTopology = publishTopology;
        }

        public override string CreateTemporaryQueueName(string prefix)
        {
            var sb = new StringBuilder(prefix);

            var host = HostMetadataCache.Host;

            foreach (var c in host.MachineName)
                if (char.IsLetterOrDigit(c))
                    sb.Append(c);
                else if (c == '.' || c == '_' || c == '-' || c == ':')
                    sb.Append(c);
            sb.Append('-');
            foreach (var c in host.ProcessName)
                if (char.IsLetterOrDigit(c))
                    sb.Append(c);
                else if (c == '.' || c == '_' || c == '-' || c == ':')
                    sb.Append(c);
            sb.Append('-');
            sb.Append(NewId.Next().ToString(_formatter));

            return sb.ToString();
        }

        public new IInMemoryMessagePublishTopology<T> Publish<T>() 
            where T : class
        {
            return _publishTopology.GetMessageTopology<T>();
        }
    }
}