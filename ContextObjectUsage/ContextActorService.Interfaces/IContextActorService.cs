﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using COFoundation;
using ContextService;

namespace ContextActorService.Interfaces
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IContextActorService : IActor
    {
        Task<string> ConsumeContext(MessageCarrier<HelloMessage> msg);
    }

    
}