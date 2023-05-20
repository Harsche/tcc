using MonsterLove.StateMachine;

namespace Game.AI{
    public class EnemyDriver{
        public StateEvent<bool> OnCheckPlayer;
        public StateEvent Update;
    }
}