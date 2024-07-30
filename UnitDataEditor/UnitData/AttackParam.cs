using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitDataEditor.UnitData;

public class AttackData
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string JpName { get; set; }
    
    public float Damage { get; set; }
    public string CpuId0 { get; set; }

    public int HitPriority { get; set; }
    public float MeterGainPlayer { get; set; }
    public float MeterGainBlock { get; set; }
    public float MeterGainHit { get; set; }
    public int UnkInt0 { get; set; }
    
    public int SelfHitstop { get; set; }
    public int CollisionDisableTimer { get; set; }
    public int Hitstop { get; set; }
    public int HitExit { get; set; }

    public int UnkInt1 { get; set; }
    public int UnkInt2 { get; set; }

    public int ComboLimitAdd { get; set; }
    public int AttackFlags { get; set; }

    public string GroundKnockbackId { get; set; }
    public string AirKnockbackId { get; set; }
    public string BlockKnockbackId { get; set; }
    public string UnkKnockbackId0 { get; set; }
    public string UnkKnockbackId1 { get; set; }
    public string SpecialKnockbackId { get; set; }
    public string GroundClashKnockbackId { get; set; }
    public string AirClashKnockbackId { get; set; }
    public string CounterHitId { get; set; }
    public string HitSe { get; set; }

    public float UnkFloat3 { get; set; }
    public float UnkFloat4 { get; set; }
    
    public string CpuId1 { get; set; }
    public string CpuId2 { get; set; }
    public string CpuId3 { get; set; }

    public int UnkInt3 { get; set; }
    public int UnkInt4 { get; set; }
    public int UnkInt5 { get; set; }
    public int UnkInt6 { get; set; }
    public int UnkInt7 { get; set; }
    public int UnkInt8 { get; set; }
    public int UnkInt9 { get; set; }
    public int UnkInt10 { get; set; }
    public int UnkInt11 { get; set; }
    public int UnkInt12 { get; set; }
    public int UnkInt13 { get; set; }
    public int UnkInt14 { get; set; }
    public int UnkInt15 { get; set; }
    public int UnkInt16 { get; set; }
    public int UnkInt17 { get; set; }
    public int UnkInt18 { get; set; }
    public int UnkInt19 { get; set; }
    public int UnkInt20 { get; set; }
    public int UnkInt21 { get; set; }
    public int UnkInt22 { get; set; }
    public int UnkInt23 { get; set; }
    public float UnkFloat5 { get; set; }
    public int UnkInt24 { get; set; }
    public int UnkInt25 { get; set; }
    public string UnkName { get; set; }
    public int UnkInt26 { get; set; }
}

public class AttackParam : IUnitData
{
    private const uint Magic = 0x4C535455;

    public uint Version { get; private set; }
    public List<AttackData>? AttackDatas { get; set; }
    
