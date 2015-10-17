// Copyright © 2015 Daniel Porrey
//
// This file is part of the iWindow solution.
// 
// iWindow is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// iWindow is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with iWindow. If not, see http://www.gnu.org/licenses/.
//
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using Porrey.iWindow.Event.Models;
using Porrey.iWindow.Interfaces;

namespace Porrey.iWindow.Services
{
	/// <summary>
	/// Provides a base class for all background services.
	/// </summary>
	public abstract class BackgroundService : IBackgroundService
	{
		private Dictionary<EventBase, SubscriptionToken> _tokens = new Dictionary<EventBase, SubscriptionToken>();

		public virtual string Name => this.OnGetName();

		[Dependency]
		protected virtual IEventAggregator EventAggregator { get; set; }

		[Dependency]
		protected virtual IApplicationSettingsRepository ApplicationSettngs { get; set; }

		protected Dictionary<EventBase, SubscriptionToken> Tokens
		{
			get
			{
				return _tokens;
			}
		}

		public async virtual Task<bool> Start()
		{
			bool returnValue = false;

			try
			{
				returnValue = await this.OnStart();
			}
			catch (Exception ex)
			{
				this.EventAggregator.GetEvent<Events.DebugEvent>().Publish(new DebugEventArgs(ex));
				returnValue = false;
			}

			return returnValue;
		}

		public async virtual Task<bool> Stop()
		{
			bool returnValue = false;

			try
			{
				returnValue = await this.OnStop();
			}
			catch (Exception ex)
			{
				this.EventAggregator.GetEvent<Events.DebugEvent>().Publish(new DebugEventArgs(ex));
				returnValue = false;
			}
			finally
			{
				// ***
				// *** Unsubscribe all events
				// ***
				foreach (KeyValuePair<EventBase, SubscriptionToken> item in this.Tokens)
				{
					item.Key.Unsubscribe(item.Value);
					item.Value.Dispose();
                }
			}

			return returnValue;
		}

		protected virtual string OnGetName()
		{
			return "Background Service";
		}

		protected virtual Task<bool> OnStart()
		{
			return Task<bool>.FromResult(false);
		}

		protected virtual Task<bool> OnStop()
		{
			return Task<bool>.FromResult(false);
		}

		protected virtual void AddSubscription(EventBase eventBase, SubscriptionToken token)
		{
			this.Tokens.Add(eventBase, token);
		}

		protected virtual void PublishException(Exception ex)
		{
			this.EventAggregator.GetEvent<Events.DebugEvent>().Publish(new DebugEventArgs(ex));
		}
	}
}
