using System;
using Server.Utilite;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

#pragma warning disable CS4014

namespace Server.Models
{
    public sealed class CharacterMovement
    {
        public readonly CharacterEntity Character;
        public bool IsMoving { get; private set; }

        public Vector3 Position
        {
            get
            {
                PerformMove();
                return LocalWorkPosition;
            }
            set
            {
                if (IsMoving)
                    NotifyStopMove();

                LocalWorkPosition = value;
            }
        }

        private Vector3 LocalWorkPosition;
        private Vector3 DestinationPosition;
        private long MovementLastTime;

        public CharacterMovement(CharacterEntity character)
        {
            Character = character;
            Position = character.Template.Position;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void PerformMove()
        {
            if (!IsMoving)
                return;

            long currentTime = DateTime.UtcNow.Ticks;
            float elapsedSeconds = (currentTime - MovementLastTime) / (float)TimeSpan.TicksPerSecond;

            if (elapsedSeconds < 0.05f) // 50 ms, skip run if last run was less then 50ms ago
                return;

            float distance = Vector3.Distance(DestinationPosition, LocalWorkPosition);

            Vector3 vectorDistance = new Vector3(
                (DestinationPosition.x - LocalWorkPosition.x) / distance,
                (DestinationPosition.y - LocalWorkPosition.y) / distance,
                (DestinationPosition.z - LocalWorkPosition.z) / distance);

            vectorDistance *= (float)Character.CharacterStats.MoveSpeed * elapsedSeconds;
            float ddistance = Vector3.Distance2D(vectorDistance.x, vectorDistance.y, vectorDistance.z);

            if (ddistance >= distance || distance < 1)
            {
                LocalWorkPosition = DestinationPosition;

                NotifyArrived();
                return;
            }

            MovementLastTime = currentTime;
            LocalWorkPosition += vectorDistance * (float)Character.CharacterStats.MoveSpeed * elapsedSeconds;
        }

        public async Task NotifyStopMove()
        {
            NotifyArrived();

        }

        public void NotifyArrived()
        {
            IsMoving = false;
        }
    }
}