    public int Parse(byte[] buffer)
    {
        if (Magic != BitConverter.ToUInt32(buffer, 0)) return 1;
        Version = BitConverter.ToUInt32(buffer, 4);
        var attackCount = BitConverter.ToInt32(buffer, 8);

        AttackDatas = [];

        var attackStart = 0x10;
        
        for (var i = 0; i < attackCount; i++)
        {
            var nameOffset = BitConverter.ToInt32(buffer, attackStart + 0x8);
            var nullTermName = nameOffset;

            while (nullTermName + 2 < buffer.Length && (buffer[nullTermName] != 0 || buffer[nullTermName + 1] != 0))
            {
                nullTermName += 2;
            }
            
            var jpNameOffset = BitConverter.ToInt32(buffer, attackStart + 0xC);
            var nullTermJpName = jpNameOffset;

            while (nullTermJpName + 2 < buffer.Length && (buffer[nullTermJpName] != 0 || buffer[nullTermJpName + 1] != 0))
            {
                nullTermJpName += 2;
            }
            
            var hitSeOffset = BitConverter.ToInt32(buffer, attackStart + 0x98);
            var nullTermHitSe = hitSeOffset;

            while (nullTermHitSe + 1 < buffer.Length && buffer[nullTermHitSe] != 0)
            {
                nullTermHitSe += 1;
            }
            
            var unkNameOffset = BitConverter.ToInt32(buffer, attackStart + 0x11C);
            var nullTermUnkName = unkNameOffset;

            while (nullTermUnkName + 2 < buffer.Length && (buffer[nullTermUnkName] != 0 || buffer[nullTermUnkName + 1] != 0))
            {
                nullTermUnkName += 2;
            }
            
            var attackData = new AttackData
            {
                Id = Encoding.UTF8.GetString(buffer, attackStart, 8),
                Name = nameOffset != 0 ? Encoding.Unicode.GetString(buffer, nameOffset, nullTermName - nameOffset) : "",
                JpName = jpNameOffset != 0 ? Encoding.Unicode.GetString(buffer, jpNameOffset, nullTermJpName - jpNameOffset) : "",
                Damage = BitConverter.ToSingle(buffer, attackStart + 0x10),
                CpuId0 = Encoding.UTF8.GetString(buffer, attackStart + 0x14, 8),
                HitPriority = BitConverter.ToInt32(buffer, attackStart + 0x1C),
                MeterGainPlayer = BitConverter.ToSingle(buffer, attackStart + 0x20),
                MeterGainBlock = BitConverter.ToSingle(buffer, attackStart + 0x24),
                MeterGainHit = BitConverter.ToSingle(buffer, attackStart + 0x28),
                UnkInt0 = BitConverter.ToInt32(buffer, attackStart + 0x2C),
                SelfHitstop = BitConverter.ToInt32(buffer, attackStart + 0x30),
                CollisionDisableTimer = BitConverter.ToInt32(buffer, attackStart + 0x34),
                Hitstop = BitConverter.ToInt32(buffer, attackStart + 0x38),
                HitExit = BitConverter.ToInt32(buffer, attackStart + 0x3C),
                UnkInt1 = BitConverter.ToInt32(buffer, attackStart + 0x40),
                UnkInt2 = BitConverter.ToInt32(buffer, attackStart + 0x44),
                ComboLimitAdd = BitConverter.ToInt32(buffer, attackStart + 0x48),
                AttackFlags = BitConverter.ToInt32(buffer, attackStart + 0x4C),
                GroundKnockbackId = Encoding.UTF8.GetString(buffer, attackStart + 0x50, 8),
                AirKnockbackId = Encoding.UTF8.GetString(buffer, attackStart + 0x58, 8),
                BlockKnockbackId = Encoding.UTF8.GetString(buffer, attackStart + 0x60, 8),
                UnkKnockbackId0 = Encoding.UTF8.GetString(buffer, attackStart + 0x68, 8),
                UnkKnockbackId1 = Encoding.UTF8.GetString(buffer, attackStart + 0x70, 8),
                SpecialKnockbackId = Encoding.UTF8.GetString(buffer, attackStart + 0x78, 8),
                GroundClashKnockbackId = Encoding.UTF8.GetString(buffer, attackStart + 0x80, 8),
                AirClashKnockbackId = Encoding.UTF8.GetString(buffer, attackStart + 0x88, 8),
                CounterHitId = Encoding.UTF8.GetString(buffer, attackStart + 0x90, 8),
                HitSe = hitSeOffset != 0 ? Encoding.UTF8.GetString(buffer, hitSeOffset, nullTermHitSe - hitSeOffset) : "",
                UnkFloat3 = BitConverter.ToSingle(buffer, attackStart + 0x9C),
                UnkFloat4 = BitConverter.ToSingle(buffer, attackStart + 0xA0),
                CpuId1 = Encoding.UTF8.GetString(buffer, attackStart + 0xA4, 8),
                CpuId2 = Encoding.UTF8.GetString(buffer, attackStart + 0xAC, 8),
                CpuId3 = Encoding.UTF8.GetString(buffer, attackStart + 0xB4, 8),
                UnkInt3 = BitConverter.ToInt32(buffer, attackStart + 0xBC),
                UnkInt4 = BitConverter.ToInt32(buffer, attackStart + 0xC0),
                UnkInt5 = BitConverter.ToInt32(buffer, attackStart + 0xC4),
                UnkInt6 = BitConverter.ToInt32(buffer, attackStart + 0xC8),
                UnkInt7 = BitConverter.ToInt32(buffer, attackStart + 0xCC),
                UnkInt8 = BitConverter.ToInt32(buffer, attackStart + 0xD0),
                UnkInt9 = BitConverter.ToInt32(buffer, attackStart + 0xD4),
                UnkInt10 = BitConverter.ToInt32(buffer, attackStart + 0xD8),
                UnkInt11 = BitConverter.ToInt32(buffer, attackStart + 0xDC),
                UnkInt12 = BitConverter.ToInt32(buffer, attackStart + 0xE0),
                UnkInt13 = BitConverter.ToInt32(buffer, attackStart + 0xE4),
                UnkInt14 = BitConverter.ToInt32(buffer, attackStart + 0xE8),
                UnkInt15 = BitConverter.ToInt32(buffer, attackStart + 0xEC),
                UnkInt16 = BitConverter.ToInt32(buffer, attackStart + 0xF0),
                UnkInt17 = BitConverter.ToInt32(buffer, attackStart + 0xF4),
                UnkInt18 = BitConverter.ToInt32(buffer, attackStart + 0xF8),
                UnkInt19 = BitConverter.ToInt32(buffer, attackStart + 0xFC),
                UnkInt20 = BitConverter.ToInt32(buffer, attackStart + 0x100),
                UnkInt21 = BitConverter.ToInt32(buffer, attackStart + 0x104),
                UnkInt22 = BitConverter.ToInt32(buffer, attackStart + 0x108),
                UnkInt23 = BitConverter.ToInt32(buffer, attackStart + 0x10C),
                UnkFloat5 = BitConverter.ToSingle(buffer, attackStart + 0x110),
                UnkInt24 = BitConverter.ToInt32(buffer, attackStart + 0x114),
                UnkInt25 = BitConverter.ToInt32(buffer, attackStart + 0x118),
                UnkName = unkNameOffset != 0 ? Encoding.Unicode.GetString(buffer, unkNameOffset, nullTermUnkName - unkNameOffset) : "",
                UnkInt26 = BitConverter.ToInt32(buffer, attackStart + 0x120),
            };
            
            AttackDatas.Add(attackData);
            attackStart += 292;
        }
        
        return 0;
    }

