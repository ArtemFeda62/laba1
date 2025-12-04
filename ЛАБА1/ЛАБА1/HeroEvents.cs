using Model.Domain;
using System;

namespace Shared.Events
{
    public class HeroUpdatedEventArgs : EventArgs
    {
        public int HeroId { get; }
        public double NewHp { get; }

        public HeroUpdatedEventArgs(int heroId, double newHp)
        {
            HeroId = heroId;
            NewHp = newHp;
        }
    }

    public class HeroDeletedEventArgs : EventArgs
    {
        public int HeroId { get; }

        public HeroDeletedEventArgs(int heroId)
        {
            HeroId = heroId;
        }
    }

    public class HeroAddedEventArgs : EventArgs
    {
        public Hero Hero { get; }

        public HeroAddedEventArgs(Hero hero)
        {
            Hero = hero;
        }
    }
}