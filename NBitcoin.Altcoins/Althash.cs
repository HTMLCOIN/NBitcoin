using NBitcoin;
using NBitcoin.DataEncoders;
using NBitcoin.Protocol;
using NBitcoin.RPC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NBitcoin.Altcoins
{
	// Reference: https://github.com/HTMLCOIN/HTMLCOIN/blob/master-2.5/src/chainparams.cpp
	public class Althash : NetworkSetBase
	{
		public static Althash Instance { get; } = new Althash();

		public override string CryptoCode => "HTML";

		private Althash()
		{

		}
		public class AlthashConsensusFactory : ConsensusFactory
		{
			private AlthashConsensusFactory()
			{
			}
			public static AlthashConsensusFactory Instance { get; } = new AlthashConsensusFactory();

			public override BlockHeader CreateBlockHeader()
			{
				return new AlthashBlockHeader();
			}
			public override Block CreateBlock()
			{
				return new AlthashBlock(new AlthashBlockHeader());
			}
		}

#pragma warning disable CS0618 // Type or member is obsolete
		public class AuxPow : IBitcoinSerializable
		{
			uint256 hashStateRoot = uint256.Zero;

			public uint256 HashStateRoot
			{
				get
				{
					return hashStateRoot;
				}
				set
				{
					hashStateRoot = value;
				}
			}

			uint256 hashUtxoRoot = uint256.Zero;

			public uint256 HashUtxoRoot
			{
				get
				{
					return hashUtxoRoot;
				}
				set
				{
					hashUtxoRoot = value;
				}
			}

			OutPoint prevoutStake = new OutPoint(uint256.Zero, uint.MaxValue);

			public OutPoint PrevoutStake
			{
				get
				{
					return prevoutStake;
				}
				set
				{
					prevoutStake = value;
				}
			}

			byte[] blockSignature = null;

			public byte[] BlockSignature
			{
				get
				{
					return blockSignature;
				}
				set
				{
					blockSignature = value;
				}
			}

			public void ReadWrite(BitcoinStream stream)
			{
				stream.ReadWrite(ref hashStateRoot);
				stream.ReadWrite(ref hashUtxoRoot);
				stream.ReadWrite(ref prevoutStake);
				stream.ReadWriteAsVarString(ref blockSignature);
			}
		}

		public class AlthashBlock : Block
		{
			public AlthashBlock(AlthashBlockHeader header) : base(header)
			{

			}

			public override ConsensusFactory GetConsensusFactory()
			{
				return AlthashConsensusFactory.Instance;
			}
		}
		public class AlthashBlockHeader : BlockHeader
		{
			AuxPow auxPow = new AuxPow();

			public AuxPow AuxPow
			{
				get
				{
					return auxPow;
				}
				set
				{
					auxPow = value;
				}
			}

			public override void ReadWrite(BitcoinStream stream)
			{
				base.ReadWrite(stream);
				stream.ReadWrite(ref auxPow);
			}
		}
#pragma warning restore CS0618 // Type or member is obsolete

		//Format visual studio
		//{({.*?}), (.*?)}
		//Tuple.Create(new byte[]$1, $2)
		//static Tuple<byte[], int>[] pnSeed6_main = null;
		//static Tuple<byte[], int>[] pnSeed6_test = null;

		protected override NetworkBuilder CreateMainnet()
		{
			var builder = new NetworkBuilder();
			builder.SetConsensus(new Consensus()
			{
				SubsidyHalvingInterval = 7680000,
				MajorityEnforceBlockUpgrade = 15120,
				MajorityRejectBlockOutdated = 15120,
				MajorityWindow = 15120,
                BIP34Hash = new uint256("966e30dd04d09232f6f690a04664cd3258abe43eeda2f2291d93706aa494aa54"),
				PowLimit = new Target(new uint256("0000ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff")),
				PowTargetTimespan = TimeSpan.FromSeconds(4000),
				PowTargetSpacing = TimeSpan.FromSeconds(2 * 64),
				PowAllowMinDifficultyBlocks = true,
                MinimumChainWork = new uint256("0000000000000000000000000000000000000000000000000000000000000000"),
				// Reference: https://github.com/HTMLCOIN/HTMLCOIN/blob/c17d801571080d0721c8eddb394f919eab5d60f6/src/consensus/consensus.h#L27
				CoinbaseMaturity = 500,
				PowNoRetargeting = false,
				ConsensusFactory = AlthashConsensusFactory.Instance,
				LitecoinWorkCalculation = false,
				SupportSegwit = true
			})
			.SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 100 })
			.SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 110 })
			.SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 239 })
			.SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x04, 0x35, 0x87, 0xCF })
			.SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x04, 0x35, 0x83, 0x94 })
			.SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("tq"))
			.SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("tq"))
			.SetMagic(0x1f2e3d4c)
			.SetPort(4888)
			.SetRPCPort(4889)
			.SetName("main")
			.AddAlias("althash-mainnet")
			.AddDNSSeeds(new[]{
				new DNSSeedData("seed1.htmlcoin.com", "seed2.htmlcoin.com"),
			})
			.AddSeeds(new NetworkAddress[0])
			.SetGenesis("0x0000bf23c6424c270a24a17a3db723361c349e0f966d7b55a6bca4bfb2d951b0");
			return builder;
		}

		protected override NetworkBuilder CreateTestnet()
		{
			var builder = new NetworkBuilder();
			builder.SetConsensus(new Consensus()
			{
				SubsidyHalvingInterval = 7680000,
				MajorityEnforceBlockUpgrade = 108,
				MajorityRejectBlockOutdated = 108,
				MajorityWindow = 108,
                BIP34Hash = new uint256("966e30dd04d09232f6f690a04664cd3258abe43eeda2f2291d93706aa494aa54"),
				PowLimit = new Target(new uint256("0000ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff")),
				PowTargetTimespan = TimeSpan.FromSeconds(4000),
				PowTargetSpacing = TimeSpan.FromSeconds(15 * 60),
				PowAllowMinDifficultyBlocks = true,
                MinimumChainWork = new uint256("0000000000000000000000000000000000000000000000000000000000000000"),
				CoinbaseMaturity = 500,
				PowNoRetargeting = false,
				ConsensusFactory = AlthashConsensusFactory.Instance,
				LitecoinWorkCalculation = false,
				SupportSegwit = true
			})
			.SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 120 })
			.SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 110 })
			.SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 239 })
			.SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x04, 0x35, 0x87, 0xCF })
			.SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x04, 0x35, 0x83, 0x94 })
			.SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("qcrt"))
			.SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("qcrt"))
			.SetMagic(0x2f3e4d5c)
			.SetPort(14888)
			.SetRPCPort(14889)
			.SetName("test")
			.AddAlias("althash-testnet")
			.AddDNSSeeds(new[]{
				new DNSSeedData("testnet-seed1.htmlcoin.com", "testnet-seed2.htmlcoin.com"),
			})
			.AddSeeds(new NetworkAddress[0])
			// Incorrect, using mainnet for now
			.SetGenesis("0x000013694772f8aeb88efeb2829fe5d71fbca3e23d5043baa770726f204f528c");
			return builder;
		}

		protected override NetworkBuilder CreateRegtest()
		{
			var builder = new NetworkBuilder();
			builder.SetConsensus(new Consensus()
			{
				SubsidyHalvingInterval = 150,
				MajorityEnforceBlockUpgrade = 108,
				MajorityRejectBlockOutdated = 108,
				MajorityWindow = 108,
                BIP34Hash = new uint256("966e30dd04d09232f6f690a04664cd3258abe43eeda2f2291d93706aa494aa54"),
				PowLimit = new Target(new uint256("7fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff")),
				PowTargetTimespan = TimeSpan.FromSeconds(4000),
				PowTargetSpacing = TimeSpan.FromSeconds(15 * 60),
				PowAllowMinDifficultyBlocks = true,
                MinimumChainWork = new uint256("0000000000000000000000000000000000000000000000000000000000000000"),
				CoinbaseMaturity = 500,
				PowNoRetargeting = true,
				ConsensusFactory = AlthashConsensusFactory.Instance,
				LitecoinWorkCalculation = false,
				SupportSegwit = true
			})
			.SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 120 })
			.SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 110 })
			.SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 239 })
			.SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x04, 0x35, 0x87, 0xCF })
			.SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x04, 0x35, 0x83, 0x94 })
			.SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("qcrt"))
			.SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("qcrt"))
			.SetMagic(0x3f4e5d6c)
			.SetPort(24888)
			.SetRPCPort(14889)
			.SetName("regtest")
			.AddAlias("althash-regtest")
			.AddSeeds(new NetworkAddress[0])
			// Incorrect, using mainnet for now
			.SetGenesis("0x665ed5b402ac0b44efc37d8926332994363e8a7278b7ee9a58fb972efadae943");
			return builder;
		}

		protected override void PostInit()
		{
			// Reference: https://github.com/HTMLCOIN/HTMLCOIN/blob/c17d801571080d0721c8eddb394f919eab5d60f6/src/util/system.cpp#L701
			RegisterDefaultCookiePath("HTMLCOIN");
		}

	}
}