    private void InsertDataString(string value, List<byte> bufMain, List<byte> bufData)
    {
        if (value.Length > 0)
        {
            bufMain.AddRange(BitConverter.GetBytes(bufData.Count + 0x10 + AttackDatas.Count * 292).ToList());
            bufData.AddRange(Encoding.Unicode.GetBytes(value));
            bufData.Add(0);
            bufData.Add(0);
        }
        else
        {
            bufMain.AddRange(BitConverter.GetBytes(0).ToList());
        }
    }

    private void InsertDataStringUtf8(string value, List<byte> bufMain, List<byte> bufData)
    {
        if (value.Length > 0)
        {
            bufMain.AddRange(BitConverter.GetBytes(bufData.Count + 0x10 + AttackDatas.Count * 292).ToList());
            bufData.AddRange(Encoding.UTF8.GetBytes(value));
            bufData.Add(0);
        }
        else
        {
            bufMain.AddRange(BitConverter.GetBytes(0).ToList());
        }
    }
    
    public byte[] Rebuild()
    {
        List<byte> bufMain = [];
        List<byte> bufData = [];
        
        bufMain.AddRange(BitConverter.GetBytes(Magic).ToList());
        bufMain.AddRange(BitConverter.GetBytes(Version).ToList());
        bufMain.AddRange(AttackDatas != null
            ? BitConverter.GetBytes(AttackDatas.Count).ToList()
            : BitConverter.GetBytes(0).ToList());
        bufMain.AddRange(BitConverter.GetBytes(0).ToList());

        if (AttackDatas == null)
        {
            bufMain.AddRange(bufData);
            return bufMain.ToArray();
        }
        foreach (var attackData in AttackDatas)
        {
            bufMain.AddRange(Encoding.ASCII.GetBytes(attackData.Id));
            InsertDataString(attackData.Name, bufMain, bufData);
            InsertDataString(attackData.JpName, bufMain, bufData);
            bufMain.AddRange(BitConverter.GetBytes(attackData.Damage).ToList());
            bufMain.AddRange(Encoding.ASCII.GetBytes(attackData.CpuId0));
            bufMain.AddRange(BitConverter.GetBytes(attackData.HitPriority).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.MeterGainPlayer).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.MeterGainBlock).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.MeterGainHit).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt0).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.SelfHitstop).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.CollisionDisableTimer).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.Hitstop).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.HitExit).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt1).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt2).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.ComboLimitAdd).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.AttackFlags).ToList());
            bufMain.AddRange(Encoding.ASCII.GetBytes(attackData.GroundKnockbackId));
            bufMain.AddRange(Encoding.ASCII.GetBytes(attackData.AirKnockbackId));
            bufMain.AddRange(Encoding.ASCII.GetBytes(attackData.BlockKnockbackId));
            bufMain.AddRange(Encoding.ASCII.GetBytes(attackData.UnkKnockbackId0));
            bufMain.AddRange(Encoding.ASCII.GetBytes(attackData.UnkKnockbackId1));
            bufMain.AddRange(Encoding.ASCII.GetBytes(attackData.SpecialKnockbackId));
            bufMain.AddRange(Encoding.ASCII.GetBytes(attackData.GroundClashKnockbackId));
            bufMain.AddRange(Encoding.ASCII.GetBytes(attackData.AirClashKnockbackId));
            bufMain.AddRange(Encoding.ASCII.GetBytes(attackData.CounterHitId));
            InsertDataStringUtf8(attackData.HitSe, bufMain, bufData);
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkFloat3).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkFloat4).ToList());
            bufMain.AddRange(Encoding.ASCII.GetBytes(attackData.CpuId1));
            bufMain.AddRange(Encoding.ASCII.GetBytes(attackData.CpuId2));
            bufMain.AddRange(Encoding.ASCII.GetBytes(attackData.CpuId3));
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt3).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt4).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt5).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt6).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt7).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt8).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt9).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt10).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt11).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt12).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt13).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt14).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt15).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt16).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt17).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt18).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt19).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt20).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt21).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt22).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt23).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkFloat5).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt24).ToList());
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt25).ToList());
            InsertDataString(attackData.UnkName, bufMain, bufData);
            bufMain.AddRange(BitConverter.GetBytes(attackData.UnkInt26).ToList());
        }

        bufMain.AddRange(bufData);
        return bufMain.ToArray();
    }
}