using Utility;

namespace EntityBase
{
// Option 2: Create an AttackHandler component class
    public class AttackHandler
    {
        private int numberOfAttacks = 2;
        private int currentAttackCounter = 0;
        private Timer attackCounterResetTimer;
    
        public int CurrentAttackCounter => currentAttackCounter;
    
        public AttackHandler(int numberOfAttacks = 2, float resetTime = 0.5f)
        {
            this.numberOfAttacks = numberOfAttacks;
            attackCounterResetTimer = new Timer(resetTime);
            attackCounterResetTimer.OnTimerEnded += ResetAttackCounter;
        }
    
        public void IncrementCounter()
        {
            currentAttackCounter++;
            if (currentAttackCounter >= numberOfAttacks)
            {
                currentAttackCounter = 0;
            }
            attackCounterResetTimer.StartTimer();
        }
    
        public void Tick(float deltaTime)
        {
            attackCounterResetTimer.Tick(deltaTime);
        }
    
        private void ResetAttackCounter()
        {
            currentAttackCounter = 0;
        }
    
        public void Dispose()
        {
            attackCounterResetTimer.OnTimerEnded -= ResetAttackCounter;
        }
    }
}