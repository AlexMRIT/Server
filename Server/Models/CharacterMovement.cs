using System;
using Server.Utilite;
using System.Threading.Tasks;
using Server.Network.InnerNetwork;
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
        private long MovementUpdateTime;
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

                NotifyStopMove();
                return;
            }

            MovementLastTime = currentTime;
            LocalWorkPosition += vectorDistance * (float)Character.CharacterStats.MoveSpeed * elapsedSeconds;
        }

        public void UpdatePosition()
        {
            PerformMove();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdatePositionDirection(Vector3 direction)
        {
            if (!IsMoving)
                return;

            bool slowDown = Vector3.DistanceSq(direction.x, direction.y, direction.z, DestinationPosition.x, DestinationPosition.y, DestinationPosition.z) >
                Vector3.DistanceSq(LocalWorkPosition.x, LocalWorkPosition.y, LocalWorkPosition.z, DestinationPosition.x, DestinationPosition.y, DestinationPosition.z);

            Vector3 distanceVectorOfAxis = direction - LocalWorkPosition;
            float distance = Vector3.Distance2D(distanceVectorOfAxis);
            long currentTime = DateTime.UtcNow.Ticks;

            int distanceAllowedUnsync = (int)((slowDown ? Character.CharacterStats.MoveSpeed : Character.ServerConfig.MaxSpeedUpPerSecondUnsync)
                * (currentTime - MovementUpdateTime) / TimeSpan.TicksPerSecond);

            if (distance < distanceAllowedUnsync)
                LocalWorkPosition = distanceVectorOfAxis;
            else
                LocalWorkPosition += distanceVectorOfAxis / distance * distanceAllowedUnsync;

            MovementUpdateTime = currentTime;
        }

        public async Task MoveTo(Vector3 direction)
        {
            if (IsMoving)
                UpdatePosition();

            Vector3 distanceVectorOfAxis = direction - LocalWorkPosition;
            if (Math.Pow(distanceVectorOfAxis.x, 2) + Math.Pow(distanceVectorOfAxis.y, 2) > 9900 * 9900)
                return;

            DestinationPosition = direction;

            IsMoving = true;
            MovementUpdateTime = MovementLastTime = DateTime.UtcNow.Ticks;

            await Character.BroadcastPacketAsync(CharacterMoveToLocation.ToPacket(Character.Template.Id, Position));
            Task.Factory.StartNew(BroadcastDestinationChanged);
        }

        private async void BroadcastDestinationChanged()
        {
            Vector3 oldDirection = DestinationPosition;

            await Task.Delay(Character.ServerConfig.DelayMillisecondsUpdatingCharacterPositions);
            while (IsMoving)
            {
                if (oldDirection != DestinationPosition)
                    await Character.BroadcastPacketAsync(CharacterMoveToLocation.ToPacket(Character.Template.Id, Position));

                await Task.Delay(Character.ServerConfig.DelayMillisecondsUpdatingCharacterPositions);
            }
        }

        public async void NotifyStopMoveByPosition(Vector3 vector)
        {
            LocalWorkPosition = vector;
            DestinationPosition = vector;

            await NotifyStopMove(broadcast: true, excludeYourself: false);
        }

        public async Task NotifyStopMove(bool broadcast = true, bool excludeYourself = true)
        {
            IsMoving = false;

            if (broadcast)
                await Character.BroadcastPacketAsync(StopMove.ToPacket(Character.Template.Id, DestinationPosition), excludeYourself);
        }
    }
}