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
        private ActorState state { get; set; }

        [ReadOnly(true)]
        public Task<int[]> GetGameBoardAsync()
        {            
            return Task.FromResult(state.Board);
        }

        [ReadOnly(true)]
        public Task<string> GetWinnerAsync()
        {
            return Task.FromResult(state.Winner);
        }

        public Task<bool> JoinGameAsync(long playerId, string playerName)
        {
            if (state.Players.Count >= 2 ||
                state.Players.FirstOrDefault(p => p.Item2 == playerName) != null)
            {
                return Task.FromResult(false);
            }

            state.Players.Add(new Tuple<long, string>(playerId, playerName));            
            return Task.FromResult(true); 
        }

        public Task<bool> MakeMoveAsync(long playerId, int x, int y)
        {
            if (x < 0 || x > 2 || y < 0 || y > 2
                || state.Players.Count != 2
                || state.NumberOfMoves >= 9
                || state.Winner != "")
                return Task.FromResult(false); ;
            int index = state.Players.FindIndex(p => p.Item1 == playerId);
            if (index == state.NextPlayerIndex)
            {
                if (state.Board[y * 3 + x] == 0)
                {
                    int piece = index * 2 - 1;
                    state.Board[y * 3 + x] = piece;
                    state.NumberOfMoves++;
                    if (HasWon(piece * 3))
                        state.Winner = state.Players[index].Item2 + " (" +
                        (piece == -1 ? "X" : "O") + ")";
                    else if (state.Winner == "" && state.NumberOfMoves >= 9)
                        state.Winner = "TIE";
                    state.NextPlayerIndex = (state.NextPlayerIndex + 1) % 2;
                    return Task.FromResult(true); 
                }
                else
                    return Task.FromResult(false); 
            }
            else
                return Task.FromResult(false);
        }

        private bool HasWon(int sum)
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


        protected override Task OnActivateAsync()
        {            
            if(state == null)
            {
                var astate = new ActorState
                {
                    Board = new int[9],
                    Winner = "",
                    Players = new List<Tuple<long, string>>(),
                    NextPlayerIndex = 0,
                    NumberOfMoves = 0
                };

                this.state = astate;
            }
            return Task.FromResult(true);
        }        
    }
}
