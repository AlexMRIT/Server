using System;
using Server.World;
using Server.Enums;
using Server.Events;
using Server.Network;
using Server.Utilite;
using Server.Service;
using Server.Template;
using System.Threading.Tasks;
using Server.Service.Interface;
using Server.Network.InnerNetwork;
using Server.Models.CharacterFuncStats;
using Microsoft.Extensions.DependencyInjection;

#pragma warning disable CS4014

namespace Server.Models
{
    public sealed class CharacterEntity : Entity, IManagerInstance
    {
        public readonly Config ServerConfig;
        public readonly CharacterTemplate CharacterTemplate;
        public readonly CharacterStats CharacterStats;
        public readonly CharacterMovement CharacterMovement;
        public readonly ClientProcessor ClientStream;
        public readonly EventCharacter EventsHandler;

        private readonly ICharacterServices CharacterService;

        public bool Online { get; set; }
        public bool IsDead { get; set; }
        public int Score { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }

        public CharacterEntity(IServiceProvider serviceProvider, CharacterTemplate characterTemplate, ClientProcessor client)
            : base(serviceProvider.GetService<ThreadsRoom>(), characterTemplate)
        {
            ServerConfig = serviceProvider.GetService<Config>();
            CharacterService = serviceProvider.GetService<ICharacterServices>();

            CharacterTemplate = characterTemplate;
            CharacterStats = new CharacterStats(this, characterTemplate.BaseSpecification);
            CharacterMovement = new CharacterMovement(this);
            ClientStream = client;
            EventsHandler = new EventCharacter(this);

            InitializeCharacterStats();
        }

        public void InitializeCharacterStats()
        {
            CharacterStats.AddStatFunction(FuncAttackSpeed.Instance);
            CharacterStats.AddStatFunction(FuncCriticalDamage.Instance);
            CharacterStats.AddStatFunction(FuncDamageCritical.Instance);
            CharacterStats.AddStatFunction(FuncHealthPoint.Instance);
            CharacterStats.AddStatFunction(FuncMiss.Instance);
            CharacterStats.AddStatFunction(FuncMoveSpeed.Instance);
            CharacterStats.AddStatFunction(FuncPhysicsAttack.Instance);
            CharacterStats.AddStatFunction(FuncPhysicsDefence.Instance);
        }

        public override async Task<DamageResult> TakeDamageAsync(CharacterEntity target)
        {
            if (target == null)
                return DamageResult.DamageFail;

            if (target.IsDead)
                return DamageResult.DamageFail;

            Formulas.CalculateTakeDamage(CharacterStats, target.CharacterStats);
            bool isDeath = EventsHandler.ValidDeathTarget(target, async () =>
            {
                await target.BroadcastPacketAsync(CharacterDeath.ToPacket());
            });

            if (isDeath)
            {
                await BroadcastPacketAsync(ExitAllRoomCmd.ToPacket());
                return DamageResult.DamageFail;
            }

            StatusUpdate status = new StatusUpdate();
            status.AddAttribute(StatusUpdate.CurrentHealth, target.CharacterStats.CurrentHealth);
            status.AddAttribute(StatusUpdate.MaxHealth, target.CharacterStats.BaseHealth);

            await target.BroadcastPacketAsync(StatusUpdate.ToPacket(status, target));

            return DamageResult.DamageNormal;
        }

        public async Task SendOnlyMe(NetworkPacket packet)
        {
            await ClientStream.WriteAsync(packet);
        }

        public override async Task BroadcastPacketAsync(NetworkPacket packet, bool excludeYourself = true)
        {
            await base.BroadcastPacketAsync(packet, excludeYourself);
        }

        public async void SetOnline(int roomId)
        {
            Online = true;
            RoomId = roomId;
            Rooms.AddEntity(roomId, this);
            await BroadcastPacketAsync(AddMe.ToPacket(this), excludeYourself: false);

            foreach (Entity character in Rooms.GetEntitiesByRoomId(roomId))
                if (character != this)
                    await SendOnlyMe(AddMe.ToPacket(character as CharacterEntity));
        }

        public async void SetOffline()
        {
            Online = false;

            if (CharacterMovement.IsMoving)
            {
                CharacterMovement.UpdatePosition();
                CharacterMovement.NotifyStopMove(excludeYourself: false);
            }

            CharacterService.UpdateCharacterAsync(this.ToContract());
            ClientStream.CurrentSession.SessionClientGamePlaying = false;
            await BroadcastPacketAsync(DeleteMe.ToPacket(this, ClientStream.CurrentSession), excludeYourself: false);
            Rooms.RemoveEntity(RoomId, this);
        }
    }
}