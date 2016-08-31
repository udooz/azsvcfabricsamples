using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors.Runtime;
using Game.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace Game
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class Game : Actor, IGame
    {

        [ReadOnly(true)]
        public async Task<int[]> GetGameBoardAsync()
        {
            var state = await this.StateManager.TryGetStateAsync<ActorState>("gamestate");
            return state.Value.Board;
        }

        [ReadOnly(true)]
        public async Task<string> GetWinnerAsync()
        {
            var state = await this.StateManager.TryGetStateAsync<ActorState>("gamestate");
            return state.Value.Winner;
        }

        public async Task<bool> JoinGameAsync(long playerId, string playerName)
        {
            var state = await this.StateManager.TryGetStateAsync<ActorState>("gamestate");            
            var thisState = state.Value;
            if (thisState.Players.Count >= 2 ||
                thisState.Players.FirstOrDefault(p => p.Item2 == playerName) != null)
            {
                return false;
            }

            thisState.Players.Add(new Tuple<long, string>(playerId, playerName));
            await this.StateManager.AddOrUpdateStateAsync<ActorState>("gamestate", thisState, (k, v) => thisState);
            return true;
        }

        public async Task<bool> MakeMoveAsync(long playerId, int x, int y)
        {
            var stateBox = await this.StateManager.TryGetStateAsync<ActorState>("gamestate");
            var state = stateBox.Value;

            if (x < 0 || x > 2 || y < 0 || y > 2
                || state.Players.Count != 2
                || state.NumberOfMoves >= 9
                || state.Winner != "")
                return false;
            int index = state.Players.FindIndex(p => p.Item1 == playerId);
            if (index == state.NextPlayerIndex)
            {
                if (state.Board[y * 3 + x] == 0)
                {
                    int piece = index * 2 - 1;
                    state.Board[y * 3 + x] = piece;
                    state.NumberOfMoves++;
                    if (HasWon(state, piece * 3))
                        state.Winner = state.Players[index].Item2 + " (" +
                        (piece == -1 ? "X" : "O") + ")";
                    else if (state.Winner == "" && state.NumberOfMoves >= 9)
                        state.Winner = "TIE";
                    state.NextPlayerIndex = (state.NextPlayerIndex + 1) % 2;
                    await this.StateManager.AddOrUpdateStateAsync<ActorState>("gamestate", state, (k, v) => state);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        private bool HasWon(ActorState state, int sum)
        {
            return state.Board[0] + state.Board[1] + state.Board[2] == sum
            || state.Board[3] + state.Board[4] + state.Board[5] == sum
            || state.Board[6] + state.Board[7] + state.Board[8] == sum
            || state.Board[0] + state.Board[3] + state.Board[6] == sum
            || state.Board[1] + state.Board[4] + state.Board[7] == sum
            || state.Board[2] + state.Board[5] + state.Board[8] == sum
            || state.Board[0] + state.Board[4] + state.Board[8] == sum
            || state.Board[2] + state.Board[4] + state.Board[6] == sum;
        }


        protected async override Task OnActivateAsync()
        {
            var state = await this.StateManager.TryGetStateAsync<ActorState>("gamestate");
            if(!state.HasValue)
            {
                var astate = new ActorState
                {
                    Board = new int[9],
                    Winner = "",
                    Players = new List<Tuple<long, string>>(),
                    NextPlayerIndex = 0,
                    NumberOfMoves = 0
                };
                await this.StateManager.AddOrUpdateStateAsync<ActorState>("gamestate", astate, (k, v) => astate);
            }
            return; //this.StateManager.GetStateAsync<ActorState>("gamestate");
        }        
    }
}
