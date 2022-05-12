using System;
using Server.World;
using Server.Enums;
using Server.Network;
using Server.Template;
using System.Threading.Tasks;
using Server.Models.CharacterFuncStats;
using Microsoft.Extensions.DependencyInjection;

namespace Server.Models
{
    public sealed class CharacterEntity : Entity
    {
        public readonly Config ServerConfig;
        public readonly CharacterTemplate CharacterTemplate;
        public readonly CharacterStats CharacterStats;
        public readonly CharacterMovement CharacterMovement;
        public readonly ClientProcessor ClientStream;

        public bool Online { get; set; }
        public bool IsDead { get; set; }

        public CharacterEntity(IServiceProvider serviceProvider, CharacterTemplate characterTemplate, ClientProcessor client)
            : base(serviceProvider.GetService<ThreadsRoom>(), characterTemplate)
        {
            ServerConfig = serviceProvider.GetService<Config>();
            CharacterTemplate = characterTemplate;
            CharacterStats = new CharacterStats(this, characterTemplate.BaseSpecification);
            CharacterMovement = new CharacterMovement(this);
            ClientStream = client;

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

        public void SetOffline()
        {
            Online = false;
        }

        public override async Task<DamageResult> TakeDamage(CharacterEntity target)
        {
            if (target == null)
                return DamageResult.DamageFail;

            if (target.IsDead)
                return DamageResult.DamageFail;

            throw new NotImplementedException();
        }

        public override async Task BroadcastPacketAsync(NetworkPacket packet)
        {
            await base.BroadcastPacketAsync(packet);
        }

        public async Task BroadcastPacketOnlyMeAsync(NetworkPacket packet)
        {
            await ClientStream.WriteAsync(packet);
        }
    }
}